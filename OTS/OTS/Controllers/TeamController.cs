
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using OTS.CommonFilters;
using OTS.database_Access_Layer;
using OTS.Models;
using System.Web.Script.Serialization;

namespace OTS.Controllers
{
    [LoginFilter]
    public class TeamController : Controller
    {
        TeamDB tm_layer = new TeamDB();
        successFailureModel sm = new successFailureModel();
        CustomerEntities ce = new CustomerEntities();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        #region Team Timesheet
        public ActionResult Timesheet()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetTeam_Timesheet(int month, int year)
        {
            var jsonobj = "";
            try
            {
                var dt= tm_layer.GetTeam_Timesheet(month, year, Convert.ToInt32(Session["Emp_id"].ToString()));
                jsonobj =Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch(Exception ex)
            {
                jsonobj = ex.Message;
            }
            
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult TimeSheetDetails(int Month, int Year, int Emp_id)
        {
            var jsonobj = "";
            var jsonobj1 = "";
            try
            {
                var ds= tm_layer.EmpTimeSheetMng(Month, Year, Emp_id);//Employee time sheet for Manager View 
                jsonobj = Utility.DataTableToJSONWithJSONNet(ds.Tables[0]);
                jsonobj1 = Utility.DataTableToJSONWithJSONNet(ds.Tables[1]);
            }
            catch(Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(new { jsonobj,jsonobj1},JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Manager_TeamView(int Month, int Year, int Emp_id)
        {
            var jsonobj = "";
            try
            {
                var dt = tm_layer.GetTeam_Timesheet(Month, Year, Emp_id);//Employee time sheet for Manager Team View 
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TMApproved_Mng(int EMP_ID,int MONTH,int YEAR,string APPROVE_REMARK)// function For TimeSheet Approved By Manager
        {
            try
            {
                string res = tm_layer.TMApproved_Mng(MONTH, YEAR, EMP_ID, APPROVE_REMARK, Convert.ToInt32(Session["Emp_id"].ToString()));
                string[] splitmsg = res.Split(',');
                if (splitmsg[0] == "Update")
                {
                    //string page_type = "Timesheet";
                    //string reply_mail = SendMail_ManagerApproval(month, year, splitmsg[1], splitmsg[2], splitmsg[3], page_type, Approve_Remark);
                    sm.SuccessMsg = "" + splitmsg[3].ToUpper() + " Timesheet has been Approved.";
                }
                else
                {
                    sm.ErrorMsg = res;
                }
            }
            catch (Exception ex)
            {
                sm.ErrorMsg = ex.Message;
            }
            return Json(sm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TMRejected_Mng(int EMP_ID, int MONTH, int YEAR, string REJECT_REMARK)//Fuction for Timesheet Rejected By Manager
        {
            try
            {
                string res = tm_layer.TMReject_Mng(MONTH, YEAR, EMP_ID, REJECT_REMARK, Convert.ToInt32(Session["Emp_id"].ToString()));
                string[] splitmsg = res.Split(',');
                if (splitmsg[0] == "Update")
                {
                    //string page_type = "Timesheet";
                    //string reply_mail = SendMail_ManagerRejection(month, year, splitmsg[1], splitmsg[2], splitmsg[3], page_type, Reject_Remark);
                    sm.SuccessMsg = "" + splitmsg[2].ToUpper() + " Timesheet has been Rejected.";
                }
                else
                {
                    sm.ErrorMsg = res;
                }
            }
            catch (Exception ex)
            {
                sm.ErrorMsg = ex.Message;
            }
            return Json(sm, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Team Expense
        public ActionResult Expense()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetTeam_Expense(int month, int year)
        {
            var jsonobj = "";
            try
            {
                var dt = tm_layer.GetTeam_Expense(month, year, Convert.ToInt32(Session["Emp_id"].ToString()));
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;
            }

            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ExpenseDetails(int Month, int Year, int Emp_id)
        {
            var jsonobj = "";
            try
            {
                var dt = tm_layer.EmpExpenseMng(Month, Year, Emp_id);//Employee Expense for Manager View 
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch(Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Attendance
        public ActionResult Attendance()
        {
            return View();
        }
        public JsonResult GetTeam_Attendance(int month, int year)
        {
            var jsonobj = "";
            try
            {
                var dt = tm_layer.GetTeam_Attendance(month, year);
                jsonobj =Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;
            }

            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Goal
        public ActionResult Goal()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetTeam_Goal(string goal_quarter, int quarter_year)
        {            
            var jsonobj = "";            
            try
            {
                var dt = tm_layer.GetTeam_Goal(goal_quarter, quarter_year, Convert.ToInt32(Session["Emp_id"].ToString()));
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTeam_GoalHistory(string quarter, int year, int emp_id)
        {
            var goal = "";
            var personal = "";
            var org = "";
            //Convert.ToInt32(Session["Emp_id"])
            try
            {
                var ds = tm_layer.GetTeam_Goal_History(quarter, year, emp_id);
                goal = Utility.DataTableToJSONWithJSONNet(ds.Tables[0]);
                personal = Utility.DataTableToJSONWithJSONNet(ds.Tables[1]);
                org = Utility.DataTableToJSONWithJSONNet(ds.Tables[2]);
            }
            catch (Exception ex)
            {

            }
            return Json(new { goal, personal, org }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTeamManager_GoalHistory(string quarter, int year, int manager_id)
        {
            var goal = "";
            var personal = "";
            var org = "";
            //Convert.ToInt32(Session["Emp_id"])
            try
            {
                var ds = tm_layer.GetTeam_Goal_History(quarter, year, manager_id);
                goal = Utility.DataTableToJSONWithJSONNet(ds.Tables[0]);
                personal = Utility.DataTableToJSONWithJSONNet(ds.Tables[1]);
                org = Utility.DataTableToJSONWithJSONNet(ds.Tables[2]);
            }
            catch (Exception ex)
            {

            }
            return Json(new { goal, personal, org }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult update_goal(string status, string goal_quarter, int quarter_year ,List<Team_Goal> goal,List<Team_Personal> personal, List<Team_Organizational> organizational)
        {
            Apply_Goal cg = new Apply_Goal();
            try
            {
                int emp_id = Convert.ToInt32(Session["Emp_id"].ToString());
                string res = tm_layer.update_goal(emp_id, status, goal_quarter, quarter_year, goal,personal,organizational);
                if (res == "SUCCESS")
                {
                    cg.Success_msg = res;
                }
                else
                {
                    cg.Error_msg = res;
                }
            }

            catch (Exception ex)
            {
                cg.Error_msg = ex.Message;
            }
            return Json(cg, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region bootstrapTable
        public ActionResult bootstrapTable()
        {            
            return View();            
        }
        [HttpPost]
        public JsonResult CreateTempCustomer(CustomerEntities tempModel)
        {
            MastersModel mm = new MastersModel();
            string res = "";
            try
            {
                res = tm_layer.CreateTempCustomer(tempModel);
                if (res == "Success")
                {
                    mm.SuccessMsg = "Temp Customer is successfully created.";
                }
                else
                {
                    mm.ErrorMsg = res;
                }
            }
            catch (Exception ex)
            {
                mm.ErrorMsg = ex.Message;
            }
            return Json(mm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateBulkCustomer(List<CustomerEntities> tempModel)
        {
            MastersModel mm = new MastersModel();
            string res = "";
            try
            {
                res = tm_layer.CreateBulkCustomer(tempModel);
                if (res == "Success")
                {
                    mm.SuccessMsg = "Temp Customer is successfully created.";
                }
                else
                {
                    mm.ErrorMsg = res;
                }
            }
            catch (Exception ex)
            {
                mm.ErrorMsg = ex.Message;
            }
            return Json(mm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateSimpleTempCustomer(CustomerEntities ce)
        {
            MastersModel mm = new MastersModel();
            string res = "";
            try
            {
                res = tm_layer.CreateSimpleTempCustomer(ce.Customer_Code, ce.Customer_Name, ce.Address, ce.City, ce.State, ce.Country, ce.Zip_Code, ce.Status);
                if (res == "Success")
                {
                    mm.SuccessMsg = ce.Customer_Code.ToUpper() + " is successfully created.";
                }
                else
                {
                    mm.ErrorMsg = res;
                }
            }
            catch (Exception ex)
            {
                mm.ErrorMsg = ex.Message;
            }
            return Json(mm, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Get_TempCustomerDetails()
        {
            List<CustomerEntities> custlist = new List<CustomerEntities>();
            try
            {
                
                ds = tm_layer.Get_TempCustomerDetails();
                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    custlist.Add(new CustomerEntities {
                        //Customer_Id = Convert.ToInt32(dr["SR_NO"]),
                        Customer_Name = dr["customer_name"].ToString(),
                        Customer_Code = dr["customer_code"].ToString(),                                                
                        City = dr["city"].ToString(),
                        State =dr["state"].ToString(),
                        Country =dr["country"].ToString(),
                        Zip_Code =Convert.ToInt32(dr["zip_code"]),
                        Address = dr["address"].ToString(),
                        Status =Convert.ToInt32(dr["status"])
                    });
                    
                }
                //custlist = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                
            }
            return Json(custlist, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Demo
        public ActionResult Demo()
        {
            return View();
        }
        #endregion
    }
}