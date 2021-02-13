using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTS.CommonFilters;
using OTS.Models;
using OTS.database_Access_Layer;

namespace OTS.Controllers
{
    [LoginFilter]
    public class SettingsController : Controller
    {
        PermissionsDB db_layer = new PermissionsDB();
        SettingDB dblayer = new SettingDB();

        successFailureModel successStatus = new successFailureModel();
        LoginValues cm = new LoginValues();

        #region Role & Permission

        public ActionResult RolesAndPermissions(int Id = 0)
        {
            DataTable dtroles = db_layer.getroles();
            //If id is not passed, then added first roles to active
            if (Id == 0 && dtroles.Rows.Count > 0)
            {
                Id = Convert.ToInt32(dtroles.Rows[0]["ROLE_ID"]);
            }
            ViewBag.roleId = Id;

            var roles_list = (from DataRow dr in dtroles.Rows
                              select new roleModel()
                              {
                                  ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString()),
                                  ROLE_NAME = dr["ROLE_NAME"].ToString(),
                                  ROLEACTIVEINDEX = (Id == Convert.ToInt32(dr["ROLE_ID"].ToString())) ? 1 : 0
                              }).ToList();
            ViewBag.roles = roles_list;

            DataTable dtmodules = db_layer.getmodules(Id);
            var modules_list = (from DataRow dr in dtmodules.Rows
                                select new moduleModel()
                                {
                                    MODULE_ID = Convert.ToInt32(dr["MODULE_ID"].ToString()),
                                    MODULE_NAME = dr["MODULE_NAME"].ToString(),
                                    ROLEACTIVEINDEX = Convert.ToInt32(dr["ISACTIVE"].ToString())
                                }).ToList();
            ViewBag.modules = modules_list;

            DataTable dtPagePermission = db_layer.getPagePermission(Id);

            #region Create table with columns

            DataTable dtfilterPagePermission = new DataTable();

            dtfilterPagePermission.Columns.Add("ROLE_PERMISSION_ID");
            dtfilterPagePermission.Columns.Add("ROLE_ID");
            dtfilterPagePermission.Columns.Add("PAGE_ID");

            dtfilterPagePermission.Columns.Add("PERMISSION_ID");
            dtfilterPagePermission.Columns.Add("MODULE_ID");
            dtfilterPagePermission.Columns.Add("STATUS");
            dtfilterPagePermission.Columns.Add("PAGE_NAME");
            dtfilterPagePermission.Columns.Add("MODULE_NAME");

            int noOfColumns = dtPagePermission.Columns.Count - 1;
            int noOfColumnsDynamic = 0;

            DataView view = new DataView(dtPagePermission);
            DataTable distinctValues = view.ToTable(true, "PERMISSION_NAME");
            foreach (DataRow pname in distinctValues.Rows)
            {
                dtfilterPagePermission.Columns.Add(pname["PERMISSION_NAME"].ToString());
                noOfColumnsDynamic += 1;
            }

            #endregion

            var pageName = "";
            var moduleName = "";
            int fillColumns = 0;
            DataRow row = dtfilterPagePermission.NewRow();

