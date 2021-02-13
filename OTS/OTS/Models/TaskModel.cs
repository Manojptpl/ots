using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class TaskModel
    {
        public int TASK_ID { get; set; }
        public string TaskTitle { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public string DueDate { get; set; }
        public string Description { get; set; }
        public string EmpDescription { get; set; }
        public int Status { get; set; }
        public string Show_Status { get; set; }
    }
}