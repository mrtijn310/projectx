using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crouny.DAL.EntityModel;
using Crouny.DAL.Interfaces;
using Crouny.DAL.Repositories;
using Crouny.Models;
using Crouny.Models.Helpers;
using Crouny.PushServer.Models;
using TableDependency.Enums;
using TableDependency.SqlClient;

namespace Crouny.PushServer
{
    public class PushServer
    {
        private readonly Queue<DependencyResults> _messagesToSend = new Queue<DependencyResults>();
        private readonly string _connectionString;

        public PushServer(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void SetupListner()
        {
            var sqlTableDependency = new SqlTableDependency<DependencyResults>(_connectionString, nameof(Rule));
            sqlTableDependency.OnChanged += TableDependencyOnChanged;
            sqlTableDependency.OnStatusChanged += TableDependencyOnStatusChanged;
            sqlTableDependency.Start();
        }

        private void TableDependencyOnStatusChanged(object sender, TableDependency.EventArgs.StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case TableDependencyStatus.StoppedDueToCancellation:
                case TableDependencyStatus.StoppedDueToError:

                    var tableDependency = sender as SqlTableDependency<DependencyResults>;
                    if (tableDependency != null)
                    {
                        tableDependency.OnChanged -= TableDependencyOnChanged;
                        tableDependency.OnStatusChanged -= TableDependencyOnStatusChanged;
                    }

                    SetupListner();
                    break;
            }
        }

        private void TableDependencyOnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<DependencyResults> e)
        {
            Console.WriteLine("Entity changed " + e.Entity.DeviceId);

            switch (e.ChangeType)
            {
                case ChangeType.Insert:
                case ChangeType.Update:
                    _messagesToSend.Enqueue(e.Entity);
                    break;
            }
        }


        public async Task SetupPushServer(int port)
        {
            SetupListner();

            //todo: add termination variable.
            await Task.Factory.StartNew(() =>
            {
                using (NetMQSocket serverSocket = new PublisherSocket())
                {
                    serverSocket.Bind("tcp://*:" + port);

                    // Setup base call to send information to connected clients.
                    Action<string, string, string> sendPayload = (frame, script, parameters) =>
                     {
                         string payload = JsonConvert.SerializeObject(new Payload
                         {
                             Script = script,
                             Params = parameters
                         });
                         //The sendmore frame allows clients to filter based on topics. todo: check if every device still gets the frame without using a topic.
                         serverSocket.SendMoreFrame(frame).SendFrame(payload);
                     };

                    IDeviceRepository deviceRepository = new DeviceRepository(new CrounyEntities());

                    while (true)
                    {
                        try
                        {
                            if (_messagesToSend.Count <= 0)
                            {
                                // Sleep to ease off the cpu, todo: rewrite to use waithandle.
                                Thread.Sleep(TimeSpan.FromMilliseconds(50));
                                continue;
                            }

                            // See if there is anything in the queue to send.
                            var item = _messagesToSend.Dequeue();
                            if (item == null)
                                continue;

                            var states = JsonConvert.DeserializeObject<IEnumerable<StateParameter>>(item.State);
                            // For now convert null to false, because null would result in no parameters which would crash the python script.
                            var programArguments = string.Join(" ", states.Select(s => s.Value ?? "false"));

                            sendPayload(item.DeviceId, deviceRepository.GetScript(item.PluginId) + ".py", programArguments);
                        }
                        catch (Exception ex)
                        {
                            // todo: create logging (log4net?).
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            });
        }

#if DEBUG
        /// <summary>
        /// Creates a testing client to see if messages are correctly received.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task SetupPushClient(int port, string deviceId)
        {
            // Todo: add terminiation option.
            await Task.Factory.StartNew(() =>
            {
                using (var subSocket = new SubscriberSocket())
                {
                    subSocket.Connect("tcp://localhost:" + port);
                    subSocket.Subscribe(deviceId);
                    Console.WriteLine("Subscriber socket connecting...");
                    while (true)
                    {
                        // First frame is always the topic.
                        subSocket.ReceiveFrameString();
                        // Second frame is the message.
                        string messageReceived = subSocket.ReceiveFrameString();
                        Console.WriteLine(deviceId + ":" + messageReceived);
                    }
                }
            });
        }
#endif
    }
}


