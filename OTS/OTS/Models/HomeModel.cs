using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OTS.Models
{
    public class HomeModel
    {
        [Required]
        [Display(Name ="Date")]
        public DateTime PostingDate { set; get; }
        [Display(Name = "Date")]
        public string ShowDate { set; get; }
        public string WorkType { set; get; }
        public int WorkType_id { set; get; }
        public string ExpenseType { set; get; }
        public string Project { set; get; }
        public DateTime Time { set; get; }
        public DateTime time_from { set; get; }
        public DateTime time_to { set; get; }
        [MaxLength(200)]
        [Display(Name ="Description")]
        public string Details { set; get; }
        public string From { set; get; }
        public string To { set; get; }
        public decimal Km { set; get; }
        public string ByMode { set; get; }
        public decimal Amount { set; get; }
        public decimal Other { set; get; }
        public decimal DA { set; get; }
        public string Mobile { set; get; }
        [Display(Name ="Total Time")]
        public string Total_Time { set; get; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
        public string User_Name { set; get; }
        [Display(Name ="No Of Projects")]
        public int No_of_projects { set; get; }
        [Display(Name = "No Of Hours")]
        public string No_of_hours { set; get; }
        public string Departments { set; get; }
        public int Department_id { set; get; }
        public int Project_id { set; get; }
        public string Employees { set; get; }
        public string Customer { set; get; }
        public int Customer_id { set; get; }
        public string Project_Name { set; get; }
        public int Transaction_id { set; get; }
        public int Id { set; get; }
        [Display(Name ="Time From")]
        public string ShowTimeFrom { set; get; }
        [Display(Name ="Time To")]
        public string ShowTimeTo { set; get; }
        public string transport_name { set; get; }
        public int transport_id { set; get; }
        public int expense_id { set; get; }
        [Display(Name ="Total Cost")]
        public string Total_cost { set; get; }
        public string Project_cost { set; get; }
        public string Expense_cost { set; get; }
        public decimal Employee_Cost { set; get; }
        public string Employee_Expense { set; get; }
        public string No_of_Emp { set; get; }
        public string Status { set; get; }
        public string submission_date { set; get; }
        public string Expsubmission_date { set; get; }
        public string approval_date { set; get; }
        public string Expapproval_date { set; get; }
        public string hr_approval { set; get; }
        public string Exphr_approval { set; get; }
        public int TransactionMonth { set; get; }
        public int TransactionYear { set; get; }
        public int Emp_id { set; get; }
        public string uploadfile { set; get; }
        public int uploadfile_id { set; get; }
        public string ExpenseStatus { set; get; }
        public string Emp_code { set; get; }
        [Display(Name ="Expense Amount")]
        public string ExpenseAmount { set; get; }
        public string Common_Cost { set; get; }
        public string PerHour_Cost { set; get; }
        public int entryCreation_mth { set; get; }
        public string Submit_Date { set; get; }
        public string Approved_Date { set; get; }
        public string Rejected_Date { set; get; }
        public string HR_Date { set; get; }
        public int Role { set; get; }
        public bool web_entry { set; get; }
        public bool android_entry { set; get; }
        public int Manager_id { set; get; }
    }
}