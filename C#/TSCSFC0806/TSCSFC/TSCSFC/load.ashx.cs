using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSCSFC.DB;
using System.Data.SqlClient;

namespace access
{
    /// <summary>
    ///load 的摘要描述
    /// </summary>
    public class load : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string loginName = string.Empty;
            if (context.Request["mode"] == "load")
            {
                string user = context.Request["user"];
                
                string password = context.Request["pass"];
                SqlParameter[] op ={
                                  new SqlParameter("@user",user),
                                  new SqlParameter("@password",password)
                };
                DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select userid,username from SFC_Users where userid=@user and password=@password", CommandType.Text, op));
                int num = dt.Rows.Count;
                if (num > 0)
                {
                    string mm = "[";
                    mm = mm + DBHelper.DataTable2Json(dt);
                    mm = mm + "]";
                    context.Response.Write(mm);
                }
                else
                {
                    context.Response.Write("NG");
                }
                loginName = user;
            }
            if (context.Request["mode"] == "alert")
            {
                int num=0;
                string user = context.Request["user"];
                string password = context.Request["pass"];
                SqlParameter[] op ={
                                  new SqlParameter("@user",user),
                                  new SqlParameter("@password",password)
                };
                num = DBHelper.ExecuteNonQuery("update SFC_Users set password=@password where userid=@user", CommandType.Text, op);
                if (num > 0)
                {
                    context.Response.Write(password);
                }
                else
                {
                    context.Response.Write("NG");
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}