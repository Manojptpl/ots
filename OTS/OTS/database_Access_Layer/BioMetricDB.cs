using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace OTS.database_Access_Layer
{
    public class BioMetricDB
    {
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn1"].ToString();
            con = new SqlConnection(constr);
        }
        public DataTable Get_BioMetricDetails()
        {
            try
            {
                
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetBioMetricDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }                  
            }
            catch(Exception ex)
            {

            }
            return dt;
        }
        public DataTable Get_BioMetricEmp()
        {
            try
            {

                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetBioMetricEmployee", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
    }
}