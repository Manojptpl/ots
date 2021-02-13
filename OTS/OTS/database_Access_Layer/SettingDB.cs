using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using OTS.Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace OTS.database_Access_Layer
{
    public class SettingDB
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }
        public DataSet EmployeeSummary()
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmployeeDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public string CreateSystemUser(string Emp_code,string First_Name,string Middle_Name,string Last_Name,string Email,DateTime Dob,DateTime Doj,string Designation,
            string Password,int Role,int Department_Id,int Manager_Id,int Status,DateTime? Last_WDay,string contact,int Sex,int Add_By,DateTime Add_Date,int Deleted,
            DateTime Start_Date,DateTime? End_Date)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_CreateEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_code", Emp_code);
                cmd.Parameters.AddWithValue("@first_name", First_Name);
                cmd.Parameters.AddWithValue("@middle_name", Middle_Name);
                cmd.Parameters.AddWithValue("@last_name", Last_Name);
                cmd.Parameters.AddWithValue("@email", Email);
                cmd.Parameters.AddWithValue("@date_of_birth", Dob);
                cmd.Parameters.AddWithValue("@date_of_joining", Doj);
                cmd.Parameters.AddWithValue("@designation", Designation);
                cmd.Parameters.AddWithValue("@password", Password);
                cmd.Parameters.AddWithValue("@gender", Sex);
                cmd.Parameters.AddWithValue("@start_date", Start_Date);
                cmd.Parameters.AddWithValue("@end_date", End_Date);
                cmd.Parameters.AddWithValue("@role", Role);
                cmd.Parameters.AddWithValue("@department_id", Department_Id);
                cmd.Parameters.AddWithValue("@manager_id", Manager_Id);
                cmd.Parameters.AddWithValue("@add_by", Add_By);
                cmd.Parameters.AddWithValue("@add_date", Add_Date);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@deleted", Deleted);
                cmd.Parameters.AddWithValue("@last_wday",Last_WDay);
                cmd.Parameters.AddWithValue("@contact", contact);

                SqlParameter sp = new SqlParameter();
                sp.ParameterName = "@message";
                sp.SqlDbType = SqlDbType.NVarChar;
                sp.Size = 100;
                sp.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sp);

                con.Open();
                cmd.ExecuteNonQuery();
                res = sp.Value.ToString();

                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string UpdateSystemUser(int Emp_id, string First_Name, string Middle_Name, string Last_Name, string Email, DateTime Dob, DateTime Doj, string Designation,
            string Password, int Role, int Department_Id, int Manager_Id, int Status, DateTime? Last_WDay, string contact, int Sex,DateTime Start_Date, DateTime? End_Date, int Edit_By)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_UEmployeeData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_id", Emp_id);
                    cmd.Parameters.AddWithValue("@first_name", First_Name);
                    cmd.Parameters.AddWithValue("@middle_name", Middle_Name);
                    cmd.Parameters.AddWithValue("@last_name", Last_Name);
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@date_of_birth", Dob);
                    cmd.Parameters.AddWithValue("@date_of_joining", Doj);
                    cmd.Parameters.AddWithValue("@designation", Designation);
                    cmd.Parameters.AddWithValue("@password", Password);
                    cmd.Parameters.AddWithValue("@gender", Sex);
                    cmd.Parameters.AddWithValue("@start_date", Start_Date);
                    cmd.Parameters.AddWithValue("@end_date", End_Date);
                    cmd.Parameters.AddWithValue("@role", Role);
                    cmd.Parameters.AddWithValue("@department_id", Department_Id);
                    cmd.Parameters.AddWithValue("@manager_id", Manager_Id);
                    cmd.Parameters.AddWithValue("@edit_by", Edit_By);
                    cmd.Parameters.AddWithValue("@status", Status);
                    cmd.Parameters.AddWithValue("@last_wday", Last_WDay);
                    cmd.Parameters.AddWithValue("@contact", contact);

                    SqlParameter sp = new SqlParameter();
                    sp.ParameterName = "@message";
                    sp.SqlDbType = SqlDbType.NVarChar;
                    sp.Size = 50;
                    sp.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(sp);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = sp.Value.ToString();
                    con.Close();
                    return res;
                }                    
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string updateUserRolePermission(IEnumerable<UserPermissionsStatus> ups, string login_user)
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

        public DataTable GetEmployeeById(int EMP_ID)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetEmployeeById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", EMP_ID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
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

        public DataSet Get_Employee_Mail_info(int id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_Notification_EMAILAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public DataSet Get_Employee_Mail_info_on_approve(int id)
        {
            string res = "";
            DataSet dt = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_Notification_EMAILAddress_on_approve", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@applyleave_id", id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }

        public DataTable getRoleById(int role_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetRoleById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@role_id", role_id);
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataSet GetHierarchy()
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Bind", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataTable GetHierarchyByName(string hierarchy_name)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_HierarchyByName", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@hierarchy_name", hierarchy_name);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }                   
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public string save_hierarchy(DataTable tabledata)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_InsertHierarchy", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@hierarcy_data", tabledata);

                    SqlParameter obj_hier = new SqlParameter();
                    obj_hier.ParameterName = "@message";
                    obj_hier.SqlDbType = SqlDbType.NVarChar;
                    obj_hier.Size = 70;
                    obj_hier.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(obj_hier);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(obj_hier.Value);
                    con.Close();
                }                   
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
    }
}