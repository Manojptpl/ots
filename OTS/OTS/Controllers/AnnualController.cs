using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Configuration.Internal;
using System.Web.Script.Serialization;
using OTS.Models;
using OTS.database_Access_Layer;
using OTS.CommonFilters;
using System.Threading.Tasks;

namespace OTS.Controllers
{
    [LoginFilter]
    public class AnnualController : Controller
    {
        #region General Information   
        HomeModel hm = new HomeModel();
        TranExitModel te = new TranExitModel();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        AnnualAppraisalDB ap_layer = new AnnualAppraisalDB();
        db dblayer = new db();
        #endregion
        // GET: Annual
        public ActionResult Appraisal()
        {
            List<AnnualAppraisalModel> UserProfileList = new List<AnnualAppraisalModel>();
            try
            {                
                ds = ap_layer.GetUserProfile(Convert.ToInt32(Session["Emp_id"]));
                dt = ds.Tables[3];              
                foreach (DataRow dr in dt.Rows)
                {
                    AnnualAppraisalModel ap = new AnnualAppraisalModel();
                    ap.EMP_ID = Convert.ToInt32(Session["emp_id"]);
                    ap.NAME = dr["FULL_NAME"].ToString();
                    ap.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                    ap.DESIGNATION = dr["DESIGNATION"].ToString();
                    ap.CURRENT_DATE = dr["Cr_DATE"].ToString();                    
                    ap.DEPARTMENT = dr["department_name"].ToString();                    
                    ap.GOAL_QUARTER = dr["GOAL_QUARTER"].ToString();                                       
                    ap.FINAL_RATING = dr["MNG_GOAL_RATING"].ToString();
                    UserProfileList.Add(ap);                    
                }
                return View(UserProfileList);
            }
            catch (Exception)
            {
                return View();
            }
        }

