using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace OTS.Models
{
    public class LeavePolicy
    {
        public int ID { get; set; } 
        public string LeaveTypeID { get; set; } 
        public string PolicyName { get; set; }
        public string PolicyDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; } 
        public string Status { get; set; } 
        public int IsInformationOnly { get; set; }
    }

    public class LeavePlan
    {
        public int ID { get; set; }
        public int AttachmentRequired { get; set; }
        public string Gender { get; set; }
        public string LeaveYear { get; set; }
        public string CreditFrequency { get; set; }
        public string Credit { get; set; }
        public string IncludePublicHolidays { get; set; }
        public string IncludeWeekends { get; set; }
        public string CanbeclubbedwithEL { get; set; }
        public string CanbeclubbedwithCL { get; set; } 
        public string Canbehalfday { get; set; }
        public string Notice_Period_P { get; set; }
        public string Probation_Period_P { get; set; }
        public string Confirmed_P { get; set; }
        public string Contract_Period_P { get; set; }
        public List<CreateRule>  tblCreateRule { get; set; }

    }

    public class YearEndProcessing
    {
        public int ID { get; set; }
        public string AllowCarryover_CarryoverLimit { get; set; }
        public string PayatYearend_MinBal { get; set; }
        public string PayatYearend_MaxEncashment  { get; set; }
        public string EligibleForEncashment_Limit { get; set; }
        public string EligibleForEncashment_MinBal { get; set; }
        public string CarryForwardtoEL_CFLimit { get; set; }

    }
    public class CreateRule
    {
    
        public int CREATERULE_ID { get; set; } 
        public string Grade { get; set; }
        public string GradeID { get; set; }
        public string NoOfDays { get; set; }
        public string UOM { get; set; }
        public string UOMID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; } 
        public string IslossofPay { get; set; } 
        public string Employer_Type { get; set; }
        public string  Plan_Status  { get; set; }
        public string row_class { get; set; }

    }


    public class ListtoDataTableConverter
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    
                    var ab = Props[i].PropertyType.Name;
                    if (ab.ToString() == "DateTime")
                    {
                        string format = "yyyy-MM-dd HH:mm:ss";
                        values[i] = Props[i].GetValue(item, null) != null ? Convert.ToDateTime(Props[i].GetValue(item, null)).ToString(format) : null;
                    }
                    else
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

    }

    public class editpolicy
    {
        public LeavePlan LeavePlan { get; set; }
        public LeavePolicy LeavePolicy { get; set; }
        public YearEndProcessing YearEndProcessing{get; set;}
    }
    public class LeaveApprove
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string Approval_Level { get; set; }

        public string Remarks { get; set; }
        public int Max_Level { get; set; }
    }

    public class Approve_Emp_Detail
    {
        public int EMP_ID { get; set; }
        public string From_Date { get; set; }
        public string To_Date { get; set; }

        public string Type { get; set; }

    }
}