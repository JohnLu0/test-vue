using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Data;
using System.Threading;
using GetDataCSV;


namespace GetDataCSV
{
    class CSVCopy
    {
        //定时器
        System.Timers.Timer timer;
        //间隔时间
        int intervalTime;
        //文件原来的地址
        string path1;
        //文件的目标目录
        string path2;
        //数据量
        int dataCount = 0;
        //制程
        string stepName;

        public CSVCopy(string path1, string path2, int intervalTime)
        {
            this.path1 = path1;
            this.path2 = path2;
            this.intervalTime = intervalTime;
        }

        //修改配置文件类
        //internal void writeIntoConfig()
        //{
        //   FileInfo FileInfo = new FileInfo(@"../../App.config");//注意e799bee5baa6e78988e69d8331333262356130更改对应的路径        
        //   XmlDocument xmlDocument = new XmlDocument();
        //   xmlDocument.Load(FileInfo.FullName);
        //   //<appSettings><add key="ConnectionString"
        //   foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["appSettings"])
        //   {
        //       if (Node.Name == "add")
        //       {
        //           if (Node.Attributes.GetNamedItem("key").Value == "path1")
        //           {
        //               Node.Attributes.GetNamedItem("value").Value = this.path1;//注意更改对应的服务器名，数据库名，登陆名和密码
        //           }
        //           if (Node.Attributes.GetNamedItem("key").Value == "path2")
        //           {
        //               Node.Attributes.GetNamedItem("value").Value = this.path2;//注意更改对应的服务器名，数据库名，登陆名和密码
        //           }
        //       }
        //   }
        //   xmlDocument.Save(FileInfo.FullName);


        //}
        //private int tickCounter = 0;
        //private void tmrTimersTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{ 
        //    System.Threading.Interlocked.Increment(ref tickCounter); 
        //    Thread.Sleep(5000);
        //}
        public void setInternal()
        {
            //double internalTime = Double.Parse(ConfigurationManager.AppSettings["internal"]);
            this.timer = new System.Timers.Timer(1000 * this.intervalTime);//实例化Timer类，设置时间间隔
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.copy);//当到达时间的时候执行copy事件 
            timer.AutoReset = true;//false是执行一次，true是一直执行
            timer.Enabled = true;//设置是否执行System.Timers.Timer.Elapsed事件 
        }
        public void removeInternal()
        {
            this.timer.Stop();
            this.timer.Close();
        }

