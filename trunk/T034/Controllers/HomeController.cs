﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace T034.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Sites");
        }
        public ActionResult Sites()
        {
            return View();
        }
    }
}
