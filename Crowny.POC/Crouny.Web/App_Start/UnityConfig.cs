using Crouny.DAL.EntityModel;
using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Mvc;

namespace Crouny.Web
{
    public static class UnityConfig
    {
        public static UnityContainer RegisterComponents()
        {
            var container = new UnityContainer();

            //Inject all interfaces that match by convention.
            container.RegisterTypes(AllClasses.FromLoadedAssemblies(),
               WithMappings.FromMatchingInterface, WithName.Default, WithLifetime.Hierarchical);

            // Register the entity type explicity for injection to have a single instance of the context per request.
            container.RegisterType<CrounyEntities,CrounyEntities>();

            // Configures container for ASP.NET MVC
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            // Configure for webapi.
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

            return container;
        }
    }
}