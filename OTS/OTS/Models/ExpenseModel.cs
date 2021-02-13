using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class ExpenseModel
    {
        public ExpenseEntities CreateExpense { set; get; }
        public List<ExpenseFiles> Files { set; get; }
    }
    public class ExpenseEntities
    {
        public int EXPENSE_ID { get; set; }
        public string EXPENSE_CODE { set; get; }
        public DateTime EXPENSE_DATE { get; set; }
        public int EXPENSE_TYPE_ID { set; get; }
        public int TRAVEL_MODE { set; get; }
        public int PROJECT_ID { set; get; }
        public string TRAVEL_FROM { set; get; }
        public string TRAVEL_TO { set; get; }
        public int TRAVEL_DISTANCE { set; get; }
        public decimal EXPENSE_AMOUNT { set; get; }
        public string DESCRIPTION { set; get; }
        
    }
    public class ExpenseFiles
    {
        public string file_name { set; get; }
    }
}