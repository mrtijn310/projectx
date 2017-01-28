using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Crouny.DAL.EntityModel;
using Crouny.DAL.Interfaces;
using Crouny.Models;

namespace Crouny.DAL.Repositories
{
    public sealed class DeviceRepository: BaseRepository, IDeviceRepository
    {
        public DeviceRepository(CrounyEntities context) : base(context)
        {
        }

        public IEnumerable<DeviceModel> GetDevices(int accountId)
        {
            return Context.Devices.Where(d => d.AccountId == accountId)
                .Include(d=>d.Plugins)
                .ToList()
                .Select(Mapper.Map<DeviceModel>);
        }

        public IEnumerable<PluginModel> GetDeviceScripts(Guid deviceId)
        {
            return Context.Plugins
                .Where(p=>p.Devices.Any(d=>d.DeviceId == deviceId))
                .ToList()
                .Select(Mapper.Map<PluginModel>);
        }

        public string GetScript(int pluginId)
        {
            return Context.Plugins
                .Where(p => p.PluginId == pluginId).Select(p => p.Scriptname)
                .FirstOrDefault();
        }

        public void EditPlugin(int pluginId, PluginModel pluginModel)
        {
            // todo: concurrency checks? maybe version timestamp?
            var plugin = Context.Plugins.FirstOrDefault(p => p.PluginId == pluginId);
            if (plugin != null)
                plugin.Parameters = pluginModel.Parameters;

            Save();
        }
    }
}
