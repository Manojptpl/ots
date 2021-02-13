using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;

namespace OTS.Models
{
    public class FileDownloads
    {
        database_Access_Layer.db dblayer = new database_Access_Layer.db();
        DataTable dt = new DataTable();
        public List<FileInfo> GetFile(int id,int month,int year)
        {
            dt = dblayer.GetExpenseFiles(id,month,year);
          
            List<FileInfo> listFiles = new List<FileInfo>();
            string fileSavePath = System.Web.Hosting.HostingEnvironment.MapPath("~/UploadFiles/");
            DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            int i = 0;
            foreach (DataRow item in dt.Rows)
            {
                listFiles.Add(new FileInfo()
                {
                    FileName = item["filename"].ToString(),
                    FilePath = dirInfo.FullName + item["filename"].ToString()
                });
                i = i + 1;
            }
            return listFiles;
        }
    }
}