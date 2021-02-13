using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTS.Models
{
    public class AnnualAppraisalModel
    {
        public int EMP_ID { get; set; }
        public string NAME { get; set; }
        public string MANAGER_NAME { get; set; }
        public string DESIGNATION { get; set; }                      
        public string CURRENT_DATE { get; set; }        
        public string DEPARTMENT { get; set; }
        public string GOAL_QUARTER { get; set; }
        public string FINAL_RATING { get; set; }        

    }
    public class AppraisalPerformanceModel
    {
        public int APPRAISAL_ID { get; set; }
        public int EMP_ID { get; set; }
        public int CURRENTROLL_SELFRATING { get; set; }
        public string CURRENTROLL_SELFEXP { get; set; }
        public int CURRENTROLL_MGRRATING { get; set; }
        public string CURRENTROLL_MGREXP { get; set; }
        public int QUALITYWORK_SELFRATING { get; set; }
        public string QUALITYWORK_SELFEXP { get; set; }
        public int QUALITYWORK_MGRRATING { get; set; }
        public string QUALITYWORK_MGREXP { get; set; }
        public int ORGWORK_SELFRATING { get; set; }
        public string ORGWORK_SELFEXP { get; set; }
        public int ORGWORK_MGRRATING { get; set; }
        public string ORGWORK_MGREXP { get; set; }
        public int INITIATE_SELFRATING { get; set; }
        public string INITIATE_SELFEXP { get; set; }
        public int INITIATE_MGRRATING { get; set; }
        public string INITIATE_MGREXP { get; set; }
        public int PERSONAL_SELFRATING { get; set; }
        public string PERSONAL_SELFEXP { get; set; }
        public int PERSONAL_MGRRATING { get; set; }
        public string PERSONAL_MGREXP { get; set; }
        public int VSKILL_SELFRATING { get; set; }
        public string VSKILL_SELFEXP { get; set; }
        public int VSKILL_MGRRATING { get; set; }
        public string VSKILL_MGREXP { get; set; }
        public int WSKILL_SELFRATING { get; set; }
        public string WSKILL_SELFEXP { get; set; }
        public int WSKILL_MGRRATING { get; set; }
        public string WSKILL_MGREXP { get; set; }
        public int TEAM_SELFRATING { get; set; }
        public string TEAM_SELFEXP { get; set; }
        public int TEAM_MGRRATING { get; set; }
        public string TEAM_MGREXP { get; set; }
        public int CONFIDENTIALITY_SELFRATING { get; set; }
        public string CONFIDENTIALITY_SELFEXP { get; set; }
        public int CONFIDENTIALITY_MGRRATING { get; set; }
        public string CONFIDENTIALITY_MGREXP { get; set; }
        public int ATTENDANCE_SELFRATING { get; set; }
        public string ATTENDANCE_SELFEXP { get; set; }
        public int ATTENDANCE_MGRRATING { get; set; }
        public string ATTENDANCE_MGREXP { get; set; }
        public float SECTION_SECOND_TOTALRATING { get; set; }
    }
    public class AppraisalRating
    {
        public int APPRAISAL_ID { get; set; }
        public int EMP_ID { get; set; }
        public float SECTION_ONE_OVERALL_TOTALRATING { get; set; }
        public float SECTION_2_OVERALL_TOTALRATING { get; set; }
        public float SECTION_2_OVERALL_TOTALRATING_MGR { get; set; }
        public float SUBTOTAL { get; set; }
        public float SUBTOTAL_MGR { get; set; }
        public float OVERALL_RATING { get; set; }
        public float OVERALL_RATING_MGR { get; set; }
        public float EMP_RATING { get; set; }
        public float MGR_RATING { get; set; }

    }
    public class AppraisalModel
    {
        public List<AnnualAppraisalModel> Annual_Appraisal { get; set; }
        public List<AppraisalPerformanceModel> Annual_Performance { get; set; }
        public List<AppraisalRating> Annual_Rating { get; set; }
    }

    public class CommonAppraisalModel
    {
        public int EMP_ID { get; set; }
        public string NAME { get; set; }
        public string MANAGER_NAME { get; set; }
        public string DESIGNATION { get; set; }
        public string DEPARTMENT { get; set; }
        public string GOAL_QUARTER { get; set; }
        public float SECTION_ONE_OVERALL_TOTALRATING { get; set; }
        public string strSECTION_ONE_OVERALL_TOTALRATING { get; set; }
        public float SECTION_2_OVERALL_TOTALRATING { get; set; }
        public string strSECTION_2_OVERALL_TOTALRATING { get; set; }
        public float SECTION_2_OVERALL_TOTALRATING_MGR { get; set; }
        public string strSECTION_2_OVERALL_TOTALRATING_MGR { get; set; }
        public float SUBTOTAL_MGR { get; set; }
        public string strSUBTOTAL_MGR { get; set; }
        public float OVERALL_RATING { get; set; }        
        public string strOVERALL_RATING { get; set; }
        public float OVERALL_RATING_MGR { get; set; }
        public string strOVERALL_RATING_MGR { get; set; }
        public float MGR_RATING { get; set; }
        public string Year { get; set; }
    }
    public class SingleModel
    {
        public int EMP_ID { get; set; }
        public string NAME { get; set; }
        public string MANAGER_NAME { get; set; }
        public string DESIGNATION { get; set; }
        public string CURRENT_DATE { get; set; }        
        public string DEPARTMENT { get; set; }
        public string GOAL_QUARTER { get; set; }
        public string FINAL_RATING { get; set; }
        
        public int CURRENTROLL_SELFRATING { get; set; }
        public string CURRENTROLL_SELFEXP { get; set; }
        public int CURRENTROLL_MGRRATING { get; set; }
        public string strCURRENTROLL_MGRRATING { get; set; }
        public string CURRENTROLL_MGREXP { get; set; }
        public int QUALITYWORK_SELFRATING { get; set; }
        public string QUALITYWORK_SELFEXP { get; set; }
        public int QUALITYWORK_MGRRATING { get; set; }
        public string strQUALITYWORK_MGRRATING { get; set; }
        public string QUALITYWORK_MGREXP { get; set; }
        public int ORGWORK_SELFRATING { get; set; }
        public string ORGWORK_SELFEXP { get; set; }
        public int ORGWORK_MGRRATING { get; set; }
        public string strORGWORK_MGRRATING { get; set; }
        public string ORGWORK_MGREXP { get; set; }
        public int INITIATE_SELFRATING { get; set; }
        public string INITIATE_SELFEXP { get; set; }
        public int INITIATE_MGRRATING { get; set; }
        public string strINITIATE_MGRRATING { get; set; }
        public string INITIATE_MGREXP { get; set; }
        public int PERSONAL_SELFRATING { get; set; }
        public string PERSONAL_SELFEXP { get; set; }
        public int PERSONAL_MGRRATING { get; set; }
        public string strPERSONAL_MGRRATING { get; set; }
        public string PERSONAL_MGREXP { get; set; }
        public int VSKILL_SELFRATING { get; set; }
        public string VSKILL_SELFEXP { get; set; }
        public int VSKILL_MGRRATING { get; set; }
        public string strVSKILL_MGRRATING { get; set; }
        public string VSKILL_MGREXP { get; set; }
        public int WSKILL_SELFRATING { get; set; }
        public string WSKILL_SELFEXP { get; set; }
        public int WSKILL_MGRRATING { get; set; }
        public string strWSKILL_MGRRATING { get; set; }
        public string WSKILL_MGREXP { get; set; }
        public int TEAM_SELFRATING { get; set; }
        public string TEAM_SELFEXP { get; set; }
        public int TEAM_MGRRATING { get; set; }
        public string strTEAM_MGRRATING { get; set; }
        public string TEAM_MGREXP { get; set; }
        public int CONFIDENTIALITY_SELFRATING { get; set; }
        public string CONFIDENTIALITY_SELFEXP { get; set; }
        public int CONFIDENTIALITY_MGRRATING { get; set; }
        public string strCONFIDENTIALITY_MGRRATING { get; set; }
        public string CONFIDENTIALITY_MGREXP { get; set; }
        public int ATTENDANCE_SELFRATING { get; set; }
        public string ATTENDANCE_SELFEXP { get; set; }
        public int ATTENDANCE_MGRRATING { get; set; }
        public string strATTENDANCE_MGRRATING { get; set; }
        public string ATTENDANCE_MGREXP { get; set; }
        public decimal SECTION_SECOND_TOTALRATING { get; set; }               
        public decimal SECTION_ONE_OVERALL_TOTALRATING { get; set; }
        public decimal SECTION_2_OVERALL_TOTALRATING { get; set; }
        public decimal SECTION_2_OVERALL_TOTALRATING_MGR { get; set; }
        public decimal SUBTOTAL { get; set; }
        public decimal SUBTOTAL_MGR { get; set; }
        public decimal OVERALL_RATING { get; set; } 
        public decimal OVERALL_RATING_MGR { get; set; }
        public decimal EMP_RATING { get; set; }
        public decimal MGR_RATING { get; set; }
    }
}