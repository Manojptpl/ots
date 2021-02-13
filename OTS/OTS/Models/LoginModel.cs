using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OTS.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { set; get; }
        [Required]
        public string Password { set; get; }
        public string ErrorMsg { set; get; }
        public string SuccessMsg { set; get; }
    }
}