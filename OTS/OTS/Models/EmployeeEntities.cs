using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace OTS.Models
{
    public class EmployeeEntities
    {
        public string Emp_code { set; get; }
        public string First_Name { set; get; }
        public string Middle_Name { set; get; }
        public string Last_Name { set; get; }
        public string Email { set; get; }
        public DateTime Dob { set; get; }
        public string show_dob { set; get; }
        public DateTime Doj { set; get; }
        public string show_doj { set; get; }
        public string Designation { set; get; }
        public string Password { set; get; }
        public int Role { set; get; }
        public int Department_Id { set; get; }
        public int Manager_Id { set; get; }
        public int Status { set; get; }
        public DateTime? Last_WDay { set; get; }
        public string contact { set; get; }
        public int Sex { set; get; }
        public int Add_By { set; get; }
        public DateTime Add_Date { set; get; }
        public int Deleted { set; get; }
        public string ShowLast_WDay { set; get; }
        public string success_msg { set; get; }
        public string error_msg { set; get; }
        public List<UserPermissionsStatus> userPermissions { set; get; }
    }
    public class Employee
    {
        public int Sr_No { set; get; }
        public int Emp_id { get; set; }
        public string Emp_code { get; set; }
        public string Emp_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Password { get; set; }
        public string Created_Date { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string Department_ID { get; set; }
        public string Department_Name { get; set; }
        public string Designation_Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public int Gender_ID { get; set; }
        public string Gender { get; set; }
        public string Manager_ID { get; set; }
        public string Manager_NAME { get; set; }
        public string Dob { get; set; }
        public string Doj { get; set; }
        public string Letter { get; set; }
        public string ROLE_NAME { get; set; }
        public string Last_Working_Day { get; set; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
        public int ROLE_ID { set; get; }
        public string Section_Name { get; set; }
        public string Location { get; set; }
        public string Grade { set; get; }
        public int Grade_id { set; get; }
        public string Manager_Name { set; get; }
        public string Employment_Type { set; get; }
        public string basicpay { set; get; }

        public string User_Code { set; get; }
        public string Profile_Img { set; get; }
    }
    public class SystemUser
    {
        public int Emp_ID { set; get; }
        public string Emp_code { set; get; }
        public string First_Name { set; get; }
        public string Middle_Name { set; get; }
        public string Last_Name { set; get; }
        public string Email { set; get; }
        public DateTime Dob { set; get; }
        public DateTime Doj { set; get; }
        public string Designation { set; get; }
        public string Password { set; get; }
        public int Role { set; get; }
        public int Department_Id { set; get; }
        public int Manager_Id { set; get; }
        public int Status { set; get; }
        public DateTime Start_Date { set; get; }
        public DateTime? End_Date { set; get; }
        public DateTime? Last_WDay { set; get; }
        public string contact { set; get; }
        public int Sex { set; get; }
        public int Add_By { set; get; }
        public DateTime Add_Date { set; get; }
        public int Deleted { set; get; }
        public string success_msg { set; get; }
        public string error_msg { set; get; }
        public List<UserPermissionsStatus> userPermissions { set; get; }
    }
}