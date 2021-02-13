using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Script.Serialization;

namespace OTS.Models
{
    public static class Utility
    {
        public static void IgnoreBadCertificates()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }

        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["CBSServiceUrl"]);
            webRequest.MaximumAutomaticRedirections = 10;
            //webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.ContinueTimeout = 10;
            return webRequest;
        }

        public static HttpWebRequest CreateWebRequest2()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["CBSServiceUrl2"]);
            webRequest.MaximumAutomaticRedirections = 10;
            //webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.ContinueTimeout = 10;
            return webRequest;
        }


        #region PDF Generation

        //public static string GeneratePDF(string path,string html)
        //{
        //    //Create a byte array that will eventually hold our final PDF
        //    Byte[] bytes;

        //    //Boilerplate iTextSharp setup here
        //    //Create a stream that we can write to, in this case a MemoryStream
        //    using (var ms = new MemoryStream())
        //    {

        //        //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
        //        using (var doc = new Document())
        //        {

        //            //Create a writer that's bound to our PDF abstraction and our stream
        //            using (var writer = PdfWriter.GetInstance(doc, ms))
        //            {

        //                //Open the document for writing
        //                doc.Open();

        //                //Our sample HTML and CSS
        //                var example_html = html;
        //                //var example_css = @".headline{font-size:200%}";

        //                /**************************************************
        //                 * Example #1                                     *
        //                 *                                                *
        //                 * Use the built-in HTMLWorker to parse the HTML. *
        //                 * Only inline CSS is supported.                  *
        //                 * ************************************************/

        //                //Create a new HTMLWorker bound to our document
        //                using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
        //                {

        //                    //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
        //                    using (var sr = new StringReader(example_html))
        //                    {

        //                        //Parse the HTML
        //                        htmlWorker.Parse(sr);
        //                    }
        //                }

        //                /**************************************************
        //                 * Example #2                                     *
        //                 *                                                *
        //                 * Use the XMLWorker to parse the HTML.           *
        //                 * Only inline CSS and absolutely linked          *
        //                 * CSS is supported                               *
        //                 * ************************************************/

        //                //XMLWorker also reads from a TextReader and not directly from a string
        //                using (var srHtml = new StringReader(example_html))
        //                {

        //                    //Parse the HTML
        //                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
        //                }

        //                /**************************************************
        //                 * Example #3                                     *
        //                 *                                                *
        //                 * Use the XMLWorker to parse HTML and CSS        *
        //                 * ************************************************/

        //                //In order to read CSS as a string we need to switch to a different constructor
        //                //that takes Streams instead of TextReaders.
        //                //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
        //                //using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
        //                //{
        //                //    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
        //                //    {

        //                //        //Parse the HTML
        //                //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
        //                //    }
        //                //}



        //                    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
        //                    {

        //                        //Parse the HTML
        //                        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml);
        //                    }



        //                doc.Close();
        //            }
        //        }

        //        //After all of the PDF "stuff" above is done and closed but **before** we
        //        //close the MemoryStream, grab all of the active bytes from the stream
        //        bytes = ms.ToArray();
        //    }

        //    //Now we just need to do something with those bytes.
        //    //Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
        //    //You could also write the bytes to a database in a varbinary() column (but please don't) or you
        //    //could pass them to another function for further PDF processing.
        //    var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
        //    System.IO.File.WriteAllBytes(testFile, bytes);
        //}

        #endregion

        public static string GenerateRandomNumber(int noOfDigit)
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < noOfDigit; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }


        #region Leave Notificaiton

        public static string Leave_Apply_Notification_Send_Mail(string ToEmail , string CCEmail , string msgSub, string msgBody,string Type)
        {
            //string strFinalHtml = "";
            //StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Email_ChangePassword.html");
            //string read = null;
            //while ((read = s.ReadLine()) != null)
            //{
            //    strFinalHtml += read;
            //}
            //s.Close();

            //strFinalHtml = strFinalHtml.Replace("{name}", Name);


            DateTime cDate = DateTime.Now;



            string MailDeliver = "Sent";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                //if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BCC_FORGOT"]))
                //{
                //    foreach (string ccEmail in ConfigurationManager.AppSettings["BCC_FORGOT"].ToString().Split(','))
                //    {
                //        myMessage.Bcc.Add(new MailAddress(ccEmail));
                //    }
                //}
                myMessage.CC.Add(new MailAddress(CCEmail));

                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), Type);
                myMessage.Subject = msgSub;
                myMessage.Body = msgBody;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
            }
            catch (Exception ex)
            {

            }



            return MailDeliver;
        }
        #endregion

        public static string DataTableToJSONWithJSONNet(DataTable table)
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

    public class CommonFunctions
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }

        public bool CheckUserPermission(string pageId, string permissionId, string userId)
        {
            bool hasPermission = false;

            try
            {
                connection();
                string perId = "";
                foreach (string pid in permissionId.Split(','))
                {
                    perId = "'" + pid + "',";
                }
                string query = "SELECT * FROM MST_ROLE_PERMISSION WHERE PAGE_ID='" + pageId + "' AND STATUS='Active' and PERMISSION_ID IN (" + perId.Trim(',') + ")";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            hasPermission = true;
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
            }

            return hasPermission;
        }

        public DataTable GetEmailAddress(int STORE_ID)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_Usp_EmailAddress", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@STORE_ID", STORE_ID);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }

    public class EmailService
    {
        public static string Send_Email_ForgetPassword(string EmailId, string Name, string hexaId)
        {
            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Email_ForgotPassword.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{name}", Name);
            strFinalHtml = strFinalHtml.Replace("{resetLink}", ConfigurationManager.AppSettings["URLForService"].ToString() + "Home/ResetPassword/" + hexaId);


            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(EmailId);
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BCC_FORGOT"]))
                {
                    foreach (string ccEmail in ConfigurationManager.AppSettings["BCC_FORGOT"].ToString().Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Reset Password");
                myMessage.Subject = Name + ", here's the link to reset your password";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    NetworkCredential basicAuthenticationInfo = new NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }

        public static string Send_Email_ChangePassword(string EmailId, string Name)
        {
            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Email_ChangePassword.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{name}", Name);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(EmailId);
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BCC_FORGOT"]))
                {
                    foreach (string ccEmail in ConfigurationManager.AppSettings["BCC_FORGOT"].ToString().Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Changed Password");
                myMessage.Subject = "Your password has been changed";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    NetworkCredential basicAuthenticationInfo = new NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }

        public static string Send_Email_AttendanceSubmit(string Start_Date, string End_Date, string EmailId, string Manager_Name)
        {
            string strFinalHtml = "";

            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Attendance_Register.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{manager}", Manager_Name);
            strFinalHtml = strFinalHtml.Replace("{startdate}", Start_Date);
            strFinalHtml = strFinalHtml.Replace("{enddate}", End_Date);

            DateTime cDate = DateTime.Now;

            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(EmailId);
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BCC_FORGOT"]))
                {
                    foreach (string ccEmail in ConfigurationManager.AppSettings["BCC_FORGOT"].ToString().Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "TashiCell");
                myMessage.Subject = "Request for Approval | Attendance | " + Start_Date + " to " + End_Date;
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }

            return MailDeliver;
        }

        public static string Send_Email_AttendanceApproved(string Manager_name, string EmailId, string Start_Date, string End_Date)
        {
            string strFinalHtml = "";

            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Attendance_Register_Approve.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{manager_name}", Manager_name);
            strFinalHtml = strFinalHtml.Replace("{start_date}", Start_Date);
            strFinalHtml = strFinalHtml.Replace("{end_date}", End_Date);

            DateTime cDate = DateTime.Now;

            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(EmailId);
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BCC_FORGOT"]))
                {
                    foreach (string ccEmail in ConfigurationManager.AppSettings["BCC_FORGOT"].ToString().Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "TashiCell");
                myMessage.Subject = "Attendance Approved | " + Start_Date + " to " + End_Date;
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    NetworkCredential basicAuthenticationInfo = new NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }

            return MailDeliver;
        }

        public static string Send_Email_AttendanceRejected(string EmailId, string BCC_Email, string Requistion_No, string Store_Name, string Status)
        {
            string strFinalHtml = "";

            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Email_RequistionRM.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{no}", Requistion_No);
            //strFinalHtml = strFinalHtml.Replace("{store_name}", Store_Name);
            strFinalHtml = strFinalHtml.Replace("{status}", Status);

            DateTime cDate = DateTime.Now;

            string MailDeliver = "Sent";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(EmailId);
                if (!string.IsNullOrWhiteSpace(BCC_Email))
                {
                    foreach (string ccEmail in BCC_Email.Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "TashiCell");
                myMessage.Subject = "Requistion with Reference " + Requistion_No + " has been Rejected";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
            }
            catch (Exception ex)
            {

            }

            return MailDeliver;
        }

        public static string Send_Email_RequistionRejectRA(string EmailId, string Name, string Requistion_No, string Store_Name)
        {
            string strFinalHtml = "";

            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Email_RequistionRejection.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{name}", Name);
            strFinalHtml = strFinalHtml.Replace("{no}", Requistion_No);
            strFinalHtml = strFinalHtml.Replace("{store_name}", Store_Name);

            DateTime cDate = DateTime.Now;

            string MailDeliver = "Sent";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(EmailId);
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BCC_FORGOT"]))
                {
                    foreach (string ccEmail in ConfigurationManager.AppSettings["BCC_FORGOT"].ToString().Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "TashiCell");
                myMessage.Subject = "Requistion with Reference " + Requistion_No + " rejected";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
            }
            catch (Exception ex)
            {

            }

            return MailDeliver;
        }

        public static string Send_Email_Update(string Manager_Name, string User_Name, int Year, string Manager_EmailId, string Reply_EmailId, string Page_Type)
        {            
            string date = DateTime.Today.ToString("dd-MMM-yyyy");
            string strFinalHtml = "";

            StreamReader s;
            if (Page_Type == "AnnualAppraisal")
            {
                s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Email_AnnualAppraisalSubmit.html");
            }
            else
            {
                s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Email_AnnualAppraisalSubmit.html");
            }

            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{MANAGER_NAME}", Manager_Name);            
            strFinalHtml = strFinalHtml.Replace("{YEAR}", Convert.ToString(Year));
            strFinalHtml = strFinalHtml.Replace("{USER_NAME}", User_Name);
            strFinalHtml = strFinalHtml.Replace("{DATE}", date);
            strFinalHtml = strFinalHtml.Replace("{Logo_Path}", ConfigurationManager.AppSettings["LogoPath"].ToString());

            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(Manager_EmailId);
                myMessage.ReplyToList.Add(Reply_EmailId);
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BCC_FORGOT"]))
                {
                    foreach (string ccEmail in ConfigurationManager.AppSettings["BCC_FORGOT"].ToString().Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }

                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "TashiCell");
                myMessage.Subject = "test " ;
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    NetworkCredential basicAuthenticationInfo = new NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = true;
                    smtp.EnableSsl = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }
                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }






        #region leave approve     
        // Leave apply Notification

        public static string Send_Email_Leave_Notification(string ToEmail, string BCC_Email, string name,
       string designation, string department, string section, string apply_leave_type, string fromdate, string todate, string msg_name, string no_of_days)
        {


            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Leave_Apply.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{mgr_name}", msg_name);
            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{designation}", designation);
            strFinalHtml = strFinalHtml.Replace("{department}", department);
            strFinalHtml = strFinalHtml.Replace("{section}", section);
            strFinalHtml = strFinalHtml.Replace("{leavetype}", apply_leave_type);
            strFinalHtml = strFinalHtml.Replace("{fromdate}", fromdate);
            strFinalHtml = strFinalHtml.Replace("{todate}", todate);
            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{no_of_days}", no_of_days);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["BCC_FORGOT"]*/BCC_Email))
                {
                    foreach (string ccEmail in/* ConfigurationManager.AppSettings["BCC_FORGOT"].ToString()*/BCC_Email.Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Leave");
                myMessage.Subject = "Apply Leave";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }


        //leave approve by hierarchy
        public static string Send_Email_Leave_Approve_Notification_byhearchy(string ToEmail, string BCC_Email, string name,
        string designation, string department, string section, string apply_leave_type, string fromdate, string todate, string msg_name,string no_of_days)
        {


            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Leave_Apply.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{mgr_name}", msg_name);
            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{designation}", designation);
            strFinalHtml = strFinalHtml.Replace("{department}", department);
            strFinalHtml = strFinalHtml.Replace("{section}", section);
            strFinalHtml = strFinalHtml.Replace("{leavetype}", apply_leave_type);
            strFinalHtml = strFinalHtml.Replace("{fromdate}", fromdate);
            strFinalHtml = strFinalHtml.Replace("{todate}", todate);
            strFinalHtml = strFinalHtml.Replace("{no_of_days}", no_of_days);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["BCC_FORGOT"]*/BCC_Email))
                {
                    foreach (string ccEmail in/* ConfigurationManager.AppSettings["BCC_FORGOT"].ToString()*/BCC_Email.Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Leave");
                myMessage.Subject = "Leave Approval";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }

        // Leave Approve notification
        public static string Send_Email_Leave_Approve_Notification(string ToEmail,  string name,
                  string designation, string department, string section, string apply_leave_type, string fromdate, string todate,string approve_status,string mail_subject, string no_of_days)
        {


            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Leave_Approve.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{designation}", designation);
            strFinalHtml = strFinalHtml.Replace("{department}", department);
            strFinalHtml = strFinalHtml.Replace("{section}", section);
            strFinalHtml = strFinalHtml.Replace("{leavetype}", apply_leave_type);
            strFinalHtml = strFinalHtml.Replace("{fromdate}", fromdate);
            strFinalHtml = strFinalHtml.Replace("{todate}", todate);
            strFinalHtml = strFinalHtml.Replace("{approve_status}", approve_status);
            strFinalHtml = strFinalHtml.Replace("{no_of_days}", no_of_days);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                //if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["BCC_FORGOT"]*/BCC_Email))
                //{
                //    foreach (string ccEmail in/* ConfigurationManager.AppSettings["BCC_FORGOT"].ToString()*/BCC_Email.Split(','))
                //    {
                //        myMessage.Bcc.Add(new MailAddress(ccEmail));
                //    }
                //}
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Leave");
                myMessage.Subject = mail_subject;
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }
        //Leave approve Notification to Staf
        public static string Send_Email_Leave_Approve_Notification_ToStaff(string ToEmail, string name,
                  string designation, string department, string section, string apply_leave_type, string fromdate, string todate,  string mail_subject ,string no_of_days)
        {


            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Leave_Approve _ToStaff.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{designation}", designation);
            strFinalHtml = strFinalHtml.Replace("{department}", department);
            strFinalHtml = strFinalHtml.Replace("{section}", section);
            strFinalHtml = strFinalHtml.Replace("{leavetype}", apply_leave_type);
            strFinalHtml = strFinalHtml.Replace("{fromdate}", fromdate);
            strFinalHtml = strFinalHtml.Replace("{todate}", todate);
            strFinalHtml = strFinalHtml.Replace("{no_of_days}", no_of_days);
            // strFinalHtml = strFinalHtml.Replace("{approve_status}", approve_status);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                //if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["BCC_FORGOT"]*/BCC_Email))
                //{
                //    foreach (string ccEmail in/* ConfigurationManager.AppSettings["BCC_FORGOT"].ToString()*/BCC_Email.Split(','))
                //    {
                //        myMessage.Bcc.Add(new MailAddress(ccEmail));
                //    }
                //}
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Leave");
                myMessage.Subject = mail_subject;
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }

        //Leave Apply Withdrown
        public static string Send_Email_Leave_Withdrow_Notification(string ToEmail, string BCC_Email, string name,
      string designation, string department, string section, string apply_leave_type, string fromdate, string todate, string msg_name, string no_of_days)
        {


            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Leave_Apply_withdrow.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{mgr_name}", msg_name);
            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{designation}", designation);
            strFinalHtml = strFinalHtml.Replace("{department}", department);
            strFinalHtml = strFinalHtml.Replace("{section}", section);
            strFinalHtml = strFinalHtml.Replace("{leavetype}", apply_leave_type);
            strFinalHtml = strFinalHtml.Replace("{fromdate}", fromdate);
            strFinalHtml = strFinalHtml.Replace("{todate}", todate);
            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{no_of_days}", no_of_days);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["BCC_FORGOT"]*/BCC_Email))
                {
                    foreach (string ccEmail in/* ConfigurationManager.AppSettings["BCC_FORGOT"].ToString()*/BCC_Email.Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Leave");
                myMessage.Subject = "Withdraw Leave";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }
        //Leave Cancel Notificaiton
        public static string Send_Email_Leave_Cancel_Notification(string ToEmail, string BCC_Email, string name,
            string designation, string department, string section, string apply_leave_type, string fromdate, string todate, string msg_name, string no_of_days)
        {


            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Leave_Cancellation_request.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();
            strFinalHtml = strFinalHtml.Replace("{mgr_name}", msg_name);
            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{designation}", designation);
            strFinalHtml = strFinalHtml.Replace("{department}", department);
            strFinalHtml = strFinalHtml.Replace("{section}", section);
            strFinalHtml = strFinalHtml.Replace("{leavetype}", apply_leave_type);
            strFinalHtml = strFinalHtml.Replace("{fromdate}", fromdate);
            strFinalHtml = strFinalHtml.Replace("{todate}", todate);
            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{no_of_days}", no_of_days);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["BCC_FORGOT"]*/BCC_Email))
                {
                    foreach (string ccEmail in/* ConfigurationManager.AppSettings["BCC_FORGOT"].ToString()*/BCC_Email.Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Leave");
                myMessage.Subject = "Cancel Leave";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }

        //leave cancellation approve by hierarchy
        public static string Send_Email_Leave_Cancellation_Approve_Notifi_byhearchy(string ToEmail, string BCC_Email, string name,
        string designation, string department, string section, string apply_leave_type, string fromdate, string todate, string msg_name, string no_of_days)
        {


            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "Leave_Cancellation_request.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{mgr_name}", msg_name);
            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{designation}", designation);
            strFinalHtml = strFinalHtml.Replace("{department}", department);
            strFinalHtml = strFinalHtml.Replace("{section}", section);
            strFinalHtml = strFinalHtml.Replace("{leavetype}", apply_leave_type);
            strFinalHtml = strFinalHtml.Replace("{fromdate}", fromdate);
            strFinalHtml = strFinalHtml.Replace("{todate}", todate);
            strFinalHtml = strFinalHtml.Replace("{no_of_days}", no_of_days);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["BCC_FORGOT"]*/BCC_Email))
                {
                    foreach (string ccEmail in/* ConfigurationManager.AppSettings["BCC_FORGOT"].ToString()*/BCC_Email.Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Leave");
                myMessage.Subject = "Leave Approval";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }


        #endregion
        #region leave Encashment  
        public static string Send_Email_Leave_Encashment_To_HR(string ToEmail, string BCC_Email, string name,string emp_id)
        {


            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "leave_EncashmentToHR.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{emp_id}", emp_id);
            //strFinalHtml = strFinalHtml.Replace("{designation}", designation);
            //strFinalHtml = strFinalHtml.Replace("{department}", department);
            //strFinalHtml = strFinalHtml.Replace("{section}", section);
            //strFinalHtml = strFinalHtml.Replace("{leavetype}", apply_leave_type);
            //strFinalHtml = strFinalHtml.Replace("{fromdate}", fromdate);
            //strFinalHtml = strFinalHtml.Replace("{todate}", todate);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["BCC_FORGOT"]*/BCC_Email))
                {
                    foreach (string ccEmail in/* ConfigurationManager.AppSettings["BCC_FORGOT"].ToString()*/BCC_Email.Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Leave");
                myMessage.Subject = "Apply Leave";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }

        public static string Send_Email_Leave_Encashment_To_Employee(string ToEmail, string BCC_Email, string name, string emp_id)
        {


            string strFinalHtml = "";
            StreamReader s = File.OpenText(ConfigurationManager.AppSettings["TemplatePath"].ToString() + "leave_EncashmentToEmployee.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{name}", name);
            strFinalHtml = strFinalHtml.Replace("{emp_id}", emp_id);
            //strFinalHtml = strFinalHtml.Replace("{designation}", designation);
            //strFinalHtml = strFinalHtml.Replace("{department}", department);
            //strFinalHtml = strFinalHtml.Replace("{section}", section);
            //strFinalHtml = strFinalHtml.Replace("{leavetype}", apply_leave_type);
            //strFinalHtml = strFinalHtml.Replace("{fromdate}", fromdate);
            //strFinalHtml = strFinalHtml.Replace("{todate}", todate);
            DateTime cDate = DateTime.Now;
            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(ToEmail);
                if (!string.IsNullOrWhiteSpace(/*ConfigurationManager.AppSettings["BCC_FORGOT"]*/BCC_Email))
                {
                    foreach (string ccEmail in/* ConfigurationManager.AppSettings["BCC_FORGOT"].ToString()*/BCC_Email.Split(','))
                    {
                        myMessage.Bcc.Add(new MailAddress(ccEmail));
                    }
                }
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDER_EMAIL"].ToString(), "Leave Encashment");
                myMessage.Subject = "Encashment Approval";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                if (ConfigurationManager.AppSettings["IsDefaultCredentials"].ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER_NAME"], ConfigurationManager.AppSettings["MAIL_PASSWORD"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = basicAuthenticationInfo;

                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }
        #endregion
    }

}