        public void copy(object source, System.Timers.ElapsedEventArgs e)
        {
            //文件原来的地址
            string path1 = this.path1;
            //文件的目标目录
            string path2 = this.path2;
            //文件的全名
            string filename = "\\" + Path.GetFileName(path1);
            //获取文件名(不带扩展名)
            string filename2 = Path.GetFileNameWithoutExtension(path1);

            FileInfo fi = new FileInfo(path1);
            FileInfo fi2 = new FileInfo(path2 + filename);



            //判断目的地是否有重名文件，有则删除
            if (fi2.Exists)
            {
                fi2.Delete();
                Console.WriteLine("文件已存在!删除完成");
            }
            fi.CopyTo(path2 + filename);

            List<CSVData> csvList = this.read(path2 + filename);

            int size = filename2.IndexOf("日");
            string sql2 = null;
            if (filename2.Contains("_MOSFET_TEST"))
            {
                sql2 = "select count(*) from dbo.MOSFET_TEST where Time > convert(varchar(10), getdate(), 120)+' 00:00:00'";
                this.sqlReaderlExcutor(sql2);
                //查询数据库里面是否有数据,如果有数据
                if (!(this.dataCount == 0))
                {
                    //有数据则获取数据行数，根据数据行数从CSV文件相应的数据行读取数据
                    for (int i = this.dataCount - 1; i < csvList.Count-1; i++)
                    {
                        //闯将数据类型来保存数据
                        CSVData data = csvList[i];
                        //将文件名的年月日格式进行日期格式转化
                        //string filename3 = filename2.Replace("年", "-").Replace("月", "-").Replace("日", " ").Substring(0, size);
                        string[] a = filename2.Split('_');
                        StringBuilder sb = new StringBuilder();
                        sb.Append(a[0]);
                        sb.Append("-");
                        sb.Append(a[1]);
                        sb.Append("-");
                        sb.Append(a[2]);
                        sb.Append(" ");
                        string sql = "insert into dbo.MOSFET_TEST values('" + data.SN1 + "','" + data.TEST11 + "','" + data.TEST21 + "','" + data.TEST31 + "','" + data.TEST41 + "','" + data.Result1 + "','" + sb.ToString() + " " + data.Time1 + "','" + data.SNcheck1 + "')";
                        this.sqlExcutor(sql);
                    }
                }
                else
                {
                    //没有则遍历所有数据
                    foreach (CSVData data in csvList)
                    {
                        //string filename3 = filename2.Replace("年", "-").Replace("月", "-").Replace("日", " ").Substring(0, size);
                        string[] a = filename2.Split('_');
                        StringBuilder sb = new StringBuilder();
                        sb.Append(a[0]);
                        sb.Append("-");
                        sb.Append(a[1]);
                        sb.Append("-");
                        sb.Append(a[2]);
                        sb.Append(" ");
                        string sql = "insert into dbo.MOSFET_TEST values('" + data.SN1 + "','" + data.TEST11 + "','" + data.TEST21 + "','" + data.TEST31 + "','" + data.TEST41 + "','" + data.Result1 + "','" + sb.ToString() + " " + data.Time1 + "','" + data.SNcheck1 + "')";
                        this.sqlExcutor(sql);
                    }
                }
            }
            else if (filename2.Contains("_LOOP TEST"))//2018年3月1日_LOOP TEST
            {
                sql2 = "select count(*) from dbo.LOOP_TEST where TestTime > convert(varchar(10), getdate(), 120)+' 00:00:00'";
                this.sqlReaderlExcutor(sql2);
                //查询数据库里面是否有数据
                if (!(this.dataCount == 0))
                {
                    //有数据则获取数据行数，根据数据行数从CSV文件相应的数据行读取数据
                    for (int i = this.dataCount - 1; i < csvList.Count-1; i++)
                    {
                        //闯将数据类型来保存数据
                        CSVData data = csvList[i];
                        //将文件名的年月日格式进行日期格式转化
                        //string filename3 = filename2.Replace("年", "-").Replace("月", "-").Replace("日", " ").Substring(0, size);
                        string sql = "insert into dbo.LOOP_TEST values('" + data.TestTime1 + "','" + data.BarCode1 + "','" + data.TEST11 + "','" + data.TEST21 + "','" + data.TEST31 + "','" + data.TEST41 + "','" + data.Result1 + "')";
                        this.sqlExcutor(sql);
                    }
                }
                else
                {
                    //没有则遍历所有数据
                    foreach (CSVData data in csvList)
                    {
                        string[] a = data.TestTime1.Split('_');
                        StringBuilder sb = new StringBuilder();
                        sb.Append(a[0]);
                        sb.Append("-");
                        sb.Append(a[1]);
                        sb.Append("-");
                        sb.Append(a[2]);
                        sb.Append(" ");
                        sb.Append(a[3]);
                        sb.Append(":");
                        sb.Append(a[4]);
                        sb.Append(":");
                        sb.Append(a[5]);
                        string sql = "insert into dbo.LOOP_TEST values('" + sb.ToString() + "','" + data.BarCode1 + "','" + data.TEST11 + "','" + data.TEST21 + "','" + data.TEST31 + "','" + data.TEST41 + "','" + data.Result1 + "')";
                        this.sqlExcutor(sql);
                    }
                }
            }//3.光強測試
            else if (filename2.Contains("_Light Intensity TEST"))//20200728_Light Intensity TEST
            {
                sql2 = "select count(*) from dbo.Light_Intensity_TEST where TestTime > convert(varchar(10), getdate(), 120)+' 00:00:00' ";
                this.sqlReaderlExcutor(sql2);
                if (!(this.dataCount == 0))
                {
                    //有数据则获取数据行数，根据数据行数从CSV文件相应的数据行读取数据
                    for (int i = this.dataCount - 1; i < csvList.Count-1; i++)
                    {
                        //闯将数据类型来保存数据
                        CSVData data = csvList[i];
                        //将文件名的年月日格式进行日期格式转化
                        //string filename3 = filename2.Replace("年", "-").Replace("月", "-").Replace("日", " ").Substring(0, size);
                        string sql = "insert into dbo.Light_Intensity_TEST values('" + data.SN1 + "','" + data.Value1 + "','" + data.Result1 + "','" + data.TestTime1 + "')";
                        this.sqlExcutor(sql);
                    }
                }
                else
                {
                    //没有则遍历所有数据
                    foreach (CSVData data in csvList)
                    {
                        //string time = filename2.Replace("年", "-").Replace("月", "-").Replace("日", " ").Substring(0, size) + " " + data.TestTime1;
                        string sql = "insert into dbo.Light_Intensity_TEST values('" + data.SN1 + "','" + data.Value1 + "','" + data.Result1 + "','" + data.TestTime1 + "')";
                        this.sqlExcutor(sql);
                    }
                }

            }
        }

