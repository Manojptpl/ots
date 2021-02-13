using OTS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using OTS.CommonFilters;
using System.Web.Mvc;
using OTS.database_Access_Layer;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

namespace OTS.Controllers
{
    [LoginFilter]
    public class ExpenseController : Controller
    {
        HomeModel hm = new HomeModel();
        db dblayer = new db();
        ExpenseDB edb_layer = new ExpenseDB();

        public ActionResult Create_Expense()
        {                       
            try
            {
                ViewBag.emptype = Session["Role_Id"].ToString();
                DataSet ds = dblayer.Bind_DropDownList();

                List<SelectListItem> li = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Select", Value = "0" },
                };
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    li.Add(new SelectListItem { Text = @dr["project_name"].ToString(), Value = @dr["project_id"].ToString() });
                }

                ViewBag.projectlist = li;

                List<SelectListItem> lii = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Select", Value = "0" },
                };
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    lii.Add(new SelectListItem { Text = @dr["transport_name"].ToString(), Value = @dr["transport_id"].ToString() });
                }
                ViewBag.ByModule = lii;

                List<SelectListItem> expenseli = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Select", Value = "0" },
                };
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    expenseli.Add(new SelectListItem { Text = @dr["expense_type_name"].ToString(), Value = @dr["expense_type_id"].ToString() });
                }
                ViewBag.expenseType = expenseli;
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult Expense_History()
        {
            try
            {
                ViewBag.emptype = Session["Role_Id"].ToString();
                DataSet ds = dblayer.Bind_DropDownList();

                List<SelectListItem> li = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Select", Value = "0" },
                };
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    li.Add(new SelectListItem { Text = @dr["project_name"].ToString(), Value = @dr["project_id"].ToString() });
                }

                ViewBag.projectlist = li;

                List<SelectListItem> lii = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Select", Value = "0" },
                };
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    lii.Add(new SelectListItem { Text = @dr["transport_name"].ToString(), Value = @dr["transport_id"].ToString() });
                }
                ViewBag.ByModule = lii;

                List<SelectListItem> expenseli = new List<SelectListItem>()
                {
                     new SelectListItem { Text = "Select", Value = "0" },
                };
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    expenseli.Add(new SelectListItem { Text = @dr["expense_type_name"].ToString(), Value = @dr["expense_type_id"].ToString() });
                }
                ViewBag.expenseType = expenseli;
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult UploadExpense_Files()
        {
            List<string> FileName = new List<string>();
            var file_name = "";
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    string randomstr = RandomString(8);
                    Session["RandomNumber"] = randomstr;
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
                        file_name = fname + "_" + randomstr + "." + extension;

                        FileName.Add(file_name);
                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/UploadFiles/"), fname + "_" + randomstr + "." + extension);
                        file.SaveAs(fname);
                    }
                    return Json(FileName, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteFile(string Filename)
        {
            string FileName = Filename;
            if (FileName != "")
            {
                try
                {
                    // Get the complete folder path and store the file inside it.  
                    FileName = Path.Combine(Server.MapPath("~/UploadFiles/"), FileName);

                    if (System.IO.File.Exists(FileName))
                    {
                        System.IO.File.Delete(FileName);
                    }
                    return Json(Filename + " Deleted Successfully.", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteExpenseFile(string Filename,int Expense_ID)
        {
            string FileName = Filename;
            string res = "";
            if (FileName != "")
            {
                try
                {
                    // Get the complete folder path and store the file inside it.  
                    FileName = Path.Combine(Server.MapPath("~/UploadFiles/"), FileName);

                    if (System.IO.File.Exists(FileName))
                    {
                        System.IO.File.Delete(FileName);
                    }
                    res = edb_layer.DeleteExpenseFile(Expense_ID, Filename);
                    if(res=="Success")
                    {
                        res = "Success,"+Filename;
                    }
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }        

        [HttpPost]
        public ActionResult DeleteExpenseFiles()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        string[] extn;
                        string extension = "";
                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                            extn = fname.Split('.');
                            fname = extn[0];
                            extension = extn[1];
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/UploadFiles/"), fname + "_" + Session["RandomNumber"].ToString() + "." + extension);

                        if (System.IO.File.Exists(fname))
                        {
                            System.IO.File.Delete(fname);
                        }
                    }
                    return Json("File Deleted Successfully.");
                }
                catch (Exception ex)
                {
                    return Json(ex.Message,JsonRequestBehavior.AllowGet);
                }
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult ExpenseDuplicateRecord(DateTime Date, int Project_id, int Expense_id)
        {
            string res = "";
            try
            {
                res = edb_layer.GetExpenseDuplicate(Convert.ToInt32(Session["Emp_id"]), Date, Project_id, Expense_id);
                hm.SuccessMsg = res;
            }
            catch (Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }

            return Json(hm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitExpense(ExpenseModel ExpenseModel)
        {
            try
            {
                var CEXP = ExpenseModel.CreateExpense;
                var EXP_FILE = ExpenseModel.Files;

                string res = edb_layer.Create_Expense(Convert.ToInt32(Session["Emp_id"]), CEXP,EXP_FILE);
                string[] res_array = res.Split(',');
                if (res_array[0] == "Success")
                {
                    hm.SuccessMsg = res_array[1];
                }
                else
                {
                    hm.ErrorMsg = res_array[1];
                }
            }
            catch(Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }           
            return Json(hm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExpenseReport()
        {
            var jsonobj = "";
            try
            {
                var dt = edb_layer.LastSevenEntry(Convert.ToInt32(Session["Emp_id"].ToString()));
                jsonobj = DataTableToJSONWithJSONNet(dt);
            }
            catch(Exception ex)
            {
                jsonobj = ex.Message;
            }           
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExpense_History(int Month, int Year)
        {
            var history = "";
            var remarks = "";
            try
            {
                DataSet ds = edb_layer.Expense_History(Month, Year, Convert.ToInt32(Session["Emp_id"].ToString()));
                var dt = ds.Tables[0];
                var dt1 = ds.Tables[1];
                history = DataTableToJSONWithJSONNet(dt);
                remarks = DataTableToJSONWithJSONNet(dt1);
            }
            catch (Exception ex)
            {

            }
            return Json(new { history, remarks }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelExpenseRecord(int Expense_id)
        {
            try
            {
                string res = edb_layer.DeleteExpenseRecord(Expense_id);
                hm.SuccessMsg = res;
            }
            catch(Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }
            return Json(hm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExpenseFiles(int File_Id)
        {
            var jsonobj = "";
            try
            {
                DataSet ds = edb_layer.GetExpenseData(File_Id);
                var dt = ds.Tables[1];
                jsonobj = DataTableToJSONWithJSONNet(dt);
            }
            catch(Exception ex)
            {

            }            
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExpenseDetails(int Expense_Id)
        {
            var expense = "";
            var files = "";
            try
            {
                DataSet ds = edb_layer.GetExpenseData(Expense_Id);
                var dt = ds.Tables[0];
                var dt1 = ds.Tables[1];
                expense = DataTableToJSONWithJSONNet(dt);
                files = DataTableToJSONWithJSONNet(dt1);
            }
            catch (Exception ex)
            {

            }
            return Json(new { expense,files }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ExpenseForApproval(int MONTH, int YEAR)
        {
            try
            {
                string res = edb_layer.Expenseapproval(MONTH, YEAR, Convert.ToInt32(Session["Emp_id"].ToString()));
                string[] splitmsg = res.Split(',');
                if (splitmsg[0] == "Update")
                {
                    //string page_type = "Expense";
                    //string reply_mail = SendMail_UserSubmission(splitmsg[1], month, year, splitmsg[2], splitmsg[3], page_type);
                    //hm.SuccessMsg = "Expense Submited & " + reply_mail;
                    hm.SuccessMsg = "Expense Submited For Approval.";
                }
                else
                {
                    hm.ErrorMsg = res;
                }
            }
            catch (Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }
            return Json(hm, JsonRequestBehavior.AllowGet);
        }//Funtion for Expense Submited by User

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

        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
    }
}