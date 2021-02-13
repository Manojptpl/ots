using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class ProjectEntities
    {
        public int Project_Id { set; get; }
        public string Project_Code { set; get; }
        public string Project_Name { set; get; }
        public string Project_Desc { set; get; }
        public DateTime? Start_Date { set; get; }
        public DateTime? End_Date { set; get; }
        public int Project_TypeId { set; get; }
        public decimal Project_Cost { set; get; }
        public int Department_Id { set; get; }
        public int Customer_Id { set; get; }
        public int Manager_Id { set; get; }
        public int Status { set; get; }
    }
}