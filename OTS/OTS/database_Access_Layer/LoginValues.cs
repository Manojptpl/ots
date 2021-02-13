using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class LoginValues
    {
        public DateTime CurrentDate { get { return DateTime.Now; } }
        public string CurrentSession_User_Id { get { return HttpContext.Current.Session["Emp_id"].ToString(); } }
        public string createdBy { get { return HttpContext.Current.Session["UserName"].ToString(); } }
    }
}