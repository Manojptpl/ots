using OTS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using OTS.CommonFilters;
using System.Web.Mvc;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Web.Script.Serialization;
using OTS.database_Access_Layer;

namespace OTS.Controllers
{
    [LoginFilter]
    public class TimesheetController : Controller
    {
        HomeModel hm = new HomeModel();
        db dblayer = new db();
        TimesheetDB ts_layer = new TimesheetDB();

        public ActionResult Create_Timesheet()
        {
            try
            {
                DataSet ds = dblayer.Bind_DropDownList();
                DataTable dt = ds.Tables[0];

                DataRow[] result = dt.Select("worktype_id <> 8");
                List<SelectListItem> lii = new List<SelectListItem>()
                {
                    new SelectListItem{Text="Select",Value="0"},
                };
                foreach (DataRow dr in result)
                {
                    lii.Add(new SelectListItem { Text = @dr["worktype_name"].ToString(), Value = @dr["worktype_id"].ToString() });
                }
                ViewBag.worklist = lii;

                ViewBag.custtype = ds.Tables[4];
                List<SelectListItem> lst = new List<SelectListItem>()
                {
                    new SelectListItem{Text="Select",Value="0"},
                };
                foreach (DataRow dr in ViewBag.custtype.Rows)
                {
                    lst.Add(new SelectListItem { Text = @dr["customer_name"].ToString(), Value = @dr["customer_id"].ToString() });
                }
                ViewBag.Customerlist = lst;

                List<SelectListItem> li = new List<SelectListItem>() {
                    new SelectListItem{Text="Select",Value="0"},
                };

                ViewBag.projectlist = li;
            }

            catch (Exception e)
            {

            }

            return View();
        }
        [HttpPost]
        public JsonResult GetDuplicateWorkType(DateTime Date, int WorkType_id)
        {
            string res = "";
            try
            {
                res = ts_layer.GetDuplicateWorkType(Convert.ToInt32(Session["Emp_id"]), Date, WorkType_id);
                hm.SuccessMsg = res;
            }
            catch (Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }

            return Json(hm, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProjectList(int Customer_Id)
        {
            var jsonobj = "";
            try
            {
                var dt= dblayer.Bind_ProjectList(Customer_Id);
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch(Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerList()
        {
            DataSet ds = dblayer.Bind_DropDownList();
            ViewBag.custtype = ds.Tables[4];
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (System.Data.DataRow dr in ViewBag.custtype.Rows)
            {
                hmlist.Add(new HomeModel { Customer = @dr["customer_name"].ToString(), Customer_id = Convert.ToInt32(@dr["customer_id"].ToString()) });
            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWorkTypeDRecord(int WorkType_id, DateTime Time_From, DateTime Time_To, DateTime Timesheet_date)
        {
            string res = "";
            try
            {
                res = ts_layer.CheckTimeSlot(Convert.ToInt32(Session["Emp_id"]), Time_From, Time_To, Timesheet_date);
                if (res != "Slot Exists.")
                {
                    res = ts_layer.GetWorkTypeDRecord(Convert.ToInt32(Session["Emp_id"]), Timesheet_date, WorkType_id);
                    hm.SuccessMsg = res;

                }
                else
                {
                    hm.ErrorMsg = "Entry for the same time already exists.";
                }
            }
            catch (Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }
            return Json("hm", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDuplicateRecord(DateTime Date, int WorkType_id, int Project_id, int Customer_id)
        {
            int Emp_id = Convert.ToInt32(Session["Emp_id"]);
            string res = ts_layer.GetTimeSheetDuplicate(Emp_id, Date, WorkType_id, Project_id, Customer_id);
            hm.SuccessMsg = res;
            return Json(hm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SubmitMainPage(TimeSheetEntities entity)
        {
            int Emp_id = Convert.ToInt32(Session["Emp_id"]);
            int Add_By = Convert.ToInt32(Session["Emp_id"]);
            DateTime Add_Date = DateTime.Today;
            int Status = 1;
            int Deleted = 0;

            string res = ts_layer.SubmitTimeSheet(Emp_id, entity.Date, entity.WorkType_id, entity.Project_id, entity.Time_from, entity.Time_to, entity.Description, entity.Customer_id, Add_By, Add_Date, Status, Deleted);

            if (res == "Success")
            {
                hm.SuccessMsg = "Entry Successfully Created.";
            }
            else
            {
                hm.ErrorMsg = res;
            }

            return Json(hm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult BindTimesheetGrid()
        {
            DataTable dt = dblayer.BindTSGrid(Convert.ToInt32(Session["Emp_id"].ToString()));
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.Transaction_id = Convert.ToInt32(dr["id"].ToString());
                hm.ShowDate = dr["Date"].ToString();
                hm.Project = dr["Project"].ToString();
                hm.Total_Time = dr["Total_Time"].ToString();
                hm.Details = dr["description"].ToString();
                hm.Status = dr["Doc_Status"].ToString();
                ViewBag.DetailsData = dr["description"].ToString();
                hmlist.Add(hm);
            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        string GetJson(DataTable table)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            foreach (DataRow dataRow in table.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    row.Add(column.ColumnName.Trim(), dataRow[column]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        public ActionResult Timesheet_History()
        {
            try { 

                DataSet ds = dblayer.Bind_DropDownList();
            List<SelectListItem> li = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "Select", Value = "0"},

            };
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    li.Add(new SelectListItem { Text = dr["worktype_name"].ToString(), Value = dr["worktype_id"].ToString() });
                }
                ViewBag.worklist = li;

                List<SelectListItem> lii = new List<SelectListItem>()
                {
                    new SelectListItem{Text = "Select", Value = "0"},
                };
                foreach(DataRow dr in ds.Tables[4].Rows)
                {
                    lii.Add(new SelectListItem { Text = dr["customer_name"].ToString(), Value = dr["customer_id"].ToString() });
                }
                ViewBag.Customerlist = lii;


                List<SelectListItem> projectli = new List<SelectListItem>()
                {
                    new SelectListItem{Text = "Select", Value = "0" },
                };
                foreach(DataRow dr in ds.Tables[1].Rows)
                {
                    projectli.Add(new SelectListItem { Text = dr["project_name"].ToString(), Value = dr["project_id"].ToString() });
                }
                ViewBag.projectlist = projectli;
            }
            catch (Exception ex){

            }
            return View();
        }
        [HttpPost]
        public JsonResult TimeSheetSummary(int month, int year)
        {
            List<HomeModel> hmlist = new List<HomeModel>();
            List<Remarks_Model> rmlist = new List<Remarks_Model>();
            try
            {
                DataSet ds = dblayer.DateWiseSummaryRpt(month, year, Convert.ToInt32(Session["Emp_id"].ToString()));
                DataTable dt = ds.Tables[0];
                DataTable remarks = ds.Tables[1];
                DataTable dt1 = ts_layer.View_Report(Convert.ToInt32(Session["Emp_id"].ToString()), month, year);
                ViewBag.ResultData = GetJson(dt1);
                foreach (DataRow dr in dt.Rows)
                {
                    HomeModel hm = new HomeModel();
                    hm.Transaction_id = Convert.ToInt32(dr["id"].ToString());
                    hm.ShowDate = dr["Date"].ToString();
                    hm.WorkType = dr["WorkType"].ToString();
                    hm.Customer = dr["Customer"].ToString();
                    hm.Project = dr["Project"].ToString();
                    hm.ShowTimeFrom = dr["time_from"].ToString();
                    hm.ShowTimeTo = dr["time_to"].ToString();
                    hm.Total_Time = dr["Total_Time"].ToString();
                    hm.Details = dr["description"].ToString();
                    hm.Status = dr["Doc_Status"].ToString();
                    hm.submission_date = dr["user_submission"].ToString();
                    hm.approval_date = dr["mng_approval"].ToString();
                    hm.hr_approval = dr["hr_approval"].ToString();
                    hm.entryCreation_mth = Convert.ToInt32(dr["entryCreation_mth"].ToString());
                    hm.web_entry = Convert.ToBoolean(dr["web_entry"].ToString());
                    hm.android_entry = Convert.ToBoolean(dr["android_entry"].ToString());
                    ViewBag.DetailsData = dr["description"].ToString();
                    hmlist.Add(hm);
                }
                foreach (DataRow dr in remarks.Rows)
                {
                    Remarks_Model rm = new Remarks_Model();
                    rm.approve_remark_manager = dr["Approve_Remark_Manager"].ToString();
                    rm.approve_remark_hr = dr["Approve_Remark_Hr"].ToString();
                    rm.reject_remark_manager = dr["Reject_Remark_Manager"].ToString();
                    rm.reject_remark_hr = dr["Reject_Remark_Hr"].ToString();
                    rmlist.Add(rm);
                }
                return Json(new { hmlist, ViewBag.ResultData, rmlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TimeSheetForApproval(int month, int year)
        {
            try
            {
                string res = ts_layer.timesheetapproval(month, year, Convert.ToInt32(Session["Emp_id"].ToString()));
                string[] splitmsg = res.Split(',');
                if (splitmsg[0] == "Update")
                {
                    string page_type = "Timesheet";
                    string sms = SendMail_UserSubmission(splitmsg[1], month, year, splitmsg[2], splitmsg[3], page_type);
                    hm.SuccessMsg = "Submited.";
                    //hm.SuccessMsg = " Time Sheet Submited.";
                }
                else
                {
                    hm.ErrorMsg = res;
                }
            }
            catch (Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }
            return Json(hm, JsonRequestBehavior.AllowGet);
        }//Function for Timesheet Submit By User
        public string SendMail_UserSubmission(string manager_name, int month, int year, string manager_email, string Reply_ToMail, string page_type)
        {
            string month_name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

            using (MailMessage mm = new MailMessage("ots.support@prudencesoftech.net", manager_email))
            {
                mm.ReplyTo = new MailAddress(Reply_ToMail);
                string mail_body = "";
                if (page_type == "Timesheet")
                {
                    mm.Subject = "Time Sheet Submission Notification (" + Session["Emp_Name"].ToString() + ")";
                    mail_body = "<html><body>Hello " + manager_name + "," +
                                  "<p>This is an automatic notification from the Online Time Sheet system, informing you that " +
                                  Session["Emp_Name"].ToString() + " has submitted their Time Sheet for the month of " + month_name + " " + year + " for your Approval.</p>" +
                                  "<p>Login to the <a href = 'http://ots.prudencesoftech.in' title = 'Online Time Sheet system' target = _blank> OTS system </a> to view the details.<br/></p>" +
                                  "<p>Thank You,<br/>Human Resources<br/>Prudence Technology Pvt. Ltd.</p></body></html>";
                }
                else
                {
                    mm.Subject = "Expense Submission Notification (" + Session["Emp_Name"].ToString() + ")";
                    mail_body = "<html><body>Hello " + manager_name + "," +
                                  "<p>This is an automatic notification from the Online Time Sheet system, informing you that " +
                                  Session["Emp_Name"].ToString() + " has submitted their Expense for the month of " + month_name + " " + year + " for your Approval.</p>" +
                                  "<p>Login to the <a href = 'http://ots.prudencesoftech.in' title = 'Online Time Sheet system' target = _blank> OTS system </a> to view the details.<br/></p>" +
                                  "<p>Thank You,<br/>Human Resources<br/>Prudence Technology Pvt. Ltd.</p></body></html>";
                }

                mm.Body = mail_body;
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "mail.prudencesoftech.net";
                    smtp.EnableSsl = false;
                    NetworkCredential NetworkCred = new NetworkCredential("ots.support@prudencesoftech.net", "Tc#wo6s3eyYutbdaC5PP.BxC");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    try
                    {
                        smtp.Send(mm);
                        ViewBag.Message = "Email Sent Your Manager " + manager_name + " for Approval.";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                    }

                }
            }
            return ViewBag.Message;
        }
        public JsonResult DateWiseSummary(int month, int year)
        {
            List<HomeModel> hmlist = new List<HomeModel>();
            List<Remarks_Model> rmlist = new List<Remarks_Model>();
            try
            {
                DataSet ds = dblayer.DateWiseSummaryRpt(month, year, Convert.ToInt32(Session["Emp_id"].ToString()));
                DataTable dt = ds.Tables[0];
                DataTable remarks = ds.Tables[1];
                DataTable dt1 = ts_layer.View_Report(Convert.ToInt32(Session["Emp_id"].ToString()), month, year);
                ViewBag.ResultData = GetJson(dt1);
                foreach (DataRow dr in dt.Rows)
                {
                    HomeModel hm = new HomeModel();
                    hm.Transaction_id = Convert.ToInt32(dr["id"].ToString());
                    hm.ShowDate = dr["Date"].ToString();
                    hm.WorkType = dr["WorkType"].ToString();
                    hm.Customer = dr["Customer"].ToString();
                    hm.Project = dr["Project"].ToString();
                    hm.ShowTimeFrom = dr["time_from"].ToString();
                    hm.ShowTimeTo = dr["time_to"].ToString();
                    hm.Total_Time = dr["Total_Time"].ToString();
                    hm.Details = dr["description"].ToString();
                    hm.Status = dr["Doc_Status"].ToString();
                    hm.submission_date = dr["user_submission"].ToString();
                    hm.approval_date = dr["mng_approval"].ToString();
                    hm.hr_approval = dr["hr_approval"].ToString();
                    hm.entryCreation_mth = Convert.ToInt32(dr["entryCreation_mth"].ToString());
                    hm.web_entry = Convert.ToBoolean(dr["web_entry"].ToString());
                    hm.android_entry = Convert.ToBoolean(dr["android_entry"].ToString());
                    ViewBag.DetailsData = dr["description"].ToString();
                    hmlist.Add(hm);
                }
                foreach (DataRow dr in remarks.Rows)
                {
                    Remarks_Model rm = new Remarks_Model();
                    rm.approve_remark_manager = dr["Approve_Remark_Manager"].ToString();
                    rm.approve_remark_hr = dr["Approve_Remark_Hr"].ToString();
                    rm.reject_remark_manager = dr["Reject_Remark_Manager"].ToString();
                    rm.reject_remark_hr = dr["Reject_Remark_Hr"].ToString();
                    rmlist.Add(rm);
                }
                return Json(new { hmlist, ViewBag.ResultData, rmlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTimeSheetEdit(int Timesheet_id)
        {
            HomeModel hm = new HomeModel();
            var jsonobj = "";
            try
            {


                var dt = ts_layer.GetTimesheet(Timesheet_id);
                jsonobj = DataTableToJSONWithJSONNet(dt);
         

            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateMainPage(DateTime Date, int WorkType_id, int Project_id, DateTime Time_from, DateTime Time_to, string Description, int Customer_id, int ID)
        {

            string res = ts_layer.UpdateTimeSheet(Date, WorkType_id, Project_id, Time_from, Time_to, Description, Customer_id, ID);
            if (res == "Success Updated")
            {
                hm.SuccessMsg = res;
            }
            else
            {
                hm.ErrorMsg = res;
            }
            return Json(hm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTimeSheet(int Timesheet_id)
        {
            List<HomeModel> hmlist = new List<HomeModel>();
            try
            {
                DataTable dt = ts_layer.GetTimesheet(Timesheet_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        HomeModel hm = new HomeModel();
                        hm.ShowDate = dr["timesheet_date"].ToString();
                        hm.WorkType = dr["worktype"].ToString();
                        hm.Customer = dr["customer"].ToString();
                        hm.Project = dr["project"].ToString();
                        hm.ShowTimeFrom = dr["timefrom"].ToString();
                        hm.ShowTimeTo = dr["timeto"].ToString();
                        hm.Details = dr["description"].ToString();
                        hmlist.Add(hm);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelTimeSheetRecord(int transaction_id)
        {
            string res = ts_layer.DeleteRecordTS(transaction_id);
            hm.SuccessMsg = res;
            return Json(hm, JsonRequestBehavior.AllowGet);
        }

        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }

    }
}