using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class LeaveEncashment
    {
        public int ID { get; set; }
        public int TOTAL_LEAVES_FOR_ENCASHMENT { get; set; }
        public int LEAVE_ELIGIBLE_FOR_ENCASHMENT { get; set; }
        public int LEAVE_APPLY_FOR_ENCASHMENT { get; set; }
        public int LEAVE_ENCASHMENT_YEAR { get; set; }
        public int LEAVE_ENCASHED_AMOUNT { get; set; }
    }
}