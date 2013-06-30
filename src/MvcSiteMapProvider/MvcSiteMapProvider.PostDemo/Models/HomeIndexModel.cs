using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcSiteMapProvider.PostDemo.Models
{
    public class HomeIndexModel
    {
        [Required (AllowEmptyStrings = false)]
        public string Name { get; set; }

        [HiddenInput (DisplayValue = false)]
        public HttpVerbs Verb { get; set; }
    }
}
