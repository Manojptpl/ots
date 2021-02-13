using System;
using System.Configuration;
using System.Configuration.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OTS.Models
{
    public class ResignationModel
    {
        public int RESIGNATION_ID { get; set; }
        public int EMP_ID { get; set; }
        public string NAME { get; set; }
        public string REPORTING_MANAGER { get; set; }
        public string DEPARTMENT { get; set; }
        public DateTime RESIGNATION_DATE { get; set; }
        public DateTime LAST_WORKING_DATE { get; set; }
        public string Current_Date { set; get; }
        public string LastWorking_Date { set; get; }
        public int Status { set; get; }
        public string Show_Status { set; get; }
        public int Role { get; set; }
        public int MANAGER_ID { get; set; }

    }
}