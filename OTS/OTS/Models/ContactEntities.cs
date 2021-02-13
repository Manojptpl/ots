using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class ContactEntities
    {
        public int Contact_Id { set; get; }
        public string First_Name { set; get; }
        public string Middle_Name { set; get; }
        public string Last_Name { set; get; }
        public string Email { set; get; }
        public string Designation { set; get; }
        public int Customer_Id { set; get; }
        public int Status { set; get; }
    }
}