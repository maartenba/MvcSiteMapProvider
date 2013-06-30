using System.Web.Mvc;
using MvcSiteMapProvider.PostDemo.Models;

namespace MvcSiteMapProvider.PostDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [HttpGet]
        public ActionResult Index ()
        {
            var model = (HomeIndexModel) Session["Home.Index"] ?? new HomeIndexModel ();
            model.Verb = HttpVerbs.Get;

            return View (model);
        }

        [HttpPost]
        public ActionResult Index (HomeIndexModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Verb = HttpVerbs.Post;
                return View (model);
            }

            Session["Home.Index"] = model;

            return RedirectToAction ("Index");
        }

        [HttpGet]
        public ActionResult About ()
        {
            // GET`ing this action displays something and is available to
            // non-privildged users.

            return View ();
        }

        [HttpPost]
        [Authorize (Roles = "NoSuchRole")]
        public ActionResult About (int _)
        {
            // POST`ing to this action changes some data and is only available
            // to priviledged users.

            return View ();
        }

    }
}
