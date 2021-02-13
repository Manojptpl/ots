using OTS.CommonFilters;
using System;
using System.Collections.Generic;
using System.Data;
using OTS.Models;
using System.Web.Mvc;
using OTS.database_Access_Layer;

namespace OTS.Controllers
{
    public class GoalController : Controller
    {
        DataTable dt = new DataTable();
        HomeModel hm = new HomeModel();
        GoalDB Gdb = new GoalDB();
        database_Access_Layer.db dblayer = new database_Access_Layer.db();
        
        [LoginFilter]
        public ActionResult Create_Goal()
        {
            try
            {
                DataSet ds = dblayer.Bind_DropDownList();
                ViewBag.custtype = ds.Tables[4];
                List<SelectListItem> lst = new List<SelectListItem>();
                foreach (DataRow dr in ViewBag.custtype.Rows)
                {
                    lst.Add(new SelectListItem { Text = @dr["customer_name"].ToString(), Value = @dr["customer_id"].ToString() });
                }
                ViewBag.Customerlist = lst;
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public JsonResult GetProjectList(int Customer_Id)
        {
            DataTable dt = dblayer.Bind_ProjectList(Customer_Id);

            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                hmlist.Add(new HomeModel { Project_Name = @dr["project_name"].ToString(), Project_id = Convert.ToInt32(@dr["project_id"].ToString()) });
            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Goal_History()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GoalDetail(string quarter, int year)
        {
            var goal = "";
            var personal = "";
            var org = "";
            try
            {
                var ds = Gdb.GetGoalHistory(quarter, year, Convert.ToInt32(Session["Emp_id"]));
                goal = Utility.DataTableToJSONWithJSONNet(ds.Tables[0]);
                personal = Utility.DataTableToJSONWithJSONNet(ds.Tables[1]);
                org = Utility.DataTableToJSONWithJSONNet(ds.Tables[2]);
            }
            catch (Exception ex)
            {

            }
            return Json(new { goal, personal, org }, JsonRequestBehavior.AllowGet);
        }        
        
    }
}