using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OTS.database_Access_Layer
{
   
    public class ReportDB
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
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
        public DataTable MonthlyStatus(int month, int year)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ViewStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
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
        public DataTable EmpExpenseMng(int month, int year, int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ViewExpenseForManager", con);
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
        public string ApprovedExpenseByHr(int month, int year, int emp_id, string approval_remark, int hr_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UExpHrApproved", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@hr_id", hr_id);
                cmd.Parameters.AddWithValue("@Approve_Remark_Hr", approval_remark);
                cmd.Parameters.AddWithValue("@Approve_Remark_For", "Expense");
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
        public string RejectExpenseByHr(int month, int year, int emp_id, string reject_remark, int hr_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UExpenseHrReject", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@hr_id", hr_id);
                cmd.Parameters.AddWithValue("@Reject_Remark_Hr", reject_remark);
                cmd.Parameters.AddWithValue("@Reject_Remark_For", "Expense");
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
        public DataSet Bind_DropDownList()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Bind", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds; 
            }
            catch(Exception ex)
            {

            }
            return ds;
        }
        public DataTable BindEmployeeList(int Dept_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_BindEmployeeList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@department_id", Dept_id);
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
        public DataTable GetAttendaceDetails(int? emp_id, DateTime? from, DateTime? to)
        {
            connection();
            SqlCommand cmd = new SqlCommand("Prc_CheckInOut_ByDate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);
            cmd.Parameters.AddWithValue("@emp_id", emp_id);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable EmployeeWiseExpense(int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmpTotalExpense", con);
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
        public DataTable ExpenseMBreakUp(int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmpExpenseBreakUp", con);
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
        public DataTable BindProjectList(int Department_id)
        {
            //DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Bind_ProjectList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dept_id", Department_id);
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
        public DataTable ProjectWiseTotalExpense(int Project_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ProjectTotalExpense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@project_id", Project_id);
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
        public DataTable ProjectTotalExpenseBreakup(int project_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ProjectTotalExpenseBreakup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@project_id", project_id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Open();
                sda.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable ProjectWiseMExpBreakup(int Proj_id, int month, int year)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ProjectWiseMExpBreakup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@project_id", Proj_id);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            { }
            return dt;
        }
        public DataTable AdminReport()
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Admin_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
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
        public DataTable MonthWiseExpenseCost(int month, int year, int Department_id)
        
{
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_AdminMonthExpenseRpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@department_id", Department_id);
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
        public DataTable EmployeeWiseExpenseReport(int project_id, int month, int year)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmployeeExpenseRpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@project_id", project_id);
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
        public DataTable ProjectWiseTotalHours(int project_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ProjectTotalHours", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@project_id", project_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex)
            { }
            return dt;
        }
        public DataTable ProjectTotalHoursBreakup(int project_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ProjectTotalHoursBreakup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@project_id", project_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception ex) { }
            return dt;
        }
        public DataSet ProjectWiseTotalCostReport(int project_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_PROJECTWISETOTALCOST", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PROJECT_ID", project_id);
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
        public DataSet EmployeeWiseReport(int project_id, int month, int year)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmployeeWiseRpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@project_id", project_id);
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
        public DataTable GetCheckInOutHistory(int emp_id, int month, int year)
        {
            connection();
            SqlCommand cmd = new SqlCommand("Prc_ChkInOutHistoryWeb", con);
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
        public DataTable GetAttendanceStatus(int month, int year)
        {
            connection();
            SqlCommand cmd = new SqlCommand("Prc_AttendenceStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable MonthWiseProjectCost(int month, int year, int Department_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_AdminMonthWiseProject", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@department_id", Department_id);
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
        public DataTable GetExpenseDetails(int emp_id,int Department_id, int project_id, string from, string to)
        {
            connection();
            try
            {
                SqlCommand cmd = new SqlCommand("PRC_EXPENSE_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EMP_ID", emp_id);
                cmd.Parameters.AddWithValue("@DEPARTMENT_ID", Department_id);
                cmd.Parameters.AddWithValue("@PROJECT_ID", project_id);
                cmd.Parameters.AddWithValue("@FROM", from);
                cmd.Parameters.AddWithValue("@TO", to);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch (Exception)
            {

                throw;
            }           
            return dt;
        }

    }


}