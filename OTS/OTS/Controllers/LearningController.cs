using System;
using System.Collections.Generic;
using System.Linq;
using OTS.CommonFilters;
using System.Web.Mvc;

namespace OTS.Controllers
{
    [LoginFilter]
    public class LearningController : Controller
    {
        // GET: Learning
        public ActionResult Traning()
        {
            return View();
        }
    }
}