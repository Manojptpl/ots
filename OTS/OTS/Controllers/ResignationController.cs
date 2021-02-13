using System;
using System.Data;
using System.Data.SqlClient;
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
    public class ResignationController : Controller
    {
        
        ResignationDB r_layer = new ResignationDB();
        ResignationModel rm = new ResignationModel();        
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        // GET: Resignation
        public ActionResult Emp_Resignation()
        {
            List<ResignationModel> EmpResignationList = new List<ResignationModel>();
            try
            {
                dt = r_layer.GetEmpResignation(Convert.ToInt32(Session["Emp_id"]));
                foreach (DataRow dr in dt.Rows)
                {

                    ResignationModel rm = new ResignationModel();
                    rm.Role = Convert.ToInt32(dr["Role"]);                   
                    rm.EMP_ID = Convert.ToInt32(Session["Emp_id"]);
                    rm.NAME = dr["Emp_Name"].ToString();
                    rm.REPORTING_MANAGER = dr["MANAGER_NAME"].ToString();
                    rm.DEPARTMENT = dr["department_name"].ToString();
                    rm.Current_Date = dr["currentDate"].ToString();
                    rm.LastWorking_Date = dr["LastWorkingDate"].ToString();
                    EmpResignationList.Add(rm);
                    //GetStatus();
                    //GetRole_id();
                }
                return View(EmpResignationList);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult CreateEmpResignation(ResignationModel rmodel)
        {            
            MastersModel mm = new MastersModel();
            string res = "";
            try
            {
                res = r_layer.CreateEmpResignation(rmodel.EMP_ID, rmodel.NAME, rmodel.REPORTING_MANAGER, rmodel.DEPARTMENT, rmodel.RESIGNATION_DATE, rmodel.LAST_WORKING_DATE,rmodel.Status);
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    mm.SuccessMsg = response[1];
                }
                else if(response[0] == "Error")
                {
                    mm.ErrorMsg = response[1];
                }
            }
            catch (Exception ex)
            {
                mm.ErrorMsg = ex.Message;
            }
            return Json(mm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateResignation(ResignationModel rmodel)
        {

            MastersModel mm = new MastersModel();
            string res = "";
            try
            {
                //res = r_layer.Update_Resignation(Convert.ToInt32(Session["EMP_ID"]), rmodel.Status);
                res = r_layer.Update_Resignation(rmodel.EMP_ID, rmodel.Status, Convert.ToInt32(Session["EMP_ID"]));
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    mm.SuccessMsg = response[1];
                }
                else if (response[0] == "Error")
                {
                    mm.ErrorMsg = response[1];
                }
            }
            catch (Exception ex)
            {
                mm.ErrorMsg = ex.Message;
            }
            return Json(mm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Resignation_History()
        {
            List<ResignationModel> ResignationList = new List<ResignationModel>();
            try
            {
                dt = r_layer.GetResignation_History(Convert.ToInt32(Session["EMP_ID"]));
                foreach (DataRow dr in dt.Rows)
                {
                    ResignationModel rm = new ResignationModel();
                    rm.Role = Convert.ToInt32(dr["Role"]);                                  
                    rm.EMP_ID = Convert.ToInt32(dr["EMP_ID"]);
                    rm.NAME = dr["NAME"].ToString();
                    rm.REPORTING_MANAGER = dr["REPORTING_MANAGER"].ToString();
                    rm.DEPARTMENT = dr["DEPARTMENT"].ToString();
                    rm.Current_Date = dr["RESIGNATION_DATE"].ToString();
                    rm.LastWorking_Date = dr["LAST_WORKING_DATE"].ToString();
                    rm.Show_Status = dr["Status"].ToString();
                    ResignationList.Add(rm);
                    GetStatus();
                    GetRole_id();
                }                
            }
            catch (Exception)
            {
                throw;
            }
            return View(ResignationList);            
        }

        public JsonResult EditResignation(int EMP_ID)
        {
            ResignationModel objRM = new ResignationModel();
            objRM = r_layer.GetResignationDetails(EMP_ID);
            return Json(objRM, JsonRequestBehavior.AllowGet);            
        }

        public ActionResult GetStatus()
        {
            List<SelectListItem> statusli = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Accepted",Value="1" },
                new SelectListItem { Text = "Rejected",Value="2" },
                new SelectListItem { Text = "In Progress",Value="3" }
            };
            ViewBag.Status_TYPE = statusli;
            return View(statusli);
        }

        public ActionResult GetRole_id()
        {
            try
            {
                int Role_id = r_layer.GetRole_id(Convert.ToInt32(Session["Emp_id"]));                
                ViewBag.Role_idType = Role_id;
                return View(Role_id);
            }
            catch (Exception)
            {
                return View();
            }
        }
        
    }
}