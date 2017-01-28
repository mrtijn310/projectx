using System;
using System.Collections.Generic;

namespace Crouny.Models
{
    public class DeviceModel
    {
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }

        public IEnumerable<PluginModel> DeviceScripts { get; set; }
    }
}
