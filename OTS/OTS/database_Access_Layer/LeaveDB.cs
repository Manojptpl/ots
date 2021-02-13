using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using OTS.Models;
namespace OTS.database_Access_Layer
{
    public class LeaveDB
    {
        DataTable dt = new DataTable();
        db dblayer = new db();
      
        private string connection()
        {
            return ConfigurationManager.ConnectionStrings["conn"].ToString();
        }

        //LEAVE TYPE 
        public List<LeaveType> GetLeaveType()
        {
            List<LeaveType> leavetypeli = new List<LeaveType>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetLeavType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        LeaveType leavetype = new LeaveType();
                        leavetype.ID = Convert.ToInt32(dr["LEAVE_TYPE_ID"].ToString());
                        leavetype.leavetypecode = dr["LEAVE_TYPE_CODE"].ToString();
                        leavetype.leavetypename = dr["LEAVE_TYPE_NAME"].ToString();
                        leavetype.status = dr["status"].ToString();
                        leavetypeli.Add(leavetype);
                    }
                    con.Close();
                }
            }
            return leavetypeli;
        }
        //LEAVE POLICY

        public List<SelectListItem> Leavetypelist()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_Dropdown_Create_LeavType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["LEAVE_TYPE_NAME"].ToString(),
                            Value = dr["LEAVE_TYPE_ID"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            countryli.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            return countryli;
        }


        public List<SelectListItem> Leavetypelist_by_emp_gender(string emp_id)
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("PRC_GETEMPLOYEE_LEAVETYPE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Employee_id", emp_id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["LEAVE_TYPE_NAME"].ToString(),
                            Value = dr["LEAVE_TYPE_ID"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            countryli.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            return countryli;
        }

        public List<SelectListItem> Leavetypelistcreatepolicy()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_Dropdown_Create_LeavType_create_policy", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["LEAVE_TYPE_NAME"].ToString(),
                            Value = dr["LEAVE_TYPE_ID"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            countryli.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            return countryli;
        }
        public List<SelectListItem> Leavetypelistedit()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetLeavType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["LEAVE_TYPE_NAME"].ToString(),
                            Value = dr["LEAVE_TYPE_NAME"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            countryli.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            return countryli;
        }
        //APPLY LEAVE
        public string addapplyleave(ApplyLeave applyleave, string Createdby)
        {
            string res = "";
          
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_ApplyLeaves", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@employeeid", applyleave.EmployeeID);
                        cmd.Parameters.AddWithValue("@leavetypeid", applyleave.LeaveTypeID);
                        cmd.Parameters.AddWithValue("@leavebalance", applyleave.LeaveBalance);
                        cmd.Parameters.AddWithValue("@leavefromdate", applyleave.FromDate);
                        cmd.Parameters.AddWithValue("@leavetodate", applyleave.ToDate);
                        cmd.Parameters.AddWithValue("@tohalfday", applyleave.LeaveDay);
                        cmd.Parameters.AddWithValue("@fromhalfday", applyleave.LeaveHalfDay);
                        cmd.Parameters.AddWithValue("@numberofdays", applyleave.NumberOfDays);
                        cmd.Parameters.AddWithValue("@description", applyleave.Remarks);
                        cmd.Parameters.AddWithValue("@attachment", applyleave.Attachment == null ? "" : applyleave.Attachment);
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
                        cmd.Parameters.AddWithValue("@sandwich", applyleave.sandwich);
                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();  
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }

        //CREATE LEAEV POLICY
        public string createLeavepolicy(LeavePolicy leavepolicy, LeavePlan leaveplan, YearEndProcessing yearendprocessing, string Createdby)
        {
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dt = new DataTable();
            if (leaveplan.tblCreateRule != null)
            {
                dt = converter.ToDataTable(leaveplan.tblCreateRule);
            }
            else
            { dt = null; }
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_CreatePolicy", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@employeeid", "0");
                        cmd.Parameters.AddWithValue("@LeaveTypeID", leavepolicy.LeaveTypeID);
                        cmd.Parameters.AddWithValue("@PolicyName", leavepolicy.PolicyName);
                        cmd.Parameters.AddWithValue("@PolicyDescription", leavepolicy.PolicyDescription == null ? "" : leavepolicy.PolicyDescription);
                        cmd.Parameters.AddWithValue("@StartDate", leavepolicy.StartDate == "" ? null : leavepolicy.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", leavepolicy.EndDate == "" ? null : leavepolicy.EndDate);
                        cmd.Parameters.AddWithValue("@Status", leavepolicy.Status);
                        cmd.Parameters.AddWithValue("@IsInformationOnly", leavepolicy.IsInformationOnly);
                        cmd.Parameters.AddWithValue("@AttachmentRequired", leaveplan.AttachmentRequired);
                        cmd.Parameters.AddWithValue("@Gender", leaveplan.Gender);
                        cmd.Parameters.AddWithValue("@LeaveYear", leaveplan.LeaveYear);
                        cmd.Parameters.AddWithValue("@CreditFrequency", leaveplan.CreditFrequency);
                        cmd.Parameters.AddWithValue("@Credit", leaveplan.Credit);
                        cmd.Parameters.AddWithValue("@IncludePublicHolidays", leaveplan.IncludePublicHolidays);
                        cmd.Parameters.AddWithValue("@IncludeWeekends", leaveplan.IncludeWeekends);
                        cmd.Parameters.AddWithValue("@CanbeclubbedwithEL", leaveplan.CanbeclubbedwithEL);
                        cmd.Parameters.AddWithValue("@CanbeclubbedwithCL", leaveplan.CanbeclubbedwithCL);
                        cmd.Parameters.AddWithValue("@Canbehalfday", leaveplan.Canbehalfday);
                        cmd.Parameters.AddWithValue("@IS_NOTICE_PERIOD_P", leaveplan.Notice_Period_P);
                        cmd.Parameters.AddWithValue("@IS_PROBATION_PERIOD_P", leaveplan.Probation_Period_P);
                        cmd.Parameters.AddWithValue("@CONFIRMED_P", leaveplan.Confirmed_P);
                        cmd.Parameters.AddWithValue("@IS_Contract_PERIOD_P", leaveplan.Contract_Period_P);
                        cmd.Parameters.AddWithValue("@AllowCarryover_CarryoverLimit", yearendprocessing.AllowCarryover_CarryoverLimit);
                        cmd.Parameters.AddWithValue("@PayatYearend_MinBal", yearendprocessing.PayatYearend_MinBal);
                        cmd.Parameters.AddWithValue("@PayatYearend_MaxEncashment", yearendprocessing.PayatYearend_MaxEncashment);
                        cmd.Parameters.AddWithValue("@EligibleForEncashment_Limit", yearendprocessing.EligibleForEncashment_Limit);
                        cmd.Parameters.AddWithValue("@EligibleForEncashment_MinBal", yearendprocessing.EligibleForEncashment_MinBal);
                        cmd.Parameters.AddWithValue("@CarryForwardtoEL_CFLimit", yearendprocessing.CarryForwardtoEL_CFLimit);
                        cmd.Parameters.AddWithValue("@tblCreateRule", dt);
                        cmd.Parameters.AddWithValue("@createdby", Createdby);

                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string updateLeavepolicy(List<CreateRule> tblCreateRule, string leavepolicyid, string enddate, string status, string Createdby)
        {
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dt = new DataTable();
            if (tblCreateRule != null)
            {
                dt = converter.ToDataTable(tblCreateRule);
            }
            else
            { dt = null; }
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_UpdatePolicy", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@leavepolicyid", leavepolicyid);
                        cmd.Parameters.AddWithValue("@enddate", enddate == "" ? null : enddate);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@tblCreateRule", dt);
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }
        public DataTable LeavePolicyList()
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_BindList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds.Tables[6];
        }

        public DataSet GetLeavePolicyDetail(string ID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetLeavePolicy", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }

        //CANCLE LEAVE
        public DataSet GetLeaveCancelHistory(string emp_id)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                // using (SqlCommand cmd = new SqlCommand("Prc_GetLeaveCancellation", con))     
                using (SqlCommand cmd = new SqlCommand("Prc_GetLeaveHistory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@persion_id", emp_id);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }

        public List<ApplyLeave> EditApplyLeave(string id)
        {
            List<ApplyLeave> applyleaveli = new List<ApplyLeave>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_EditLeaveCancellation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ApplyLeave applyleave = new ApplyLeave();
                        applyleave.EmployeeID = dr["EMPLOYEE_ID"].ToString();
                        applyleave.LeaveTypeID = Convert.ToInt32(dr["LEAVETYPE_ID"].ToString());
                        applyleave.FromDate = dr["LEAVE_FROM_DATE"].ToString();
                        applyleave.ToDate = dr["LEAVE_TO_DATE"].ToString();
                        applyleave.LeaveDay = dr["LEAVE_DAY"].ToString();
                        applyleave.LeaveHalfDay = dr["LEAVE_HALF_DAY"].ToString();
                        applyleave.NumberOfDays = Convert.ToDouble(dr["NUMBER_OF_DAYS"].ToString());
                        applyleave.LeaveBalance = Convert.ToDouble(dr["LEAVE_BALANCE"].ToString());
                        applyleave.Remarks = dr["DESCRIPTION"].ToString();
                        applyleave.Attachment = dr["IS_ATTACHMENT"].ToString();
                        applyleaveli.Add(applyleave);
                    }
                    con.Close();
                }
            }
            return applyleaveli;
        }
        public string ApplyCancelLeave(ApplyLeave applycanceleave, string Createdby)
        {
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_ApplyCancelLeave", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@leaveapplyid", applycanceleave.ID);
                        cmd.Parameters.AddWithValue("@employeeid", applycanceleave.EmployeeID);
                        cmd.Parameters.AddWithValue("@leavetypeid", applycanceleave.LeaveTypeID);
                        cmd.Parameters.AddWithValue("@leavefromdate", applycanceleave.FromDate);
                        cmd.Parameters.AddWithValue("@leavetodate", applycanceleave.ToDate);
                        cmd.Parameters.AddWithValue("@leaveday", applycanceleave.LeaveDay);
                        cmd.Parameters.AddWithValue("@leavehalfday", applycanceleave.LeaveHalfDay);
                        cmd.Parameters.AddWithValue("@numberofdays", applycanceleave.NumberOfDays);
                        cmd.Parameters.AddWithValue("@cancelfromdate", applycanceleave.CancelFromDate);
                        cmd.Parameters.AddWithValue("@canceltodate", applycanceleave.CancelToDate);
                        cmd.Parameters.AddWithValue("@cancelnumberofdays", applycanceleave.CancelNumberOfDays);
                        cmd.Parameters.AddWithValue("@cancelleavehalffromday", applycanceleave.CancelLeaveHalffromDay);
                        cmd.Parameters.AddWithValue("@cancelleavehalftoday", applycanceleave.CancelLeaveHalftoDay);
                        cmd.Parameters.AddWithValue("@description", applycanceleave.Remarks);
                        cmd.Parameters.AddWithValue("@attachment", applycanceleave.Attachment);
                        cmd.Parameters.AddWithValue("@createdby", Createdby);

                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }
        public string Cancelleavewithdrawn(string applyleaveid, string status, string Createdby)
        {
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_CancelLeavewithdraw", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@leaveapplyid", applyleaveid);
                        cmd.Parameters.AddWithValue("@status", status);                        
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }
        //Leave Approval
        public DataSet GetLeaveapprovalhistory( int Employee_ID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetLeaveApproval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMPLOYEEID", Employee_ID);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }
        //Leave Approved list
        public DataSet GetLeaveapproved_list_history(int Employee_ID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("PRC_GetLeaveApprovalDone", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", Employee_ID);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }
        public DataSet GetLeave_encashment_done_approval(int Employee_ID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("PRC_GetEncashmentApprovalDone", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMP_ID", Employee_ID);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }
        public DataSet GetLeave_encashment_approval(int Employee_ID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("prc_GetEncashmentApproval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMPLOYEEID", Employee_ID);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }
        public string LeaveApprove(List<LeaveApprove> LApp, String Status, string Createdby)
        {
            string res = "";
            try
            {
                ListtoDataTableConverter converter = new ListtoDataTableConverter();
                DataTable dt = new DataTable();

                if (LApp != null)
                {
                    dt = converter.ToDataTable(LApp);
                }
                else
                { dt = null; }
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_LeaveApprove", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LeaveApprove", dt);
                        cmd.Parameters.AddWithValue("@status", Status);
                        //cmd.Parameters.AddWithValue("@remarks", LApp.remarks);                       
                        cmd.Parameters.AddWithValue("@createdby", Createdby);

                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }

        //Leave History
        public DataSet GetLeaveHistory(string emp_id)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetLeaveHistory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@persion_id", emp_id);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }
        public DataSet GetLeaveHistoryall(string emp_id)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetLeaveHistory_All", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@persion_id", emp_id);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }
        public List<ApplyLeave> ViewApplyLeaveDetails(string id, string flag)
        {
            List<ApplyLeave> applyleaveli = new List<ApplyLeave>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_ViewLeaveHistory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@flag", flag);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ApplyLeave applyleave = new ApplyLeave();
                        applyleave.EmployeeID = dr["EMPLOYEE_ID"].ToString();
                        applyleave.Employee_Name = dr["EMPLOYEE_NAME"].ToString();
                        applyleave.LeaveType_Name = dr["LEAVE_TYPE_NAME"].ToString();
                        applyleave.LeaveTypeID = Convert.ToInt32(dr["LEAVETYPE_ID"].ToString());
                        applyleave.FromDate = dr["LEAVE_FROM_DATE"].ToString();
                        applyleave.ToDate = dr["LEAVE_TO_DATE"].ToString();
                        applyleave.LeaveDay = dr["LEAVE_DAY"].ToString();
                        applyleave.LeaveHalfDay = dr["LEAVE_HALF_DAY"].ToString();
                        applyleave.NumberOfDays = Convert.ToDouble(dr["NUMBER_OF_DAYS"].ToString());
                        applyleave.Remarks = dr["DESCRIPTION"].ToString();
                        applyleave.Attachment = dr["IS_ATTACHMENT"].ToString();
                        applyleave.CancelFromDate = dr["LEAVE_CANCEL_FROM_DATE"].ToString();
                        applyleave.CancelToDate = dr["LEAVE_CANCEL_TO_DATE"].ToString();
                        applyleave.CancelLeaveHalffromDay = dr["CANCEL_LEAVE_FROMHALF_DAY"].ToString();
                        applyleave.CancelLeaveHalftoDay = dr["CANCEL_LEAVE_TOHALF_DAY"].ToString();
                        applyleave.CancelNumberOfDays = Convert.ToDouble(dr["CANCEL_NUMBER_OF_DAYS"].ToString());
                        applyleave.LeaveBalance = Convert.ToDouble(dr["LEAVE_BALANCE"].ToString());
                        applyleave.REJECT_REMARK = dr["REJECT_REMARK"].ToString();
                        applyleaveli.Add(applyleave);
                    }
                    con.Close();
                }
            }
            return applyleaveli;
        }

        public string UpdateApplyLeave(string id, string leaveday, string leavehalfday, string fromdate, string todate, string noofdays, string Createdby , int sandwich)
        {
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_UApplyLeaves", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@leaveday", leaveday);
                        cmd.Parameters.AddWithValue("@leavehalfday", leavehalfday);
                        cmd.Parameters.AddWithValue("@fromdate", fromdate);
                        cmd.Parameters.AddWithValue("@todate", todate);
                        cmd.Parameters.AddWithValue("@noofdays", noofdays);
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
                        cmd.Parameters.AddWithValue("@sandwich", sandwich);
                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }

        public string InsertLeaveEncashment(LeaveEncashment lc, string Createdby)
        {
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_Leave_Encashment", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@total_leaves_for_encashment", lc.TOTAL_LEAVES_FOR_ENCASHMENT);
                        cmd.Parameters.AddWithValue("@leave_eligible_for_encashment", lc.LEAVE_ELIGIBLE_FOR_ENCASHMENT);
                        cmd.Parameters.AddWithValue("@leave_apply_for_encashment", lc.LEAVE_APPLY_FOR_ENCASHMENT);
                        cmd.Parameters.AddWithValue("@leave_encashed_amount", lc.LEAVE_ENCASHED_AMOUNT);
                        cmd.Parameters.AddWithValue("@createdby", Createdby);

                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        public string LeaveEncashmentApprove(List<LeaveApprove> LApp, String Status, string Createdby)
        {
            string res = "";
            try
            {
                ListtoDataTableConverter converter = new ListtoDataTableConverter();
                DataTable dt = new DataTable();

                if (LApp != null)
                {
                    dt = converter.ToDataTable(LApp);
                }
                else
                { dt = null; }
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_LeaveEncashmentApprove", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LeaveApprove", dt);
                        cmd.Parameters.AddWithValue("@status", Status);
                        //cmd.Parameters.AddWithValue("@remarks", LApp.remarks);                       
                        cmd.Parameters.AddWithValue("@createdby", Createdby);

                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }
        public string GetLeaveEncashment( string employeeid)
        { 
            string res = ""; 
            string[] val = new string[5];
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_Get_Leave_Encashment", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; 
                        cmd.Parameters.AddWithValue("@employeeid", employeeid); 
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            val[0] = dr["TOTAL_EL_BAL"].ToString();
                            val[1] = dr["ELIGIBLE_FOR_ENCASHMENT_MIN_BALANCE"].ToString();
                            val[2] = dr["ELIGIBLE_FOR_ENCASHMENT_ENCASHMENT_LIMIT"].ToString();
                            val[3] = dr["BASIC_PAY"].ToString();
                            val[4] = dr["IsAvail"].ToString();
                        }
                        con.Close();
                    }
                }
                return res=String.Join(",",val);
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }
        public string updatecancelapplyleave(string id, string cancelfrom, string cancelfromleavehalfday, string cancelto, string canceltoleavehalfday, string noofdays, string Remarks, string Createdby)
        {
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_UCancelApplyLeaves", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@cancelfrom", cancelfrom);
                        cmd.Parameters.AddWithValue("@cancelfromleavehalfday", cancelfromleavehalfday);
                        cmd.Parameters.AddWithValue("@cancelto", cancelto);
                        cmd.Parameters.AddWithValue("@canceltoleavehalfday", canceltoleavehalfday);
                        cmd.Parameters.AddWithValue("@noofdays", noofdays);
                        cmd.Parameters.AddWithValue("@Remarks", Remarks);
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
                        SqlParameter oblogin = new SqlParameter();
                        oblogin.ParameterName = "@message";
                        oblogin.SqlDbType = SqlDbType.NVarChar;
                        oblogin.Size = 50;
                        oblogin.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oblogin);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(oblogin.Value);
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }
        public List<SelectListItem> GetDropdownEmployee()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            DataTable dt = dblayer.Bind_DropDownList().Tables[10];
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["TEMP_NAME"].ToString(),
                            Value = dr["EMP_ID"].ToString()

                        });
                    }
                }

                countryli.Insert(0, new SelectListItem { Text = "Select", Value = "0" });
            }
            catch (Exception)
            {
            }
            return countryli;
        }
        public List<SelectListItem> GetDropdownGrade()
        {
            List<SelectListItem> gradeli = new List<SelectListItem>();
            try
            {

                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_GetdropdownGrade", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            gradeli.Add(new SelectListItem
                            {
                                Text = dr["GRADE_NAME"].ToString(),
                                Value = dr["GRADE_ID"].ToString()
                            });

                        }
                        con.Close();
                    }
                }
                // gradeli.Insert(0, new SelectListItem { Text = "Select", Value = "0" });
                //    gradeli.Insert(gradeli.Count, new SelectListItem { Text = "ALL", Value = "ALL" });
            }
            catch (Exception)
            {
            }
            return gradeli;
        }
        public string GetEmpLeaveBalontype(int empid, int leaveid)
        {
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_LeaveBalance", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Employee_ID", empid);
                        cmd.Parameters.AddWithValue("@Leave_Type_ID", leaveid);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            res = dr["LeaveBalance"].ToString();
                        }
                        con.Close();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        public string GetEmpLeaveBalontype_sandwich(int empid, int leaveid,string fromdate, string todate)
        {
            // check is sandwich for el only
            string res = "";
            string[] strarr = new string[2];
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("PRC_GET_STATUS_FOR_LEAVE", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployeeID", empid);
                        cmd.Parameters.AddWithValue("@Leave_TYPE_ID", leaveid);
                        cmd.Parameters.AddWithValue("@FromDate", fromdate);
                        cmd.Parameters.AddWithValue("@ToDate", todate);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            strarr[0] = dr["LStatus"].ToString();
                            strarr[1]= dr["fdate"].ToString();
                        }
                        con.Close();
                    }
                }
                return res= String.Join(",",strarr);
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        public string GetEmpLeaveBaldayontype(int empid, int leaveid)
        {
            string res = "";
            string[] arrstr = new string[6];
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("PRC_GETLEAVETYPE_INFO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Employee_ID", empid);
                        cmd.Parameters.AddWithValue("@LeaveID", leaveid);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            arrstr[0] = dr["PERSON_ID"].ToString();
                            arrstr[1] = dr["LEAVE_TYPE_NAME"].ToString();
                            arrstr[2] = dr["ISINFORMATIONONLY"].ToString();
                            arrstr[3] = dr["ATTACHMENT_ALLOWED"].ToString();                         
                            arrstr[4] = dr["IS_HALF_DAY"].ToString();
                            arrstr[5] = dr["Days"].ToString();
                        }
                        con.Close();
                    }
                }
                return res = String.Join(",", arrstr);
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }      
        public string GetEmpLeaveBal(string empid, int year)
        {
            string res = "";
            DataTable dt = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();            
            using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_GetLeaveBalance", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EMPLOYEE_ID", empid);
                        cmd.Parameters.AddWithValue("@Lyear", year);
                        con.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                           adp.Fill(dt); 
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        foreach (DataRow dr in dt.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                          
                        }
                        res= serializer.Serialize(rows);
                    }
                    con.Close();
                    }
                }
            return res;  
        }
        public string GetEmpLeaveBalsandwith(int empid, int leavetype,string fromdate,string todate)
        {
            //sanwich value get 
            string res = "";
            DataTable dt = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("PRC_LEAVE_Actual_Status", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeID", empid);
                    cmd.Parameters.AddWithValue("@Leave_TYPE_ID", leavetype);
                    cmd.Parameters.AddWithValue("@FromDate", fromdate);
                    cmd.Parameters.AddWithValue("@ToDate", todate);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        res = dr["deductleave"].ToString();
                    }
                   
                    con.Close();
                }
            }
            return res;
        }
        public string GetEmp_Mgr_LeaveBal(string empid, int year)
        {
            string res = "";
            DataTable dt = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetLeaveBalance_mgr", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMPLOYEE_ID", empid);
                    cmd.Parameters.AddWithValue("@Lyear", year);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        foreach (DataRow dr in dt.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);

                        }
                        res = serializer.Serialize(rows);
                    }
                    con.Close();
                }
            }
            return res;
        }

        public DataSet GetDelegationLeaveapprovalhistory(int Employee_ID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetDelegationLeaveApproval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMPLOYEEID", Employee_ID);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }
    }


}