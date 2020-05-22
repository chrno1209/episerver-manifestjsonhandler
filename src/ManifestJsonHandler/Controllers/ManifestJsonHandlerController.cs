using System.Linq;
using System.Web.Mvc;
using EPiServer.Web;
using ManifestJsonHandler.Services;

namespace ManifestJsonHandler.Controllers
{
    public class ManifestJsonHandlerController : Controller
    {
        private readonly IManifestJsonService _manifestJsonService;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;

        public ManifestJsonHandlerController(IManifestJsonService manifestJsonService, ISiteDefinitionRepository siteDefinitionRepository)
        {
            _manifestJsonService = manifestJsonService;
            _siteDefinitionRepository = siteDefinitionRepository;
        }

        public ActionResult Index()
        {
            var hostLookUpArray = _siteDefinitionRepository.List().SelectMany(sd => sd.Hosts, (sd, host) => host.Name).ToArray();
            var siteKey = _manifestJsonService.GetSiteKey(SiteDefinition.Current.Id, hostLookUpArray.Contains(Request.Url.Host) ? Request.Url.Host : "*");

            return Content(_manifestJsonService.Get(siteKey), "application/json");
        }
    }
}
