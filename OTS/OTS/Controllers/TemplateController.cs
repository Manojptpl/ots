using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTS.Models;
using OTS.database_Access_Layer;
using OTS.CommonFilters;
namespace OTS.Controllers
{
    [LoginFilter]
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Settings()
        {
            return View();
        }
        public ActionResult CustomSearch()
        {
            return View();
        }
        public ActionResult TimesheetCategory()
        {
            return View();
        }
        public ActionResult CreateCustomer()
        {
            return View();
        }        

    }
}