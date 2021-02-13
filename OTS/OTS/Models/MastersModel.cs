using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace OTS.Models
{
    public class MastersModel
    {
        public int Emp_Id { set; get; }
        public string Emp_code { set; get; }
        public string First_Name { set; get; }
        public string Middle_Name { set; get; }
        public string Last_Name { set; get; }
        public string Email { set; get; }
        public DateTime Dob { set; get; }
        public DateTime Doj { set; get; }
        public string Designation { set; get; }
        public string Password { set; get; }
        public string Confirm_Password { set; get; }
        public int Emp_Type { set; get; }
        public int Role { set; get; }
        public int Department_Id { set; get; }
        public int Manager_Id { set; get; }
        public DateTime? Last_WDay { set; get; }
        public int Project_Id { set; get; }
        public string Project_Code { set; get; }
        public string Project_Name { set; get; }
        public string Project_Desc { set; get; }
        public DateTime Start_Date { set; get; }
        public DateTime End_Date { set; get; }
        public int Project_TypeId { set; get; }
        public string Project_type { set; get; }
        public decimal Project_Cost { set; get; }
        public int Customer_Id { set; get; }
        public string Customer_Code { set; get; }
        public string Customer_Name { set; get; }
        public string Address { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Country { set; get; }
        public int Zip_Code { set; get; }
        public string Department_Code { set; get; }
        public string Department_Name { set; get; }
        public int Division_Id { set; get; }
        public string Division_Code { set; get; }
        public string Division_Name { set; get; }
        public int Status { set; get; }
        public string Show_Status { set; get; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
        public int Module_id { set; get; }
        public string Module_name { set; get; }
        public int Application_id { set; get; }
        public string Application_name { set; get; }
        public int Contact_id { set; get; }
        public string Contact_name { set; get; }
        public string Show_MappingDate { set; get; }
        public string Holiday_Name { set; get; }
        public DateTime Holiday_Date { set; get; }
        public string Show_HolidayDate { set; get; }
        public int HolidayType_Id { set; get; }
        public string Holiday_Type { set; get; }
        public string Show_Role { set; get; }
        public string JOB_NANE { set; get; }
        public string strDOJ { set; get; }
        public string Emp_Name { set; get; }
        public string Manager_Name { set; get; }
        public string Current_Date { set; get; }
        public string LastWorking_Date { set; get; }
        public string CombinedData { set; get; }
        
    }
}