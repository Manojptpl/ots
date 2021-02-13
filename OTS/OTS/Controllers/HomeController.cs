using System;
using System.Linq;
using System.Web.Mvc;
using OTS.Models;
using System.Data;
using System.Configuration;
using OTS.CommonFilters;
using OTS.database_Access_Layer;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;

namespace OTS.Controllers
{
    public class HomeController : Controller
    {
        LoginDB dblayer = new LoginDB();
        ProfileDB pdb = new ProfileDB();
        HomeModel hm = new HomeModel();

        #region Login/Logout
        public ActionResult login()
        {
            return View();
        }
        
        // Ajax Function For Login 
        [HttpPost]
        public JsonResult SubmitLoginData( string UserName, string Password)
        {
            try
            {
                DataTable res = dblayer.LoginDetails(UserName, Password);
                if (res.Rows.Count > 0)
                {
                    hm.SuccessMsg = "Successfully Login.";
                    Session["Emp_id"] = res.Rows[0]["emp_id"];
                    Session["Manager_id"] = res.Rows[0]["manager_id"];
                    Session["UserName"] = res.Rows[0]["emp_code"];
                    Session["Role_Id"] = res.Rows[0]["role"];
                    Session["Role_NAME"] = res.Rows[0]["role_name"];
                    Session["Emp_Type"] = res.Rows[0]["emp_type"];
                    Session["Emp_Name"] = res.Rows[0]["emp_name"];
                    //Session["Image_name"] = "https://ots.prudencesoftech.in/profilepictures/" + res.Rows[0]["image"];
                    Session["Profile_image"] =  res.Rows[0]["image"];
                }
                else
                {
                    hm.ErrorMsg = "Invalid Username And Password.";
                }
            }
            catch(Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }        
            return Json(hm, JsonRequestBehavior.AllowGet);
        }

        [LoginFilter]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
            Response.Expires = -1500;
            Response.CacheControl = "no-cache";
            return View();
        }
        #endregion

        #region Dashboard

        [LoginFilter]
        public ActionResult Dashboard()
        {
            try
            {
                string CurrentBaseUrl = "";
                string Scheme = Request.Url.Scheme;
                if (string.IsNullOrEmpty(Scheme))
                {
                    Scheme = "http";
                }
                if (Request.Url.Port > 0 && Request.Url.Host.Contains("localhost"))
                {
                    CurrentBaseUrl = Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;
                }
                else
                {
                    CurrentBaseUrl = Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;
                    //CurrentBaseUrl = Scheme + "://" + Request.Url.Host;
                }
                Int32 empId = Convert.ToInt32(Session["Emp_id"]);
                DataSet menuByPermission = dblayer.getMenuByPermission(empId);
                string menuString = "<li id=\"myli\"><a href=\"" + CurrentBaseUrl + "/home/dashboard\">DashBoard</a></li>";
                int rowcount = 1;

                if (menuByPermission.Tables[0].Rows.Count > 0)
                {
                    string moduleName = "";
                    DataRow[] rows = menuByPermission.Tables[0].Select("MODULE_NAME <> 'Settings'");
                    foreach (DataRow dr in rows.OrderBy(p => p["MORDER_NO"]).ThenBy(p => p["PORDER_NO"]))
                    {
                        //If module is different then insert new row in table.
                        if (moduleName == "" || moduleName != dr["MODULE_NAME"].ToString())
                        {
                            if (moduleName != "")
                            {
                                menuString += "</ul></li>";
                            }
                            menuString += "<li class=\"submenu\">";
                            menuString += "<a href=\"javascript: void(0); \"><span>" + dr["MODULE_NAME"].ToString() + "</span> <span class=\"menu-arrow\"></span></a>";
                            menuString += "<ul class=\"list-unstyled\" style=\"display:none;\">";
                            moduleName = dr["MODULE_NAME"].ToString();
                        }

                        menuString += "<li><a href=\"" + CurrentBaseUrl + "" + dr["PAGE_URL"].ToString() + "\">" + dr["PAGE_NAME"].ToString() + "</a></li>";

                        if (rowcount == rows.Count())
                        {
                            menuString += "</ul></li>";
                        }

                        rowcount++;
                    }

                    //Add submenu for settings
                    string settingMenu = "";
                    DataRow[] dsSubMenu = menuByPermission.Tables[0].Select("MODULE_NAME = 'Settings'");
                    if (dsSubMenu.Count() > 0)
                    {
                        //Add setting menu in main page
                        menuString += "<li><a href=\"" + CurrentBaseUrl + "" + dsSubMenu[0]["PAGE_URL"] + "\">Settings</a></li>";
                    }

                    foreach (DataRow dr in dsSubMenu.OrderBy(p => p["MORDER_NO"]).ThenBy(p => p["PORDER_NO"]))
                    {
                        string activeStatus = settingMenu == "" ? " class=\"active\"" : "";
                        settingMenu += "<li><a href=\"" + CurrentBaseUrl + "" + dr["PAGE_URL"].ToString() + "\">" + dr["PAGE_NAME"].ToString() + "</a></li>";
                    }

                    Session["SettingMenus"] = settingMenu;
                    Session["Menus"] = menuString;


                    return View();
                }
                else
                {
                    return RedirectToAction("Logout");
                }
            }
            catch(Exception)
            {

            }
            return View();
        }