            foreach (DataRow pp in dtPagePermission.Rows)
            {

                if (pageName == "" || (pageName != pp["PAGE_NAME"].ToString()) || (moduleName != pp["MODULE_NAME"].ToString()))
                {
                    pageName = pp["PAGE_NAME"].ToString();
                    moduleName = pp["MODULE_NAME"].ToString();
                    fillColumns = 1;
                    row = dtfilterPagePermission.NewRow();

                    row["ROLE_PERMISSION_ID"] = pp["ROLE_PERMISSION_ID"].ToString();
                    row["ROLE_ID"] = pp["ROLE_ID"].ToString();
                    row["PAGE_ID"] = pp["PAGE_ID"].ToString();
                    row["PERMISSION_ID"] = pp["PERMISSION_ID"].ToString();
                    row["MODULE_ID"] = pp["MODULE_ID"].ToString();
                    row["STATUS"] = pp["STATUS"].ToString();
                    row["PAGE_NAME"] = pp["PAGE_NAME"].ToString();
                    row["MODULE_NAME"] = pp["MODULE_NAME"].ToString();
                    row[noOfColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString();
                }
                else if (pageName == pp["PAGE_NAME"].ToString() && moduleName==pp["MODULE_NAME"].ToString())
                {
                    row[noOfColumns + fillColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString();
                    fillColumns += 1;
                }
                //If original value including dynamic column 
                if (noOfColumnsDynamic == fillColumns)
                {
                    dtfilterPagePermission.Rows.Add(row);
                }
            }


            ViewBag.pagePermission = dtfilterPagePermission;
            ViewBag.fixedColumns = noOfColumns;

            return View();
        }

        //Ajax call
        public JsonResult CreateRole(string roleName)
        {
            try
            {
                string error = db_layer.addroles(roleName, cm.CurrentDate, Session["Emp_Code"].ToString());
                if (error == "")
                {
                    successStatus.SuccessMsg = "success";
                }
                successStatus.ErrorMsg = error;
            }
            catch (Exception ex)
            {
                successStatus.ErrorMsg = "Exception while creating role";
            }


            return Json(successStatus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateRolePermissionModule(List<RolePermissionsStatus> rolePermissions)
        {
            string error = "";
            try
            {
                if (rolePermissions.Count > 0)
                {
                    error = db_layer.updateRolePermission(rolePermissions.AsEnumerable());

                    if (error == "")
                    {
                        successStatus.SuccessMsg = "success";
                    }
                }
                else
                {
                    error = "No records to update";
                }
                successStatus.ErrorMsg = error;
            }
            catch (Exception ex)
            {
                successStatus.ErrorMsg = "Exception while creating role";
            }


            return Json(successStatus, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetRoelsForEdit(int roleId)
        {
            DataTable dtroles = db_layer.getRolesByRoleId(roleId);
            var roles_list = (from DataRow dr in dtroles.Rows
                              select new roleModel()
                              {
                                  ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString()),
                                  ROLE_NAME = dr["ROLE_NAME"].ToString(),
                                  STATUS = dr["STATUS"].ToString()
                              }).ToList();

            return PartialView("_UpdateRoles", roles_list);
        }

        [HttpPost]
        public JsonResult UpdateRole(roleModel rm)
        {
            SystemUser system = new SystemUser();
            try
            {
                string res = db_layer.updateRoleById(rm);
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    system.success_msg = response[1];
                }
                else
                {
                    system.error_msg = response[1];
                }
            }
            catch (Exception ex)
            {
                system.error_msg = ex.Message;
            }
            return Json(system, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRoleById(int role_id)
        {
            List<roleModel> rm_list = new List<roleModel>();
            try
            {
                DataTable dt = dblayer.getRoleById(role_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        roleModel rm = new roleModel();
                        rm.ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString());
                        rm.ROLE_NAME = dr["ROLE_NAME"].ToString();
                        rm.STATUS = dr["STATUS"].ToString();
                        rm_list.Add(rm);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(rm_list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Assign Roles

        public ActionResult AssignRoles()
        {
            try
            {
                DataTable dtEmployee = db_layer.getEmployeeSystemUser();
                var employees_list = (from DataRow dr in dtEmployee.Rows
                                      select new employeeModel()
                                      {
                                          EMPLOYEE_ID = Convert.ToInt64(dr["PERSON_ID"].ToString()),
                                          USER_ID = Convert.ToInt32(dr["USER_ID"].ToString()),
                                          ROLE_ID = dr["ROLE_ID"].ToString(),
                                          ROLE_NAME = dr["ROLE_NAME"].ToString(),
                                          EMPLOYEE_EMAIL_ADDRESS = dr["EMAIL_ADDRESS"].ToString(),
                                          EMP_NAME = dr["EMP_NAME"].ToString(),
                                          NATIONALITY = dr["NATIONALITY"].ToString(),
                                          ORIGINAL_DATE_OF_HIRE = dr["DATE_OF_JOINING"].ToString()
                                      }).ToList();

                string roleId = employees_list.FirstOrDefault().ROLE_ID;

                ViewBag.employees = employees_list;

                DataTable dtroles = db_layer.getroles();
                var roles_list = (from DataRow dr in dtroles.Rows
                                  select new roleModel()
                                  {
                                      ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString()),
                                      ROLE_NAME = dr["ROLE_NAME"].ToString()
                                  }).ToList();
                ViewBag.roles = new SelectList(roles_list, "ROLE_ID", "ROLE_NAME", roleId); ;
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        #endregion

        #region System User
        public ActionResult systemuser()
        {
            List<Employee> li_users = new List<Employee>();
            try
            {
                DataSet ds = dblayer.EmployeeSummary();

                ViewBag.SYSTEM_USER = ds.Tables[0];
                ViewBag.role = ds.Tables[1];               
                ViewBag.mng = ds.Tables[2];
                ViewBag.dept = ds.Tables[3];

                List<SelectListItem> mnglist = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Select", Value = "0" },
                };

                foreach (DataRow dr in ViewBag.mng.Rows)
                {
                    mnglist.Add(new SelectListItem { Text = dr["EMP_NAME"].ToString(), Value = dr["EMP_ID"].ToString() });
                }
                ViewBag.MNG_List = mnglist;

                List<SelectListItem> li_role = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Select", Value = "0" },
                };

                foreach (DataRow dr in ViewBag.role.Rows)
                {
                    li_role.Add(new SelectListItem { Text = @dr["ROLE_NAME"].ToString(), Value = @dr["ROLE_ID"].ToString() });
                }
                ViewBag.ROLE_List = li_role;

                List<SelectListItem> li_dept = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Select", Value = "0" },
                };

                foreach (DataRow dr in ViewBag.dept.Rows)
                {
                    li_dept.Add(new SelectListItem { Text = @dr["DEPARTMENT_NAME"].ToString(), Value = @dr["DEPARTMENT_ID"].ToString() });
                }
                ViewBag.DEPT_List = li_dept;

                foreach (DataRow dr in ViewBag.SYSTEM_USER.Rows)
                {
                    Employee su = new Employee();
                    su.Sr_No = Convert.ToInt32(dr["SR_NO"].ToString());
                    su.Emp_id = Convert.ToInt32(dr["EMP_ID"].ToString());
                    su.Emp_Name = dr["EMP_NAME"].ToString();
                    su.Letter = dr["LETTER"].ToString();
                    su.Email = dr["EMAIL"].ToString();
                    su.Department_Name= dr["DEPARTMENT_NAME"].ToString();
                    su.Designation_Name = dr["DESIGNATION"].ToString();
                    su.Created_Date = dr["CREATED_DATE"].ToString();
                    su.ROLE_NAME = dr["ROLE_NAME"].ToString();
                    su.ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString());
                    li_users.Add(su);
                }
            }
            catch (Exception ex)
            {

            }
            return View(li_users);
        }

        public PartialViewResult GetUserPermissionByRoleId(int roleId, int empId)
        {

            #region Create user permission

            DataTable dtPagePermission = db_layer.getPagePermissionForUser(roleId, empId);

            #region Create table with columns

            DataTable dtfilterPagePermission = new DataTable();

            dtfilterPagePermission.Columns.Add("ROLE_PERMISSION_ID");
            dtfilterPagePermission.Columns.Add("ROLE_ID");
            dtfilterPagePermission.Columns.Add("PAGE_ID");

            dtfilterPagePermission.Columns.Add("PERMISSION_ID");
            dtfilterPagePermission.Columns.Add("MODULE_ID");
            dtfilterPagePermission.Columns.Add("STATUS");
            dtfilterPagePermission.Columns.Add("UPSTATUS");
            dtfilterPagePermission.Columns.Add("PAGE_NAME");
            dtfilterPagePermission.Columns.Add("MODULE_NAME");

            int noOfColumns = dtPagePermission.Columns.Count - 1;
            int noOfColumnsDynamic = 0;

            DataView view = new DataView(dtPagePermission);
            DataTable distinctValues = view.ToTable(true, "PERMISSION_NAME");
            foreach (DataRow pname in distinctValues.Rows)
            {
                dtfilterPagePermission.Columns.Add(pname["PERMISSION_NAME"].ToString());
                noOfColumnsDynamic += 1;
            }

            #endregion

            var pageName = "";
            int fillColumns = 0;
            DataRow row = dtfilterPagePermission.NewRow();

            foreach (DataRow pp in dtPagePermission.Rows)
            {

                if (pageName == "" || (pageName != pp["PAGE_NAME"].ToString()))
                {
                    pageName = pp["PAGE_NAME"].ToString();
                    fillColumns = 1;
                    row = dtfilterPagePermission.NewRow();

                    row["ROLE_PERMISSION_ID"] = pp["ROLE_PERMISSION_ID"].ToString();
                    row["ROLE_ID"] = pp["ROLE_ID"].ToString();
                    row["PAGE_ID"] = pp["PAGE_ID"].ToString();
                    row["PERMISSION_ID"] = pp["PERMISSION_ID"].ToString();
                    row["MODULE_ID"] = pp["MODULE_ID"].ToString();
                    row["STATUS"] = pp["STATUS"].ToString();
                    row["PAGE_NAME"] = pp["PAGE_NAME"].ToString();
                    row["MODULE_NAME"] = pp["MODULE_NAME"].ToString();
                    row[noOfColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString() + "~" + pp["UPSTATUS"].ToString();
                }
                else if (pageName == pp["PAGE_NAME"].ToString())
                {
                    row[noOfColumns + fillColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString() + "~" + pp["UPSTATUS"].ToString();
                    fillColumns += 1;
                }
                //If original value including dynamic column 
                if (noOfColumnsDynamic == fillColumns)
                {
                    dtfilterPagePermission.Rows.Add(row);
                }
            }
            ViewBag.pagePermission = dtfilterPagePermission;
            ViewBag.fixedColumns = noOfColumns;

            UserPermissions up = new UserPermissions();
            up.DATATABLE_VALUE = dtfilterPagePermission;
            up.NO_FIXED_COLUMN = noOfColumns;
            up.EMPLOYEE_ID = empId;
            up.ROLE_ID = roleId;

            #endregion

            if (empId == 0)
            {
                return PartialView("_UserPermissionAdd", up);
            }
            else
            {
                return PartialView("_UserPermission", up);
            }
        }

        [HttpPost]
        public JsonResult CreateUser(SystemUser su)
        {
            SystemUser system = new SystemUser();
            try
            {
                su.Add_By = Convert.ToInt32(Session["Emp_id"].ToString());
                su.Add_Date = DateTime.Today;
                su.Deleted = 0;

                string res = dblayer.CreateSystemUser(su.Emp_code, su.First_Name, su.Middle_Name, su.Last_Name, su.Email, su.Dob, su.Doj, su.Designation, su.Password, su.Role, su.Department_Id,
                    su.Manager_Id, su.Status, su.Last_WDay, su.contact, su.Sex, su.Add_By, su.Add_Date, su.Deleted,su.Start_Date,su.End_Date);

                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    string resPermission = "Not updated";
                    if (su.userPermissions != null)
                    {
                        su.userPermissions.ToList().ForEach(a => a.EMPLOYEE_ID = Convert.ToInt32(response[2]));
                        resPermission = dblayer.updateUserRolePermission(su.userPermissions.AsEnumerable(), response[2]);
                    }
                    system.success_msg = su.Emp_code.ToUpper() + " " + response[1];
                }
                else
                {
                    system.error_msg = response[1];
                }
            }
            catch (Exception ex)
            {
                system.error_msg = ex.Message;
            }
            return Json(system, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeById(int EMP_ID)
        {
            List<Employee> li_system = new List<Employee>();
            try
            {
                DataTable emp_dt = dblayer.GetEmployeeById(EMP_ID);
                foreach (DataRow dr in emp_dt.Rows)
                {
                    Employee su = new Employee();
                    su.Emp_id = Convert.ToInt32(dr["EMP_ID"].ToString());
                    su.Emp_code = dr["EMP_CODE"].ToString();
                    su.First_Name = dr["FIRST_NAME"].ToString();
                    su.Middle_Name = dr["MIDDLE_NAME"].ToString();
                    su.Last_Name = dr["LAST_NAME"].ToString();
                    su.Email = dr["EMAIL"].ToString();
                    su.ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString());
                    su.Password = dr["PASSWORD"].ToString();
                    su.Gender_ID =Convert.ToInt32(dr["GENDER_ID"].ToString());
                    su.Status = dr["STATUS"].ToString();
                    su.Start_Date = dr["START_DATE"].ToString();
                    su.End_Date = dr["END_DATE"].ToString();
                    su.Department_ID = dr["Department_id"].ToString();
                    su.Designation_Name = dr["Designation"].ToString();
                    su.Manager_ID= dr["Manager_id"].ToString();
                    su.Phone = dr["Contact"].ToString();
                    su.Dob = dr["Dob"].ToString();
                    su.Doj = dr["Doj"].ToString();
                    su.Last_Working_Day= dr["L_WDAY"].ToString(); 
                    li_system.Add(su);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(li_system, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ViewEmployeeById(int EMP_ID)
        {
            List<Employee> li_system = new List<Employee>();
            try
            {
                DataTable emp_dt = dblayer.GetEmployeeById(EMP_ID);
                foreach (DataRow dr in emp_dt.Rows)
                {
                    Employee su = new Employee();

                    su.Emp_code = dr["EMP_CODE"].ToString();
                    su.First_Name = dr["FIRST_NAME"].ToString();
                    su.Middle_Name = dr["MIDDLE_NAME"].ToString();
                    su.Last_Name = dr["LAST_NAME"].ToString();
                    su.Email = dr["EMAIL"].ToString();
                    su.ROLE_NAME = dr["ROLE_NAME"].ToString();
                    su.Password = dr["PASSWORD"].ToString();
                    su.Gender = dr["GENDER"].ToString();
                    su.Status = dr["EMP_STATUS"].ToString();
                    su.Start_Date = dr["START_DATE"].ToString();
                    su.End_Date = dr["END_DATE"].ToString();
                    su.Department_Name = dr["DEPARTMENT_NAME"].ToString();
                    su.Designation_Name = dr["Designation"].ToString();
                    su.Manager_NAME = dr["MANAGER_NAME"].ToString();
                    su.Phone = dr["Contact"].ToString();
                    su.Dob = dr["Dob"].ToString();
                    su.Doj = dr["Doj"].ToString();
                    su.Last_Working_Day = dr["L_WDAY"].ToString();
                    li_system.Add(su);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(li_system, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateUser(SystemUser su)
        {
            SystemUser system = new SystemUser();
            try
            {
                int Edit_by = Convert.ToInt32(Session["emp_id"].ToString());
                string res = dblayer.UpdateSystemUser(su.Emp_ID, su.First_Name,su.Middle_Name,su.Last_Name,su.Email,su.Dob,su.Doj,su.Designation,su.Password
                    ,su.Role, su.Department_Id, su.Manager_Id, su.Status, su.Last_WDay, su.contact, su.Sex, su.Start_Date,su.End_Date, Edit_by);
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    string resPermission = "Not updated";
                    if (su.userPermissions != null)
                    {
                        resPermission = dblayer.updateUserRolePermission(su.userPermissions.AsEnumerable(),su.Emp_ID.ToString());
                    }
                    system.success_msg = response[1];
                }
                else
                {
                    system.error_msg = response[1];
                }
            }
            catch (Exception ex)
            {
                system.error_msg = ex.Message;
            }
            return Json(system, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Theme Settings
        public ActionResult Themesettings()
        {
            return View();
        }
        #endregion

        #region Hierarchy
        public ActionResult Hierarchy()
        {
            try
            {
                Page_Permission_Detail info = new Page_Permission_Detail();
                info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 43);
                ViewBag.event_permission = info;
                List<SelectListItem> li_emp = new List<SelectListItem>();
                ViewBag.employee = li_emp;
            }
            catch(Exception)
            {
                throw;
            }           
            return View();
        }

        [HttpGet]
        public JsonResult GetHierarchy()
        {
            var Hierarchy = "";
            var Employee = "";
            try
            {
                DataSet ds = dblayer.GetHierarchy();
                Hierarchy = Utility.DataTableToJSONWithJSONNet(ds.Tables[11]);
                Employee = Utility.DataTableToJSONWithJSONNet(ds.Tables[10]);
            }
            catch (Exception)
            {
                throw;
            }
            return Json(new { Hierarchy, Employee }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult save_hierarchy(string TableData)
        {
            successFailureModel sm = new successFailureModel();

            try
            {
                var serializeData = JsonConvert.DeserializeObject<List<HierarchyModel>>(TableData);

                ListtoDataTableConverter converter = new ListtoDataTableConverter();

                DataTable dt = converter.ToDataTable(serializeData);

                string res = dblayer.save_hierarchy(dt);
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    sm.SuccessMsg = response[1];
                }
                else
                {
                    sm.ErrorMsg = response[1];
                }
            }
            catch (Exception ex)
            {
                sm.ErrorMsg = ex.Message;
            }
            return Json(sm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetHierarchyByName(string hierarchy_name)
        {
            var jsonobj = "";
            try
            {
                var dt = dblayer.GetHierarchyByName(hierarchy_name);
                jsonobj = Utility.DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception ex)
            {
                jsonobj = ex.Message;
            }
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Approval Rules
        public ActionResult approval_rules()
        {
            try
            {
                Page_Permission_Detail info = new Page_Permission_Detail();
                info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 35);
                ViewBag.event_permission = info;

                List<SelectListItem> li_level = new List<SelectListItem>();
                ViewBag.level = li_level;
            }
            catch(Exception ex)
            {

            }
            return View();
        }

        public ActionResult approval_rules_setting()
        {
            try
            {
                List<SelectListItem> li_value = new List<SelectListItem>();
                ViewBag.value = li_value;

                List<SelectListItem> li_hierarchy = new List<SelectListItem>();
                ViewBag.hierarchy = li_hierarchy;

                List<SelectListItem> li_emp = new List<SelectListItem>();
                ViewBag.Employee = li_emp;
            }
            catch(Exception)
            {

            }           
            return View();
        }

        [HttpGet]
        public JsonResult GetEmployee()
        {
            var Employee = "";
            try
            {
                DataSet ds = dblayer.GetHierarchy();
                Employee = Utility.DataTableToJSONWithJSONNet(ds.Tables[10]);
            }
            catch (Exception ex)
            {
                Employee = ex.Message;
            }
            return Json(Employee, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}