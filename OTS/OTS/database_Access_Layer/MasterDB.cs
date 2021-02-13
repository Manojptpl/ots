using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using OTS.Models;

namespace OTS.database_Access_Layer
{
    public class MasterDB
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }
        public string CreateHoliday(string holiday_name, DateTime holiday_date, int holidaytype_id, int Add_By, int Status)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_CreateHoliday", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@holiday_name", holiday_name);
                    cmd.Parameters.AddWithValue("@holiday_date", holiday_date);
                    cmd.Parameters.AddWithValue("@holiday_type", holidaytype_id);
                    cmd.Parameters.AddWithValue("@add_by", Add_By);
                    cmd.Parameters.AddWithValue("@status", Status);

                    SqlParameter oblogin = new SqlParameter();
                    oblogin.ParameterName = "@message";
                    oblogin.SqlDbType = SqlDbType.NVarChar;
                    oblogin.Size = 50;
                    oblogin.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(oblogin);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(oblogin.Value);
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
        public string CreateProject(string Proj_Code, string Proj_Name, string Proj_Desc, DateTime? start_date, DateTime? end_date, int Proj_TypeId, decimal Proj_Cost, int Dept_id, int Cust_id, int Mng_id, int Add_By, int Status)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_CreateProject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@project_code", Proj_Code);
                    cmd.Parameters.AddWithValue("@project_name", Proj_Name);
                    cmd.Parameters.AddWithValue("@project_desc", Proj_Desc);
                    cmd.Parameters.AddWithValue("@start_date", start_date);
                    cmd.Parameters.AddWithValue("@end_date", end_date);
                    cmd.Parameters.AddWithValue("@project_TypeId", Proj_TypeId);
                    cmd.Parameters.AddWithValue("@Project_cost", Proj_Cost);
                    cmd.Parameters.AddWithValue("@department_id", Dept_id);
                    cmd.Parameters.AddWithValue("@customer_id", Cust_id);
                    cmd.Parameters.AddWithValue("@manager_id", Mng_id);
                    cmd.Parameters.AddWithValue("@add_by", Add_By);
                    cmd.Parameters.AddWithValue("@status", Status);


                    SqlParameter oblogin = new SqlParameter();
                    oblogin.ParameterName = "@message";
                    oblogin.SqlDbType = SqlDbType.NVarChar;
                    oblogin.Size = 50;
                    oblogin.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(oblogin);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(oblogin.Value);
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
        public string CreateCustomer(string Cust_Code, string Cust_Name, string address, string city, string state, string country, int zip_code, int Add_By, int Status)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_CreateCustomer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customer_code", Cust_Code);
                    cmd.Parameters.AddWithValue("@customer_name", Cust_Name);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@state", state);
                    cmd.Parameters.AddWithValue("@country", country);
                    cmd.Parameters.AddWithValue("@zip_code", zip_code);
                    cmd.Parameters.AddWithValue("@add_by", Add_By);
                    cmd.Parameters.AddWithValue("@status", Status);

                    SqlParameter oblogin = new SqlParameter();
                    oblogin.ParameterName = "@message";
                    oblogin.SqlDbType = SqlDbType.NVarChar;
                    oblogin.Size = 50;
                    oblogin.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(oblogin);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(oblogin.Value);
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

        
        public DataTable HolidaySummary()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ViewHolidays", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }                   
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable DepartmentSummary()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ViewDepartment", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }                   
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable CustomerSummary()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ViewCustomer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }                    
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable EmployeeSummary()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_EmployeeDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }                   
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable ProjectSummary()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ViewProject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }                   
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable ContactSummary()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ViewContact", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }                   
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable TaskSummary()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ViewTask", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public TaskModel GetTaskDetails(int task_id)
        {
            TaskModel objTask = new TaskModel();
            try
            {
                connection();
                using (SqlCommand objCmd = new SqlCommand("Prc_ViewTaskBYID", con))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@Task_ID", task_id);
                    con.Open();
                    SqlDataReader objDr = objCmd.ExecuteReader();
                    while (objDr.Read())
                    {
                        objTask.TASK_ID = Convert.ToInt32(objDr["TASK_ID"]);
                        objTask.TaskTitle = objDr["TaskTitle"].ToString();
                        objTask.Client = objDr["Client"].ToString();
                        objTask.Project = objDr["Project"].ToString();
                        objTask.DueDate = objDr["DueDate"].ToString();
                        objTask.Description = objDr["Description"].ToString();
                        objTask.EmpDescription = objDr["EmpDescription"].ToString();                        
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTask;
        }
        public DataTable GetUserProfile(int Emp_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ViewUserProfileBYID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_ID", Emp_id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        #region Resignation Methods
        public DataTable GetEmpResignation(int Emp_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_EmpResignationBYID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_ID", Emp_id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataSet Bind_MultipleColumnDropdown()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_MultipleDataBYID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return ds;
        }       
        #endregion


    }
}