        public JsonResult CreateAnnualAppraisal(AnnualAppraisalModel objResponsibility, AppraisalPerformanceModel objPerformance, AppraisalRating objRating)
        {            
            List<AnnualAppraisalModel> AnnualResponsibilitylist = new List<AnnualAppraisalModel>();
            AnnualResponsibilitylist.Add(objResponsibility);
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dt = new DataTable();
            dt = converter.ToDataTable(AnnualResponsibilitylist);

            List<AppraisalPerformanceModel> AnnualPerformanceList = new List<AppraisalPerformanceModel>();
            AnnualPerformanceList.Add(objPerformance);
            ListtoDataTableConverter PerformanceConverter = new ListtoDataTableConverter();
            DataTable dt1 = new DataTable();
            dt1 = PerformanceConverter.ToDataTable(AnnualPerformanceList);

            List<AppraisalRating> RatingList = new List<AppraisalRating>();
            RatingList.Add(objRating);
            ListtoDataTableConverter RatingConverter = new ListtoDataTableConverter();
            DataTable dt2 = new DataTable();
            dt2 = RatingConverter.ToDataTable(RatingList);           

            MastersModel mm = new MastersModel();
            string res = "";
            try
            {
                res = ap_layer.CreateAnnualAppraisal(dt, dt1, dt2);
                
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

        public ActionResult GetAppraisal_id()
        {

            try
            {
                int Appraisal_id = ap_layer.GetAppraisal_id(Convert.ToInt32(Session["Emp_id"]));
                ViewBag.Appraisal_idType = Appraisal_id;
                return View(Appraisal_id);
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult Performance()
        {
            List<AnnualAppraisalModel> UserProfileList = new List<AnnualAppraisalModel>();
            try
            {
                ds = ap_layer.GetUserProfile(Convert.ToInt32(Session["Emp_id"]));
                dt = ds.Tables[3];
                foreach (DataRow dr in dt.Rows)
                {
                    AnnualAppraisalModel ap = new AnnualAppraisalModel();
                    ap.EMP_ID = Convert.ToInt32(Session["emp_id"]);
                    ap.NAME = dr["FULL_NAME"].ToString();
                    ap.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                    ap.DESIGNATION = dr["DESIGNATION"].ToString();
                    ap.CURRENT_DATE = dr["Cr_DATE"].ToString();                    
                    ap.DEPARTMENT = dr["department_name"].ToString();
                    ap.GOAL_QUARTER = dr["GOAL_QUARTER"].ToString();
                    ap.FINAL_RATING = dr["MNG_GOAL_RATING"].ToString();
                    UserProfileList.Add(ap);
                }
                return View(UserProfileList);
            }
            catch (Exception)
            {
                return View();
            }
        }
        public ActionResult Appraisal_Performance()
        {
            List<AnnualAppraisalModel> UserProfileList = new List<AnnualAppraisalModel>();
            try
            {
                ds = ap_layer.GetUserProfile(Convert.ToInt32(Session["Emp_id"]));
                dt = ds.Tables[3];
                foreach (DataRow dr in dt.Rows)
                {
                    AnnualAppraisalModel ap = new AnnualAppraisalModel();
                    ap.EMP_ID = Convert.ToInt32(Session["emp_id"]);
                    ap.NAME = dr["FULL_NAME"].ToString();                   
                    ap.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                    ap.DESIGNATION = dr["DESIGNATION"].ToString();
                    ap.CURRENT_DATE = dr["Cr_DATE"].ToString();                    
                    ap.DEPARTMENT = dr["department_name"].ToString();                    
                    ap.GOAL_QUARTER = dr["GOAL_QUARTER"].ToString();
                    ap.FINAL_RATING = dr["MNG_GOAL_RATING"].ToString();                    
                    UserProfileList.Add(ap);
                }                
                return View(UserProfileList);
            }
            catch (Exception)
            {
                return View();
            }
        }
        public ActionResult Appraisal_PerformanceList()
        {
            List<CommonAppraisalModel> AppraisalList = new List<CommonAppraisalModel>();
            try
            {
                dt = ap_layer.GetAppraisal_History(Convert.ToInt32(Session["EMP_ID"]));
                foreach (DataRow dr in dt.Rows)
                {
                    CommonAppraisalModel rm = new CommonAppraisalModel();
                    rm.EMP_ID = Convert.ToInt32(dr["EMP_ID"]);                    
                    rm.NAME = dr["EMP_NAME"].ToString();
                    rm.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                    rm.DESIGNATION = dr["POSITION_TITLE"].ToString();
                    rm.DEPARTMENT = dr["DEPARTMENT"].ToString();
                    rm.GOAL_QUARTER = dr["GOAL_QUARTER"].ToString();
                    rm.strSECTION_ONE_OVERALL_TOTALRATING = dr["FIRST_SECTION_RATING"].ToString();
                    rm.strSECTION_2_OVERALL_TOTALRATING_MGR = dr["SECTION_2_OVERALL_TOTALRATING_MGR"].ToString();
                    rm.strOVERALL_RATING_MGR = dr["FINAL_RATING"].ToString();
                    rm.strOVERALL_RATING = dr["OVERALL_RATING"].ToString();
                    rm.Year = dr["Year"].ToString();
                    AppraisalList.Add(rm);                    
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(AppraisalList);
        }        
        [HttpPost]
        public JsonResult appraisaldetailsview(string id)
        {
            SingleModel data = new SingleModel();            
            data = ap_layer.GetAppraisalDetails(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult appraisaldetailsedit(string id)
        {
            SingleModel data = new SingleModel();
            data = ap_layer.GetAppraisalDetails(id);
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateAppraisal(SingleModel smodel)
        {            

            MastersModel mm = new MastersModel();         
            string res = "";                      
            try
            {                
                res = ap_layer.Update_Appraisal(smodel.EMP_ID, smodel.CURRENTROLL_MGRRATING, smodel.CURRENTROLL_MGREXP, smodel.QUALITYWORK_MGRRATING, smodel.QUALITYWORK_MGREXP, smodel.ORGWORK_MGRRATING, smodel.ORGWORK_MGREXP, smodel.INITIATE_MGRRATING, smodel.INITIATE_MGREXP,
            smodel.PERSONAL_MGRRATING, smodel.PERSONAL_MGREXP, smodel.VSKILL_MGRRATING, smodel.VSKILL_MGREXP, smodel.WSKILL_MGRRATING, smodel.WSKILL_MGREXP, smodel.TEAM_MGRRATING, smodel.TEAM_MGREXP, smodel.CONFIDENTIALITY_MGRRATING, smodel.CONFIDENTIALITY_MGREXP,
            smodel.ATTENDANCE_MGRRATING, smodel.ATTENDANCE_MGREXP, smodel.SECTION_SECOND_TOTALRATING, smodel.SECTION_ONE_OVERALL_TOTALRATING, smodel.SECTION_2_OVERALL_TOTALRATING_MGR, smodel.SUBTOTAL_MGR, smodel.OVERALL_RATING_MGR,smodel.MGR_RATING);

                //string res1 =ap_layer.AnnualAppraisal(Year, Convert.ToInt32(Session["Emp_id"].ToString()))                
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    mm.SuccessMsg = response[1];

                }                
                else if (response[0] == "Error")
                {
                    mm.ErrorMsg = response[1];
                }
                AnnualAppraisalForSubmit(2019);
            }
            catch (Exception ex)
            {
                mm.ErrorMsg = ex.Message;
            }            
            return Json(mm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AnnualAppraisalForSubmit(int Year)
        {
            try
            {
                string res = ap_layer.AnnualAppraisal(Year, Convert.ToInt32(Session["Emp_id"].ToString()));
                string[] splitmsg = res.Split(',');
                if (splitmsg[0] == "Update")
                {
                    string manager_name = splitmsg[1].ToString();
                    string manager_email = splitmsg[2].ToString();
                    string user_email = splitmsg[3].ToString();
                    string reply_mail = EmailService.Send_Email_Update(manager_name, Session["Emp_Name"].ToString(), Year, manager_email, user_email, "AnnualAppraisal");

                    string notification_msg = Session["Emp_Name"].ToString() + " has submitted annual appraisal for " + Year + ".";
                    hm.SuccessMsg = "Annual Appraisal Successfully Submited & Email Send to Your Manager Approval.";
                }
                else
                {
                    hm.ErrorMsg = res ;
                }
            }
            catch (Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }
            return Json(hm, JsonRequestBehavior.AllowGet);
        }//Function for Timesheet Submit By User   
        public ActionResult MasterCheckBox()
        {
            return View();
        }

        public ActionResult AppraisalSearch()
        {            
            ViewBag.Emp_Name = ap_layer.GetDropdownEmployee();
            return View();
        }
        [HttpGet]
        public JsonResult GetAppraisal_Rating(int year, int Emp_ID)
        {
            System.Threading.Thread.Sleep(5000);     
            var jsonobj = "";
            try
            {
                var dt = ap_layer.GetAppraisal_Rating(year, Emp_ID);
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;                
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SoluGrowth()
        {
            return View();
        }
        public ActionResult BootStrap()
        {
            return View();
        }
        public ActionResult Grid()
        {
            return View();
        }
        
    }
}