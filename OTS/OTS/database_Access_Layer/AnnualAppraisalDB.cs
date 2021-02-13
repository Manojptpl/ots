using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using OTS.Models;
using System.Web.Mvc;
using OTS.database_Access_Layer;

namespace OTS.database_Access_Layer
{
    public class AnnualAppraisalDB
    {
        #region General Information 
        int iAppraisal_id = 0;
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        db dblayer = new db();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }
        #endregion

        #region Methods
        public string CreateAnnualAppraisal(DataTable objResponsibilityModel, DataTable objPerformance, DataTable objRating)
        {

            //DataTable objSupervisor, DataTable objOrganization, DataTable objJobAspect
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_ANNUAL_APPRAISAL_INSERT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@APPRAISAL_RESPONSIBILITY_Type", objResponsibilityModel);
                    cmd.Parameters.AddWithValue("@APPRAISAL_PERFORMANCE_TYPE", objPerformance);
                    cmd.Parameters.AddWithValue("@APPRAISAL_RATING_TYPE", objRating);                    
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
        public DataSet GetUserProfile(int Emp_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_GOAL_BYID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_ID", Emp_id);                    
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);                   
                    da.Fill(ds);
                    con.Close();                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public int GetAppraisal_id(int Emp_id)
        {

            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("SELECT APPRAISAL_ID from MST_APPRAISAL_RESPONSIBILITY where EMP_ID=@Emp_ID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Emp_ID", Emp_id);
                    con.Open();
                    var Appraisal_id = cmd.ExecuteScalar();
                    if (Appraisal_id != null)
                    {
                        iAppraisal_id = Convert.ToInt32(Appraisal_id);
                    }
                    con.Close();
                    //return iExit_id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return iAppraisal_id;
        }
        public DataTable GetAppraisal_History(int EMP_ID)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_APPRAISAL_HISTORY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();                    
                }

            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetAppraisal_Rating(int year, int Emp_ID)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_APPRAISAL_HISTORY_Search", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@Emp_ID", Emp_ID);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();                    
                }

            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return dt;
        }
        public string Update_Appraisal(int EMP_ID, int CURRENTROLL_MGRRATING, string CURRENTROLL_MGREXP,int QUALITYWORK_MGRRATING,string QUALITYWORK_MGREXP,int ORGWORK_MGRRATING, string ORGWORK_MGREXP, int INITIATE_MGRRATING, string INITIATE_MGREXP,
            int PERSONAL_MGRRATING, string PERSONAL_MGREXP,int VSKILL_MGRRATING, string VSKILL_MGREXP, int WSKILL_MGRRATING, string WSKILL_MGREXP, int TEAM_MGRRATING, string TEAM_MGREXP, int CONFIDENTIALITY_MGRRATING, string CONFIDENTIALITY_MGREXP,
            int ATTENDANCE_MGRRATING, string ATTENDANCE_MGREXP,decimal SECTION_SECOND_TOTALRATING, decimal SECTION_ONE_OVERALL_TOTALRATING, decimal SECTION_2_OVERALL_TOTALRATING_MGR, decimal SUBTOTAL_MGR, decimal OVERALL_RATING_MGR, decimal MGR_RATING)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_AppraisalUpdate", con))
                {                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                    cmd.Parameters.AddWithValue("@CURRENTROLL_MGRRATING", CURRENTROLL_MGRRATING);
                    cmd.Parameters.AddWithValue("@CURRENTROLL_MGREXP", CURRENTROLL_MGREXP);
                    cmd.Parameters.AddWithValue("@QUALITYWORK_MGRRATING", QUALITYWORK_MGRRATING);
                    cmd.Parameters.AddWithValue("@QUALITYWORK_MGREXP", QUALITYWORK_MGREXP);
                    cmd.Parameters.AddWithValue("@ORGWORK_MGRRATING", ORGWORK_MGRRATING);
                    cmd.Parameters.AddWithValue("@ORGWORK_MGREXP", ORGWORK_MGREXP);
                    cmd.Parameters.AddWithValue("@INITIATE_MGRRATING", INITIATE_MGRRATING);
                    cmd.Parameters.AddWithValue("@INITIATE_MGREXP", INITIATE_MGREXP);
                    cmd.Parameters.AddWithValue("@PERSONAL_MGRRATING", PERSONAL_MGRRATING);
                    cmd.Parameters.AddWithValue("@PERSONAL_MGREXP", PERSONAL_MGREXP);
                    cmd.Parameters.AddWithValue("@VSKILL_MGRRATING", VSKILL_MGRRATING);
                    cmd.Parameters.AddWithValue("@VSKILL_MGREXP", VSKILL_MGREXP);
                    cmd.Parameters.AddWithValue("@WSKILL_MGRRATING", WSKILL_MGRRATING);
                    cmd.Parameters.AddWithValue("@WSKILL_MGREXP", WSKILL_MGREXP);
                    cmd.Parameters.AddWithValue("@TEAM_MGRRATING", TEAM_MGRRATING);
                    cmd.Parameters.AddWithValue("@TEAM_MGREXP", TEAM_MGREXP);
                    cmd.Parameters.AddWithValue("@CONFIDENTIALITY_MGRRATING", CONFIDENTIALITY_MGRRATING);
                    cmd.Parameters.AddWithValue("@CONFIDENTIALITY_MGREXP", CONFIDENTIALITY_MGREXP);
                    cmd.Parameters.AddWithValue("@ATTENDANCE_MGRRATING", ATTENDANCE_MGRRATING);
                    cmd.Parameters.AddWithValue("@ATTENDANCE_MGREXP", ATTENDANCE_MGREXP);
                    cmd.Parameters.AddWithValue("@SECTION_SECOND_TOTALRATING", SECTION_SECOND_TOTALRATING);
                    cmd.Parameters.AddWithValue("@SECTION_ONE_OVERALL_TOTALRATING", SECTION_ONE_OVERALL_TOTALRATING);
                    //cmd.Parameters.AddWithValue("@SECTION_2_OVERALL_TOTALRATING", SECTION_2_OVERALL_TOTALRATING);
                    cmd.Parameters.AddWithValue("@SECTION_2_OVERALL_TOTALRATING_MGR", SECTION_2_OVERALL_TOTALRATING_MGR);
                    //cmd.Parameters.AddWithValue("@SUBTOTAL", SUBTOTAL);
                    cmd.Parameters.AddWithValue("@SUBTOTAL_MGR", SUBTOTAL_MGR);
                    //cmd.Parameters.AddWithValue("@OVERALL_RATING", OVERALL_RATING);
                    cmd.Parameters.AddWithValue("@OVERALL_RATING_MGR", OVERALL_RATING_MGR);
                    cmd.Parameters.AddWithValue("@MGR_RATING", MGR_RATING);
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

        public SingleModel GetAppraisalDetails(string id)
        {
            SingleModel appraisal = new SingleModel();            
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_APPRAISAL_RESPONSIBILITY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        appraisal.NAME = dr["EMP_NAME"].ToString();
                        appraisal.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                        appraisal.DESIGNATION = dr["POSITION_TITLE"].ToString();
                        appraisal.CURRENT_DATE = dr["CUR_DATE"].ToString();                        
                        appraisal.DEPARTMENT = dr["DEPARTMENT"].ToString();
                        appraisal.GOAL_QUARTER = dr["GOAL_QUARTER"].ToString();
                        appraisal.FINAL_RATING = dr["FINAL_RATING"].ToString();
                        appraisal.MGR_RATING =Convert.ToDecimal(dr["MGR_RATING"]);

                        appraisal.CURRENTROLL_SELFRATING =Convert.ToInt32(dr["CURRENTROLL_SELFRATING"]);
                        appraisal.CURRENTROLL_SELFEXP = dr["CURRENTROLL_SELFEXP"].ToString();
                        //appraisal.CURRENTROLL_MGRRATING =Convert.ToInt32(dr["CURRENTROLL_MGRRATING"]);
                        appraisal.strCURRENTROLL_MGRRATING = dr["CURRENTROLL_MGRRATING"].ToString();
                        appraisal.CURRENTROLL_MGREXP = dr["CURRENTROLL_MGREXP"].ToString();
                        appraisal.QUALITYWORK_SELFRATING =Convert.ToInt32(dr["QUALITYWORK_SELFRATING"]);
                        appraisal.QUALITYWORK_SELFEXP = dr["QUALITYWORK_SELFEXP"].ToString();
                        //appraisal.QUALITYWORK_MGRRATING = Convert.ToInt32(dr["QUALITYWORK_MGRRATING"]);
                        appraisal.strQUALITYWORK_MGRRATING = dr["QUALITYWORK_MGRRATING"].ToString();
                        appraisal.QUALITYWORK_MGREXP = dr["QUALITYWORK_MGREXP"].ToString();
                        appraisal.ORGWORK_SELFRATING = Convert.ToInt32(dr["ORGWORK_SELFRATING"]);
                        appraisal.ORGWORK_SELFEXP = dr["ORGWORK_SELFEXP"].ToString();
                        //appraisal.ORGWORK_MGRRATING = Convert.ToInt32(dr["ORGWORK_MGRRATING"]);
                        appraisal.strORGWORK_MGRRATING = dr["ORGWORK_MGRRATING"].ToString();
                        appraisal.ORGWORK_MGREXP = dr["ORGWORK_MGREXP"].ToString();
                        appraisal.INITIATE_SELFRATING = Convert.ToInt32(dr["INITIATE_SELFRATING"]);
                        appraisal.INITIATE_SELFEXP = dr["INITIATE_SELFEXP"].ToString();
                        //appraisal.INITIATE_MGRRATING = Convert.ToInt32(dr["INITIATE_MGRRATING"]);
                        appraisal.strINITIATE_MGRRATING = dr["INITIATE_MGRRATING"].ToString();
                        appraisal.INITIATE_MGREXP = dr["INITIATE_MGREXP"].ToString();                       
                        appraisal.PERSONAL_SELFRATING = Convert.ToInt32(dr["PERSONAL_SELFRATING"]);
                        appraisal.PERSONAL_SELFEXP = dr["PERSONAL_SELFEXP"].ToString();
                        //appraisal.PERSONAL_MGRRATING = Convert.ToInt32(dr["PERSONAL_MGRRATING"]);
                        appraisal.strPERSONAL_MGRRATING = dr["PERSONAL_MGRRATING"].ToString();
                        appraisal.PERSONAL_MGREXP = dr["PERSONAL_MGREXP"].ToString();
                        appraisal.VSKILL_SELFRATING = Convert.ToInt32(dr["VSKILL_SELFRATING"]);
                        appraisal.VSKILL_SELFEXP = dr["VSKILL_SELFEXP"].ToString();
                        //appraisal.VSKILL_MGRRATING = Convert.ToInt32(dr["VSKILL_MGRRATING"]);
                        appraisal.strVSKILL_MGRRATING = dr["VSKILL_MGRRATING"].ToString();
                        appraisal.VSKILL_MGREXP = dr["VSKILL_MGREXP"].ToString(); 
                        appraisal.WSKILL_SELFRATING = Convert.ToInt32(dr["WSKILL_SELFRATING"]);
                        appraisal.WSKILL_SELFEXP = dr["WSKILL_SELFEXP"].ToString();
                        //appraisal.WSKILL_MGRRATING = Convert.ToInt32(dr["WSKILL_MGRRATING"]);
                        appraisal.strWSKILL_MGRRATING = dr["WSKILL_MGRRATING"].ToString();
                        appraisal.WSKILL_MGREXP = dr["PERSONAL_MGREXP"].ToString();
                        appraisal.TEAM_SELFRATING = Convert.ToInt32(dr["TEAM_SELFRATING"]);
                        appraisal.TEAM_SELFEXP = dr["TEAM_SELFEXP"].ToString();
                        //appraisal.TEAM_MGRRATING = Convert.ToInt32(dr["TEAM_MGRRATING"]);
                        appraisal.strTEAM_MGRRATING = dr["TEAM_MGRRATING"].ToString();
                        appraisal.TEAM_MGREXP = dr["TEAM_MGREXP"].ToString();
                        appraisal.CONFIDENTIALITY_SELFRATING = Convert.ToInt32(dr["CONFIDENTIALITY_SELFRATING"]);
                        appraisal.CONFIDENTIALITY_SELFEXP = dr["CONFIDENTIALITY_SELFEXP"].ToString();
                        //appraisal.CONFIDENTIALITY_MGRRATING = Convert.ToInt32(dr["CONFIDENTIALITY_MGRRATING"]);
                        appraisal.strCONFIDENTIALITY_MGRRATING = dr["CONFIDENTIALITY_MGRRATING"].ToString();
                        appraisal.CONFIDENTIALITY_MGREXP = dr["CONFIDENTIALITY_MGREXP"].ToString();
                        appraisal.ATTENDANCE_SELFRATING = Convert.ToInt32(dr["ATTENDANCE_SELFRATING"]);
                        appraisal.ATTENDANCE_SELFEXP = dr["ATTENDANCE_SELFEXP"].ToString();
                        //appraisal.ATTENDANCE_MGRRATING = Convert.ToInt32(dr["ATTENDANCE_MGRRATING"]);
                        appraisal.strATTENDANCE_MGRRATING = dr["ATTENDANCE_MGRRATING"].ToString();
                        appraisal.ATTENDANCE_MGREXP = dr["ATTENDANCE_MGREXP"].ToString();
                        appraisal.SECTION_SECOND_TOTALRATING = Convert.ToDecimal(dr["SECTION_SECOND_TOTALRATING"]);

                        appraisal.SECTION_ONE_OVERALL_TOTALRATING =Convert.ToDecimal(dr["SECTION_ONE_OVERALL_TOTALRATING"]);
                        appraisal.SECTION_2_OVERALL_TOTALRATING = Convert.ToDecimal(dr["SECTION_2_OVERALL_TOTALRATING"]);
                        appraisal.SUBTOTAL =Convert.ToDecimal(dr["SUBTOTAL"]);
                        appraisal.OVERALL_RATING =Convert.ToDecimal(dr["OVERALL_RATING"]);
                        appraisal.OVERALL_RATING_MGR = Convert.ToDecimal(dr["OVERALL_RATING_MGR"]);
                        appraisal.EMP_RATING = Convert.ToDecimal(dr["EMP_RATING"]);

    }
                    con.Close();
                }


            }
            catch (Exception)
            {
                throw;
            }
            return appraisal;
        }
        public string AnnualAppraisal(int year, int emp_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_AnnualAppraisal_FOR_MESSAGE", con);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                SqlParameter oblogin1 = new SqlParameter();
                oblogin1.ParameterName = "@message";
                oblogin1.SqlDbType = SqlDbType.NVarChar;
                oblogin1.Size = int.MaxValue;
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

        public List<SelectListItem> GetDropdownEmployee()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            DataTable dt = dblayer.Bind_DropDownList().Tables[14];
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["EMP_NAME"].ToString(),
                            Value = dr["EMP_ID"].ToString()
                        });
                    }
                }
                countryli.Insert(0, new SelectListItem { Text = "Select", Value = "0" });
            }
            catch (Exception)
            {
            }
            return countryli;
        }

        #endregion
    }
}