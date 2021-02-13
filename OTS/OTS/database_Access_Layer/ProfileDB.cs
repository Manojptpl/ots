using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using OTS.Models;

namespace OTS.database_Access_Layer
{
    public class ProfileDB
    {
        DataTable dt = new DataTable();
        private string connection()
        {
            return ConfigurationManager.ConnectionStrings["conn"].ToString();
        }
        public Employee GetEmployeeDetails(string id)
        {
            Employee employee = new Employee();

            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetEmployeeDetail", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        employee.ROLE_NAME = dr["ROLE_NAME"].ToString();
                        employee.Emp_code = dr["EMPLOYEE_CODE"].ToString();
                        employee.Emp_Name = dr["FULL_NAME"].ToString();
                        employee.Dob = dr["DOB"].ToString();
                        employee.Doj = dr["DOJ"].ToString();
                        //employee.DepartmentID = dr["DEPARTMENT_NAME"].ToString();
                        //employee.DesignationID = dr["DESIGNATION_NAME"].ToString();
                        employee.Department_Name = dr["DEPARTMENT_NAME"].ToString();
                        employee.Designation_Name = dr["DESIGNATION_NAME"].ToString();
                        employee.Section_Name = dr["SECTION_NAME"].ToString();
                        employee.Location = dr["LOCATION"].ToString();
                        employee.Email = dr["EMAIL"].ToString();
                        employee.Gender = dr["GENDER"].ToString();
                        employee.Emp_id = Convert.ToInt32(dr["PERSON_ID"].ToString());
                        employee.Grade = dr["GRADE_NAME"].ToString();
                        employee.Grade_id = Convert.ToInt32(dr["GRADE_ID"].ToString());
                        employee.Manager_Name = dr["MANAGER_NAME"].ToString();
                        employee.Employment_Type = dr["EMPLOYMENT_TYPE"].ToString();
                        employee.basicpay = dr["BASIC_PAY"].ToString();
                        employee.User_Code = dr["USER_CODE"].ToString();
                        employee.Profile_Img = dr["PROFILE_IMG"].ToString();


                    }
                    con.Close();
                }
            }
            return employee;
        }
        public DataTable SearchDirectory(string Search_Letter)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_SearchDirectory", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@search_letter", Search_Letter);
                        con.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.Fill(dt);
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public void addprofile_image(string emp_id, string img_name)
        {
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("PRC_ADD_PROFILE_IMG", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);
                    cmd.Parameters.AddWithValue("@img_name", img_name);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
        }
    }
}