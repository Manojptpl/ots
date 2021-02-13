using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class DepartmentEntities
    {
        public int Department_Id { set; get; }
        public string Department_Code { set; get; }
        public string Department_Name { set; get; }
        public int Division_Id { set; get; }
        public string Division_Code { set; get; }
        public string Division_Name { set; get; }
        public int Status { set; get; }
    }
}