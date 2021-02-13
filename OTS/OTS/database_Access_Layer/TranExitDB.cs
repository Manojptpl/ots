using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration.Internal;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OTS.Models;
using OTS.database_Access_Layer;


namespace OTS.database_Access_Layer
{
    #region General Information

    #endregion
    public class TranExitDB
    {
        #region General Information
        int iExit_id = 0;
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        TranExit objTranExit = new TranExit();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }
        #endregion
        #region Methods
        public string CreateTranExit(DataTable  objTranExitModel, DataTable objDissatisfaction, DataTable objAnotherJob, DataTable objSupervisor, DataTable objOrganization, DataTable objJobAspect)
        {                       

            //DataTable objSupervisor, DataTable objOrganization, DataTable objJobAspect
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_TRAN_EXIT_INSERT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tran_ExitType", objTranExitModel);
                    cmd.Parameters.AddWithValue("@DISSATISFACTION_TYPE", objDissatisfaction);
                    cmd.Parameters.AddWithValue("@TRAN_ANOTHERJOB_TYPE", objAnotherJob);
                    cmd.Parameters.AddWithValue("@SUPERVISOR_TYPE", objSupervisor);
                    cmd.Parameters.AddWithValue("@ORGANIZATION_ASPECTS_TYPE", objOrganization);
                    cmd.Parameters.AddWithValue("@JOB_ASPECTS_TYPE", objJobAspect);
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
                    //return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public int GetExit_id(int Emp_id)
        {
            
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SELECT EXIT_ID from TRAN_EXITFORM where EMP_ID=@Emp_ID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Emp_ID", Emp_id);
                    con.Open();
                    var Exit_id = cmd.ExecuteScalar();
                    if (Exit_id !=null)
                    {
                        iExit_id = Convert.ToInt32(Exit_id);
                    }
                    con.Close();
                    //return iExit_id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return iExit_id;
        }

        #endregion
    }
}