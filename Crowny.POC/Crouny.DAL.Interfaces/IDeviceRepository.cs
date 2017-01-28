using System;
using System.Collections.Generic;
using Crouny.Models;

namespace Crouny.DAL.Interfaces
{
    public interface IDeviceRepository
    {
        IEnumerable<DeviceModel> GetDevices(int accountId);
        IEnumerable<PluginModel> GetDeviceScripts(Guid deviceId);
        string GetScript(int pluginId);
        void EditPlugin(int pluginId, PluginModel pluginModel);
    }
}
