using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OTS.CommonFilters
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["Emp_id"] == null)
            {
                filterContext.Result = new RedirectResult("/home/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }

    }
}