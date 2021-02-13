using OTS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using OTS.CommonFilters;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OTS.Controllers
{
    [LoginFilter]
    public class AttendanceController : Controller
    {
        database_Access_Layer.db dblayer = new database_Access_Layer.db();
        Models.CheckInOutModel chkm = new CheckInOutModel();
        DataTable dt = new DataTable();
        // GET: Attendance
        [HttpPost]
        public JsonResult GetAttendence_Status(int Month, int Year)
        {
            List<CheckInOutModel> chklist = new List<CheckInOutModel>();
            if (Session["Emp_id"] != null)
            {
                dt = dblayer.GetAttendanceStatus(Month, Year);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CheckInOutModel chk_m = new CheckInOutModel();
                        chk_m.emp_name = dr["emp_name"].ToString();
                        chk_m.emp_id = Convert.ToInt32(dr["emp_id"].ToString());
                        chk_m.Status = dr["attandance_status"].ToString();
                        chk_m.emp_code = dr["emp_code"].ToString();
                        chklist.Add(chk_m);
                    }
                    return Json(chklist, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(chklist, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult EmployeeInoutDetail(AttendanceModel am)
        {
         
            var jsonobj = "";
            try
            {
                var dt = dblayer.GetCheckInOutHistory(Convert.ToInt32(am.Month), Convert.ToInt32(am.Year), Convert.ToInt32(am.Emp_id));
                jsonobj = DataTableToJSONWithJSONNet(dt);
            }
            catch (Exception e)
            {
                jsonobj = e.Message;
            }
            return Json(dt, JsonRequestBehavior.AllowGet);
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