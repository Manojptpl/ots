using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class ChangePasswordModel
    {
        public string OldPassword { set; get; }
        public string NewPassword { set; get; }
        public string ConfirmNewPassword { set; get; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
    }
}