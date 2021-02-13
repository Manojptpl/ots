using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using OTS.Models;
using System.Collections.Generic;
using System.Reflection;

namespace OTS.database_Access_Layer
{
    public class ExpenseDB
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
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

        public string Create_Expense(int EMP_ID,ExpenseEntities EXP, List<ExpenseFiles> files)
        {
            string response = "";
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            try
            {
                DataTable dt_files = new DataTable();

                if (files != null)
                {
                    dt_files = converter.ToDataTable(files);
                }
                else
                {
                    dt_files.Columns.Add("file_name");
                }

                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_InsertExpense", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", EXP.EXPENSE_ID);
                    cmd.Parameters.AddWithValue("@EXPENSE_CODE", EXP.EXPENSE_CODE);
                    cmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                    cmd.Parameters.AddWithValue("@EXPENSE_TYPE_ID", EXP.EXPENSE_TYPE_ID);
                    cmd.Parameters.AddWithValue("@EXPENSE_DATE", EXP.EXPENSE_DATE);
                    cmd.Parameters.AddWithValue("@PROJECT_ID", EXP.PROJECT_ID);          
                    cmd.Parameters.AddWithValue("@TRAVEL_FROM", EXP.TRAVEL_FROM);
                    cmd.Parameters.AddWithValue("@TRAVEL_TO", EXP.TRAVEL_TO);
                    cmd.Parameters.AddWithValue("@TRAVEL_DISTANCE", EXP.TRAVEL_DISTANCE);
                    cmd.Parameters.AddWithValue("@TRAVEL_MODE_ID", EXP.TRAVEL_MODE);
                    cmd.Parameters.AddWithValue("@EXPENSE_AMT", EXP.EXPENSE_AMOUNT);
                    cmd.Parameters.AddWithValue("@DESCRIPTION", EXP.DESCRIPTION);
                    cmd.Parameters.AddWithValue("@files_tbl", dt_files);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 250;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    response = Convert.ToString(outData.Value);
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }

        public DataTable LastSevenEntry(int emp_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ExpenseSummary", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_id", emp_id);
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

        public DataSet Expense_History(int month, int year, int Emp_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_Expense_Summary", con))
                {
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
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public string DeleteExpenseRecord(int transaction_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_DeleteExpense", con))
                {
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
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public string DeleteExpenseFile(int Expense_Id,string File_Name)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DELETE_EXP_FILES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Expense_Id);
                    cmd.Parameters.AddWithValue("@File_Name", File_Name);

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
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public DataSet GetExpenseData(int Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_EditExpense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
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

        public string Expenseapproval(int month, int year, int emp_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_UExpenseApproval", con))
                {
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
                
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public class ListtoDataTableConverter
        {
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {

                        var ab = Props[i].PropertyType.Name;
                        if (ab.ToString() == "DateTime")
                        {
                            string format = "yyyy-MM-dd HH:mm:ss";
                            values[i] = Props[i].GetValue(item, null) != null ? Convert.ToDateTime(Props[i].GetValue(item, null)).ToString(format) : null;
                        }
                        else
                        {
                            values[i] = Props[i].GetValue(item, null);
                        }

                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }

        }
    }
}