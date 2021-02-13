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
    public class GoalDB
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }
        public string Apply_Goal(Goal_Line GM,string Emp_Desc,string Mgr_Desc,int Emp_Id, string QUARTER, int YEAR)
        {
            string res = "";
            int ID = 0;
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ApplyGoal", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID",ID);
                cmd.Parameters.AddWithValue("@GOAL_ID", GM.GoalId);
                cmd.Parameters.AddWithValue("@EMP_ID", Emp_Id);
                cmd.Parameters.AddWithValue("@SELF_RATING", GM.SelfRating);
                cmd.Parameters.AddWithValue("@SELF_REMARK", GM.SelfRemark);
                cmd.Parameters.AddWithValue("@MANAGER_RATING", GM.ManagerRating);
                cmd.Parameters.AddWithValue("@MANAGER_REMARK", GM.ManagerRemark);
                cmd.Parameters.AddWithValue("@EMP_DESC", Emp_Desc);
                cmd.Parameters.AddWithValue("@MGR_DESC", Mgr_Desc);
                cmd.Parameters.AddWithValue("@QUARTER", QUARTER);
                cmd.Parameters.AddWithValue("@YEAR", YEAR);

                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@MESSAGE";
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
        public string Update_Goal(Goal_Line GM, string Emp_Desc)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateGoal", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", GM.Id);
                cmd.Parameters.AddWithValue("@SELF_RATING", GM.SelfRating);
                cmd.Parameters.AddWithValue("@SELF_REMARK", GM.SelfRemark);
                cmd.Parameters.AddWithValue("@EMP_DESC", Emp_Desc);

                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@MESSAGE";
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
        public string Approve_Goal(Goal_Line GM, string Emp_Desc, string Mgr_Desc, int Emp_Id, string QUARTER, int YEAR)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_ApplyGoal", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", GM.Id);
                cmd.Parameters.AddWithValue("@GOAL_ID", GM.GoalId);
                cmd.Parameters.AddWithValue("@EMP_ID", Emp_Id);
                cmd.Parameters.AddWithValue("@SELF_RATING", GM.SelfRating);
                cmd.Parameters.AddWithValue("@SELF_REMARK", GM.SelfRemark);
                cmd.Parameters.AddWithValue("@MANAGER_RATING", GM.ManagerRating);
                cmd.Parameters.AddWithValue("@MANAGER_REMARK", GM.ManagerRemark);
                cmd.Parameters.AddWithValue("@EMP_DESC", Emp_Desc);
                cmd.Parameters.AddWithValue("@MGR_DESC", Mgr_Desc);
                cmd.Parameters.AddWithValue("@QUARTER", QUARTER);
                cmd.Parameters.AddWithValue("@YEAR", YEAR);

                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@MESSAGE";
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
        public string Reject_Goal(int Emp_Id, string QUARTER, int YEAR)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_RejectGoal", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EMP_ID", Emp_Id);
                cmd.Parameters.AddWithValue("@QUARTER", QUARTER);
                cmd.Parameters.AddWithValue("@YEAR", YEAR);

                SqlParameter oblogin = new SqlParameter();
                oblogin.ParameterName = "@MESSAGE";
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
        public DataTable Bind_ApplyGoals(string quarter, int year, int emp_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetAppyGoals", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_id", emp_id);
                    cmd.Parameters.AddWithValue("@quarter", quarter);
                    cmd.Parameters.AddWithValue("@year", year);
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
        public DataTable GetTeamGoals(string quarter, int year, int manager_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_EmpGoals", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@quarter", quarter);
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
        public DataTable GetEmpGoals(string quarter, int year, int emp_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetEmpGoals", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@quarter", quarter);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);
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
        public DataSet GetGoalHistory(string goal_quarter, int quarter_year, int emp_id)
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

        public string submit_goal(DataTable goal, DataTable personal, DataTable organizational, int EMP_ID, string quarter, int year, string GOAL_STATUS)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_GOAL_ENTRY", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                cmd.Parameters.AddWithValue("@GOAL_QUARTER", quarter);
                cmd.Parameters.AddWithValue("@QUARTER_YEAR", year);
                cmd.Parameters.AddWithValue("@TBL_GOAL", goal);
                cmd.Parameters.AddWithValue("@TBL_PERSONAL_COMPITENCY", personal);
                cmd.Parameters.AddWithValue("@TBL_ORGANIZATIONAL_COMPITENCY", organizational);
                cmd.Parameters.AddWithValue("@GOAL_STATUS", GOAL_STATUS);

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

    }
}