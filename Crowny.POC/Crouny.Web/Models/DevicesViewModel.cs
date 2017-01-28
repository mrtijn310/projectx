using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crouny.Models;

namespace Crouny.Web.Models
{
    public class DevicesViewModel
    {

        public IEnumerable<DeviceModel> Devices { get; set; }
    }
}