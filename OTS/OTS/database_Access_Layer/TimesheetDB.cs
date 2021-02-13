using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OTS.database_Access_Layer
{
    public class TimesheetDB
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }
        public string GetWorkTypeDRecord(int Emp_id, DateTime TimeSheet_date, int Worktype_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_WorkTypeDuplicate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
                cmd.Parameters.AddWithValue("@timesheet_date", TimeSheet_date);
                //cmd.Parameters.AddWithValue("@worktype_id", Worktype_id);
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
            catch (Exception ex)
            {
                res = ex.Message;

            }
            return res;
        }
        public string GetDuplicateWorkType(int Emp_id, DateTime TimeSheet_date, int Worktype_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_DuplicateWorkType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
                cmd.Parameters.AddWithValue("@timesheet_date", TimeSheet_date);
                cmd.Parameters.AddWithValue("@worktype_id", Worktype_id);
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
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string SubmitTimeSheet(int Emp_id, DateTime TimeSheet_date,
           int Worktype_id, int Project_id, DateTime Time_from, DateTime Time_to, string Description, int Customer_id, int Add_By,
           DateTime Add_Date, int Status, int Deleted)
        {
            string res = "";
            try
            {

                string Transaction_code = "";
                int entryCreation_mth = 0;
                connection();
                SqlCommand cmd = new SqlCommand("Prc_InsertTimeSheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@transaction_code", Transaction_code);
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
                cmd.Parameters.AddWithValue("@timesheet_date", TimeSheet_date);
                cmd.Parameters.AddWithValue("@worktype_id", Worktype_id);
                cmd.Parameters.AddWithValue("@project_id", Project_id);
                cmd.Parameters.AddWithValue("@time_from", Time_from);
                cmd.Parameters.AddWithValue("@time_to", Time_to);
                cmd.Parameters.AddWithValue("@description", Description);
                cmd.Parameters.AddWithValue("@add_by", Add_By);
                cmd.Parameters.AddWithValue("@add_date", Add_Date);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@deleted", Deleted);
                cmd.Parameters.AddWithValue("@customer_id", Customer_id);
                cmd.Parameters.AddWithValue("@entryCreation_mth", entryCreation_mth);
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
            catch (Exception ex)
            {
                res = ex.Message;

            }
            return res;
        }

        public string UpdateTimeSheet(DateTime TimeSheet_date,
            int Worktype_id, int Project_id, DateTime Time_from, DateTime Time_to, string Description, int Customer_id, int Transaction_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateTimeSheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@transaction_id", Transaction_id);
                cmd.Parameters.AddWithValue("@timesheet_date", TimeSheet_date);
                cmd.Parameters.AddWithValue("@worktype_id", Worktype_id);
                cmd.Parameters.AddWithValue("@project_id", Project_id);
                cmd.Parameters.AddWithValue("@time_from", Time_from);
                cmd.Parameters.AddWithValue("@time_to", Time_to);
                cmd.Parameters.AddWithValue("@description", Description);
                cmd.Parameters.AddWithValue("@customer_id", Customer_id);
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
            catch (Exception ex)
            {
                res = ex.Message;

            }
            return res;
        }

        public string UpdateExpense(int Expense_id, DateTime Transaction_date, int Project_id, string From_Location, string To_Location, int Distance,
           int Transport_id, decimal Tran_Amt, string Description, int Transaction_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateExpense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@transaction_id", Transaction_id);
                cmd.Parameters.AddWithValue("@expense_id", Expense_id);
                cmd.Parameters.AddWithValue("@transaction_date", Transaction_date);
                cmd.Parameters.AddWithValue("@project_id", Project_id);
                cmd.Parameters.AddWithValue("@from_location", From_Location);
                cmd.Parameters.AddWithValue("@to_location", To_Location);
                cmd.Parameters.AddWithValue("@distance", Distance);
                cmd.Parameters.AddWithValue("@transport_id", Transport_id);
                cmd.Parameters.AddWithValue("@transaction_amount", Tran_Amt);
                cmd.Parameters.AddWithValue("@description", Description);
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
            catch (Exception ex)
            {
                res = ex.Message;

            }
            return res;
        }

        public string GetTimeSheetDuplicate(int Emp_id, DateTime TimeSheet_date, int Worktype_id, int Project_id, int Customer_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_FatchDuplicateRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
                cmd.Parameters.AddWithValue("@timesheet_date", TimeSheet_date);
                cmd.Parameters.AddWithValue("@worktype_id", Worktype_id);
                cmd.Parameters.AddWithValue("@project_id", Project_id);
                cmd.Parameters.AddWithValue("@customer_id", Customer_id);
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
            catch (Exception ex)
            {
                res = ex.Message;

            }
            return res;
        }

        public DataTable GetTimesheet(int timesheet_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetTimesheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@timesheet_id", timesheet_id);
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
        public DataTable TimeSheetMBreakUp(int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmpHoursBreakUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable MonthTimeSheetMBreakUp(int emp_id, int dept_id, int month, int year)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_MonthTimeSheetBreakup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@dept_id", dept_id);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable EmpTimeSheetMng(int month, int year, int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ViewSummaryForManager", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@Emp_Id", emp_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public string timesheetapproval(int month, int year, int emp_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UPTimeSheetApproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
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
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public string TimesheetApprovedByHr(int month, int year, int emp_id, string approval_remark, int hr_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UTSHrApproved", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@hr_id", hr_id);
                cmd.Parameters.AddWithValue("@approve_remark_hr", approval_remark);
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
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public DataTable EditTimeSheet(int Id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EditTimeSheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter();
                parm = new SqlParameter("@Id", Id);
                parm.Direction = ParameterDirection.Input;
                parm.DbType = DbType.Int32;
                cmd.Parameters.Add(parm);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public string CheckTimeSlot(int emp_id, DateTime time_from, DateTime time_to, DateTime timesheet_date)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_CheckTimeSlot", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@time_from", time_from);
                cmd.Parameters.AddWithValue("@time_to", time_to);
                cmd.Parameters.AddWithValue("@timesheet_date", timesheet_date);
                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@message";
                oblogin.SqlDbType = SqlDbType.NVarChar;
                oblogin.Size = 100;
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

        public string DeleteRecordTS(int transaction_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_DeleteTimeSheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@transaction_id", transaction_id);
                SqlParameter oblogin1 = new SqlParameter();
                oblogin1.ParameterName = "@message";
                oblogin1.SqlDbType = SqlDbType.NVarChar;
                oblogin1.Size = 50;
                oblogin1.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(oblogin1);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(oblogin1.Value);
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }
        public DataTable View_Report(int emp_id, int month, int year)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_TimeSheetReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;

            }
            catch (Exception ex)
            {

            }
            return dt;
        }
    }
}