        //普通的数据库操作类
        public void sqlExcutor(string sql)
        {
            //连接数据库并插入数据
            string sqlSource = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
            SqlConnection conn = new SqlConnection(sqlSource);
            conn.Open();
            SqlCommand sc = new SqlCommand(sql, conn);
            //添加事务
            SqlTransaction st = conn.BeginTransaction();
            sc.Transaction = st;
            try
            {
                sc.ExecuteNonQuery();
                st.Commit();
            }
            catch
            {
                st.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }


        //读取数据库数据行数并操作数据库
        public void sqlReaderlExcutor(string sql)
        {
            //连接数据库并插入数据
            string sqlSource = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
            SqlConnection conn = new SqlConnection(sqlSource);
            conn.Open();
            SqlCommand sc = new SqlCommand(sql, conn);
            SqlTransaction st = conn.BeginTransaction();
            sc.Transaction = st;
            //添加事务
            SqlDataReader reader = sc.ExecuteReader();
            while (reader.Read())
            {
                //数据库里的行数
                this.dataCount = int.Parse(reader[0].ToString());
            }
            reader.Close();
            try
            {
                sc.ExecuteNonQuery();
                st.Commit();
            }
            catch
            {
                st.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }





        public List<CSVData> read(string filePath)
        {

            int size = filePath.IndexOf("日");
            this.stepName = filePath.Substring(size + 1, filePath.Length - size - 5);
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            string type = System.Configuration.ConfigurationManager.AppSettings["documentType"].ToString();
            string[] cNmae = null;//每列的列名
            List<CSVData> csvdataList = new List<CSVData>();

            //根据数据行号直接从第该行读取
            //if (sr.Peek() > this.dataCount)
            //{

            //}

            //逐行读取CSV中的数据
            while (!(string.IsNullOrEmpty(strLine = sr.ReadLine()) || strLine.Equals("\0\0\0")))
            {
                //strLine = Common.ConvertStringUTF8(strLine, encoding);
                //strLine = Common.ConvertStringUTF8(strLine);
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        cNmae = tableHead;
                        DataColumn dc = new DataColumn(tableHead[i]);
                        //dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    CSVData c = new CSVData();
                    if (filePath.Contains("_MOSFET_TEST"))
                    {
                        //将数据保存在CSVData数据类型中
                        c.SN1 = aryLine[0].Replace("\"", "").Replace("\t", "");
                        c.TEST11 = aryLine[1].Replace("\"\t", "").Replace("\t\"", "").Replace("\t", "");
                        c.TEST21 = aryLine[2].Replace("\"\t", "").Replace("\t\"", "").Replace("\t", "");
                        c.TEST31 = aryLine[3].Replace("\"\t", "").Replace("\t\"", "").Replace("\t", "");
                        c.TEST41 = aryLine[4].Replace("\"\t", "").Replace("\t\"", "").Replace("\t", "");
                        c.Result1 = aryLine[5].Replace("\"\t", "").Replace("\t\"", "").Replace("\t", "");
                        c.Time1 = aryLine[6].Replace("\"\t", "").Replace("\t\"", "").Replace("\t", "");
                        try
                        {
                            c.SNcheck1 = aryLine[7].Replace("\t", "");
                        }
                        catch (IndexOutOfRangeException)
                        {
                            c.SNcheck1 = "";
                        }
                        csvdataList.Add(c);
                    }
                    else if (filePath.Contains("_LOOP TEST"))
                    {
                        c.TestTime1 = aryLine[0].Replace("\0", "");
                        c.BarCode1 = aryLine[1].Replace("\0", "");
                        c.TEST11 = aryLine[2].Replace("\0", "");
                        c.TEST21 = aryLine[3].Replace("\0", "");
                        c.TEST31 = aryLine[4].Replace("\0", "");
                        c.TEST41 = aryLine[5].Replace("\0", "");
                        c.Result1 = aryLine[6].Replace("\0", "");
                        csvdataList.Add(c);
                    }
                    else if (filePath.Contains("_Light Intensity TEST"))
                    {
                        c.SN1 = aryLine[0].Replace("\0", "");
                        c.Value1 = aryLine[1].Replace("\0", "");
                        c.Result1 = aryLine[2].Replace("\0", "");
                        c.TestTime1 = aryLine[3].Replace("\0", "");
                        csvdataList.Add(c);
                    }
                }
            }
            sr.Close();
            fs.Close();
            return csvdataList;

        }

        private void SqlBulkCopyByDatatable(string connectionString, string TableName, DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlbulkcopy.DestinationTableName = TableName;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlbulkcopy.WriteToServer(dt);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}
