//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Crouny.DAL.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class DeviceSetting
    {
        public int DeviceSettingsId { get; set; }
        public System.Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
    
        public virtual Device Device { get; set; }
    }
}
