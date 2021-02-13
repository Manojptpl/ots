using OTS.CommonFilters;
using OTS.database_Access_Layer;
using OTS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OTS.Controllers
{
    [LoginFilter]
    public class ReportController : Controller
    {
        ReportDB Rp_Layer = new ReportDB();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        HomeModel hm = new HomeModel();
        CheckInOutModel cm = new CheckInOutModel();
        // GET: Report
        public ActionResult Status_Report()
        {
            return View();
        }
        public JsonResult MonthlyStatus(int Month, int Year)
        {
            List<HomeModel> hmlist = new List<HomeModel>();
            try
            {
                DataTable dt = Rp_Layer.MonthlyStatus(Month, Year);

                foreach (DataRow dr in dt.Rows)
                {
                    HomeModel hm = new HomeModel();
                    hm.Emp_code = dr["emp_code"].ToString();
                    hm.Employees = dr["emp_name"].ToString();
                    hm.Emp_id = Convert.ToInt32(dr["emp_id"].ToString());
                    hm.Status = dr["Ts_Status"].ToString();
                    hm.ExpenseStatus = dr["Exp_Status"].ToString();
                    ViewBag.EXPENSE_STATUS = dr["Exp_Status"].ToString();
                    hmlist.Add(hm);
                   
                }
                return Json(hmlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public JsonResult Expensedatatable(int Month, int Year, int Emp_id, string Emp_Code, string Emp_Name)
        {
            HomeModel hm = new HomeModel();
            var jsonobj = "";
            try
            {
                var dt = Rp_Layer.EmpExpenseMng(Month, Year, Emp_id);
                jsonobj = DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ExpenseAppByHr(int Month, int Year, int Emp_Id, string Approval_Remark)
        {
            try
            {
                string res = Rp_Layer.ApprovedExpenseByHr(Month, Year, Emp_Id, Approval_Remark, Convert.ToInt32(Session["Emp_id"].ToString()));
                string[] splitmsg = res.Split(',');
                if (splitmsg[0] == "Update")
                {
                    string page_type = "Expense";
                    string reply_mail = SendMail_HRApproval(Month, Year, splitmsg[1], splitmsg[2], page_type, Approval_Remark);
                    hm.SuccessMsg = "" + splitmsg[2].ToUpper() + " Expense has been Approved.";
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
        }// Expense Approved by HR
        public string SendMail_HRApproval(int month, int year, string User_Mail, string User_name, string page_type, string Approve_Remark)
        {
            string month_name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

            using (MailMessage mm = new MailMessage("ots.support@prudencesoftech.net", User_Mail))
            {
                mm.ReplyTo = new MailAddress("info@prudencesoftech.com");
                string mail_body = "";
                if (page_type == "Timesheet")
                {
                    mm.Subject = "Time Sheet Approval Notification";
                    mail_body = "<html><body>Hello " + User_name + "," +
                                  "<p>This is an automatic notification from the Online Time Sheet system, informing you that your HR has approved your Time Sheet for the month of " + month_name + " " + year + " .</p>" +
                                  "<p>Login to the <a href = 'http://ots.prudencesoftech.in' title = 'Online Time Sheet system' target = _blank> OTS system </a> to view the details.<br/></p>" +
                                  "<p><strong>Remark:</strong> <strong><mark>" + Approve_Remark + "</mark></strong></p>" +
                                  "<p>Thank You,<br/>Human Resources<br/>Prudence Technology Pvt. Ltd.</p></body></html>";
                }
                else
                {
                    mm.Subject = "Expense Approval Notification";
                    mail_body = "<html><body>Hello " + User_name + "," +
                                  "<p>This is an automatic notification from the Online Time Sheet system, informing you that your HR has approved your Expense for the month of " + month_name + " " + year + " .</p>" +
                                  "<p>Login to the <a href = 'http://ots.prudencesoftech.in' title = 'Online Time Sheet system' target = _blank> OTS system </a> to view the details.<br/></p>" +
                                  "<p><strong>Remark:</strong> <strong><mark>" + Approve_Remark + "</mark></strong></p>" +
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
                        ViewBag.Message = "Email Sent Employee " + User_name + ".";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                    }

                }
            }
            return ViewBag.Message;
        }
        public JsonResult RejectExpenseByHr(int Month, int Year, int Emp_Id, string Reject_Remark)// Ajax Function for Expense Rejected By HR  
        {
            try
            {
                string res = Rp_Layer.RejectExpenseByHr(Month, Year, Emp_Id, Reject_Remark, Convert.ToInt32(Session["Emp_id"].ToString()));
                string[] splitmsg = res.Split(',');
                if (splitmsg[0] == "Update")
                {
                    string page_type = "Expense";
                    string reply_mail = SendMail_HRRejection(Month, Year, splitmsg[1], splitmsg[2], page_type, Reject_Remark);
                    hm.SuccessMsg = "Rejected";
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
        }
        public string SendMail_HRRejection(int month, int year, string User_name, string User_email, string page_type, string Reject_Remark)
        {
            string month_name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

            using (MailMessage mm = new MailMessage("ots.support@prudencesoftech.net", User_email))
            {
                mm.ReplyTo = new MailAddress("info@prudencesoftech.com");
                string mail_body = "";
                if (page_type == "Timesheet")
                {
                    mm.Subject = "Time Sheet Rejection Notification";
                    mail_body = "<html><body>Hello " + User_name + "," +
                                  "<p>This is an automatic notification from the Online Time Sheet system, informing you that your HR has rejected  your Time Sheet for the month of " + month_name + " " + year + " .</p>" +
                                  "<p>Login to the <a href = 'http://ots.prudencesoftech.in' title = 'Online Time Sheet system' target = _blank> OTS system </a> to view the details.<br/></p>" +
                                  "<p><strong>Remark:</strong> <strong><mark>" + Reject_Remark + "</mark></strong></p>" +
                                  "<p>Thank You,<br/>Human Resources<br/>Prudence Technology Pvt. Ltd.</p></body></html>";
                }
                else
                {
                    mm.Subject = "Expense Rejection Notification";
                    mail_body = "<html><body>Hello " + User_name + "," +
                                  "<p>This is an automatic notification from the Online Time Sheet system, informing you that your HR has rejected  your Expense for the month of " + month_name + " " + year + " .</p>" +
                                  "<p>Login to the <a href = 'http://ots.prudencesoftech.in' title = 'Online Time Sheet system' target = _blank> OTS system </a> to view the details.<br/></p>" +
                                  "<p><strong>Remark:</strong> <strong><mark>" + Reject_Remark + "</mark></strong></p>" +
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
                        ViewBag.Message = "Email Sent Employee " + User_name + ".";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                    }

                }
            }
            return ViewBag.Message;
        }
        public ActionResult Employee_Attendance()
        {
            DataSet ds = Rp_Layer.Bind_DropDownList();
            ViewBag.emplist = ds.Tables[6];
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (DataRow dr in ViewBag.emplist.Rows)
            {
                li.Add(new SelectListItem { Text = @dr["emp_name"].ToString(), Value = @dr["emp_id"].ToString() });
            }
            ViewBag.Emplist = li;
            return View();
        }
        public JsonResult GetAttendanceHistory(CheckInOutModel chk)
        {
            dt = Rp_Layer.GetAttendaceDetails(chk.Employee_id, chk.From_date, chk.To_date);
            List<CheckInOutModel> chklist = new List<CheckInOutModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CheckInOutModel chk_m = new CheckInOutModel();
                    chk_m.emp_name = dr["Emp_name"].ToString();
                    chk_m.worktype = dr["worktype_name"].ToString();
                    chk_m.customer = dr["customer_name"].ToString();
                    chk_m.project = dr["project_name"].ToString();
                    chk_m.chk_in_date = dr["check_in_date"].ToString();
                    chk_m.chk_out_date = dr["check_out_date"].ToString();
                    string chk_lat = dr["Chk_in_lat"].ToString();
                    double l;
                    double.TryParse(chk_lat, out l);
                    chk_m.chk_in_lat = Convert.ToDouble(l);
                    string chk_long = dr["chk_in_long"].ToString();
                    double l1;
                    double.TryParse(chk_long, out l1);
                    chk_m.chk_in_long = Convert.ToDouble(l1);
                    string str = dr["chk_out_lat"].ToString();
                    double d;
                    double.TryParse(str, out d);
                    chk_m.chk_out_lat = Convert.ToDouble(d);
                    string str1 = dr["chk_out_long"].ToString();
                    double d1;
                    double.TryParse(str1, out d1);
                    chk_m.chk_out_long = Convert.ToDouble(d1);
                    chk_m.location = dr["location"].ToString();
                    chklist.Add(chk_m);
                }
            }
            return Json(chklist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmpExpense_Report()
        {
            try
            {
                ds = Rp_Layer.Bind_DropDownList();
                ViewBag.Dept = ds.Tables[5];
                List<SelectListItem> li = new List<SelectListItem>()
                {
                    new SelectListItem{ Text="Select",Value="0"},
                };
                foreach (DataRow dr in ViewBag.Dept.Rows)
                {
                    li.Add(new SelectListItem { Text = @dr["department_name"].ToString(), Value = @dr["department_id"].ToString() });
                }
                ViewBag.Deptlist = li;

                List<SelectListItem> lii = new List<SelectListItem>()
                {
                    new SelectListItem{ Text="Select",Value="0"},
                };
                ViewBag.Emplist = lii;
            }
            catch(Exception ex)
            {

            }
            
            return View();
        }
        [HttpGet]
        public JsonResult GetEmployeeList(int Dept_Id)
        {
            var jsonobj = "";
            try
            {
                var dt= Rp_Layer.BindEmployeeList(Dept_Id);
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch(Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult EmployeeWiseExpense(int Emp_id)
        {
            var jsonobj = "";
            try
            {
                var dt = Rp_Layer.EmployeeWiseExpense(Emp_id);
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ExpenseMBreakUp(int Emp_id)
        {
            List<HomeModel> hmlist = new List<HomeModel>();
            try
            {
                DataTable dt = Rp_Layer.ExpenseMBreakUp(Emp_id);

                foreach (DataRow dr in dt.Rows)
                {
                    HomeModel hm = new HomeModel();
                    hm.ShowDate = dr["tran_date"].ToString();
                    hm.ExpenseAmount = dr["Final_Amt"].ToString();
                    hmlist.Add(hm);
                }
                return Json(hmlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProjectExpense_Report()
        {
            ds = Rp_Layer.Bind_DropDownList();
            ViewBag.Dept = ds.Tables[5];
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.Dept.Rows)
            {
                li.Add(new SelectListItem { Text = @dr["department_name"].ToString(), Value = @dr["department_id"].ToString() });
            }
            ViewBag.Deptlist = li;

            List<SelectListItem> lii = new List<SelectListItem>();
            ViewBag.Projectlist = lii;
            return View();
        }
        public JsonResult GetProjectList(int Dept_Id)
        {
            dt = Rp_Layer.BindProjectList(Dept_Id);
            ViewBag.proj = dt;
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (System.Data.DataRow dr in ViewBag.proj.Rows)
            {
                hmlist.Add(new HomeModel { Project_Name = @dr["project_name"].ToString(), Project_id = Convert.ToInt32(@dr["project_id"].ToString()) });
            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProjectWiseTotalExpense(int Project_Id)
        {
            dt = Rp_Layer.ProjectWiseTotalExpense(Project_Id);
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.Project_Name = dr["project_name"].ToString();
                hm.Expense_cost = dr["Expense_Amt"].ToString();
                hmlist.Add(hm);

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProjectTotalExpenseBreakup(int Project_Id)
        {
            dt = Rp_Layer.ProjectTotalExpenseBreakup(Project_Id);
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.ShowDate = dr["tran_date"].ToString();
                hm.Expense_cost = dr["Expense_Amt"].ToString();
                hmlist.Add(hm);
            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProjectWiseMExpBreakup(int Project_id, int Month, int Year)
        {
            dt = Rp_Layer.ProjectWiseMExpBreakup(Project_id, Month, Year);
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.Emp_code = dr["emp_name"].ToString();
                hm.Expense_cost = dr["Expense_Amt"].ToString();
                hmlist.Add(hm);
            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }       
        public ActionResult DeptExpense_Report()
        {
            try
            {
                dt = Rp_Layer.AdminReport();

                List<HomeModel> hmlist = new List<HomeModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    HomeModel hm = new HomeModel();
                    hm.Departments = dr["Department"].ToString();
                    hm.Department_id = Convert.ToInt32(dr["Department_id"].ToString());
                    hm.No_of_projects = Convert.ToInt32(dr["No_of_Projects"].ToString());
                    hm.Total_cost = dr["total_cost"].ToString();
                    hmlist.Add(hm);
                }
                return View(hmlist);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public JsonResult MonthWiseExpenseAmt(int Month, int Year)
        {
            dt = Rp_Layer.MonthWiseExpenseCost(Month, Year, Convert.ToInt32(Session["Expense_DeptId"].ToString()));
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.Project = dr["Projects"].ToString();
                hm.No_of_Emp = dr["No_of_Emp"].ToString();
                hm.Expense_cost = dr["Expense_Amt"].ToString();
                hm.Project_id = Convert.ToInt32(dr["Project_id"].ToString());
                hmlist.Add(hm);

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProjectWiseExpense(int Month, int Year, int department_id)
        {
            Session["Expense_DeptId"] = department_id;
            dt = Rp_Layer.MonthWiseExpenseCost(Month, Year, department_id);
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.Project = dr["Projects"].ToString();
                hm.No_of_Emp = dr["No_of_Emp"].ToString();
                hm.Expense_cost = dr["Expense_Amt"].ToString();
                hm.Project_id = Convert.ToInt32(dr["Project_id"].ToString());
                hmlist.Add(hm);

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmpWiseExpense(int Project_id, int Month, int Year)
        {
            dt = Rp_Layer.EmployeeWiseExpenseReport(Project_id, Month, Year);
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.Employees = dr["Emp_Name"].ToString();
                hm.Employee_Expense = dr["Tran_Amt"].ToString();
                hmlist.Add(hm);
            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProjectTimesheet_Report()
        {
            ds = Rp_Layer.Bind_DropDownList();
            ViewBag.Dept = ds.Tables[5];
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.Dept.Rows)
            {
                li.Add(new SelectListItem { Text = @dr["department_name"].ToString(), Value = @dr["department_id"].ToString() });
            }
            ViewBag.Deptlist = li;

            
            List<SelectListItem> lii = new List<SelectListItem>();
            ViewBag.Projectlist = lii;
            return View();
        }
        public JsonResult ProjectWiseTotalHours(int Project_id)
        {
            dt = Rp_Layer.ProjectWiseTotalHours(Project_id);
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.Project_Name = dr["project_name"].ToString();
                hm.No_of_hours = dr["No_of_hours"].ToString();
                hmlist.Add(hm);
            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProjectTotalHoursBreakup(int Project_id)
        {
            dt = Rp_Layer.ProjectTotalHoursBreakup(Project_id);
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.ShowDate = dr["tran_date"].ToString();
                hm.No_of_hours = dr["no_hours"].ToString();
                hmlist.Add(hm);
            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProjectWiseTotalCost(int Project_id)
        {
            List<HomeModel> hmlist = new List<HomeModel>();
            try
            {
                ds = Rp_Layer.ProjectWiseTotalCostReport(Project_id);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                decimal common_cost = Convert.ToDecimal(ds.Tables[1].Rows[0]["total"]);
                ViewBag.Common_Cost = common_cost;

                foreach (DataRow dr in dt.Rows)
                {
                    HomeModel hm = new HomeModel();
                    hm.Employees = dr["Employee"].ToString();
                    hm.No_of_hours = dr["No_of_hours"].ToString();
                    hm.Employee_Cost = Convert.ToDecimal(dr["Emp_cost"].ToString());
                    hm.Common_Cost = common_cost.ToString();
                    hmlist.Add(hm);
                }
                return Json(hmlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProjectWiseMTSBreakUp(int Project_id, int Month, int Year)
        {
            List<HomeModel> hmlist = new List<HomeModel>();
            try
            {
                ds = Rp_Layer.EmployeeWiseReport(Project_id, Month, Year);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                decimal common_cost = Convert.ToDecimal(ds.Tables[1].Rows[0]["total"]);
                ViewBag.Common_Cost = common_cost;

                foreach (DataRow dr in dt.Rows)
                {
                    HomeModel hm = new HomeModel();
                    hm.Employees = dr["Employee"].ToString();
                    hm.No_of_hours = dr["No_of_hours"].ToString();
                    hm.Employee_Cost = Convert.ToDecimal(dr["Emp_cost"].ToString());
                    hm.Common_Cost = common_cost.ToString();
                    hmlist.Add(hm);
                }
                return Json(hmlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }

        // GET: Attendance
        public ActionResult Attendance_Status()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAttendence_Status(int Month, int Year)
        {
            List<CheckInOutModel> chklist = new List<CheckInOutModel>();
            if (Session["Emp_id"] != null)
            {
                dt = Rp_Layer.GetAttendanceStatus(Month, Year);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CheckInOutModel chk_m = new CheckInOutModel();
                        chk_m.emp_name = dr["emp_name"].ToString();
                        chk_m.emp_id = Convert.ToInt32(dr["emp_id"].ToString());
                        chk_m.Status = dr["attandance_status"].ToString();
                        chk_m.emp_code = dr["emp_code"].ToString();
                        chklist.Add(chk_m);
                    }
                    return Json(chklist, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(chklist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
           public JsonResult EmployeeInoutDetail(AttendanceModel am)
        {

            var jsonobj = "";
            try
            {
                var dt = Rp_Layer.GetCheckInOutHistory(Convert.ToInt32(am.Month), Convert.ToInt32(am.Year), Convert.ToInt32(am.Emp_id));
                jsonobj = DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception e)
            {
                jsonobj = e.Message;
            }
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
           public ActionResult DeptTimesheet_Report()
        {
            try
            {
                dt = Rp_Layer.AdminReport();

                List<HomeModel> hmlist = new List<HomeModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    HomeModel hm = new HomeModel();
                    hm.Departments = dr["Department"].ToString();
                    hm.Department_id = Convert.ToInt32(dr["Department_id"].ToString());
                    hm.No_of_projects = Convert.ToInt32(dr["No_of_Projects"].ToString());
                    hm.Total_cost = dr["total_cost"].ToString();
                    hmlist.Add(hm);

                }
                return View(hmlist);

            }
            catch (Exception ex)
            {

            }
            return View();
        }
           public JsonResult AdminMonthWiseProjectHours(int month, int year)
        {
            dt = Rp_Layer.MonthWiseProjectCost(month, year, Convert.ToInt32(Session["department_id"].ToString()));
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.Project = dr["Projects"].ToString();
                hm.No_of_hours = dr["No_of_hours"].ToString();
                hm.Project_cost = dr["project_cost"].ToString();
                hm.Project_id = Convert.ToInt32(dr["Project_id"].ToString());
                hmlist.Add(hm);

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
           public JsonResult AdminProjectWiseHours(int Month, int Year, int department_id)
        {
            Session["department_id"] = department_id;
            //dt = dblayer.AdminProjectWiseReport(department_id); old function for all record without month filter
            dt = Rp_Layer.MonthWiseProjectCost(Month, Year, department_id);
            List<HomeModel> hmlist = new List<HomeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                HomeModel hm = new HomeModel();
                hm.Project = dr["Projects"].ToString();
                hm.No_of_hours = dr["No_of_hours"].ToString();
                hm.Project_cost = dr["project_cost"].ToString();
                hm.Project_id = Convert.ToInt32(dr["Project_id"].ToString());
                hmlist.Add(hm);

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
           public JsonResult HoursWiseEmployee(int Project_id, int Month, int Year)
        {
            List<HomeModel> hmlist = new List<HomeModel>();
            try
            {
                ds = Rp_Layer.EmployeeWiseReport(Project_id, Month, Year);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                decimal common_cost = Convert.ToDecimal(ds.Tables[1].Rows[0]["total"]);
                ViewBag.Common_Cost = common_cost;

                foreach (DataRow dr in dt.Rows)
                {
                    HomeModel hm = new HomeModel();
                    hm.Employees = dr["Employee"].ToString();
                    hm.No_of_hours = dr["No_of_hours"].ToString();
                    hm.Employee_Cost = Convert.ToDecimal(dr["Emp_cost"].ToString());
                    hm.Common_Cost = common_cost.ToString();
                    hmlist.Add(hm);
                }
                return Json(hmlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(hmlist, JsonRequestBehavior.AllowGet);
        }
           public ActionResult EmpTimesheet_Report()
        {

            return View();
        }
        #region ExpenseReport
        public ActionResult Expense_Report()
        {
            DataSet ds = Rp_Layer.Bind_DropDownList();

            List<SelectListItem> li = new List<SelectListItem>()
            {
                new SelectListItem { Text="Select",Value="0" },
            };
            foreach (DataRow dr in ds.Tables[6].Rows)
            {
                li.Add(new SelectListItem { Text = @dr["emp_name"].ToString(), Value = @dr["emp_id"].ToString() });
            }
            ViewBag.Emplist = li;
            
            List<SelectListItem> deptlist = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select", Value = "0" },
            };
            foreach (DataRow dr in ds.Tables[5].Rows)
            {
                deptlist.Add(new SelectListItem { Text = @dr["department_name"].ToString(), Value = @dr["department_id"].ToString() });
            }
            ViewBag.Deptlist = deptlist;


            List<SelectListItem> projectlist = new List<SelectListItem>()
            {
                 new SelectListItem { Text = "Select", Value = "0" },
            };
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                projectlist.Add(new SelectListItem { Text = @dr["project_name"].ToString(), Value = @dr["project_id"].ToString() });
            }
            ViewBag.Projlist = projectlist;
            
            return View();
        }
        [HttpGet]
        public JsonResult ExpenseReport( int emp_id, int department_id, int project_id, string from, string to)
        {
            var jsonobj = "";
            try
            {
                var dt = Rp_Layer.GetExpenseDetails(emp_id, department_id, project_id, from, to);
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;
            }

            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}