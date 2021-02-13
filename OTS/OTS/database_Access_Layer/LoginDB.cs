using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OTS.database_Access_Layer
{
    public class LoginDB
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
                using (SqlCommand cmd = new SqlCommand("WebValidate_User", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@Password", Password);
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
        public DataSet getMenuByPermission(int user_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetMenuByEmpId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetEmpDashBoard(int emp_id, int month, int year)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_EmployeeDashBoard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);

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

        #region Reset Password
        public string validateresetpassword(string guid, int expireTime)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("prc_validateResetPassword", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@guid", guid);
                    cmd.Parameters.AddWithValue("@expireTime", expireTime);

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
                }
                    
            }
            catch(Exception ex)
            {
                res = ex.Message;
            }          
            return res;
        }

        public string resetpassword(string new_password, int emp_id, int expireTime)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ResetPassword", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Newpassword", new_password);
                    cmd.Parameters.AddWithValue("@UserId", emp_id);
                    cmd.Parameters.AddWithValue("@expireTime", expireTime);

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
                }
            }
            catch(Exception ex)
            {
                res = ex.Message;
            }               
            return res;
        }
        #endregion

        public string changepassword(string old_password, string new_password, int emp_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_ChangePassword", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Oldpassword", old_password);
                    cmd.Parameters.AddWithValue("@Newpassword", new_password);
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);

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
                }
                    
            }
            catch(Exception ex)
            {
                res = ex.Message;
            }
            
            return res;
        }

        public string forgotpassword(string email_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ForgotPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", email_id);

                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@message";
                oblogin.SqlDbType = SqlDbType.NVarChar;
                oblogin.Size = 200;
                oblogin.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(oblogin);

                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(oblogin.Value);
                con.Close();
            }
            catch(Exception ex)
            {
                res = ex.Message;
            }
            
            return res;
        }
    }
}