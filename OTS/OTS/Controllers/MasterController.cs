using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTS.Models;
using OTS.CommonFilters;
using OTS.database_Access_Layer;


namespace OTS.Controllers
{
    [LoginFilter]
    public class MasterController : Controller
    {        
        MastersModel mm = new MastersModel();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        MasterDB mdb_layer = new MasterDB();
        db dblayer = new db();

        #region Holiday
        public ActionResult Create_Holiday()
        {
            List<SelectListItem> holiday_typelist = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select", Value = "0" },
                new SelectListItem { Text = "Gazetted Holiday", Value = "1" },
                new SelectListItem { Text = "Declared Holiday", Value = "2" },
                new SelectListItem { Text = "Regional Holiday", Value = "3" },
                new SelectListItem { Text = "Public Holiday", Value = "4" }
            };
            ViewBag.holiday = holiday_typelist;
            return View();
        }

        [HttpPost]
        public JsonResult CreateHoliday(string Holiday_Name, DateTime Holiday_Date, int HolidayType_Id, int Status)
        {
            string res = "";
            try
            {
                res = mdb_layer.CreateHoliday(Holiday_Name, Holiday_Date, HolidayType_Id, Convert.ToInt32(Session["Emp_id"].ToString()), Status);
                if (res == "Success")
                {
                    mm.SuccessMsg = Holiday_Name + " is successfully created.";
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

        public ActionResult Holiday_List()
        {
            List<MastersModel> hmlist = new List<MastersModel>();
            try
            {
                dt = mdb_layer.HolidaySummary();
                foreach (DataRow dr in dt.Rows)
                {
                    MastersModel mm = new MastersModel();
                    mm.Holiday_Name = dr["holiday_name"].ToString();
                    mm.Show_HolidayDate = dr["holiday_date"].ToString();
                    mm.Holiday_Type = dr["holiday_type"].ToString();
                    mm.Show_Status = dr["holiday_Status"].ToString();
                    hmlist.Add(mm);
                }
                return View(hmlist);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        #endregion

        #region Department
        public ActionResult Department_List()
        {
            List<MastersModel> dmlist = new List<MastersModel>();
            try
            {
                dt = mdb_layer.DepartmentSummary();
                foreach (DataRow dr in dt.Rows)
                {

                    MastersModel mm = new MastersModel();
                    mm.Department_Name = dr["Department_Name"].ToString();
                    mm.Department_Code = dr["Department_Code"].ToString();
                    mm.Division_Name = dr["Division_Name"].ToString();
                    mm.Show_Status = dr["Status"].ToString();
                    dmlist.Add(mm);
                }
                return View(dmlist);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult Create_Department()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateDepartment(DepartmentEntities de)
        {
            string res = "";
            try
            {
                int Add_By = Convert.ToInt32(Session["Emp_id"].ToString());
                DateTime Add_Date = DateTime.Today;
                int Deleted = 0;
                res = dblayer.CreateDepartment(de.Department_Code, de.Department_Name, de.Division_Code, de.Division_Name, Add_By, Add_Date, de.Status, Deleted);
                if (res == "Success")
                {
                    mm.SuccessMsg = de.Department_Code.ToUpper() + " is successfully created.";
                }
                else
                {
                    mm.ErrorMsg = res;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(mm, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Project
        public ActionResult Project_List()
        {
            List<MastersModel> Projectlist = new List<MastersModel>();
            try
            {
                dt = mdb_layer.ProjectSummary();
                foreach (DataRow dr in dt.Rows)
                {
                    MastersModel mm = new MastersModel();
                    mm.Project_Name = dr["project_name"].ToString();
                    mm.Project_Code = dr["project_code"].ToString();
                    mm.Project_Desc = dr["project_desc"].ToString();
                    mm.Show_Status = dr["Status"].ToString();
                    Projectlist.Add(mm);
                }
                return View(Projectlist);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult Create_Project()
        {
            try
            {
                ds = dblayer.Bind_DropDownList();
                ViewBag.projtype = ds.Tables[8];
                List<SelectListItem> ProjectType_li = new List<SelectListItem>();
                foreach (DataRow dr in ViewBag.projtype.Rows)
                {
                    ProjectType_li.Add(new SelectListItem { Text = dr["project_type"].ToString(), Value = dr["project_type_id"].ToString() });
                }
                ViewBag.Project_type = ProjectType_li;


                ViewBag.deptt = ds.Tables[5];
                List<SelectListItem> dplist = new List<SelectListItem>();
                foreach (DataRow dr in ViewBag.deptt.Rows)
                {
                    dplist.Add(new SelectListItem { Text = dr["department_name"].ToString(), Value = dr["department_id"].ToString() });
                }
                ViewBag.dept = dplist;
                ViewBag.custt = ds.Tables[4];
                List<SelectListItem> custlist = new List<SelectListItem>();
                foreach (DataRow dr in ViewBag.custt.Rows)
                {
                    custlist.Add(new SelectListItem { Text = dr["customer_name"].ToString(), Value = dr["customer_id"].ToString() });
                }
                ViewBag.cust = custlist;
                ViewBag.empp = ds.Tables[6];
                List<SelectListItem> emplist = new List<SelectListItem>();
                foreach (DataRow dr in ViewBag.empp.Rows)
                {
                    emplist.Add(new SelectListItem { Text = dr["emp_name"].ToString(), Value = dr["emp_id"].ToString() });
                }
                ViewBag.emp = emplist;

                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public JsonResult CreateProject(ProjectEntities pe)
        {
            string res = "";
            try
            {
                res = mdb_layer.CreateProject(pe.Project_Code, pe.Project_Name, pe.Project_Desc, pe.Start_Date, pe.End_Date, pe.Project_TypeId, pe.Project_Cost, pe.Department_Id, pe.Customer_Id, pe.Manager_Id, Convert.ToInt32(Session["Emp_id"].ToString()), pe.Status);
                if (res == "Success")
                {
                    mm.SuccessMsg = pe.Project_Code.ToUpper() + " is successfully created.";
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
        #endregion

        #region Customer
        public ActionResult Customer_List()
        {
            List<MastersModel> Customerlist = new List<MastersModel>();
            try
            {
                dt = mdb_layer.CustomerSummary();
                foreach (DataRow dr in dt.Rows)
                {

                    MastersModel mm = new MastersModel();
                    mm.Customer_Name = dr["customer_name"].ToString();
                    mm.Customer_Code = dr["customer_code"].ToString();
                    mm.Address = dr["address"].ToString();
                    mm.City = dr["city"].ToString();
                    mm.Show_Status = dr["Status"].ToString();
                    Customerlist.Add(mm);
                }
                return View(Customerlist);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult Create_Customer()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateCustomer(CustomerEntities ce)
        {
            string res = "";
            try
            {
                res = mdb_layer.CreateCustomer(ce.Customer_Code, ce.Customer_Name, ce.Address, ce.City, ce.State, ce.Country, ce.Zip_Code, Convert.ToInt32(Session["Emp_id"].ToString()), ce.Status);
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
       
        #endregion

        #region Contact
        public ActionResult Contact_List()
        {
            List<MastersModel> mmlist = new List<MastersModel>();
            try
            {
                dt = mdb_layer.ContactSummary();
                foreach (DataRow dr in dt.Rows)
                {
                    MastersModel mm = new MastersModel();
                    mm.First_Name = dr["first_name"].ToString();
                    mm.Middle_Name = dr["middle_name"].ToString();
                    mm.Last_Name = dr["last_name"].ToString();
                    mm.Email = dr["email"].ToString();
                    mm.Designation = dr["designation"].ToString();
                    mm.Customer_Name = dr["customer_name"].ToString();
                    mm.Show_Status = dr["Status"].ToString();
                    mmlist.Add(mm);
                }
                return View(mmlist);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult Create_Contact()
        {
            try
            {
                ds = dblayer.Bind_DropDownList();
                ViewBag.custt = ds.Tables[4];
                List<SelectListItem> custlist = new List<SelectListItem>();
                foreach (DataRow dr in ViewBag.custt.Rows)
                {
                    custlist.Add(new SelectListItem { Text = dr["customer_name"].ToString(), Value = dr["customer_id"].ToString() });
                }
                ViewBag.cust = custlist;
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public JsonResult CreateContact(ContactEntities ce)
        {
            string res = "";
            try
            {
                int Add_By = Convert.ToInt32(Session["Emp_id"].ToString());
                DateTime Add_Date = DateTime.Today;
                int Deleted = 0;
                res = dblayer.CreateContact(ce.First_Name, ce.Middle_Name, ce.Last_Name, ce.Email, ce.Designation, ce.Customer_Id, Add_By, Add_Date, ce.Status, Deleted);
                if (res == "Success")
                {
                    mm.SuccessMsg = ce.First_Name.ToUpper() + " " + ce.Last_Name.ToUpper() + " is successfully created.";
                }
                else
                {
                    mm.ErrorMsg = res;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(mm, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ClientHistory
        public ActionResult Task_History()
        {
            List<TaskModel> Tasklist = new List<TaskModel>();
            try
            {
                dt = mdb_layer.TaskSummary();
                foreach (DataRow dr in dt.Rows)
                {

                    TaskModel tm = new TaskModel();
                    tm.TASK_ID = Convert.ToInt32(dr["TASK_ID"]);
                    tm.TaskTitle = dr["TaskTitle"].ToString();
                    tm.Client = dr["Client"].ToString();
                    tm.Project = dr["Project"].ToString();
                    tm.DueDate = dr["DueDate"].ToString();
                    tm.Description = dr["Description"].ToString();
                    tm.EmpDescription = dr["EmpDescription"].ToString();
                    tm.Show_Status = dr["Status"].ToString();
                    Tasklist.Add(tm);
                    GetStatus();
                }
                return View(Tasklist);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult GetStatus()
        {
            List<SelectListItem> statusli = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Open",Value="1" },
                new SelectListItem { Text = "Completed",Value="2" },
                new SelectListItem { Text = "Progress",Value="3" }               
            };
            ViewBag.Status_TYPE = statusli;
            return View(statusli);
        }
        public JsonResult EditTask(int Task_ID)
        {
            TaskModel objTM = new TaskModel();
            objTM = mdb_layer.GetTaskDetails(Task_ID);
            return Json(objTM, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Exit Controller
        public ActionResult Exit()
        {
            List<MastersModel> UserProfileList = new List<MastersModel>();
            try
            {
                dt = mdb_layer.GetUserProfile(Convert.ToInt32(Session["Emp_id"]));
                foreach (DataRow dr in dt.Rows)
                {

                    MastersModel mm = new MastersModel();
                    mm.Emp_Id = Convert.ToInt32(Session["Emp_id"]);
                    mm.First_Name = dr["first_name"].ToString();
                    mm.JOB_NANE = dr["job_name"].ToString();
                    mm.Designation = dr["designation"].ToString();
                    mm.Department_Name = dr["department_name"].ToString();
                    mm.strDOJ = dr["date_of_joining"].ToString();

                    UserProfileList.Add(mm);                    
                }
                return View(UserProfileList);
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }
        #endregion
        #region Employee Resignation
        public ActionResult Employee_Resignation()
        {
            List<MastersModel> EmpResignationList = new List<MastersModel>();
            try
            {
                dt = mdb_layer.GetEmpResignation(Convert.ToInt32(Session["Emp_id"]));
                foreach (DataRow dr in dt.Rows)
                {

                    MastersModel mm = new MastersModel();
                    mm.Emp_Id = Convert.ToInt32(Session["Emp_id"]);
                    mm.Emp_Name = dr["Emp_Name"].ToString();
                    mm.Manager_Name = dr["MANAGER_NAME"].ToString();                   
                    mm.Department_Name = dr["department_name"].ToString();
                    mm.Current_Date = dr["currentDate"].ToString();
                    mm.LastWorking_Date = dr["LastWorkingDate"].ToString();                    
                    EmpResignationList.Add(mm);
                    FillDropdownbox();
                    
                }
                return View(EmpResignationList);
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }

        public ActionResult FillDropdownbox()
        {
            try
            {
                ds = mdb_layer.Bind_MultipleColumnDropdown();
                ViewBag.Resignation = ds.Tables[0];
                List<SelectListItem> ResignationList = new List<SelectListItem>();
                foreach (DataRow dr in ViewBag.Resignation.Rows)
                {
                    ResignationList.Add(new SelectListItem { Text = dr["CombinedData"].ToString(), Value = dr["emp_id"].ToString() });
                }
                ViewBag.ResignationType = ResignationList;
                return View();
            }
            catch (Exception ex)
            {
                
            }
            return View();         
        }
        
        #endregion
    }
}