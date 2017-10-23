using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfferWebSystem.Controllers
{
    public class WelcomeController : AppBaseController
    {
        // GET: Welcome
        public ActionResult Index()
        {
            if (IsSessionExist())
                return RedirectToAction("Index", "Dashboard");
            else {
                return View();
            }
        }
    }
}