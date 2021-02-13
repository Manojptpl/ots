using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class TimeSheetEntities
    {
       public DateTime Date { set; get; }
       public int WorkType_id { set; get; }
       public int Project_id { set; get; }
       public DateTime Time_from { set; get; }
       public DateTime Time_to { set; get; }
       public string Description { set; get; }
       public int Customer_id { set; get; }
    }
    public class Remarks_Model
    {
        public string approve_remark_manager { set; get; }
        public string approve_remark_hr { set; get; }
        public string reject_remark_manager { set; get; }
        public string reject_remark_hr { set; get; }
    }
}