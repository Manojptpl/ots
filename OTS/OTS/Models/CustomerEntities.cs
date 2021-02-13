using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    
    public class CustomerEntities
    {
        //public int Customer_Id { set; get; }
        public string Customer_Code { set; get; }
        public string Customer_Name { set; get; }
        public string Address { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Country { set; get; }
        public int Zip_Code { set; get; }
        public int Status { set; get; }
    }
}