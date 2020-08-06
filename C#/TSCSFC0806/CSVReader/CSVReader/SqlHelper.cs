using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace CSVReader
{
   public class SqlHelper
    {
       private static SqlConnection open_DB()
       {
           string constr = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString ;
           //string constr = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString + "uid=sa;pwd=saadmin;";
           SqlConnection sc = new SqlConnection(constr);
           if (sc.State == ConnectionState.Closed)
           {
               sc.Open();
           }
           return sc;
       }
    }
}
