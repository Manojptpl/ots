using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using OTS.CommonFilters;
using OTS.Models;
using System.Data;
using OTS.database_Access_Layer;
namespace OTS.Controllers
{
    [LoginFilter]
    public class leaveController : Controller
    {
        LeaveDB dllleave = new LeaveDB();
        SettingDB dblayer = new SettingDB();
        // GET: leave
        DataTable mydt = new DataTable();
        DataSet myds = new DataSet();
        public ActionResult Leave_Type()
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 16);
            ViewBag.event_permission = info;

            List<LeaveType> leavetypelist = new List<LeaveType>();
            leavetypelist = dllleave.GetLeaveType();
            return View(leavetypelist);
        }
        
        #region Leave Policy
        public ActionResult LeavePolicy_History()
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 17);
            ViewBag.event_permission = info;

            Session["LeavePolicy"] = null;
            Session["YearEndProcessing"] = null;
            Session["LeavePlan"] = null;
            DataTable dt = new DataTable();
            dt = dllleave.LeavePolicyList();
            return View(dt);
        }
        public ActionResult Leave_Policy()
        {
            List<SelectListItem> islosseofpay = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select",Value="0" },
                new SelectListItem { Text = "Yes",Value="Yes" },
                new SelectListItem { Text = "No",Value="No" }
            };
            ViewBag.Islosseofpay = islosseofpay;
            List<SelectListItem> EmployerType = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select",Value="0" },
                new SelectListItem { Text = "Probation Period",Value="P" },
                new SelectListItem { Text = "Regular Period",Value="R" },
                new SelectListItem { Text = "Contract Period",Value="C" },
                new SelectListItem { Text = "Notice Period",Value="N" },
                new SelectListItem { Text = "All",Value="A" }
            };
            ViewBag.employertype = EmployerType;
            List<SelectListItem> status = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select",Value="0" },
                //new SelectListItem { Text = "Active",Value="Active" },
                //new SelectListItem { Text = "In-Active",Value="In-Active" },
                new SelectListItem { Text = "Draft",Value="Draft" },
                new SelectListItem { Text = "Enforce",Value="Enforce" }
            };
            ViewBag.Status = status;

            List<SelectListItem> statuscreaterule = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select",Value="0" },
                new SelectListItem { Text = "Active",Value="Active" },
                new SelectListItem { Text = "In-Active",Value="In-Active" }
            };
            ViewBag.statuscreaterule = statuscreaterule;
            ViewBag.leavetypeli = dllleave.Leavetypelistcreatepolicy();
            List<SelectListItem> gender = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select",Value="0" },
                new SelectListItem { Text = "Male",Value="M" },
                new SelectListItem { Text = "Female",Value="F" },
             //   new SelectListItem{ Text="Other",Value="O"},
                new SelectListItem{ Text="All",Value="A"}
            };
            ViewBag.Gender = gender;
            List<SelectListItem> leaveyear = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select",Value="0" },
                new SelectListItem { Text = "Financial Year",Value="Financial Year" },
                new SelectListItem { Text = "Calendar Year",Value="Calendar Year" }
            };
            ViewBag.LeaveYear = leaveyear;
            List<SelectListItem> liPeriod = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select",Value="0" },
                //new SelectListItem { Text = "Daily",Value="Daily" },
                //new SelectListItem { Text = "Weekly",Value="Weekly" },
                //new SelectListItem { Text = "Bi-weekly",Value="Bi-weekly" },
                new SelectListItem { Text = "Monthly",Value="Monthly" },
                //new SelectListItem { Text = "Bi-monthly",Value="Bi-monthly" },
                //new SelectListItem { Text = "Quarterly",Value="Quarterly" },
                //new SelectListItem { Text = "Half Yearly",Value="Half Yearly" },
                new SelectListItem { Text = "Yearly",Value="Yearly" }
            };
            ViewBag.Period = liPeriod;

            ViewBag.Grade = dllleave.GetDropdownGrade();

            List<SelectListItem> liUOM = new List<SelectListItem>()
            {   new SelectListItem { Text = "Select",Value="0" },
                // new SelectListItem { Text = "Hours",Value="Hours" },
                new SelectListItem { Text = "Day",Value="Day" },
                new SelectListItem { Text = "Month",Value="Month" },
                new SelectListItem { Text = "Year",Value="Year" }
            };
            ViewBag.UOM = liUOM;
            List<SelectListItem> liCredit = new List<SelectListItem>()
            {   new SelectListItem { Text = "Select",Value="0" },
                new SelectListItem { Text = "Start Of Period",Value="Start Of Period" },
                new SelectListItem { Text = "End Of Period",Value="End Of Period" }
            };
            ViewBag.Credit = liCredit;
            return View();
        }

        [HttpPost]
        public JsonResult leavepolicyview(string tbl_id)
        {
            LeavePolicy leavepolicy = new LeavePolicy();
            LeavePlan leaveplan = new LeavePlan();
            YearEndProcessing yearendprocessing = new YearEndProcessing();

            DataSet ds = new DataSet();
            ds = dllleave.GetLeavePolicyDetail(tbl_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                leavepolicy.LeaveTypeID = ds.Tables[0].Rows[0]["LEAVETYPE"].ToString();
                leavepolicy.PolicyName = ds.Tables[0].Rows[0]["POLICY_NAME"].ToString();
                leavepolicy.PolicyDescription = ds.Tables[0].Rows[0]["POLICY_DESCRIPTION"].ToString();
                leavepolicy.StartDate = ds.Tables[0].Rows[0]["STARTDATE"].ToString();
                leavepolicy.EndDate = ds.Tables[0].Rows[0]["ENDDATE"].ToString();
                leavepolicy.Status = ds.Tables[0].Rows[0]["STATUS"].ToString();
                leavepolicy.IsInformationOnly = Convert.ToInt32(ds.Tables[0].Rows[0]["ISINFORMATIONONLY"].ToString());
                leaveplan.AttachmentRequired = Convert.ToInt32(ds.Tables[0].Rows[0]["ATTACHMENT_ALLOWED"].ToString());
                leaveplan.Gender = ds.Tables[0].Rows[0]["GENDER"].ToString();
                leaveplan.LeaveYear = ds.Tables[0].Rows[0]["LEAVE_YEAR"].ToString();
                leaveplan.CreditFrequency = ds.Tables[0].Rows[0]["CREDITFREQUENCY"].ToString();
                leaveplan.Credit = ds.Tables[0].Rows[0]["CREDIT"].ToString();
                leaveplan.IncludePublicHolidays = ds.Tables[0].Rows[0]["INCLUDE_PUBLIC_HOLIDAYS"].ToString();
                leaveplan.IncludeWeekends = ds.Tables[0].Rows[0]["INCLUDE_WEEKENDS"].ToString();
                leaveplan.CanbeclubbedwithEL = ds.Tables[0].Rows[0]["CAN_BE_CLUBBED_WITH_EL"].ToString();
                leaveplan.CanbeclubbedwithCL = ds.Tables[0].Rows[0]["CAN_BE_CLUBBED_WITH_CL"].ToString();
                leaveplan.Canbehalfday = ds.Tables[0].Rows[0]["CAN_BE_HALF_DAY"].ToString();

                leaveplan.Probation_Period_P = ds.Tables[0].Rows[0]["IS_PROBATION_PERIOD_P"].ToString();
                leaveplan.Confirmed_P = ds.Tables[0].Rows[0]["CONFIRMED_P"].ToString();
                leaveplan.Contract_Period_P = ds.Tables[0].Rows[0]["IS_Contract_PERIOD_P"].ToString();
                leaveplan.Notice_Period_P = ds.Tables[0].Rows[0]["IS_NOTICE_PERIOD_P"].ToString();

                yearendprocessing.AllowCarryover_CarryoverLimit = ds.Tables[0].Rows[0]["CARRYOVER_LIMIT"].ToString();
                yearendprocessing.PayatYearend_MinBal = ds.Tables[0].Rows[0]["PAY_AT_YEAR_END_MIN_BALANCE"].ToString();
                yearendprocessing.PayatYearend_MaxEncashment = ds.Tables[0].Rows[0]["PAY_AT_YEAR_END_MAX_ENCASHMENT"].ToString();
                yearendprocessing.EligibleForEncashment_Limit = ds.Tables[0].Rows[0]["ELIGIBLE_FOR_ENCASHMENT_ENCASHMENT_LIMIT"].ToString();
                yearendprocessing.EligibleForEncashment_MinBal = ds.Tables[0].Rows[0]["ELIGIBLE_FOR_ENCASHMENT_MIN_BALANCE"].ToString();
                yearendprocessing.CarryForwardtoEL_CFLimit = ds.Tables[0].Rows[0]["CARRY_FORWARD_TO_EL"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                List<CreateRule> createruledli = new List<CreateRule>();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    CreateRule createrule = new CreateRule();
                    createrule.GradeID = ds.Tables[1].Rows[i]["GRADE_ID"].ToString();
                    createrule.Grade = ds.Tables[1].Rows[i]["GRADE_NAME"].ToString();
                    createrule.NoOfDays = ds.Tables[1].Rows[i]["DURATION"].ToString();
                    createrule.UOM = ds.Tables[1].Rows[i]["UOM"].ToString();
                    createrule.StartDate = ds.Tables[1].Rows[i]["START_DATE"].ToString();
                    createrule.EndDate = ds.Tables[1].Rows[i]["END_DATE"].ToString();
                    createrule.IslossofPay = ds.Tables[1].Rows[i]["IS_LOSS_OF_PAY"].ToString();
                    createrule.Employer_Type = ds.Tables[1].Rows[i]["Employer_Type"].ToString();
                    createrule.Plan_Status = ds.Tables[1].Rows[i]["PLAN_STATUS"].ToString();
                    createruledli.Add(createrule);
                }
                leaveplan.tblCreateRule = createruledli;
            }
            return Json(new { leavepolicy, leaveplan, yearendprocessing }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Editleave_Policy(string id)
        {
            try
            {
                LeavePolicy leavepolicy = new LeavePolicy();
                LeavePlan leaveplan = new LeavePlan();
                YearEndProcessing yearendprocessing = new YearEndProcessing();
                DataSet ds = new DataSet();
                editpolicy ev = new editpolicy();
                string leaveid = id;
                if (leaveid != null)
                {
                    ds = dllleave.GetLeavePolicyDetail(leaveid);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        leavepolicy.ID = Convert.ToInt32(leaveid);
                        leavepolicy.LeaveTypeID = ds.Tables[0].Rows[0]["LEAVETYPE"].ToString();
                        leavepolicy.PolicyName = ds.Tables[0].Rows[0]["POLICY_NAME"].ToString();
                        leavepolicy.PolicyDescription = ds.Tables[0].Rows[0]["POLICY_DESCRIPTION"].ToString();
                        leavepolicy.StartDate = ds.Tables[0].Rows[0]["STARTDATE"].ToString();
                        leavepolicy.EndDate = ds.Tables[0].Rows[0]["ENDDATE"].ToString();
                        leavepolicy.Status = ds.Tables[0].Rows[0]["STATUS"].ToString();
                        leavepolicy.IsInformationOnly = Convert.ToInt32(ds.Tables[0].Rows[0]["ISINFORMATIONONLY"].ToString());
                        leaveplan.AttachmentRequired = Convert.ToInt32(ds.Tables[0].Rows[0]["ATTACHMENT_ALLOWED"].ToString());
                        leaveplan.Gender = ds.Tables[0].Rows[0]["GENDER"].ToString();
                        leaveplan.LeaveYear = ds.Tables[0].Rows[0]["LEAVE_YEAR"].ToString();
                        leaveplan.CreditFrequency = ds.Tables[0].Rows[0]["CREDITFREQUENCY"].ToString();
                        leaveplan.Credit = ds.Tables[0].Rows[0]["CREDIT"].ToString();
                        leaveplan.IncludePublicHolidays = ds.Tables[0].Rows[0]["INCLUDE_PUBLIC_HOLIDAYS"].ToString();
                        leaveplan.IncludeWeekends = ds.Tables[0].Rows[0]["INCLUDE_WEEKENDS"].ToString();
                        leaveplan.CanbeclubbedwithEL = ds.Tables[0].Rows[0]["CAN_BE_CLUBBED_WITH_EL"].ToString();
                        leaveplan.CanbeclubbedwithCL = ds.Tables[0].Rows[0]["CAN_BE_CLUBBED_WITH_CL"].ToString();
                        leaveplan.Canbehalfday = ds.Tables[0].Rows[0]["CAN_BE_HALF_DAY"].ToString();

                        leaveplan.Probation_Period_P = ds.Tables[0].Rows[0]["IS_PROBATION_PERIOD_P"].ToString();
                        leaveplan.Confirmed_P = ds.Tables[0].Rows[0]["CONFIRMED_P"].ToString();
                        leaveplan.Contract_Period_P = ds.Tables[0].Rows[0]["IS_Contract_PERIOD_P"].ToString();
                        leaveplan.Notice_Period_P = ds.Tables[0].Rows[0]["IS_NOTICE_PERIOD_P"].ToString();

                        yearendprocessing.AllowCarryover_CarryoverLimit = ds.Tables[0].Rows[0]["CARRYOVER_LIMIT"].ToString();
                        yearendprocessing.PayatYearend_MinBal = ds.Tables[0].Rows[0]["PAY_AT_YEAR_END_MIN_BALANCE"].ToString();
                        yearendprocessing.PayatYearend_MaxEncashment = ds.Tables[0].Rows[0]["PAY_AT_YEAR_END_MAX_ENCASHMENT"].ToString();
                        yearendprocessing.EligibleForEncashment_Limit = ds.Tables[0].Rows[0]["ELIGIBLE_FOR_ENCASHMENT_ENCASHMENT_LIMIT"].ToString();
                        yearendprocessing.EligibleForEncashment_MinBal = ds.Tables[0].Rows[0]["ELIGIBLE_FOR_ENCASHMENT_MIN_BALANCE"].ToString();
                        yearendprocessing.CarryForwardtoEL_CFLimit = ds.Tables[0].Rows[0]["CARRY_FORWARD_TO_EL"].ToString();
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        List<CreateRule> createruledli = new List<CreateRule>();
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            CreateRule createrule = new CreateRule();
                            createrule.CREATERULE_ID = Convert.ToInt32(ds.Tables[1].Rows[i]["LEAVE_PLAN_ID"].ToString());
                            createrule.GradeID = ds.Tables[1].Rows[i]["GRADE_ID"].ToString();
                            createrule.Grade = ds.Tables[1].Rows[i]["GRADE_NAME"].ToString();
                            createrule.NoOfDays = ds.Tables[1].Rows[i]["DURATION"].ToString();
                            createrule.UOM = ds.Tables[1].Rows[i]["UOM"].ToString();
                            createrule.StartDate = ds.Tables[1].Rows[i]["START_DATE"].ToString();
                            createrule.EndDate = ds.Tables[1].Rows[i]["END_DATE"].ToString();
                            createrule.IslossofPay = ds.Tables[1].Rows[i]["IS_LOSS_OF_PAY"].ToString();
                            createrule.Employer_Type = ds.Tables[1].Rows[i]["Employer_Type"].ToString();
                            createrule.Plan_Status = ds.Tables[1].Rows[i]["PLAN_STATUS"].ToString();
                            createrule.row_class = ds.Tables[1].Rows[i]["ROW_CLASS"].ToString();
                            createruledli.Add(createrule);
                        }
                        leaveplan.tblCreateRule = createruledli;
                    }
                }
                else
                {
                    leaveid = null;
                    ds = null;
                }
                ev.LeavePolicy = leavepolicy;
                ev.LeavePlan = leaveplan;
                ev.YearEndProcessing = yearendprocessing;
                List<SelectListItem> islosseofpay = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Select",Value="0" },
                    new SelectListItem { Text = "Yes",Value="Yes" },
                    new SelectListItem { Text = "No",Value="No" }

                };
                ViewBag.Islosseofpay = islosseofpay;

                List<SelectListItem> statuscreaterule = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Select",Value="0" },
                    new SelectListItem { Text = "Active",Value="Active" },
                    new SelectListItem { Text = "In-Active",Value="In-Active" },
                };
                ViewBag.statuscreaterule = statuscreaterule;
                List<SelectListItem> EmployerType = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Select",Value="0" },
                    new SelectListItem { Text = "Probation Period",Value="P" },
                    new SelectListItem { Text = "Regular Period",Value="R" },
                    new SelectListItem { Text = "Contract Period",Value="C" },
                    new SelectListItem { Text = "Notice Period",Value="N" },
                    new SelectListItem { Text = "All",Value="A" }
                };
                ViewBag.employertype = EmployerType;
                List<SelectListItem> status = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Select",Value="Select" },
                    //new SelectListItem { Text = "Active",Value="Active" },
                    //new SelectListItem { Text = "In-Active",Value="In-Active" }
                    new SelectListItem { Text = "Draft",Value="Draft" },
                    new SelectListItem { Text = "Enforce",Value="Enforce" }
                 };
                ViewBag.Status = status;
                ViewBag.leavetypeli = dllleave.Leavetypelistedit();
                List<SelectListItem> gender = new List<SelectListItem>()
                {
                  new SelectListItem { Text = "Select",Value="0" },
                  new SelectListItem { Text = "Male",Value="M" },
                  new SelectListItem { Text = "Female",Value="F" },
             //   new SelectListItem{ Text="Other",Value="O"},
                  new SelectListItem{ Text="All",Value="A"}
               };
                ViewBag.Gender = gender;
                List<SelectListItem> leaveyear = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Select",Value="Select" },
                    new SelectListItem { Text = "Financial Year",Value="Financial Year" },
                    new SelectListItem { Text = "Calendar Year",Value="Calendar Year" }
                };
                ViewBag.LeaveYear = leaveyear;
                List<SelectListItem> liPeriod = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Select",Value="0" },
                    //new SelectListItem { Text = "Daily",Value="Daily" },
                    //new SelectListItem { Text = "Weekly",Value="Weekly" },
                    //new SelectListItem { Text = "Bi-weekly",Value="Bi-weekly" },
                    new SelectListItem { Text = "Monthly",Value="Monthly" },
                    //new SelectListItem { Text = "Bi-monthly",Value="Bi-monthly" },
                    //new SelectListItem { Text = "Quarterly",Value="Quarterly" },
                    //new SelectListItem { Text = "Half Yearly",Value="Half Yearly" },
                    new SelectListItem { Text = "Yearly",Value="Yearly" }
                };
                ViewBag.Period = liPeriod;
                ViewBag.Grade = dllleave.GetDropdownGrade();
                List<SelectListItem> liUOM = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Select",Value="0" },
                  //  new SelectListItem { Text = "Hours",Value="Hours" },
                    new SelectListItem { Text = "Day",Value="Day" },
                    new SelectListItem { Text = "Month",Value="Month" },
                    new SelectListItem { Text = "Year",Value="Year" }
                };
                ViewBag.UOM = liUOM;
                List<SelectListItem> liCredit = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Select",Value="0" },
                    new SelectListItem { Text = "Start Of Period",Value="Start Of Period" },
                    new SelectListItem { Text = "End Of Period",Value="End Of Period" }
                };
                ViewBag.Credit = liCredit;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return RedirectToAction("index", "leave");
                }
                if (leaveid == null)
                {
                    return RedirectToAction("index", "leave");
                }
                else
                {
                    return View(ev);

                }
            }
            catch (Exception Ex)
            {
                ViewBag.msg = Ex.Message.ToString();
                return RedirectToAction("CustomError", "Error");
            }
        }

        [HttpPost]
        public JsonResult leavepolicystepone(LeavePolicy leavepolicy)
        {
            Session["LeavePolicy"] = leavepolicy;
            return Json("success");
        }
        [HttpPost]
        public JsonResult leavepolicysteponeprevious()
        {
            LeavePolicy leavepolicy = new LeavePolicy();
            leavepolicy = Session["LeavePolicy"] as LeavePolicy;
            return Json(leavepolicy, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult leaveplansteptwo(LeavePlan leaveplan)
        {
            Session["LeavePlan"] = leaveplan;
            return Json("success");
        }
        [HttpPost]
        public JsonResult leaveplansteptwoprevious()
        {
            LeavePlan leaveplan = new LeavePlan();
            leaveplan = Session["LeavePlan"] as LeavePlan;
            return Json(leaveplan, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult leavepolicysubmit()
        {
            LeavePolicy leavepolicy = new LeavePolicy();

            leavepolicy = Session["LeavePolicy"] as LeavePolicy;
            string msg = dllleave.createLeavepolicy(leavepolicy, Session["LeavePlan"] as LeavePlan, Session["YearEndProcessing"] as YearEndProcessing, Session["emp_id"].ToString());
            return Json(new { msg }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult leavepolicyupdate(List<CreateRule> data, string policyid, string enddate, string status)
        {


            string SuccessMsg = "", ErrorMsg = "";
            string msg = dllleave.updateLeavepolicy(data, policyid, enddate, status, Session["emp_id"].ToString());
            if (msg == "Success") { SuccessMsg = msg; }
            else { ErrorMsg = msg; }
            return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);


        }
        #endregion


        [HttpPost]
        public JsonResult yearendprocessing(YearEndProcessing yearendprocessing)
        {
            Session["YearEndProcessing"] = yearendprocessing;
            return Json("success");
        }
        [HttpPost]
        public JsonResult yearendprocessingstepthreeprevious()
        {
            YearEndProcessing yearendprocessing = new YearEndProcessing();

            yearendprocessing = Session["YearEndProcessing"] as YearEndProcessing;
            return Json(yearendprocessing, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult Leave_History()
        {
            try
            {

                Page_Permission_Detail info = new Page_Permission_Detail();
                info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 9);
                ViewBag.event_permission = info;

                List<SelectListItem> leaveday = new List<SelectListItem>()
                {
                    new SelectListItem{ Text="Select",Value="0"},
                    new SelectListItem{ Text="Full Day",Value="Full Day"},
                    new SelectListItem{ Text="Half Day",Value="Half Day"}
                };
                ViewBag.LeaveDay = leaveday;
                ViewBag.DDEMPLOYEE = dllleave.GetDropdownEmployee();
                ViewBag.LeaveType = dllleave.Leavetypelist_by_emp_gender(Session["emp_id"].ToString());

                DataSet ds = new DataSet();
                ds = dllleave.GetLeaveHistory(Session["emp_id"].ToString());
                return View(ds);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CustomError", "Error");

            }

        }
        [HttpPost]
        public JsonResult leavebalanceshow(int year)
        {
            string msg = "";
            DataTable dt = new DataTable();

            try
            {
                msg = dllleave.GetEmpLeaveBal(Session["emp_id"].ToString(), year);

                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {

                msg = EX.Message.ToString();
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult leavebalanceshowsandwith(int employee_id, int leave_type, string fromdate, string todate)
        {
            string msg = "";
            DataTable dt = new DataTable();

            try
            {
                msg = dllleave.GetEmpLeaveBalsandwith(employee_id, leave_type, fromdate, todate);

                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {

                msg = EX.Message.ToString();
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult leavebalanceshowmanager(int year)
        {
            string msg = "";
            DataTable dt = new DataTable();

            try
            {
                msg = dllleave.GetEmp_Mgr_LeaveBal(Session["emp_id"].ToString(), year);

                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {

                msg = EX.Message.ToString();
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult applyleavebalancedrodown(int empid, int leavetypeid)
        {
            string msg = "";
            try
            {
                msg = dllleave.GetEmpLeaveBalontype(empid, leavetypeid);
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {

                msg = EX.Message.ToString();
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult applyleavetypeforsandwich(int empid, int leavetypeid, string fromdate, string todate)

        {
            string msg = "";
            try
            {
                msg = dllleave.GetEmpLeaveBalontype_sandwich(empid, leavetypeid, fromdate, todate);
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {

                msg = EX.Message.ToString();
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult applyleavetypebalday(int empid, int leavetypeid)
        {
            string msg = "";
            try
            {
                msg = dllleave.GetEmpLeaveBaldayontype(empid, leavetypeid);
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {

                msg = EX.Message.ToString();
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Applyleave()
        {
            string emp_email = "";
            string msg = "";
            try
            {
                string uploadfilename = "";
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    string randomstr = RandomString(8);
                    Session["RandomNumber"] = randomstr;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        string[] extn;
                        string extension = "";
                        string BrowserDetail = Request.UserAgent;
                        string[] newstr = BrowserDetail.Split(new char[] { '/' });
                        BrowserDetail = newstr[newstr.Length - 2];
                        newstr = BrowserDetail.Split(' ');
                        // Checking for Microsoft Edge  
                        if (newstr[1] == "Edge")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            //   uploadfilename = fname;
                            extn = fname.Split('.');
                            fname = extn[0];
                            extension = extn[1];
                        }
                        // Checking for Internet Explorer  
                        else if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            //   uploadfilename = fname;
                            extn = fname.Split('.');
                            fname = extn[0];
                            extension = extn[1];
                        }
                        else
                        {
                            fname = file.FileName;
                            //  uploadfilename = fname;
                            extn = fname.Split('.');
                            fname = extn[0];
                            extension = extn[1];
                        }

                        uploadfilename = extn[0] + "_" + randomstr + "." + extension;
                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/UploadFile/"), fname + "_" + randomstr + "." + extension);


                        file.SaveAs(fname);
                    }
                }
                ApplyLeave applyleave = new ApplyLeave();
                applyleave.EmployeeID = Request.Form["EmployeeID"];
                applyleave.LeaveTypeID = Convert.ToInt32(Request.Form["LeaveTypeID"]);
                applyleave.LeaveBalance = Request.Form["LeaveBalance"] == null ? 0 : Convert.ToDouble(Request.Form["LeaveBalance"]);
                applyleave.FromDate = Request.Form["FromDate"];
                applyleave.ToDate = Request.Form["ToDate"];
                applyleave.LeaveDay = Request.Form["ToHalfDay"];
                applyleave.LeaveHalfDay = Request.Form["FromHalfDay"];
                applyleave.NumberOfDays = Convert.ToDouble(Request.Form["NumberOfDays"]);
                applyleave.Remarks = Request.Form["Remarks"];
                applyleave.sandwich = Request.Form["sandwich"];
                applyleave.Attachment = uploadfilename;

                msg = dllleave.addapplyleave(applyleave, Session["emp_id"].ToString());

                if (msg.Split(',')[0] == "Success")
                {
                    myds = dblayer.Get_Employee_Mail_info(Convert.ToInt32(Session["emp_id"].ToString()));
                    if (myds.Tables[0].Rows.Count > 0)
                    {
                        //table 0 from Manager Info  and 1 for employee info

                        emp_email = myds.Tables[0].Rows[0]["Email_Employee"].ToString();
                       EmailService.Send_Email_Leave_Notification(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), "", myds.Tables[1].Rows[0]["Full_Name"].ToString()
                           , myds.Tables[1].Rows[0]["Designation"].ToString(), myds.Tables[1].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[1].Rows[0]["Section_Name"].ToString(), Request.Form["Leavetypename"], Request.Form["FromDate"], Request.Form["ToDate"], myds.Tables[0].Rows[0]["Full_Name"].ToString(), Request.Form["NumberOfDays"]);
                        db.GenrateEmailLogs(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), "Leave Apply", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");
                    }
                }


                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {
                db.GenrateEmailLogs(emp_email, "Leave Apply ", Convert.ToInt32(Session["emp_id"].ToString()), EX.Message.ToString(), "Failed");
                msg = EX.Message.ToString();
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult addLeaveEncashment(LeaveEncashment lc)
        {
            string msg = "";
            try
            {
                msg = dllleave.InsertLeaveEncashment(lc, Session["emp_id"].ToString());


                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {
                msg = EX.Message.ToString();
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult leaveencashmentapproveal(LeaveApprove[] data, string status, Approve_Emp_Detail[] emp_apply_date)
        {
            List<LeaveApprove> LApp = new List<LeaveApprove>();
            foreach (LeaveApprove obj in data)
            {
                LApp.Add(obj);
            }
            string SuccessMsg = "", ErrorMsg = "", emp_email = "";
            try
            {

                string msg = dllleave.LeaveEncashmentApprove(LApp, status, Session["emp_id"].ToString());
                ErrorMsg = msg;

                if (Session["role_id"].ToString() == "5")
                {
                    foreach (Approve_Emp_Detail emp_d in emp_apply_date)
                    {
                        myds = dblayer.Get_Employee_Mail_info(Convert.ToInt32(emp_d.EMP_ID));
                        if (myds.Tables[1].Rows.Count > 0)
                        {

                            string aprova_status = status == "reject" ? "Rejected. <br>Reject Remark: " + data[0].Remarks : "Approved .";
                            string mailbody = "Hi,<br>" + myds.Tables[1].Rows[0]["Full_Name"].ToString() + " ,<br>" + myds.Tables[1].Rows[0]["Designation"].ToString() + " ,<br> " + myds.Tables[1].Rows[0]["DEPARTMENT_NAME"].ToString() + " <br>  Your Leave Encashment  No. Of Days: " + emp_d.From_Date + " Encashment Amount : " + emp_d.To_Date + "  " + aprova_status + "<br><br>Note: This is an auto-generated mail. Please do not reply.";
                            //Utility.Leave_Apply_Notification_Send_Mail(mydt.Rows[0]["Email_Employee"].ToString(), mydt.Rows[0]["EMAIL_MANAGER"].ToString(), "Apply Leave", mailbody,"Leave");
                            //   Utility.Leave_Apply_Notification_Send_Mail("hare.krishna@prudencesoftech.in", "hkpandit91@gmail.com", "Leave Encashment Approval", mailbody, "Leave");

                            EmailService.Send_Email_Leave_Encashment_To_Employee(myds.Tables[1].Rows[0]["Email_Employee"].ToString(), "", myds.Tables[1].Rows[0]["Full_Name"].ToString(), "");
                            emp_email = myds.Tables[1].Rows[0]["Email_Employee"].ToString();

                            db.GenrateEmailLogs(myds.Tables[1].Rows[0]["Email_Employee"].ToString(), "Leave encashment Approve", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                db.GenrateEmailLogs(emp_email, "Leave encashment Approve ", Convert.ToInt32(Session["emp_id"].ToString()), ex.Message.ToString(), "Failed");
            }
            return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult showLeaveEncashment()
        {
            string msg = "";
            try
            {
                msg = dllleave.GetLeaveEncashment(Session["emp_id"].ToString());
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {
                msg = EX.Message.ToString();
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }
        private static string RandomString(int length)
        {
            Random rng = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[rng.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }


        public ActionResult overrideleavebalance()
        {
            return View();
        }


        public ActionResult updateleavebalance()
        {
            return View();
        }


        public ActionResult employeeoutdoorentry()
        {
            return View();
        }


        public ActionResult leaveapplication()
        {
            return View();
        }


        public ActionResult empleavecredithistory()
        { return View(); }


        public ActionResult empleavemanualentry()
        { return View(); }

        public ActionResult emprestrictedholidayentry()
        { return View(); }


        public ActionResult Leave_Approval()
        {


            try
            {
                var done = "";

                Page_Permission_Detail info = new Page_Permission_Detail();
                info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 19);
                ViewBag.event_permission = info;

                ViewBag.LeaveType = dllleave.Leavetypelist();
                ViewBag.DDEMPLOYEE = dllleave.GetDropdownEmployee();
                List<SelectListItem> leaveday = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Select",Value="0"},
                new SelectListItem{ Text="Full Day",Value="Full Day"},
                new SelectListItem{ Text="Half Day",Value="Half Day"}
            };
                ViewBag.LeaveDay = leaveday;
                DataSet ds = new DataSet();
                if (Request.Form["done"] != null)
                {
                    done = "done";
                    ds = dllleave.GetLeaveapproved_list_history(Convert.ToInt32(Session["emp_id"].ToString()));
                }
                else if (Request.Form["pending"] != null)
                {
                    ds = dllleave.GetLeaveapprovalhistory(Convert.ToInt32(Session["emp_id"].ToString()));
                }
                else
                {

                    ds = dllleave.GetLeaveapprovalhistory(Convert.ToInt32(Session["emp_id"].ToString()));
                }

                ViewBag.done = done;
                return View(ds);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CustomError", "Error");
            }
        }


        public ActionResult GET_LeaveApproval(string status)
        {
            try
            {
                DataSet ds = new DataSet();
                if (status == "done")
                {

                    ds = dllleave.GetLeaveapproved_list_history(Convert.ToInt32(Session["emp_id"].ToString()));
                }
                else if (status == "pending")
                {
                    ds = dllleave.GetLeaveapprovalhistory(Convert.ToInt32(Session["emp_id"].ToString()));
                }
                else
                {
                    ds = dllleave.GetLeaveapprovalhistory(Convert.ToInt32(Session["emp_id"].ToString()));
                }

                string result = Utility.DataTableToJSONWithJSONNet(ds.Tables[0]);

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return RedirectToAction("CustomError", "Error");
            }

        }


        public ActionResult leaveencashmentapproval()
        {
            var done = "";
            try
            {
                Page_Permission_Detail info = new Page_Permission_Detail();
                info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 49);
                ViewBag.event_permission = info;

                ViewBag.LeaveType = dllleave.Leavetypelist();
                ViewBag.DDEMPLOYEE = dllleave.GetDropdownEmployee();
                List<SelectListItem> leaveday = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Select",Value="0"},
                new SelectListItem{ Text="Full Day",Value="Full Day"},
                new SelectListItem{ Text="Half Day",Value="Half Day"}
            };
                ViewBag.LeaveDay = leaveday;
                DataSet ds = new DataSet();
                if (Request.Form["done"] != null)
                {
                    done = "done";
                    ds = dllleave.GetLeave_encashment_done_approval(Convert.ToInt32(Session["emp_id"].ToString()));
                }
                else if (Request.Form["pending"] != null)
                {
                    ds = dllleave.GetLeave_encashment_approval(Convert.ToInt32(Session["emp_id"].ToString()));
                }
                else
                {
                    ds = dllleave.GetLeave_encashment_approval(Convert.ToInt32(Session["emp_id"].ToString()));
                }

                ViewBag.done = done;
                return View(ds);
            }
            catch
            {
                return RedirectToAction("CustomError", "Error");
            }

        }
        public ActionResult GET_LeaveEncashmentApproval(string status)
        {
            try { 
            DataSet ds = new DataSet();
            if (status == "done")
            {
                
                ds = dllleave.GetLeave_encashment_done_approval(Convert.ToInt32(Session["emp_id"].ToString()));
            }
            else if (status == "pending")
            {
                ds = dllleave.GetLeave_encashment_approval(Convert.ToInt32(Session["emp_id"].ToString()));
            }
            else
            {
                ds = dllleave.GetLeave_encashment_approval(Convert.ToInt32(Session["emp_id"].ToString()));
            }

            string result = Utility.DataTableToJSONWithJSONNet(ds.Tables[0]);

            return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return RedirectToAction("CustomError", "Error");
            }

        }
        public JsonResult leaveapproveal(LeaveApprove[] data, string status, Approve_Emp_Detail[] emp_apply_date)
        {
            string SuccessMsg = "", ErrorMsg = "", emp_email = "";
            try
            {


                List<LeaveApprove> LApp = new List<LeaveApprove>();
                foreach (LeaveApprove obj in data)
                {
                    LApp.Add(obj);
                } 

                string msg = dllleave.LeaveApprove(LApp, status, Session["emp_id"].ToString());
                ErrorMsg = msg;

                foreach (LeaveApprove emp_d in data)
                {
                    myds = dblayer.Get_Employee_Mail_info_on_approve(Convert.ToInt32(emp_d.ID));
                    if (myds.Tables[0].Rows.Count > 0)
                    {

                        string aprova_status = status == "reject" ? "had been Rejected. <br>Reject Remark: " + data[0].Remarks : "had been Approved.";                        
                        emp_email = myds.Tables[0].Rows[0]["Email_Employee"].ToString();
                        if (myds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "Approved")
                        {

                            EmailService.Send_Email_Leave_Approve_Notification(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), aprova_status, "Leave Approval",  myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());
                            EmailService.Send_Email_Leave_Approve_Notification_ToStaff(myds.Tables[0].Rows[0]["Staff_email"].ToString(), myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), "Leave", myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());
                            //    db.GenrateEmailLogs(myds.Tables[0].Rows[0]["Staff_email"].ToString(), "Leave Approve", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");
                            db.GenrateEmailLogs(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), "Leave Approved", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");
                        }
                        else if (myds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "Cancellation Approved")
                        {
                            string cancel_approve_status = status == "reject" ? "Cancellation Request had been Rejected. <br>Reject Remark: " + data[0].Remarks : "Cancellation Request had been Approved.";
                            EmailService.Send_Email_Leave_Cancel_Notification(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), cancel_approve_status, "Leave Cancellation Approval", myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());
                          //  EmailService.Send_Email_Leave_Approve_Notification_ToStaff(myds.Tables[0].Rows[0]["Staff_email"].ToString(), myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), "Cancellation Leave", myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());
                           
                            db.GenrateEmailLogs(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), "Cancellation Approved", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");
                        }
                        else if (myds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "Cancellation Rejected")
                        {
                            string cancel_approve_status = status == "reject" ? "Cancellation Request had been Rejected. <br>Reject Remark: " + data[0].Remarks : "Cancellation Request had been Approved.";
                            EmailService.Send_Email_Leave_Cancel_Notification(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), cancel_approve_status, "Leave Cancellation Approval", myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());
                            //  EmailService.Send_Email_Leave_Approve_Notification_ToStaff(myds.Tables[0].Rows[0]["Staff_email"].ToString(), myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), "Cancellation Leave", myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());

                            db.GenrateEmailLogs(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), "Cancellation Rejected", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");
                        }
                        else if (myds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "Cancellation In Progress")
                        {

                            DataSet mydsnew = new DataSet();
                            mydsnew = dblayer.Get_Employee_Mail_info(Convert.ToInt32(Session["emp_id"].ToString()));

                            if (mydsnew.Tables[0].Rows.Count > 0)
                            {

                                EmailService.Send_Email_Leave_Cancellation_Approve_Notifi_byhearchy(mydsnew.Tables[0].Rows[0]["Email_Employee"].ToString(), "", myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString()
                                    , myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), mydsnew.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());
                                db.GenrateEmailLogs(mydsnew.Tables[0].Rows[0]["Email_Employee"].ToString(), "Cancellation In Progress", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");
                            }
                        }

                        else if (myds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "Rejected")
                        {
                            EmailService.Send_Email_Leave_Approve_Notification(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), aprova_status, "Leave Approval",  myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());
                            db.GenrateEmailLogs(myds.Tables[0].Rows[0]["Email_Employee"].ToString(), "Leave Approve", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");
                        }
                        else if (myds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "In Progress")
                        {
                            DataSet mydsnew = new DataSet();
                            mydsnew = dblayer.Get_Employee_Mail_info(Convert.ToInt32(Session["emp_id"].ToString()));
                            
                            if (mydsnew.Tables[0].Rows.Count > 0)
                            {
                              
                                EmailService.Send_Email_Leave_Approve_Notification_byhearchy(mydsnew.Tables[0].Rows[0]["Email_Employee"].ToString(), "", myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString()
                                    , myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), mydsnew.Tables[0].Rows[0]["Full_Name"].ToString()  , myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString()); 
                               db.GenrateEmailLogs(mydsnew.Tables[0].Rows[0]["Email_Employee"].ToString(), "Leave Approve", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");
                            }
                        }

                    }
                     
                }

            }
            catch (Exception ex)
            {

                db.GenrateEmailLogs(emp_email, "Leave Approve ", Convert.ToInt32(Session["emp_id"].ToString()), ex.Message.ToString(), "Failed");
            }
            return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Leavecancelhistory()
        { return View(); }

        public ActionResult cancelLeave()
        {


            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 20);
            ViewBag.event_permission = info;
            ViewBag.LeaveType = dllleave.Leavetypelist();

            List<SelectListItem> leaveday = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Select",Value="0"},
                new SelectListItem{ Text="Full Day",Value="Full Day"},
                new SelectListItem{ Text="Half Day",Value="Half Day"}
            };
            ViewBag.LeaveDay = leaveday;
            ViewBag.DDEMPLOYEE = dllleave.GetDropdownEmployee();
            DataSet ds = new DataSet();
            ds = dllleave.GetLeaveCancelHistory(Session["emp_id"].ToString());
            return View(ds);
        }

        [HttpPost]
        public JsonResult cancelleavewithdraw(string applyleaveid, string status, cancelleave_info leave_detail)
        {
            string msg = "", emp_email = "";
            try
            {

                msg = dllleave.Cancelleavewithdrawn(applyleaveid, status, Session["emp_id"].ToString());
                myds = dblayer.Get_Employee_Mail_info_on_approve(Convert.ToInt32(applyleaveid));
                if (myds.Tables[0].Rows.Count > 0)
                {
                    string aprova_status = status == "Withdrawn" ? "Withdrawn." : "Canceled .";
                    DataSet mydsnew = new DataSet();
                    mydsnew = dblayer.Get_Employee_Mail_info(Convert.ToInt32(Session["emp_id"].ToString()));
                    emp_email = mydsnew.Tables[0].Rows[0]["Email_Employee"].ToString();

                    if (mydsnew.Tables[0].Rows.Count > 0)
                    {
                        //table 0 from Manager Info  and 1 for employee info
                        if (status == "Withdrawn")
                        {
                            EmailService.Send_Email_Leave_Withdrow_Notification(mydsnew.Tables[0].Rows[0]["Email_Employee"].ToString(), "", myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString()
                            , myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), mydsnew.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());
                            db.GenrateEmailLogs(mydsnew.Tables[0].Rows[0]["Email_Employee"].ToString(), "Withdrawn", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");


                        }
                        else
                        {
                            if (myds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "Cancellation Submitted")
                            {
                                EmailService.Send_Email_Leave_Cancel_Notification(mydsnew.Tables[0].Rows[0]["Email_Employee"].ToString(), "", myds.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["Designation"].ToString(), myds.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString(), myds.Tables[0].Rows[0]["Section_Name"].ToString()
                                                           , myds.Tables[0].Rows[0]["LEAVE_TYPE_NAME"].ToString(), myds.Tables[0].Rows[0]["LEAVE_FROM_DATE"].ToString(), myds.Tables[0].Rows[0]["LEAVE_TO_DATE"].ToString(), mydsnew.Tables[0].Rows[0]["Full_Name"].ToString(), myds.Tables[0].Rows[0]["NUMBER_OF_DAYS"].ToString());
                                db.GenrateEmailLogs(mydsnew.Tables[0].Rows[0]["Email_Employee"].ToString(), "Cancellation Submitted", Convert.ToInt32(Session["emp_id"].ToString()), "Success", "Sent");


                            }
                        }

                    }
 
                }
             }
            catch (Exception ex)
            {

               db.GenrateEmailLogs(emp_email, "Cancel Leave ", Convert.ToInt32(Session["emp_id"].ToString()), ex.Message.ToString(), "Failed");
            }
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Applyleavecancel()
        {
            string uploadfilename = "";
            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;
                string randomstr = RandomString(8);
                Session["RandomNumber"] = randomstr;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;
                    string[] extn;
                    string extension = "";
                    string BrowserDetail = Request.UserAgent;
                    string[] newstr = BrowserDetail.Split(new char[] { '/' });
                    BrowserDetail = newstr[newstr.Length - 2];
                    newstr = BrowserDetail.Split(' ');
                    // Checking for Microsoft Edge  
                    if (newstr[1] == "Edge")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];

                        extn = fname.Split('.');
                        fname = extn[0];
                        extension = extn[1];
                    }
                    // Checking for Internet Explorer  
                    else if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                        extn = fname.Split('.');
                        fname = extn[0];
                        extension = extn[1];
                    }
                    else
                    {
                        fname = file.FileName;
                        extn = fname.Split('.');
                        fname = extn[0];
                        extension = extn[1];
                    }
                    uploadfilename = extn[0] + "_" + randomstr + "." + extension;
                    // Get the complete folder path and store the file inside it.  
                    fname = Path.Combine(Server.MapPath("~/UploadFile/"), fname + "_" + randomstr + "." + extension);
                    file.SaveAs(fname);
                }
            }
            ApplyLeave applyleave = new ApplyLeave();
            applyleave.ID = Convert.ToInt32(Request.Form["ID"]);
            applyleave.EmployeeID = Request.Form["EmployeeID"];
            applyleave.LeaveTypeID = Convert.ToInt32(Request.Form["LeaveTypeID"]);
            applyleave.FromDate = Request.Form["FromDate"];
            applyleave.ToDate = Request.Form["ToDate"];
            applyleave.LeaveDay = Request.Form["LeaveDay"];
            applyleave.LeaveHalfDay = Request.Form["LeaveHalfDay"];
            applyleave.NumberOfDays = Convert.ToDouble(Request.Form["NumberOfDays"]);
            applyleave.CancelFromDate = Request.Form["CancelFromDate"];
            applyleave.CancelToDate = Request.Form["CancelToDate"];
            applyleave.CancelLeaveHalffromDay = Request.Form["CancelLeaveHalffromDay"];
            applyleave.CancelLeaveHalftoDay = Request.Form["CancelLeaveHalftoDay"];
            applyleave.CancelNumberOfDays = Convert.ToDouble(Request.Form["CancelNumberOfDays"]);
            applyleave.Remarks = Request.Form["Remarks"];
            applyleave.Attachment = uploadfilename;
            string SuccessMsg = "", ErrorMsg = "";
            string msg = dllleave.ApplyCancelLeave(applyleave, Session["emp_id"].ToString());
            if (msg == "Success") { SuccessMsg = msg; }
            else { ErrorMsg = msg; }
            return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editapplyleave(string tbl_id)
        {
            List<ApplyLeave> CLDetail = new List<ApplyLeave>();
            CLDetail = dllleave.EditApplyLeave(tbl_id);
            return Json(CLDetail, JsonRequestBehavior.AllowGet);
        }

        //Leave History
        public ActionResult leavehistorylist()
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 21);
            ViewBag.event_permission = info;

            ViewBag.LeaveType = dllleave.Leavetypelist();
            ViewBag.DDEMPLOYEE = dllleave.GetDropdownEmployee();

            List<SelectListItem> leaveday = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Select",Value="0"},
                new SelectListItem{ Text="Full Day",Value="Full Day"},
                new SelectListItem{ Text="Half Day",Value="Half Day"}
            };
            ViewBag.LeaveDay = leaveday;
            DataSet ds = new DataSet();
            ds = dllleave.GetLeaveHistoryall(Session["emp_id"].ToString());
            return View(ds);

        }
        [HttpPost]
        public JsonResult viewapplyleavedetails(string tbl_id, string flag)
        {
            List<ApplyLeave> CLDetail = new List<ApplyLeave>();
            CLDetail = dllleave.ViewApplyLeaveDetails(tbl_id, flag);
            return Json(CLDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult updateapplyleavedetails(string id, string leaveday, string leavehalfday, string fromdate, string todate, string noofdays, int sandwich)
        {
            string msg = "";
            msg = dllleave.UpdateApplyLeave(id, leaveday, leavehalfday, fromdate, todate, noofdays, Session["emp_id"].ToString(), sandwich);
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult updateapplycancelleavedetails(string id, string cancelfrom, string cancelfromleavehalfday, string cancelto, string canceltoleavehalfday, string noofdays, string Remarks)
        {
            string msg = "";
            msg = dllleave.updatecancelapplyleave(id, cancelfrom, cancelfromleavehalfday, cancelto, canceltoleavehalfday, noofdays, Remarks, Session["emp_id"].ToString());
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }

        // Leave Delegation
        public ActionResult leaveDelegationapproval()
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 51);
            ViewBag.event_permission = info;

            ViewBag.LeaveType = dllleave.Leavetypelist();
            ViewBag.DDEMPLOYEE = dllleave.GetDropdownEmployee();
            List<SelectListItem> leaveday = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Select",Value="0"},
                new SelectListItem{ Text="Full Day",Value="Full Day"},
                new SelectListItem{ Text="Half Day",Value="Half Day"}
            };
            ViewBag.LeaveDay = leaveday;
            DataSet ds = new DataSet();
            ds = dllleave.GetDelegationLeaveapprovalhistory(Convert.ToInt32(Session["emp_id"].ToString()));
            return View(ds);
        }

        public class cancelleave_info
        {

            public int emp_id { get; set; }
            public string leave_type { get; set; }
            public string from_date { get; set; }
            public string to_date { get; set; }
            public string status { get; set; }
        }

    }
}