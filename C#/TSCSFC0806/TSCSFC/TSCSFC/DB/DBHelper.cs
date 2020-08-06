using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Data.SqlClient;

namespace TSCSFC.DB
{
    public class DBHelper
    {
        //public static SqlCommand cmd = null;
        //public static SqlConnection conn ;
        public static SqlPool pool1 = SqlPool.Instance;
        public static SqlPool pool2 = SqlPool.Instance;
        public static SqlPool pool3 = SqlPool.Instance;
        public static SqlPool pool4 = SqlPool.Instance;
        public static SqlPool pool5 = SqlPool.Instance;
        //  public static string connstr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        public DBHelper()
        { }

        #region 执行相应的sql语句，返回相应的DataSet对象
        /// <summary>
        /// 执行相应的sql语句，返回相应的DataSet对象
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <returns>返回相应的DataSet对象</returns>
        public static DataSet GetDataSet(string sqlstr)
        {
            DataSet set = new DataSet();
            SqlConnection conn = pool1.BorrowDBConnection();
            try
            { 
                SqlDataAdapter adp = new SqlDataAdapter(sqlstr, conn);
                adp.Fill(set);
                pool1.ReturnDBConnection(conn);
                adp.Dispose();
                //conn.Close();
            }
            catch (Exception e)
            {
                pool1.ReturnDBConnection(conn);
                throw new Exception(e.Message.ToString());
            }
            return set;
        }
        #endregion


        #region 执行相应的sql语句，返回相应的DataSet对象
        /// <summary>
        /// 执行相应的sql语句，返回相应的DataSet对象
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <param name="tableName">表名</param>
        /// <returns>返回相应的DataSet对象</returns>
        public static DataSet GetDataSet(string sqlstr, string tableName)
        {
            DataSet set = new DataSet();
            SqlConnection conn = pool1.BorrowDBConnection();
            try
            {
                
                SqlDataAdapter adp = new SqlDataAdapter(sqlstr, conn);
                adp.Fill(set, tableName);
                pool1.ReturnDBConnection(conn);
                adp.Dispose();
            }
            catch (Exception e)
            {
                pool1.ReturnDBConnection(conn);
                throw new Exception(e.Message.ToString());
            }
            return set;
        }
        #endregion


