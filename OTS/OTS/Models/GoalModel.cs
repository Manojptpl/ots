using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OTS.Models
{
    public class GoalModel
    {
        public IList<SelectListItem> ProjectNames { get; set; }
        public string Goal_Title { set; get; }
        public int Goal_Type { set; get; }
        public int Client_Name { set; get; }
        public DateTime Due_Date { set; get; }
        public DateTime Complete_Date { set; get; }
        public int Static { set; get; }
        public int Project_Type { set; get; }
        public int Percent { set; get; }
        public int Weighted { set; get; }
        public string Description { set; get; }
    }
    public class Apply_Goal
    {
        public IEnumerable<Goal_Line> GoalLines { get; set; }
        public string EmpDesc { set; get; }
        public string MgrDesc { set; get; }
        public string QUARTER { set; get; }
        public int YEAR { set; get; }
        public string Success_msg { set; get; }
        public string Error_msg { set; get; }
    }
    public class Goal_Line
    {
        public int Id { set; get; }
        public int GoalId { set; get; }
        public int SelfRating { set; get; }
        public string SelfRemark { set; get; }
        public int ManagerRating { set; get; }
        public string ManagerRemark { set; get; }
    }
    public class Team
    {
        public List<Team_Goal> teamgoal { set; get; }
        public List<Team_Personal> teampersonal { set; get; }
        public List<Team_Organizational> teamorganizational { set; get; }
    }
    public class Team_Goal
    {
        public int ID { get; set; }
        public string MANAGER_REMARK { get; set; }
        public decimal MANAGER_SCORE { get; set; }
    }
    public class Team_Personal
    {
        public int ID { get; set; }
        public decimal MANAGER_RATING { get; set; }
        public string MANAGER_REMARK { get; set; }
        public decimal MANAGER_SCORE { get; set; }
    }
    public class Team_Organizational
    {
        public int ID { get; set; }
        public decimal MANAGER_RATING { get; set; }
        public string MANAGER_REMARK { get; set; }
        public decimal MANAGER_SCORE { get; set; }
    }
}