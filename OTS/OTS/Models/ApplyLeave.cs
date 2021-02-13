using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class ApplyLeave
    {
        public int ID { get; set; }
        public string EmployeeID { get; set; }

        public string Employee_Name { get; set; }

        public int LeaveTypeID { get; set; }
        public string LeaveType_Name { get; set; }
        public string LeaveDay  { get; set; }
        public string LeaveHalfDay { get; set; }    
        public string FromDate { get; set; }
        public double LeaveBalance { get; set; }
        public string ToDate { get; set; }
        public double NumberOfDays { get; set; }
        public string Remarks { get; set; }
        public string Attachment { get; set; }
        public string CancelFromDate { get; set; }
        public string CancelToDate { get; set; }
        public string CancelLeaveHalffromDay { get; set; }
        public string CancelLeaveHalftoDay { get; set; }
        public double CancelNumberOfDays { get; set; }

        public string sandwich { get; set; }
        public string REJECT_REMARK { get; set; }
      
    }
}