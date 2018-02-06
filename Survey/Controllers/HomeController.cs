using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Controllers
{
	[RoutePrefix("")]
    public class HomeController : Controller
    {
        // GET: Home
		[AllowAnonymous]
		[Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}