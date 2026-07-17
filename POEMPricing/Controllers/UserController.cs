using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POEMPricing.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View("Users");
        }
    }
}