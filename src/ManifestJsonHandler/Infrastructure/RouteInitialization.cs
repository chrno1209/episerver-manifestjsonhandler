using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using System.Web.Mvc;
using System.Web.Routing;

namespace ManifestJsonHandler.Infrastructure
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class RouteInitialization: IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RouteTable.Routes.MapRoute("ManifestJsonRoute", "manifest.json", new { controller = "ManifestJsonHandler", action = "Index" });
        }

        public void Uninitialize(InitializationEngine context)
        {
            // ignore
        }
    }
}
