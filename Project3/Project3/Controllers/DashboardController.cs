using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult drivers()
        {
            return View();
        }
        public ActionResult Facebook()
        {
            return View();
        }
        public ActionResult restaurants()
        {
            return View();
        }
        public ActionResult Delivery_Time()
        {
            return View();
        }
    }
}