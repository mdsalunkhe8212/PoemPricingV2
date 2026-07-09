using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POEMPricing.Controllers
{
    public class UserController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View("Users");
        }
    }
}