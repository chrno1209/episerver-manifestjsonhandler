using EPiServer.PlugIn;
using EPiServer.Web;
using ManifestJsonHandler.Models.ViewModels;
using ManifestJsonHandler.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ManifestJsonHandler.Controllers
{
    [GuiPlugIn(
        Area = PlugInArea.AdminMenu,
        DisplayName = "Manage manifest.json content",
        Description = "Tool to manage the manifest.json",
        UrlFromModuleFolder = "ManifestJsonHandlerAdmin"
    )]
    [Authorize(Roles = "Administrators,CmsAdmins")]
    public class ManifestJsonHandlerAdminController : Controller//, ICustomPlugInLoader
    {
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly IManifestJsonService _manifestJsonService;

        public ManifestJsonHandlerAdminController(ISiteDefinitionRepository siteDefinitionRepository, IManifestJsonService manifestJsonService)
        {
            _siteDefinitionRepository = siteDefinitionRepository;
            _manifestJsonService = manifestJsonService;
        }

        public ActionResult Index()
        {
            var hostList = GetHostsSelectListItems();
            var model = new ManifestJsonViewModel()
            {
                SiteList = hostList
            };

            if (hostList.Any())
            {
                model.Data = _manifestJsonService.Get(hostList.First().Value);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult GetDataBySite(string key)
        {
            var data = _manifestJsonService.Get(key);
            return Content(data);
        }

        [HttpPost]
        public ActionResult Save(ManifestJsonViewModel model)
        {
            _manifestJsonService.Save(model.SiteId, model.Data);

            model.HasSaved = true;
            model.SiteList = GetHostsSelectListItems();

            return View("Index", model);
        }

        private List<SelectListItem> GetHostsSelectListItems()
        {
            return _siteDefinitionRepository.List().SelectMany(d => d.Hosts, (s, h) =>
                new SelectListItem
                {
                    Value = _manifestJsonService.GetSiteKey(s.Id, h.Name),
                    Text = $"{s.Name} > {h.Name}"
                }).ToList();
        }

        public PlugInDescriptor[] List()
        {
            if (!User.IsInRole("Administrators") && !User.IsInRole("WebAdmins"))
                return (PlugInDescriptor[])null;
            return new PlugInDescriptor[1]
            {
                PlugInDescriptor.Load(this.GetType())
            };
        }
    }
}