        #region 执行帶參數的sql语句，返回相应的DataSet对象
        /// <summary>
        /// 执行相应的sql语句，返回相应的DataSet对象
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <returns>返回相应的DataSet对象</returns>
        public static DataSet GetDataSet(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            DataSet set = new DataSet();
            SqlConnection conn = pool2.BorrowDBConnection();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;
                    if (cmdParms != null)
                    {
                        cmd.Parameters.AddRange(cmdParms);
                    }
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(set);
                        adp.Dispose();
                    }
                    pool2.ReturnDBConnection(conn);
                } 
            }
            catch (Exception e)
            {
                pool2.ReturnDBConnection(conn);
                throw new Exception(e.Message.ToString());
            }
            return set;
        }
        #endregion


        #region 执行不带参数sql语句，返回所影响的行数
        /// <summary>
        /// 执行不带参数sql语句，返回所影响的行数
        /// </summary>
        /// <param name="cmdstr">增，删，改sql语句</param>
        /// <returns>返回所影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText)
        {
            int count;
            SqlConnection conn = pool3.BorrowDBConnection();
            try
            {
                
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                count = cmd.ExecuteNonQuery();
                pool3.ReturnDBConnection(conn);
                //conn.Close();
            }
            catch (Exception ex)
            {
                pool3.ReturnDBConnection(conn);
                throw new Exception(ex.Message.ToString());
            }
            return count;
        }
        #endregion


        #region 执行带参数sql语句或存储过程，返回所影响的行数
        /// <summary>
        ///  执行带参数sql语句或存储过程，返回所影响的行数
        /// </summary>
        /// <param name="cmdText">带参数的sql语句和存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回所影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            int count;
            SqlConnection conn = pool3.BorrowDBConnection();
            try
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;
                    if (cmdParms != null)
                    {
                        cmd.Parameters.AddRange(cmdParms);
                    }
                    count = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    pool3.ReturnDBConnection(conn);
                }
                //conn.Close();
            }
            catch (Exception ex)
            {
                pool3.ReturnDBConnection(conn);
                throw new Exception(ex.Message.ToString());
            }
            return count;
        }
        #endregion


        #region 执行不带参数sql语句，返回一个从数据源读取数据的SqlDataReader对象
        /// <summary>
        /// 执行不带参数sql语句，返回一个从数据源读取数据的SqlDataReader对象
        /// </summary>
        /// <param name="cmdstr">相应的sql语句</param>
        /// <returns>返回一个从数据源读取数据的SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText)
        {
            SqlDataReader reader;
            SqlConnection conn = pool4.BorrowDBConnection();
            try
            {

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                pool4.ReturnDBConnection(conn);

            }
            catch (Exception ex)
            {
                pool4.ReturnDBConnection(conn);
                throw new Exception(ex.Message.ToString());
            }
            return reader;
        }
        #endregion
        #region 执行DataReaderToDataTable对象

        public static DataTable ConvertDataReaderToDataTable(SqlDataReader reader)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                int intFieldererer = reader.FieldCount;
                for (int interererer = 0; interererer < intFieldererer; ++interererer)
                {
                    objDataTable.Columns.Add(reader.GetName(interererer), reader.GetFieldType(interererer));
                }

                objDataTable.BeginLoadData();

                object[] objValues = new object[intFieldererer];
                while (reader.Read())
                {
                    reader.GetValues(objValues);
                    objDataTable.LoadDataRow(objValues, true);
                }
                reader.Close();
                objDataTable.EndLoadData();

                return objDataTable;

            }
            catch (Exception ex)
            {
                throw new Exception("转换出错!", ex);
            }

        }
        #endregion
        #region dataTable转换成Json格式
        /// <summary>  
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }

            return jsonBuilder.ToString();
        }
        #endregion dataTable转换成Json格式
        #region DataSet转换成Json格式
        /// <summary>
        /// DataSet转换成Json格式  
        /// </summary>  
        /// <param name="ds">DataSet</param> 
        /// <returns></returns>  
        public static string Dataset2Json(DataSet ds, int total = -1)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                //{"total":5,"rows":[
                json.Append("{\"total\":");
                if (total == -1)
                {
                    json.Append(dt.Rows.Count);
                }
                else
                {
                    json.Append(total);
                }
                json.Append(",\"rows\":[");
                json.Append(DataTable2Json(dt));
                json.Append("]}");
            } return json.ToString();
        }
        #endregion


        #region 从一个对象信息生成Json串
        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

        #endregion


        #region 从一个Json串生成对象信息

        public static object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return serializer.ReadObject(mStream);
        }

        #endregion


        #region 执行带参数的sql语句或存储过程，返回一个从数据源读取数据的SqlDataReader对象
        /// <summary>
        /// 执行带参数的sql语句或存储过程，返回一个从数据源读取数据的SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">sql语句或存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回一个从数据源读取数据的SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            SqlDataReader reader;
            SqlConnection conn = pool2.BorrowDBConnection();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;
                    if (cmdParms != null)
                    {
                        cmd.Parameters.AddRange(cmdParms);
                    }
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    pool2.ReturnDBConnection(conn);
                }
            }
            catch (Exception ex)
            {
                pool2.ReturnDBConnection(conn);
                throw new Exception(ex.Message.ToString());
            }
            return reader;
            
        }
        #endregion


        #region 执行不带参数sql语句,返回结果集首行首列的值object
        /// <summary>
        /// 执行不带参数sql语句,返回结果集首行首列的值object
        /// </summary>
        /// <param name="cmdstr">相应的sql语句</param>
        /// <returns>返回结果集首行首列的值object</returns>
        public static object ExecuteScalar(string cmdText)
        {
            object obj;
            SqlConnection conn = pool5.BorrowDBConnection();
            try
            {

                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    obj = cmd.ExecuteScalar();
                    pool5.ReturnDBConnection(conn);
                    //conn.Close();
                }
                
            }
            catch (Exception ex)
            {
                pool5.ReturnDBConnection(conn);
                throw new Exception(ex.Message.ToString());
            }
            return obj;
        }
        #endregion


        #region 执行带参数sql语句或存储过程,返回结果集首行首列的值object
        /// <summary>
        /// 执行带参数sql语句或存储过程,返回结果集首行首列的值object
        /// </summary>
        /// <param name="cmdText">sql语句或存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">返回结果集首行首列的值object</param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            object obj;
            SqlConnection conn = pool5.BorrowDBConnection();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;
                    if (cmdParms != null)
                    {
                        cmd.Parameters.AddRange(cmdParms);
                    }
                    obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    pool5.ReturnDBConnection(conn);
                    //conn.Close();
                }
                
            }
            catch (Exception ex)
            {
                pool5.ReturnDBConnection(conn);
                throw new Exception(ex.Message.ToString());
            }
            return obj;
        }
        #endregion
    }
}