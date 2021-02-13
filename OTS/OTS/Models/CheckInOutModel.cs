using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class CheckInOutModel
    {
        public int emp_id { set; get; }
        public string emp_name { set; get; }
        public string emp_code { set; get; }
        public string worktype { set; get; }
        public string customer { set; get; }
        public string project { set; get; }
        public string chk_in_date { set; get; }
        public string chk_out_date { set; get; }
        public string chk_in_Address { get; set; }
        public string chk_out_Address { get; set; }
        public double chk_in_lat { get; set; }
        public double chk_in_long { get; set; }
        public double chk_out_lat { get; set; }
        public double chk_out_long { get; set; }
        public string location { set; get; }
        public string Status { set; get; }
        public int TransactionMonth { set; get; }
        public int TransactionYear { set; get; }
        public int Role { set; get; }
        public int? Employee_id { set; get; }
        public DateTime? From_date { set; get; }
        public DateTime? To_date { set; get; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
    }
    public class CheckInOutEntities
    {
        public int chk_in_id { set; get; }
        public int emp_id { set; get; }
        public int worktype_id { set; get; }
        public int customer_id { set; get; }
        public int project_id { set; get; }
        public DateTime? chk_in_date { set; get; }
        public DateTime? chk_out_date { set; get; }
        public string in_location { get; set; }
        public string out_location { get; set; }
        public string chk_in_lat { get; set; }
        public string chk_in_long { get; set; }
        public string chk_out_lat { get; set; }
        public string chk_out_long { get; set; }
        public string Remark { set; get; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
    }
}