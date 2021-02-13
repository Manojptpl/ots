using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using OTS.database_Access_Layer;
using OTS.Models;
using System.Web.Script.Serialization;

namespace OTS.database_Access_Layer
{
    public class ResignationDB
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        int iRole_id = 0;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }
        #region ResignationMethod
        public string CreateEmpResignation(int EMP_ID,string NAME,string REPORTING_MANAGER,string DEPARTMENT, DateTime RESIGNATION_DATE,DateTime LAST_WORKING_DATE,int Status)
        {            
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_CreateTbl_Resignation_Details", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                    cmd.Parameters.AddWithValue("@NAME", NAME);
                    cmd.Parameters.AddWithValue("@REPORTING_MANAGER", REPORTING_MANAGER);
                    cmd.Parameters.AddWithValue("@DEPARTMENT", DEPARTMENT);
                    cmd.Parameters.AddWithValue("@RESIGNATION_DATE", RESIGNATION_DATE);
                    cmd.Parameters.AddWithValue("@LAST_WORKING_DATE", LAST_WORKING_DATE);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    SqlParameter objResignation = new SqlParameter();
                    objResignation.ParameterName = "@message";
                    objResignation.SqlDbType = SqlDbType.NVarChar;
                    objResignation.Size = 50;
                    objResignation.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(objResignation);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(objResignation.Value);
                    con.Close();                                     
                }

            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string Update_Resignation(int EMP_ID, int Status,int Session_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_Tbl_Resignation_Details_Update", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@Session_id", Session_id);                   
                    SqlParameter objResignation = new SqlParameter();
                    objResignation.ParameterName = "@message";
                    objResignation.SqlDbType = SqlDbType.NVarChar;
                    objResignation.Size = 50;
                    objResignation.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(objResignation);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(objResignation.Value);
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
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

        public DataTable GetResignation_History(int EMP_ID)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_View_ResignationHistory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return dt;
                }

            }
            catch (Exception)
            {
                throw;
            }            
        }

        public ResignationModel GetResignationDetails(int EMP_ID)
        {
            ResignationModel objResignation = new ResignationModel();
            try
            {
                connection();
                using (SqlCommand objCmd = new SqlCommand("PRC_ViewResignationBYID", con))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                    con.Open();
                    SqlDataReader objDr = objCmd.ExecuteReader();
                    while (objDr.Read())
                    {
                        objResignation.RESIGNATION_ID = Convert.ToInt32(objDr["RESIGNATION_ID"]);
                        objResignation.EMP_ID = Convert.ToInt32(objDr["EMP_ID"]);
                        objResignation.NAME = objDr["NAME"].ToString();
                        objResignation.REPORTING_MANAGER = objDr["REPORTING_MANAGER"].ToString();
                        objResignation.DEPARTMENT = objDr["DEPARTMENT"].ToString();
                        objResignation.Current_Date = objDr["RESIGNATION_DATE"].ToString();
                        objResignation.LastWorking_Date = objDr["LAST_WORKING_DATE"].ToString();                        
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objResignation;
        }

        public int GetRole_id(int Emp_id)
        {

            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("select Role from Employee where emp_id = @Emp_ID", con))
                {                    
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Emp_ID", Emp_id);
                    con.Open();
                    var Role_id = cmd.ExecuteScalar();
                    if (Role_id != null)
                    {
                        iRole_id = Convert.ToInt32(Role_id);
                    }
                    con.Close();
                    return iRole_id;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return iRole_id;
        }
        #endregion
    }
}