using AutoMapper;
using Crouny.DAL.EntityModel;
using Crouny.Models;

namespace Crouny.DAL.Configuration
{
    public static class AutomapperConfig
    {
        /// <summary>
        /// Creates all the mappings between entities and models (models serve as DTO as well).
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Account, AccountModel>();
                cfg.CreateMap<Plugin, PluginModel>();
                cfg.CreateMap<Device, DeviceModel>()
                .ForMember(dest => dest.DeviceScripts, opt => opt.MapFrom(src => src.Plugins));
                cfg.CreateMap<Rule, RuleModel>()
                    .ForMember(dest => dest.Plugin, opt => opt.MapFrom(src => src.Plugin));
            });
        }
    }
}
