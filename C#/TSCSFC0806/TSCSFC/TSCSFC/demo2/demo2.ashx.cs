using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using TSCSFC.DB;
using System.Data;
using System.Web.Configuration;

namespace TSCSFC.demo2
{
    /// <summary>
    /// demo2 的摘要描述
    /// </summary>
    public class demo2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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