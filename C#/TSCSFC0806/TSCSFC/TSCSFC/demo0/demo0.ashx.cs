using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using TSCSFC.DB;

namespace access.demo0
{
    /// <summary>
    ///demo0 的摘要描述
    /// </summary>
    public class demo0 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["mode"])
            {
                case "load":
                    load(context);
                    break;
            }
        }

        public void load(HttpContext context)
        {
            string nian0 = context.Request["nian0"];
            string nian1 = context.Request["nian1"];
            string nian2 = context.Request["nian2"];
            string yue = panduan(context.Request["yue"]);
            SqlParameter[] om ={
                                    new SqlParameter ("@nian0",nian0),
                                    new SqlParameter ("@nian1",nian1),
                                    new SqlParameter ("@nian2",nian2)
                                            };
            string st1 = DBHelper.DataTable2Json(DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select heji from FIT_YICHANG where nian=@nian2 and changqu in('淮安','昆山','寶科','廊坊','龍華','鄭州','重慶','豐城','贛州','合計')", CommandType.Text, om)));
            string st2 = DBHelper.DataTable2Json(DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select heji from FIT_YICHANG where nian=@nian1 and changqu in('淮安','昆山','寶科','廊坊','龍華','鄭州','重慶','豐城','贛州','合計')", CommandType.Text, om)));
            string st3 = DBHelper.DataTable2Json(DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select "+yue+" as heji from FIT_YICHANG where nian=@nian0 and changqu in('淮安','昆山','寶科','廊坊','龍華','鄭州','重慶','豐城','贛州','合計')", CommandType.Text, om)));
            string st4 = DBHelper.DataTable2Json(DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select heji from FIT_YICHANG where nian=@nian0 and changqu in('淮安','昆山','寶科','廊坊','龍華','鄭州','重慶','豐城','贛州','合計')", CommandType.Text, om)));
            string st = "[" + st1 + "," + st2 + "," + st3 + "," + st4 + "]";
            context.Response.Write(st);
        }
        private string panduan(string s)
        {
            string ss = "";
            switch (s)
            {
                case "01":
                    ss = "yiyue";
                    break;
                case "02":
                    ss = "eryue";
                    break;
                case "03":
                    ss = "sanyue";
                    break;
                case "04":
                    ss = "siyue";
                    break;
                case "05":
                    ss = "wuyue";
                    break;
                case "06":
                    ss = "liuyue";
                    break;
                case "07":
                    ss = "qiyue";
                    break;
                case "08":
                    ss = "bayue";
                    break;
                case "09":
                    ss = "jiuyue";
                    break;
                case "10":
                    ss = "shiyue";
                    break;
                case "11":
                    ss = "shiyiyue";
                    break;
                case "12":
                    ss = "shieryue";
                    break;
            }
            return ss;
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