using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Crouny.PushServer
{
    class Program
    {
        static void Main(string[] args)
        {
            PushServer pushServer = new PushServer(ConfigurationManager.ConnectionStrings["CrounySqlDependency"].ConnectionString);

            // wait for the task to complete.
            Task.WaitAll(pushServer.SetupPushServer(5555),
                pushServer.SetupPushClient(5555, "Test"),
                pushServer.SetupPushClient(5555, "EenT"));
        }
    }
}
