using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace ManifestJsonHandler.Models.ViewModels
{
    public class ManifestJsonViewModel
    {
        [DisplayName("Site Id")]
        public string SiteId { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }

        public string Data { get; set; }

        public bool HasSaved { get; set; }
    }
}
