using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using TSCSFC.DB;
using System.Data;
using System.Web.Configuration;

namespace access.demo1
{
    /// <summary>
    ///demo1 的摘要描述
    /// </summary>
    public class demo1 : IHttpHandler
    { 
        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["mode"])
            {
                case "load":
                    load(context);
                    break; 
                case "download":
                    download(context);
                    break;
            }
        }
        public void download(HttpContext context)
        {
            //string file = context.Request.QueryString["filename"].ToString().Trim();
            string filename = "包裝掃描" + DateTime.Now.ToString("yyyyMMddHHmmss") +".xls";

            string keyWord = Convert.ToString(context.Request["keyWord"]);
          
            string first = Convert.ToString(context.Request["first"]);
            string second = Convert.ToString(context.Request["second"]);
            DataTable dt = GetData(context, keyWord, first, second);
            try
            {
                string path = "";
                path = NPOIhelper.ExportDataTableToExcel(dt, filename, "包裝掃描", "1");
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                context.Response.Write(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetData(HttpContext context, string keyWord, string first, string second)
        {
            DataTable dt = new DataTable();
            //获取分页和排序信息：页大小，页码，排序方式，排序字段
            context.Response.ContentType = "text/html";


            string sql = @"SELECT   cableBarcode as 二維碼 ,userID as 操作員, scanTime as 掃描時間  FROM  package  where 1=1 ";

            SqlParameter[] op ={  
                                  new SqlParameter("@first",string.IsNullOrEmpty(first)?DBNull.Value.ToString():first),
                                  new SqlParameter("@second",string.IsNullOrEmpty(second)?DBNull.Value.ToString():second),
                                  new SqlParameter("@keyWord",string.IsNullOrEmpty(keyWord)?DBNull.Value.ToString():keyWord)
                                
                };

            if (!string.IsNullOrEmpty(keyWord))
            {
                sql += " and cableBarcode like '%'+@keyWord +'%' ";
            } 
            if (!string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(second))
            {
                sql += " and scanTime between @first and @second  ";
            }
            sql += " order by scanTime desc ";

           DataSet ds = DBHelper.GetDataSet(sql, CommandType.Text, op);
            dt = ds.Tables[0]; 
            return dt;
        }
         
        public void load(HttpContext context)
        {
            //获取分页和排序信息：页大小，页码，排序方式，排序字段
            context.Response.ContentType = "text/html";

            //调用分页的GetList方法
            int count = GetRecordCount(context);//获取条数
            DataSet ds = GetListByPage(context, count);

            string strJson = DBHelper.Dataset2Json(ds, count);//DataSet数据转化为Json数据
            context.Response.Write(strJson);//返回给前台页面
            context.Response.End();
        }
        public int GetRecordCount(HttpContext context)
        {
            int num;
            string keyWord = Convert.ToString(context.Request["keyWord"]); 
            string first = Convert.ToString(context.Request["first"]);
            string second = Convert.ToString(context.Request["second"]);

            string sql = "select count(*) from package where 1=1 ";


            if (!string.IsNullOrEmpty(keyWord))
            {
                sql += " and cableBarcode like '%'+@keyWord +'%' ";
            } 
            if (!string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(second))
            {
                sql += " and scanTime between @first and @second "; 
            }
            else  
            {
                sql += " and scanTime between @first and @second ";
            } 

            SqlParameter[] op ={  
                                  new SqlParameter("@first",string.IsNullOrEmpty(first)?DBNull.Value.ToString():first),
                                  new SqlParameter("@second",string.IsNullOrEmpty(second)?DBNull.Value.ToString():second),
                                  new SqlParameter("@keyWord",string.IsNullOrEmpty(keyWord)?DBNull.Value.ToString():keyWord) 
                                
                };

            if (first == "XXX" || string.IsNullOrEmpty(first))
            {
                num = Convert.ToInt32(DBHelper.ExecuteScalar("select count(*) from package where scanTime between CONVERT(varchar(10),dateadd(DAY, -7, getdate()),120) and CONVERT(varchar(10),getdate(),120) ", CommandType.Text, op));
            }
            else
            {  
                num = Convert.ToInt32(DBHelper.ExecuteScalar(sql, CommandType.Text, op));
            }
            return num;
        }
        public DataSet GetListByPage(HttpContext context, int count)
        {
            DataSet ds;
            string keyWord = Convert.ToString(context.Request["keyWord"]); 
            string first = Convert.ToString(context.Request["first"]);
            string second = Convert.ToString(context.Request["second"]);

            int page = int.Parse(context.Request["page"]);//页码
            int pageRows = int.Parse(context.Request["rows"]);//页容量
            int sum = page * pageRows;
            if (sum > count)
            {
                pageRows = count + pageRows - sum;
            }
            string sql = string.Empty;


            if (!string.IsNullOrEmpty(keyWord) && !string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(second))//keyWord
            {
                sql = "select top " + pageRows + " * from (select top " + sum + " * from package where 1=1  and cableBarcode like '%'+@keyWord +'%'  and scanTime between @first and @second order by scanTime)A order by A.scanTime desc";
            }
            else if (!string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(second))
            {
                sql = "select top " + pageRows + " * from (select top " + sum + " * from package where 1=1 and scanTime between @first and @second order by scanTime)A order by A.scanTime desc";
            }
            else
            {
                sql = "select top " + pageRows + " * from (select top " + sum + " * from package where 1=1 and scanTime between  CONVERT(varchar(10),dateadd(DAY, -7, getdate()),120) and CONVERT(varchar(10),getdate(),120) order by scanTime)A order by A.scanTime desc";
            }
             
            SqlParameter[] op ={  
                                  new SqlParameter("@first",string.IsNullOrEmpty(first)? DBNull.Value.ToString():first),
                                  new SqlParameter("@second",string.IsNullOrEmpty(second)?DBNull.Value.ToString():second),
                                  new SqlParameter("@keyWord",string.IsNullOrEmpty(keyWord)?DBNull.Value.ToString():keyWord) 
                };
           
            if (first == "XXX" || string.IsNullOrEmpty(first))
            {
                ds = DBHelper.GetDataSet("select top " + pageRows + " * from (select top " + sum + " * from package where scanTime between CONVERT(varchar(10),dateadd(DAY, -7, getdate()),120) and CONVERT(varchar(10),getdate(),120) order by scanTime )A order by A.scanTime desc", CommandType.Text, op);
            }
            else
            { 
                ds = DBHelper.GetDataSet(sql, CommandType.Text, op);
            }
            return ds;
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