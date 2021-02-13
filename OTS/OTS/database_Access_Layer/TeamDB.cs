using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using OTS.database_Access_Layer;
using OTS.Models;
using System.Web.Script.Serialization;

namespace OTS.database_Access_Layer
{
    public class TeamDB
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }

        #region TimeSheet
        public DataTable GetTeam_Timesheet(int month, int year, int manager_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_EmpTimeSheetDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@manager_id", manager_id);
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
        public DataSet EmpTimeSheetMng(int month, int year, int emp_id)
        {
            DataSet ds = new DataSet();
            try
            {               
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ViewSummaryForManager", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@Emp_Id", emp_id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    return ds;
                }                  
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public string TMApproved_Mng(int month, int year, int emp_id, string approval_remark, int manager_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_UTSMangerApproved", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);
                    cmd.Parameters.AddWithValue("@manager_id", manager_id);
                    cmd.Parameters.AddWithValue("@approve_remark_manager", approval_remark);
                    cmd.Parameters.AddWithValue("@approve_remark_For", "Timesheet");

                    SqlParameter oblogin1 = new SqlParameter();
                    oblogin1.ParameterName = "@message";
                    oblogin1.SqlDbType = SqlDbType.NVarChar;
                    oblogin1.Size = 100;
                    oblogin1.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(oblogin1);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(oblogin1.Value);
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
        public string TMReject_Mng(int month, int year, int emp_id, string reject_remark, int manager_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_UTSMangerReject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);
                    cmd.Parameters.AddWithValue("@manager_id", manager_id);
                    cmd.Parameters.AddWithValue("@reject_remark_manager", reject_remark);
                    cmd.Parameters.AddWithValue("@Reject_Remark_For", "Timesheet");

                    SqlParameter oblogin1 = new SqlParameter();
                    oblogin1.ParameterName = "@message";
                    oblogin1.SqlDbType = SqlDbType.NVarChar;
                    oblogin1.Size = 100;
                    oblogin1.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(oblogin1);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(oblogin1.Value);
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

        #endregion

        #region Expense
        public DataTable GetTeam_Expense(int month, int year, int manager_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_EmpExpenseDetail", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@manager_id", manager_id);
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
        public DataTable EmpExpenseMng(int month, int year, int emp_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ViewExpenseForManager", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@Emp_Id", emp_id);
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
        #endregion

        #region Attendance
        public DataTable GetTeam_Attendance(int month, int year)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_TEAM_ATTENDANCE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
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
        #endregion

        #region TeamGoal
        public DataTable GetTeam_Goal(string goal_quarter, int quarter_year, int emp_id)
        {            
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_TeamGoalDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", emp_id);
                    cmd.Parameters.AddWithValue("@GOAL_QUARTER", goal_quarter);
                    cmd.Parameters.AddWithValue("@QUARTER_YEAR", quarter_year);                    
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
        public DataSet GetTeam_Goal_History(string goal_quarter, int quarter_year, int emp_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GOAL_HISTORY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", emp_id);
                    cmd.Parameters.AddWithValue("@GOAL_QUARTER", goal_quarter);
                    cmd.Parameters.AddWithValue("@QUARTER_YEAR", quarter_year);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    return ds;
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetTeamManager_Goal_History(string goal_quarter, int quarter_year, int manager_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("[PRC_GOAL_TEAMHISTORY]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MANAGER_ID", manager_id);
                    cmd.Parameters.AddWithValue("@GOAL_QUARTER", goal_quarter);
                    cmd.Parameters.AddWithValue("@QUARTER_YEAR", quarter_year);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    return ds;
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public string update_goal(int EMP_ID, string status, string goal_quarter, int quarter_year, List<Team_Goal> goal, List<Team_Personal> personal, List<Team_Organizational> organizational)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("[PRC_GOAL_UPDATE_ENTRY]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                cmd.Parameters.AddWithValue("@STATUS", status);
                cmd.Parameters.AddWithValue("@GOAL_QUARTER", goal_quarter);
                cmd.Parameters.AddWithValue("@QUARTER_YEAR", quarter_year);                
                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@MESSAGE";
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
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion
        #region Bootstrap table 
        public string CreateTempCustomer(CustomerEntities tempCustomer)
        {

            List<CustomerEntities> custlist = new List<CustomerEntities>();
            
            custlist.Add(tempCustomer);
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dt = new DataTable();
            dt = converter.ToDataTable(custlist);
            string res = "";
            try
            {                
                connection();
                using (SqlCommand cmd = new SqlCommand("spInsertMST_CUSTOMERBYTableType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MST_CUSTOMERTableType", dt);
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
        public string CreateBulkCustomer(List<CustomerEntities> tempCustomer)
        {

            //List<CustomerEntities> custlist = new List<CustomerEntities>();

            //custlist.Add(tempCustomer);
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dt = new DataTable();
            dt = converter.ToDataTable(tempCustomer);
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("spInsertMST_CUSTOMERBYTableType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MST_CUSTOMERTableType", dt);
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
        public string CreateSimpleTempCustomer(string Cust_Code, string Cust_Name, string address, string city, string state, string country, int zip_code, int Status)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_CreateTempCustomer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customer_code", Cust_Code);
                    cmd.Parameters.AddWithValue("@customer_name", Cust_Name);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@state", state);
                    cmd.Parameters.AddWithValue("@country", country);
                    cmd.Parameters.AddWithValue("@zip_code", zip_code);                    
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
        public DataSet Get_TempCustomerDetails()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_TempCustomerDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();                    
                }

            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        #endregion
    }
}