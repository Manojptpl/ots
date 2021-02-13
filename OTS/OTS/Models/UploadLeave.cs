using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class UploadLeave
    {
        public string Emp_Code { set; get; }
        public int Emp_Id { set; get; }
        public string ShowApplied_On { set; get; }
        public DateTime Applied_On { set; get; }
        public string ShowLeave_Date { set; get; }
        public DateTime Leave_Date { set; get; }
        public string FirstHLeave_Type { set; get; }
        public string SecondHLeave_Type { set; get; }
        public string  Leave_Day { set; get; }
        public string ShowInTime { set; get; }
        public DateTime InTime { set; get; }
        public string ShowOutTime { set; get; }
        public DateTime OutTime { set; get; }
        public string Applicant_Remarks { set; get; }
        public string Approver_Remarks { set; get; }
        public string ShowApproved_Date { set; get; }
        public DateTime Approved_Date { set; get; }
        public string Leave_Status { set; get; }
        public string  Approved_By { set; get; }
        public string SuccessMsg { set; get; }
        public int Record_Id { set; get; }
        public string ErrorMsg { set; get; }
        public string Ots_Status { set; get; }
        public DateTime Time_from { set; get; }
        public DateTime Time_to { set; get; }
    }
}