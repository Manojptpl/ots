using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Configuration.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OTS.Models;
using OTS.database_Access_Layer;
using OTS.CommonFilters;


namespace OTS.Controllers
{
    [LoginFilter]
    public class TranExitController : Controller
    {
        #region General Information
        TranExitModel te = new TranExitModel();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        TranExitDB tedb_layer = new TranExitDB();
        db dblayer = new db();
        #endregion
        // GET: TranExit
        public ActionResult Exit()
        {
            List<TranExitFormModel> UserProfileList = new List<TranExitFormModel>();
            try
            {
                dt = tedb_layer.GetUserProfile(Convert.ToInt32(Session["Emp_id"]));
                foreach (DataRow dr in dt.Rows)
                {

                    TranExitFormModel mm = new TranExitFormModel();
                    mm.emp_id = Convert.ToInt32(Session["emp_id"]);
                    mm.first_name = dr["first_name"].ToString();
                    mm.job_name = dr["job_name"].ToString();
                    mm.designation = dr["designation"].ToString();
                    mm.department_name = dr["department_name"].ToString();
                    mm.strDOJ = dr["date_of_joining"].ToString();

                    UserProfileList.Add(mm);
                    GetExit_id();
                }
                return View(UserProfileList);
            }
            catch (Exception)
            {
                return View();
            }            
        }

        public JsonResult CreateTranExit(TranExitModel objTranExitModel, TranExitDissatisfactionModel objDissatisfaction, TranExitAnotherJobModel objAnotherJob, TRANEXITFORMSUPERVISORModel objSupervisor, TRANEXITFORMORGANIZATIONASPECTSModel objOrganization, TRANEXITFORMJOBASPECTSModel objJobAspect)
        {
             //TRANEXITFORMSUPERVISORModel objSupervisor, TRANEXITFORMORGANIZATIONASPECTSModel objOrganization, TRANEXITFORMJOBASPECTSModel objJobAspect
            List<TranExitModel> TranExitlist = new List<TranExitModel>();
            TranExitlist.Add(objTranExitModel);
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dt = new DataTable();
            dt = converter.ToDataTable(TranExitlist);

            List<TranExitDissatisfactionModel> DissatisfactionList = new List<TranExitDissatisfactionModel>();
            DissatisfactionList.Add(objDissatisfaction);
            ListtoDataTableConverter disstatisfactionConverter = new ListtoDataTableConverter();
            DataTable dt1 = new DataTable();
            dt1 = disstatisfactionConverter.ToDataTable(DissatisfactionList);

            List<TranExitAnotherJobModel> AnotherJobList = new List<TranExitAnotherJobModel>();
            AnotherJobList.Add(objAnotherJob);
            ListtoDataTableConverter anotherjobConverter = new ListtoDataTableConverter();
            DataTable dt2 = new DataTable();
            dt2 = anotherjobConverter.ToDataTable(AnotherJobList);

            List<TRANEXITFORMSUPERVISORModel> SupervisorList = new List<TRANEXITFORMSUPERVISORModel>();
            SupervisorList.Add(objSupervisor);
            ListtoDataTableConverter SupervisorConverter = new ListtoDataTableConverter();
            DataTable dt3 = new DataTable();
            dt3 = SupervisorConverter.ToDataTable(SupervisorList);

            List<TRANEXITFORMORGANIZATIONASPECTSModel> OrganizationList = new List<TRANEXITFORMORGANIZATIONASPECTSModel>();
            OrganizationList.Add(objOrganization);
            ListtoDataTableConverter OrgConverter = new ListtoDataTableConverter();
            DataTable dt4 = new DataTable();
            dt4 = OrgConverter.ToDataTable(OrganizationList);

            List<TRANEXITFORMJOBASPECTSModel> JobAspectList = new List<TRANEXITFORMJOBASPECTSModel>();
            JobAspectList.Add(objJobAspect);
            ListtoDataTableConverter JobAspectConverter = new ListtoDataTableConverter();
            DataTable dt5 = new DataTable();
            dt5 = JobAspectConverter.ToDataTable(JobAspectList);

            MastersModel mm = new MastersModel();
            string res = "";
            try
            {
                res = tedb_layer.CreateTranExit(dt,dt1,dt2,dt3,dt4,dt5);

                //objTranExitModel, objDissatisfaction, objAnotherJob
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    mm.SuccessMsg = response[1];
                }
                else if(response[0] == "Error")
                {
                    mm.ErrorMsg = response[1];
                }
                GetExit_id();
            }
            catch (Exception ex)
            {
                mm.ErrorMsg = ex.Message;
            }
            return Json(mm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExit_id()
        {
            
            try
            {
                int Exit_id = tedb_layer.GetExit_id(Convert.ToInt32(Session["Emp_id"]));
                ViewBag.Exit_idType = Exit_id;               
                return View(Exit_id);
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult TabExit()
        {
            List<TranExitFormModel> UserProfileList = new List<TranExitFormModel>();
            try
            {
                dt = tedb_layer.GetUserProfile(Convert.ToInt32(Session["Emp_id"]));
                foreach (DataRow dr in dt.Rows)
                {

                    TranExitFormModel mm = new TranExitFormModel();
                    mm.emp_id = Convert.ToInt32(Session["emp_id"]);
                    mm.first_name = dr["first_name"].ToString();
                    mm.job_name = dr["job_name"].ToString();
                    mm.designation = dr["designation"].ToString();
                    mm.department_name = dr["department_name"].ToString();
                    mm.strDOJ = dr["date_of_joining"].ToString();

                    UserProfileList.Add(mm);
                    GetExit_id();
                }
                return View(UserProfileList);
            }
            catch (Exception)
            {
                return View();
            }
        }
        public ActionResult StylishTab()
        {
            List<TranExitFormModel> UserProfileList = new List<TranExitFormModel>();
            try
            {
                dt = tedb_layer.GetUserProfile(Convert.ToInt32(Session["Emp_id"]));
                foreach (DataRow dr in dt.Rows)
                {

                    TranExitFormModel mm = new TranExitFormModel();
                    mm.emp_id = Convert.ToInt32(Session["emp_id"]);
                    mm.first_name = dr["first_name"].ToString();
                    mm.job_name = dr["job_name"].ToString();
                    mm.designation = dr["designation"].ToString();
                    mm.department_name = dr["department_name"].ToString();
                    mm.strDOJ = dr["date_of_joining"].ToString();

                    UserProfileList.Add(mm);
                    GetExit_id();
                }
                return View(UserProfileList);
            }
            catch (Exception)
            {
                return View();
            }
        }
        public ActionResult AppraisalFrm()
        {
            return View();
        }
        public ActionResult SexyTab()
        {
            return View();
        }
    }
}