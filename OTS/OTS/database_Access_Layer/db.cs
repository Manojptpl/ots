using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using OTS.Models;

namespace OTS.database_Access_Layer
{
    public class db
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }
        public DataTable LoginDetails(string UserName, string Password)
        {
            try
            {                
                connection();
                SqlCommand cmd = new SqlCommand("WebValidate_User", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Password", Password);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch(Exception ex)
            {

            }
            return dt;
        }
        public DataTable LastSevenEntry(int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ExpenseSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Emp_id",emp_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch(Exception ex)
            {

            }
            return dt;
        }
 
        public string WeeklyInsertData(DataTable dtt)
        {
            string res = "";
            connection();
            SqlCommand cmd = new SqlCommand("Prc_InsertWeeklyTimeSheet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@tbltimesheet", dtt);
            SqlParameter oblogin = new SqlParameter();
            oblogin.ParameterName = "@tbltimesheet";
            oblogin.SqlDbType = SqlDbType.Structured;
            oblogin.Value = dtt;
            cmd.Parameters.Add(oblogin);
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
      
        public string ApprovedByManager(int month,int year,int emp_id,string approval_remark,int manager_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UTSMangerApproved", con);
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
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
       
        public string ApprovedExpenseByManager(int month, int year, int emp_id,string approve_remark,int manager_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UExpMangerApproved", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@manager_id", manager_id);
                cmd.Parameters.AddWithValue("@approve_remark_manager", approve_remark);
                cmd.Parameters.AddWithValue("@approve_remark_for", "Expense");
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
        
        
        public string RejectByHr(int month,int year,int emp_id,string reject_remark,int hr_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UTSHrReject", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@hr_id", hr_id);
                cmd.Parameters.AddWithValue("@Reject_Remark_Hr", reject_remark);
                cmd.Parameters.AddWithValue("@Reject_Remark_For", "Timesheet");
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
       
        public string RejectExpenseByManager(int month, int year, int emp_id,string reject_remark,int manager_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UExpMangerReject", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@manager_id", manager_id);
                cmd.Parameters.AddWithValue("@reject_remark_manager", reject_remark);
                cmd.Parameters.AddWithValue("@reject_remark_For", emp_id);
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
        public string Expenseapproval(int month, int year, int emp_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UExpenseApproval", con);
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
        public string DeleteRecordExpense(int transaction_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_DeleteExpense", con);
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
        public string ForgotPassword(string email)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ForgotPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", email);
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
        public string SetPassword(int emp_id, string NewPassword)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_SetEmployeePassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@password", NewPassword);
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
        public Tuple<int,string> Getnoofexpensefiles(int Transaction_id)
        {
            int num=0;
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetNOOfExpenFile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", Transaction_id);
                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@no_files";
                oblogin.SqlDbType = SqlDbType.Int;
                oblogin.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(oblogin);
                con.Open();
                cmd.ExecuteNonQuery();
                num = Convert.ToInt32(oblogin.Value);
                con.Close();
                return Tuple.Create(num,res);
            }
            catch (Exception ex)
            {
               res=ex.Message;
            }

            return Tuple.Create(num, res);
        }
        public DataTable Bind_ProjectList(int Customer_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_BindProjectList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customer_id", Customer_id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
                return dt;
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataSet Bind_ModuleList(int Project_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_BindModule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter();
                parm = new SqlParameter("@project_id", Project_id);
                parm.Direction = ParameterDirection.Input;
                parm.DbType = DbType.Int32;
                cmd.Parameters.Add(parm);
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
        public DataSet Bind_ApplicationList(int Project_id,int Moduel_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_BindApplication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@project_id", Project_id);
                cmd.Parameters.AddWithValue("@module_id", Moduel_id);
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

        public DataSet Bind_ProjectType(int project_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_BindTransport", con);
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
        public DataSet Bind_TransportList()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_BindTransport", con);
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
        public DataSet Bind_ActivityTrackerDDList()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Bind_ActivityDropDown", con);
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
        public DataSet BindManagersAndRoles()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("prc_BindManager", con);
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
        public DataSet Bind_EmployeeList(int manager_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Bind_EmployeeList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter();
                parm = new SqlParameter("@manager_id", manager_id);
                parm.Direction = ParameterDirection.Input;
                parm.DbType = DbType.Int32;
                cmd.Parameters.Add(parm);
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
        
        public DataTable Static_Form_Field()
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Static_Form_Fields", con);
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
        public DataTable Module_MappingSummary()
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_View_ModuleMapping", con);
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
        
        public DataTable BindEmployees(int Emp_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_BindEmployeeName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter();
                parm = new SqlParameter("@Emp_id", Emp_id);
                parm.Direction = ParameterDirection.Input;
                parm.DbType = DbType.String;
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
        public DataTable DepartmentWiseReport(int emp_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Manager_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter();
                parm = new SqlParameter("@emp_id", emp_id);
                parm.Direction = ParameterDirection.Input;
                parm.DbType = DbType.String;
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
        public DataTable ProjectWiseReport(int month,int year,int Department_id,int Emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ProjectWiseRpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@department_id", Department_id);
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
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
            catch(Exception ex)
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
            catch(Exception ex) { }
            return dt;
        }
        public DataTable AdminProjectWiseReport(int Department_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_AdminProjectWiseRpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
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
        public DataTable MonthWiseProjectCost(int month,int year,int Department_id)
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
        public DataTable MonthWiseExpenseCost(int month,int year,int Department_id)
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
        public DataTable ProjectWiseExpense(int Department_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ProjectExpenseRpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter();
                parm = new SqlParameter("@department_id", Department_id);
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
        public DataSet EmployeeWiseReport(int project_id,int month,int year)
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
            catch(Exception ex)
            {

            }
            return ds;
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
        public DataSet EditExpense(int Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EditExpense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter();
                parm = new SqlParameter("@Id", Id);
                parm.Direction = ParameterDirection.Input;
                parm.DbType = DbType.Int32;
                cmd.Parameters.Add(parm);
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
        public DataTable EditActivity_Tracker(int Id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EditActivity_Tracker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", Id);
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
        public DataTable BindTSGrid(int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ViewData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Emp_id",emp_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
                
            }
            catch(Exception ex)
            {

            }
            return dt;
        }
        //public DataTable CopyTimeSheetData()
        //{
        //    try
        //    {
        //        connection();
        //        SqlCommand cmd = new SqlCommand("Prc_LastRecordCopy", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return dt;
        //}
        //public DataTable EmpDetailsForMngRpt(int month,int year,int Emp_id)
        //{
        //    try
        //    {
        //        connection();
        //        SqlCommand cmd = new SqlCommand("Prc_EmpTimeSheetDetail", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlParameter parm = new SqlParameter();
        //        parm = new SqlParameter("@emp_id", Emp_id);
        //        parm.Direction = ParameterDirection.Input;
        //        parm.DbType = DbType.Int32;
        //        cmd.Parameters.Add(parm);
        //        con.Open();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        con.Close();
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return dt;
        //}
        
        public DataTable EmpDeatilsCheckInOut(int month, int year, int manager_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmpCheckInOutDetails", con);
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
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable EmpActivityDetails(int Emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmpActivityDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@manager_id", Emp_id);
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
        
        public DataSet DateWiseSummaryRpt(int month,int year,int Emp_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ViewSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);              
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);              
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
        public DataTable DateWiseActivitySummary(int Emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ViewActivitySummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@month", month);
                //cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
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
        public DataTable ActivitySummary(int Emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ActivitySummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
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
        public DataTable DateWiseLeaveSummary(int month,int year)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_LeaveSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month",month);
                cmd.Parameters.AddWithValue("@year",year);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        public DataTable DtWithEmpLeaveSummary(int month,int year,int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmpLeaveSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable EmployeeWiseHours(int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EmpTotalHours", con);
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
        public DataSet DateWiseExpenseSummary(int month, int year, int Emp_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Expense_Summary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
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
        public string CreateOtsEntries(int record_id,int Emp_id, DateTime TimeSheet_date,
            int Worktype_id, int Project_id, DateTime Time_from, DateTime Time_to, string Description, int Customer_id, int Add_By,
            DateTime Add_Date, int Status, int Deleted,int entry_Method)
        {
            string res = "";
            try
            {
                string Transaction_code = "";
                connection();
                SqlCommand cmd = new SqlCommand("Prc_InsOtsLeaveEntries", con);
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
                cmd.Parameters.AddWithValue("@entryCreation_mth", entry_Method);
                cmd.Parameters.AddWithValue("@record_id", record_id);
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

        public string SubmitExpense(int Emp_id,int Expense_id, DateTime Transaction_date,int Project_id,string From_Location,string To_Location, int Distance,
            int Transport_id,decimal Tran_Amt, string Description, int Add_By,DateTime Add_Date, int Status, int Deleted)
        {
            string res = "";
            try
            {
                string Transaction_code = "";
                connection();
                SqlCommand cmd = new SqlCommand("Prc_InsertExpense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@transaction_code", Transaction_code);
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
                cmd.Parameters.AddWithValue("@expense_id", Expense_id);
                cmd.Parameters.AddWithValue("@transaction_date", Transaction_date);
                cmd.Parameters.AddWithValue("@project_id", Project_id);
                cmd.Parameters.AddWithValue("@from_location", From_Location);
                cmd.Parameters.AddWithValue("@to_location", To_Location);
                cmd.Parameters.AddWithValue("@distance", Distance);
                cmd.Parameters.AddWithValue("@transport_id", Transport_id);
                cmd.Parameters.AddWithValue("@transaction_amount", Tran_Amt);
                //cmd.Parameters.AddWithValue("@mobile_amt", Mobile_Amt);
                //cmd.Parameters.AddWithValue("@da", DA);
                //cmd.Parameters.AddWithValue("@other_charges", Other_charges);
                cmd.Parameters.AddWithValue("@description", Description);
                cmd.Parameters.AddWithValue("@add_by", Add_By);
                cmd.Parameters.AddWithValue("@add_date", Add_Date);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@deleted", Deleted);
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
        
       
       
        public string GetExpenseDuplicate(int Emp_id, DateTime Expense_date, int Project_id, int Expense_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ExpenseDuplicateRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", Emp_id);
                cmd.Parameters.AddWithValue("@expense_date", Expense_date);
                cmd.Parameters.AddWithValue("@project_id", Project_id);
                cmd.Parameters.AddWithValue("@expense_id", Expense_id);
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
       
        public DataTable GetExpenseFiles(int expemp_id,int month,int year)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetExpenseFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@Emp_Id", expemp_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                //return dt;
            }
            catch (Exception)
            {
                throw;            
            }
            return dt;
        }
        public string InsertLeaveEntries(string Emp_Code,DateTime Applied_On,DateTime Leave_Date,string FirstHLeave_Type,string  SecondHLeave_Type,string Leave_Day,
           string Applicant_Remarks,string Approver_Remarks,DateTime Approved_Date,string Leave_Status,string Approved_By,int Add_By,DateTime Add_Date,int Status,int Deleted,int Ots_Status)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_InsertLeaveEntries", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Emp_Code", Emp_Code);
                cmd.Parameters.AddWithValue("@Applied_On", Applied_On);
                cmd.Parameters.AddWithValue("@Leave_Date", Leave_Date);
                cmd.Parameters.AddWithValue("@FirstHLeave_Type", FirstHLeave_Type);
                cmd.Parameters.AddWithValue("@SecondHLeave_Type", SecondHLeave_Type);
                cmd.Parameters.AddWithValue("@Leave_Day", Leave_Day);
                cmd.Parameters.AddWithValue("@Applicant_Remarks", Applicant_Remarks);
                cmd.Parameters.AddWithValue("@Approver_Remarks", Approver_Remarks);
                cmd.Parameters.AddWithValue("@Approved_Date", Approved_Date);
                cmd.Parameters.AddWithValue("@Leave_Status", Leave_Status);
                cmd.Parameters.AddWithValue("@Approved_By", Approved_By);
                cmd.Parameters.AddWithValue("@Add_By", Add_By);
                cmd.Parameters.AddWithValue("@Add_Date", Add_Date);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@Deleted", Deleted);
                cmd.Parameters.AddWithValue("@Ots_Status", Ots_Status);
                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@Message";
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
            catch(Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string DeleteLeave(DateTime Leave_Date,int Record_id,int Emp_Id,int Del_ByEmp)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_DeleteLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Leave_Date", Leave_Date);
                cmd.Parameters.AddWithValue("@record_id", Record_id);
                cmd.Parameters.AddWithValue("@emp_id", Emp_Id);
                cmd.Parameters.AddWithValue("@del_by", Del_ByEmp);
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
            catch(Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        
        
        public string CreateEmployee(string emp_code,string first_name,string middle_name,string last_name,string email, DateTime dob,DateTime doj,string designation,string password,int emp_type,int role,int dept_id,int mng_id,int Add_By,DateTime Add_Date,int Status,int Deleted,DateTime? last_wday, string contact)
        {
            string res = "";
            try
            {
                Nullable<int> department_id=dept_id;
                if (dept_id==0)
                {
                    department_id = null;
                }
                connection();
                SqlCommand cmd = new SqlCommand("Prc_CreateEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_code", emp_code);
                cmd.Parameters.AddWithValue("@first_name", first_name);
                cmd.Parameters.AddWithValue("@middle_name", middle_name);
                cmd.Parameters.AddWithValue("@last_name", last_name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@date_of_birth", dob);
                cmd.Parameters.AddWithValue("@date_of_joining", doj);
                cmd.Parameters.AddWithValue("@designation", designation);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@emp_type", emp_type);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@department_id", department_id);
                cmd.Parameters.AddWithValue("@manager_id", mng_id);
                cmd.Parameters.AddWithValue("@add_by", Add_By);
                cmd.Parameters.AddWithValue("@add_date", Add_Date);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@deleted", Deleted);
                cmd.Parameters.AddWithValue("@last_wday", last_wday);
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
            catch(Exception ex) {
                res = ex.Message;
            }
            return res;
        }
        public string CreateDepartment(string dept_code,string dept_name,string div_code,string div_name, int Add_By, DateTime Add_Date, int Status, int Deleted)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_CreateDepartment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@department_code", dept_code);
                cmd.Parameters.AddWithValue("@department_name", dept_name);
                cmd.Parameters.AddWithValue("@division_code", div_code);
                cmd.Parameters.AddWithValue("@division_name", div_name);
                cmd.Parameters.AddWithValue("@add_by", Add_By);
                cmd.Parameters.AddWithValue("@add_date", Add_Date);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@deleted", Deleted);
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
        public string CreateContact(string first_name, string middle_name, string last_name, string email,string designation,int customer_id, int Add_By, DateTime Add_Date, int Status, int Deleted)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_CreateContact", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@first_name", first_name);
                cmd.Parameters.AddWithValue("@middle_name", middle_name);
                cmd.Parameters.AddWithValue("@last_name", last_name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@designation", designation);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@add_by", Add_By);
                cmd.Parameters.AddWithValue("@add_date", Add_Date);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@deleted", Deleted);
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
       
        //public DataTable BindCustomer(int customer_id)
        //{
        //    try
        //    {
        //        connection();
        //        SqlCommand cmd = new SqlCommand("Prc_EditCustomer", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@customer_id", customer_id);
        //        con.Open();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        con.Close();
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return dt;
        //}
        public string UpdateCustomer(int Cust_Id, string Cust_Name, string address, string city, string state, string country, int zip_code,int Edit_By,int status)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UCustomerData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customer_id", Cust_Id);
                cmd.Parameters.AddWithValue("@customer_name", Cust_Name);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Parameters.AddWithValue("@zip_code", zip_code);
                cmd.Parameters.AddWithValue("@edit_by", Edit_By);
                cmd.Parameters.AddWithValue("@status", status);
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
        public string UpdateEmployee(int emp_id, string first_name, string middle_name, string last_name,string email, DateTime dob, DateTime doj, string designation, int emp_type, int role, int dept_id, int mng_id, int Edit_By, int Status, DateTime? last_wday, string contact)
        {
            string res = "";
            try
            {
                Nullable<int> department_id = dept_id;
                if (dept_id == 0)
                {
                    department_id = null;
                }
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UEmployeeData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@first_name", first_name);
                cmd.Parameters.AddWithValue("@middle_name", middle_name);
                cmd.Parameters.AddWithValue("@last_name", last_name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@date_of_birth", dob);
                cmd.Parameters.AddWithValue("@date_of_joining", doj);
                cmd.Parameters.AddWithValue("@designation", designation);
                cmd.Parameters.AddWithValue("@emp_type", emp_type);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@department_id", department_id);
                cmd.Parameters.AddWithValue("@manager_id", mng_id);
                cmd.Parameters.AddWithValue("@edit_by", Edit_By);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@last_wday", last_wday);
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
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string UpdateProject(int project_id,string Proj_Name, string Proj_Desc, DateTime? start_date, DateTime? end_date, int Proj_TypeId, decimal Proj_Cost, int Dept_id,int Cust_id,int Mng_id,int Edit_By,int Status)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UProjectData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@project_id", project_id);
                cmd.Parameters.AddWithValue("@project_name", Proj_Name);
                cmd.Parameters.AddWithValue("@project_desc", Proj_Desc);
                cmd.Parameters.AddWithValue("@start_date", start_date);
                cmd.Parameters.AddWithValue("@end_date", end_date);
                cmd.Parameters.AddWithValue("@project_TypeId", Proj_TypeId);
                cmd.Parameters.AddWithValue("@Project_cost", Proj_Cost);
                cmd.Parameters.AddWithValue("@department_id", Dept_id);
                cmd.Parameters.AddWithValue("@customer_id", Cust_id);
                cmd.Parameters.AddWithValue("@manager_id", Mng_id);
                cmd.Parameters.AddWithValue("@edit_by", Edit_By);
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
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string UpdateDepartment(int dept_id,string dept_name,string div_code,string div_name, int Edit_By, int Status)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UDepartmentData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@department_id", dept_id);
                cmd.Parameters.AddWithValue("@department_name", dept_name);
                cmd.Parameters.AddWithValue("@division_code", div_code);
                cmd.Parameters.AddWithValue("@division_name", div_name);
                cmd.Parameters.AddWithValue("@edit_by", Edit_By);
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
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string UpdateContact(int Contact_Id, string first_name,string middle_name,string last_name, string email, string designation,int customer_id, int Edit_By, int Status)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UContactData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@contact_id", Contact_Id);
                cmd.Parameters.AddWithValue("@first_name", first_name);
                cmd.Parameters.AddWithValue("@middle_name", middle_name);
                cmd.Parameters.AddWithValue("@last_name", last_name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@designation", designation);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@edit_by", Edit_By);
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
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        //public DataSet GetCustomer(string prefix)

        //{
        //    connection();
        //    SqlCommand com = new SqlCommand("select * from customer where customer_name like '%'+@prefix+'%'", con);
        //    com.Parameters.AddWithValue("@prefix", prefix);
        //    DataSet ds = new DataSet();
        //    con.Open();
        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    da.Fill(ds);
        //    con.Close();
        //    return ds;
        //}
        public string SubmitActivityTracker(DateTime ActivityDate,int Customer_Id,int Project_Id,int Server_Id,int Module_Id,int Application_Id,string Task,string Description,int Severity,int Task_Type,int Activity_Status,DateTime? TargetDate,
                       DateTime? RevisedDate,DateTime? ResolutionDate,int Responsibility,int Dependency,string Remark,int Contact_id, int Add_By,DateTime Add_Date,int Status,int Deleted)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_InsertActivityTracker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@activity_date", ActivityDate);
                cmd.Parameters.AddWithValue("@customer_id", Customer_Id);
                cmd.Parameters.AddWithValue("@project_id", Project_Id);
                cmd.Parameters.AddWithValue("@server_id", Server_Id);
                cmd.Parameters.AddWithValue("@module_id", Module_Id);
                cmd.Parameters.AddWithValue("@application_id", Application_Id);
                cmd.Parameters.AddWithValue("@task", Task);
                cmd.Parameters.AddWithValue("@description", Description);
                cmd.Parameters.AddWithValue("@severity_id", Severity);
                cmd.Parameters.AddWithValue("@task_type", Task_Type);
                cmd.Parameters.AddWithValue("@activity_status", Activity_Status);
                cmd.Parameters.AddWithValue("@target_date", TargetDate);
                cmd.Parameters.AddWithValue("@revised_date", RevisedDate);
                cmd.Parameters.AddWithValue("@resolution_date", ResolutionDate);
                cmd.Parameters.AddWithValue("@responsibility_id", Responsibility);
                cmd.Parameters.AddWithValue("@dependency_id", Dependency);
                cmd.Parameters.AddWithValue("@remark", Remark);
                cmd.Parameters.AddWithValue("@contact_id", Contact_id);
                cmd.Parameters.AddWithValue("@add_by", Add_By);
                cmd.Parameters.AddWithValue("@add_date", Add_Date);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@deleted", Deleted);
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
            catch(Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string UpdateActivityTracker(int activity_id, string Task, string Description, int Severity, int Task_Type, int Activity_Status,
                       DateTime? RevisedDate, DateTime? ResolutionDate, int Responsibility, int Dependency, string Remark, int edit_By)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UActivityTracker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", activity_id);
                cmd.Parameters.AddWithValue("@task", Task);
                cmd.Parameters.AddWithValue("@description", Description);
                cmd.Parameters.AddWithValue("@severity_id", Severity);
                cmd.Parameters.AddWithValue("@task_type", Task_Type);
                cmd.Parameters.AddWithValue("@activity_status", Activity_Status);
                cmd.Parameters.AddWithValue("@revised_date", RevisedDate);
                cmd.Parameters.AddWithValue("@resolution_date", ResolutionDate);
                cmd.Parameters.AddWithValue("@responsibility_id", Responsibility);
                cmd.Parameters.AddWithValue("@dependency_id", Dependency);
                cmd.Parameters.AddWithValue("@remark", Remark);
                cmd.Parameters.AddWithValue("@edit_by", edit_By);
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
        public string CreateMapping(int project_id,int module_id,int application_id,int project_type_id,int contact_id, int Add_By, DateTime Add_Date, int Status, int Deleted)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_InsertMappingData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@project_id", project_id);
                cmd.Parameters.AddWithValue("@module_id", module_id);
                cmd.Parameters.AddWithValue("@application_id", application_id);
                cmd.Parameters.AddWithValue("@project_type_id", project_type_id);
                cmd.Parameters.AddWithValue("@contact_id", contact_id);
                cmd.Parameters.AddWithValue("@add_by", Add_By);
                cmd.Parameters.AddWithValue("@add_date", Add_Date);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@deleted", Deleted);
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
            catch(Exception ex)
            {

            }
            return res;
        }
        // add by krishna 8/2018
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
        public DataTable GetAttendaceDetails(int? emp_id,DateTime? from,DateTime? to)
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
        public string Check_In(CheckInOutEntities chk)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Insert_Check_INOut", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@chk_in_id", chk.chk_in_id);

                cmd.Parameters.AddWithValue("@chk_in_lat", chk.chk_in_lat);
                cmd.Parameters.AddWithValue("@chk_in_long", chk.chk_in_long);
                cmd.Parameters.AddWithValue("@chk_in_date", chk.chk_in_date);

                cmd.Parameters.AddWithValue("@chk_out_lat", chk.chk_out_lat);
                cmd.Parameters.AddWithValue("@chk_out_long", chk.chk_out_long);
                cmd.Parameters.AddWithValue("@chk_out_date", chk.chk_out_date);

                cmd.Parameters.AddWithValue("@emp_id",chk.emp_id);
                
                
                cmd.Parameters.AddWithValue("@worktype_id",chk.worktype_id);
                cmd.Parameters.AddWithValue("@customer_id",chk.customer_id);
                cmd.Parameters.AddWithValue("@project_id",chk.project_id);
                cmd.Parameters.AddWithValue("@remark",chk.Remark);
                cmd.Parameters.AddWithValue("@location", chk.in_location==null?"":"");

                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@message";
                oblogin.SqlDbType = SqlDbType.NVarChar;
                oblogin.Size = 50;
                oblogin.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(oblogin);
                con.Open();
                cmd.ExecuteNonQuery();
                res = oblogin.Value.ToString();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string Check_Out(CheckInOutEntities chk)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Insert_Check_INOut", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@chk_in_id", chk.chk_in_id);

                cmd.Parameters.AddWithValue("@chk_in_lat", chk.chk_in_lat);
                cmd.Parameters.AddWithValue("@chk_in_long", chk.chk_in_long);
                cmd.Parameters.AddWithValue("@chk_in_date", chk.chk_in_date);

                cmd.Parameters.AddWithValue("@chk_out_lat", chk.chk_out_lat);
                cmd.Parameters.AddWithValue("@chk_out_long", chk.chk_out_long);
                cmd.Parameters.AddWithValue("@chk_out_date", chk.chk_out_date);

                cmd.Parameters.AddWithValue("@emp_id", chk.emp_id);


                cmd.Parameters.AddWithValue("@worktype_id", chk.worktype_id);
                cmd.Parameters.AddWithValue("@customer_id", chk.customer_id);
                cmd.Parameters.AddWithValue("@project_id", chk.project_id);
                cmd.Parameters.AddWithValue("@remark", chk.Remark);
                cmd.Parameters.AddWithValue("@location", chk.in_location == null ? "" : "");

                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@message";
                oblogin.SqlDbType = SqlDbType.NVarChar;
                oblogin.Size = 50;
                oblogin.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(oblogin);
                con.Open();
                cmd.ExecuteNonQuery();
                res = oblogin.Value.ToString();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        
        public DataTable GetEmployeeDetails(int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EditEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                //return dt;
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable GetCheckIns(DateTime checkIn_date,int emp_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetCheckIns", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@transaction_date", checkIn_date);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch(Exception ex)
            {

            }
            return dt;
        }
        public DataTable GetGoal()
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GETGOAL", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }
            }
            catch(Exception ex)
            {

            }
            return dt;
        }

        //Ankit Rajput For Genrate Email Logs
        public static string GenrateEmailLogs(string email_id, string email_type, int Emp_Id, string message, string status)
        {
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_UspEmailLog", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EMAIL_ID", email_id);
                        cmd.Parameters.AddWithValue("@EMAIL_TYPE", email_type);
                        cmd.Parameters.AddWithValue("@EMP_ID", Emp_Id);
                        cmd.Parameters.AddWithValue("@MESSAGE", message);
                        cmd.Parameters.AddWithValue("@STATUS", status);

                        SqlParameter objdept = new SqlParameter();
                        objdept.ParameterName = "@out_message";
                        objdept.SqlDbType = SqlDbType.NVarChar;
                        objdept.Size = 50;
                        objdept.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(objdept);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(objdept.Value);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        //masters 
    
        
        
    }

}