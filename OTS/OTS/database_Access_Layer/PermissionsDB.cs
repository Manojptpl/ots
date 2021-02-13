using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTS.Models;

namespace OTS.database_Access_Layer
{
    public class PermissionsDB
    {
        LoginValues cm = new LoginValues();
        private SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }

        public DataTable getroles()
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {

                connection();
                string query = "SELECT ROLE_ID,ROLE_NAME FROM MST_ROLE WHERE STATUS='Active'";
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public DataTable getRolesByRoleId(int roleId)
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {

                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetRoleById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@roleId", roleId);
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@message";
                    param.SqlDbType = SqlDbType.NVarChar;
                    param.Size = 100;
                    param.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(param);

                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public string updateRoleById(roleModel rm)
        {
            string res = "";

            try
            {   
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_UpdateRoleById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@roleId", rm.ROLE_ID);
                    cmd.Parameters.AddWithValue("@roleName", rm.ROLE_NAME);
                    cmd.Parameters.AddWithValue("@status", rm.STATUS);
                    cmd.Parameters.Add("@LoginValue", SqlDbType.Structured).Value = cm;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@message";
                    param.SqlDbType = SqlDbType.NVarChar;
                    param.Size = 100;
                    param.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(param);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public string addroles(string roleName, DateTime currentDate, string createdBy)
        {
            string res = "";
            try
            {

                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_AddRoles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_NAME", SqlDbType.VarChar, 50).Value = roleName;
                    cmd.Parameters.Add("@STATUS", SqlDbType.VarChar, 50).Value = "Active";
                    cmd.Parameters.Add("@EFFECTIVE_START_DATE", SqlDbType.DateTime).Value = currentDate;
                    cmd.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = currentDate;
                    cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar, 50).Value = createdBy;

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 100;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public DataTable getmodules(int roleId)
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetModule", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_ID", SqlDbType.Int).Value = roleId;

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        //All the permissions of roles
        public DataTable getPagePermission(int roleId)
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetPagePermissions", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_ID", SqlDbType.Int).Value = roleId;

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        //All the permissions assigned to roles for user
        public DataTable getPagePermissionForUser(int roleId, int empId)
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetPagePermissionsForUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_ID", SqlDbType.Int).Value = roleId;
                    cmd.Parameters.Add("@EMPLOYEE_ID", SqlDbType.Int).Value = empId;

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public string updateRolePermission(IEnumerable<RolePermissionsStatus> rollPermission)
        {
            string res = "";
            try
            {
                DataTable dtups = new DataTable();
                dtups.Columns.Add("ROLE_ID");
                dtups.Columns.Add("ROLE_PERMISSION_ID");
                dtups.Columns.Add("CHECKED_STATUS");

                foreach (RolePermissionsStatus ps in rollPermission)
                {
                    DataRow dr = dtups.NewRow();
                    dr["ROLE_ID"] = ps.ROLE_ID;
                    dr["ROLE_PERMISSION_ID"] = ps.ROLE_PERMISSION_ID;
                    dr["CHECKED_STATUS"] = ps.CHECKED_STATUS;
                    dtups.Rows.Add(dr);
                }

                connection();
                //string query = "UPDATE MST_ROLE_PERMISSION SET STATUS=CASE WHEN @STATUS='TRUE' THEN 'Active' ELSE 'In-Active' END WHERE ROLE_PERMISSION_ID=@ROLEPERMISSIONID";
                using (SqlCommand cmd = new SqlCommand("prc_UpdateRolePermission", con))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@ROLEPERMISSIONID", SqlDbType.Int).Value = rollPermissionId;
                    //cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = checkedStatus;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_PERMISSION_STATUS", SqlDbType.Structured).Value = dtups;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public Page_Permission_Detail Get_Page_UserPermission(int emp_id, int page_id)
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info.View = 0;
            info.Update = 0;
            info.Create = 0;
            info.Import = 0;
            info.Export = 0;
            connection();
            SqlCommand cmd = new SqlCommand("PRC_GetUserPermission", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Emp_ID", emp_id);
            cmd.Parameters.AddWithValue("@PageID", page_id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                info.emp_id = Convert.ToInt32(dr["EMP_ID"].ToString());
                info.page_id = Convert.ToInt32(dr["PAGE_ID"].ToString());
                if (dr["PERMISSION_NAME"].ToString().Trim() == "View")
                {
                    info.View = 1;
                }
                if (dr["PERMISSION_NAME"].ToString().Trim() == "Update")
                {
                    info.Update = 1;
                }
                if (dr["PERMISSION_NAME"].ToString().Trim() == "Create")
                {
                    info.Create = 1;
                }
                if (dr["PERMISSION_NAME"].ToString().Trim() == "Import")
                {
                    info.Import = 1;
                }
                if (dr["PERMISSION_NAME"].ToString().Trim() == "Export")
                {
                    info.Export = 1;
                }

            }
            con.Close();
            return info;
        }

        #region Assign Roles

        public DataTable getEmployeeSystemUser()
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("Proc_GetEmpDetail_SystemUser", con))
                {
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public DataTable getEmployeeSystemUserById(Int64 empId)
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Proc_GetEmpDetail_SystemUserByEmpId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@EMP_ID", SqlDbType.Int).Value = empId;

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public DataTable getEmployeeNotSystemUser()
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("Proc_GetEmpDetail_NotSystemUser", con))
                {
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public DataTable getEmployeeNotSystemUserByEmpId(Int64 empId)
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Proc_GetEmpDetail_NotSystemUser_EmpId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@EMP_ID", SqlDbType.Int).Value = empId;

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public string updateUserRolePermission(IEnumerable<UserPermissionsStatus> ups, string login_user) //int empId, int rollPermissionId, string checkedStatus)
        {
            string res = "";
            try
            {


                DataTable dtups = new DataTable();
                dtups.Columns.Add("EMPLOYEE_ID");
                dtups.Columns.Add("ROLE_PERMISSION_ID");
                dtups.Columns.Add("ROLE_ID");
                dtups.Columns.Add("CHECKED_STATUS");

                foreach (UserPermissionsStatus ps in ups)
                {
                    DataRow dr = dtups.NewRow();
                    dr["EMPLOYEE_ID"] = ps.EMPLOYEE_ID;
                    dr["ROLE_PERMISSION_ID"] = ps.ROLE_PERMISSION_ID;
                    dr["ROLE_ID"] = ps.ROLE_ID;
                    dr["CHECKED_STATUS"] = ps.CHECKED_STATUS;
                    dtups.Rows.Add(dr);
                }

                connection();
                using (SqlCommand cmd = new SqlCommand("prc_SaveUpdateUserPermission", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@USER_PERMISSION_STATUS", SqlDbType.Structured).Value = dtups;
                    cmd.Parameters.AddWithValue("@login_user", login_user);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        #endregion

        #region Generate system user

        public List<SelectListItem> GetState(String selectedValue)
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            connection();
            using (SqlCommand cmd = new SqlCommand("Prc_GetStateList", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    countryli.Add(new SelectListItem
                    {
                        Text = dr["DZONGKHAG_NAME"].ToString(),
                        Value = dr["DZONGKHAG_ID"].ToString(),
                        Selected = dr["DZONGKHAG_ID"].ToString().Trim() == selectedValue.Trim() ? true : false
                    });


                }
                con.Close();
            }

            return countryli;
        }

        public List<SelectListItem> GetStore(String selectedValue)
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            connection();
            using (SqlCommand cmd = new SqlCommand("Prc_GetStoreList", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    countryli.Add(new SelectListItem
                    {
                        Text = dr["STORE_NAME"].ToString(),
                        Value = dr["STORE_ID"].ToString(),
                        Selected = dr["STORE_ID"].ToString().Trim() == selectedValue.Trim() ? true : false
                    });


                }
                con.Close();
            }

            return countryli;
        }

        public string CreateSystemUser(string emp_code, string password, int is_navone, string status, int role_id, string user_code, int store_id, string subInventory, string locator, string login_user, out Int64 empId)
        {
            string res = "";
            empId = 0;
            try
            {

                connection();
                SqlCommand cmd = new SqlCommand("Prc_InsertSystemUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_code", emp_code);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@store_id", store_id);
                cmd.Parameters.AddWithValue("@is_navone", is_navone);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@role_id", role_id);
                cmd.Parameters.AddWithValue("@user_code", user_code);
                cmd.Parameters.AddWithValue("@login_user", login_user);

                cmd.Parameters.AddWithValue("@subInventory", subInventory == null ? "" : subInventory);
                cmd.Parameters.AddWithValue("@locator", locator == null ? "" : locator);


                SqlParameter obj_hier = new SqlParameter();
                obj_hier.ParameterName = "@message";
                obj_hier.SqlDbType = SqlDbType.NVarChar;
                obj_hier.Size = 100;
                obj_hier.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(obj_hier);

                SqlParameter obj_UserId = new SqlParameter();
                obj_UserId.ParameterName = "@empId";
                obj_UserId.SqlDbType = SqlDbType.BigInt;
                obj_UserId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(obj_UserId);

                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(obj_hier.Value);
                empId = Convert.ToInt64(obj_UserId.Value);
                con.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string UpdateSystemUser(string empId, string password, int is_navone, string status, int role_id, int store_id, string subInventory, string locator, string login_user)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateSystemUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empId", empId);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@store_id", store_id);
                cmd.Parameters.AddWithValue("@is_navone", is_navone);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@role_id", role_id);
                cmd.Parameters.AddWithValue("@login_user", login_user);

                cmd.Parameters.AddWithValue("@subInventory", subInventory==null?"":subInventory);
                cmd.Parameters.AddWithValue("@locator", locator==null?"":locator);

                SqlParameter obj_hier = new SqlParameter();
                obj_hier.ParameterName = "@message";
                obj_hier.SqlDbType = SqlDbType.NVarChar;
                obj_hier.Size = 100;
                obj_hier.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(obj_hier);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(obj_hier.Value);
                con.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public List<SelectListItem> get_SubInventory(String storeId, string selectedValue = "")
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            connection();            
            using (SqlCommand cmd = new SqlCommand("Prc_getSubInv", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@STORE_ID", storeId);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    countryli.Add(new SelectListItem
                    {
                        Text = dr["Name"].ToString(),
                        Value = dr["Name"].ToString(),
                        Selected = dr["Name"].ToString().Trim() == selectedValue.Trim() ? true : false
                    });


                }
                con.Close();
            }

            return countryli;
        }

        public List<SelectListItem> get_locator(String storeId, string subInventory, string selectedValue = "")
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            connection();            
            using (SqlCommand cmd = new SqlCommand("Prc_getLocator", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@STORE_ID", storeId);
                cmd.Parameters.AddWithValue("@SUB_INV", subInventory);
                

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    countryli.Add(new SelectListItem
                    {
                        Text = dr["NAME"].ToString(),
                        Value = dr["NAME"].ToString(),
                        Selected = dr["NAME"].ToString().Trim() == selectedValue.Trim() ? true : false
                    });


                }
                con.Close();
            }

            return countryli;
        }

        #endregion
        
    }
}