        [HttpPost]
        public JsonResult GetEmployeeDashBoard(int Month, int Year)
        {

            var holiday = ""; var balance = ""; var approve_leave = ""; var Inprogress_leave = "";
            var employee = ""; var notification = ""; var leave_all_type = ""; var Attendence_detail = "";
            var Employment_type = ""; ;
            try
            {
                int Emp_id = Convert.ToInt32(Session["emp_id"].ToString());
                var ds = dblayer.GetEmpDashBoard(Emp_id, Month, Year);

                holiday = Utility.DataTableToJSONWithJSONNet(ds.Tables[0]);

                balance = Utility.DataTableToJSONWithJSONNet(ds.Tables[1]);
                approve_leave = Utility.DataTableToJSONWithJSONNet(ds.Tables[2]);
                Inprogress_leave = Utility.DataTableToJSONWithJSONNet(ds.Tables[3]);
                employee = Utility.DataTableToJSONWithJSONNet(ds.Tables[4]);
                notification = Utility.DataTableToJSONWithJSONNet(ds.Tables[6]);
                leave_all_type = Utility.DataTableToJSONWithJSONNet(ds.Tables[7]);
                Attendence_detail = Utility.DataTableToJSONWithJSONNet(ds.Tables[8]);
                Employment_type = Utility.DataTableToJSONWithJSONNet(ds.Tables[9]);
            }
            catch
            {
                RedirectToAction("Login", "Home");
            }
            var result = new { holiday, balance, approve_leave, Inprogress_leave, employee, notification, leave_all_type, Attendence_detail, Employment_type };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reset Password
        public ActionResult ResetPassword(string id)
        {
            try
            {
                int expireTime = Convert.ToInt32(ConfigurationManager.AppSettings["FORGOT_LINK_EXPIRE"].ToString());
                string msg = dblayer.validateresetpassword(id, expireTime);
                ViewBag.InvalidError = "";
                string[] response = msg.Split(',');

                if (response[0] == "success")
                {
                    ViewBag.userId = response[1];
                }
                else
                {
                    ViewBag.InvalidError = response[0];
                }
            }
            catch (Exception)
            {
                ViewBag.InvalidError = "Something wrong, please try again.";
            }
            return View();
        }

        [HttpPost]
        public JsonResult resetpassword(string new_pwd, int emp_id)
        {
            string SuccessMsg = "", ErrorMsg = "";
            try
            {
                int expireTime = Convert.ToInt32(ConfigurationManager.AppSettings["FORGOT_LINK_EXPIRE"].ToString());
                string msg = dblayer.resetpassword(new_pwd, emp_id, expireTime);
                if (msg == "Success")
                {
                    SuccessMsg = msg;
                }
                else
                {
                    ErrorMsg = msg;
                }
            }
            catch(Exception ex)
            {
                ErrorMsg = ex.Message;
            }                    
            return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Change Password
        [LoginFilter]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [LoginFilter]
        [HttpPost]
        public JsonResult changepassword(string old_pwd, string new_pwd)
        {
            string SuccessMsg = "", ErrorMsg = "";
            try
            {
                string msg = dblayer.changepassword(old_pwd, new_pwd, Convert.ToInt32(@Session["emp_id"].ToString()));
                string[] response = msg.Split(',');
                if (response[0] == "Success")
                {
                    string res = EmailService.Send_Email_ChangePassword(response[1], response[2]);
                    if (res == "Sent")
                    {
                        SuccessMsg = "Successfully Changed Password.";
                    }
                    else
                    {
                        ErrorMsg = "Successfully Changed Password.";
                    }
                }
                else
                {
                    ErrorMsg = msg;
                }
                return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ErrorMsg = "Error," + ex.ToString();
                return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Forgot Password
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult forgetpassword(string email_id)
        {
            string SuccessMsg = "", ErrorMsg = "";
            string msg = dblayer.forgotpassword(email_id);
            string[] response = msg.Split(',');
            if (response[0] == "success")
            {
                //response[1]; //User Name
                //response[2]; //Link url
                string res = EmailService.Send_Email_ForgetPassword(email_id, response[1], response[2].ToString());
                if (res == "Sent")
                {
                    SuccessMsg = "Reset link has been sent to your email.";
                }
                else
                {
                    ErrorMsg = res;
                }
            }
            else
            {
                ErrorMsg = response[1];
            }
            return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Profile

        [LoginFilter]
        public ActionResult profile()
        {
            Employee data = new Employee();
            try
            {
                data = pdb.GetEmployeeDetails(Session["emp_id"].ToString());
            }
            catch(Exception)
            {

            }           
            return View(data);
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            List<string> FileName = new List<string>();
            var file_name = "";
            string randomstr = RandomString(8);
            Session["RandomNumber"] = randomstr;
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;
                string[] extn;
                string extension = "";

                string BrowserDetail = Request.UserAgent;
                string[] newstr = BrowserDetail.Split(new char[] { '/' });
                BrowserDetail = newstr[newstr.Length - 2];
                newstr = BrowserDetail.Split(' ');
                // Checking for Microsoft Edge  
                if (newstr[1] == "Edge")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                    extn = fname.Split('.');
                    fname = extn[0];
                    extension = extn[1];
                }
                // Checking for Internet Explorer  
                else if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                    extn = fname.Split('.');
                    fname = extn[0];
                    extension = extn[1];
                }
                else
                {
                    fname = file.FileName;
                    extn = fname.Split('.');
                    fname = extn[0];
                    extension = extn[1];
                }
                file_name = fname.Replace(',', ' ') + "_" + randomstr + "." + extension;

                FileName.Add(file_name);
                // Get the complete folder path and store the file inside it.    
                fname = Path.Combine(Server.MapPath("~/ProfileImg/"), fname.Replace(',', ' ') + "_" + randomstr + "." + extension);
                file.SaveAs(fname);
                pdb.addprofile_image(Session["emp_id"].ToString(), file_name);

                if (Session["profile_image"].ToString() != null)
                {
                    string oldimg = Session["profile_image"].ToString();
                    if (Session["profile_image"].ToString() != "/assets/img/user.jpg")
                    {
                        DeleteFile(oldimg);
                    }
                }
                Session["profile_image"] = null;
                Session["profile_image"] = FileName[0];
            }
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }

        private static string RandomString(int length)
        {
            Random rng = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[rng.Next(0, pool.Length)];
                builder.Append(c);
            }
            return builder.ToString();
        }

        private string DeleteFile(string Filename)
        {
            string FileName = Filename;
            if (FileName != "")
            {
                try
                {  
                    FileName = Path.Combine(Server.MapPath("~/ProfileImg/"), FileName);

                    if (System.IO.File.Exists(FileName))
                    {
                        System.IO.File.Delete(FileName);
                    }
                    return ""; 
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            return "";
        }

        #endregion
    }
}