using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class ActivityTrackerModel
    {
        public int Sr_No { set; get; }
        public int Emp_id { set; get; }
        public int Activity_id { set; get; }
        public DateTime ActivityDate { set; get; }
        public string Show_ActivityDate { set; get; }
        public int Customer_Id { set; get; }
        public string Customer_name { set; get; }
        public string Customer_Contact { set; get; }
        public string Consultant_name { set; get; }
        public int Project_Id { set; get; }
        public string Project_name { set; get; }
        public string Project_Type { set; get; }
        public int Server_Id { set; get; }
        public string Server_name { set; get; }
        public int Module_Id { set; get; }
        public string Module_name { set; get; }
        public int Application_Id { set; get; }
        public string Application_name { set; get; }
        public string Task { set; get; }
        public string Description { set; get; }
        public int Severity { set; get; }
        public string Severity_name { set; get; }
        public int Task_Type { set; get; }
        public string TaskType_name { set; get; }
        public DateTime? TaskDate { set; get; }
        public DateTime? TargetDate { set; get; }
        public DateTime? RevisedDate { set; get; }
        public DateTime? ResolutionDate { set; get; }
        public string Show_TaskDate { set; get; }
        public string Show_TargetDate { set; get; }
        public string Show_RevisedDate { set; get; }
        public string Show_ResolutionDate { set; get; }
        public int Status { set; get; }
        public string Task_status { set; get; }
        public int Responsibility { set; get; }
        public string Responsibility_name { set; get; }
        public int Dependency { set; get; }
        public string dependency_name { set; get; }
        public string Remark { set; get; }
        public string Contact_Person { set; get; }
        public int Contact_id { set; get; }
        public int TransactionMonth { set; get; }
        public int TransactionYear { set; get; }
        public string Emp_name { set; get; }
        public int Role { set; get; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
    }
}