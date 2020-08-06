using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using TSCSFC.DB;
using System.Data;

namespace access.demo
{
    /// <summary>
    ///getData 的摘要描述
    /// </summary>
    public class getData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["mode"])
            {
                case "load":
                    load(context);
                    break;
                case "emplayee":
                    emplayee(context);
                    break;
                case "tijiao":
                    tijiao(context);
                    break;
                case "sbu":
                    sbu(context);
                    break;
                case "changqu":
                    changqu(context);
                    break;
                case "changqu1":
                    changqu1(context);
                    break;
                case "queren":
                    queren(context);
                    break;
                case "shuju":
                    shuju(context);
                    break;
                case "canshu":
                    canshu(context);
                    break;
                case "admin":
                    admin(context);
                    break;
                case "all":
                    all(context);
                    break;
            }
        }
        public void all(HttpContext context)
        {
            string sum="[";
            string[] GG = { "cable", "connector", "other", "ACET", "PDCT", "CMIT", "IBDC", "CW", "周邊", "FIT" };
            string mm;
            string load = load1(context);
            string shuju = shuju1(context);
            string dat2 = context.Request["dat1"];
            string dat20 = dat2.Substring(0, 7);
            string dat3 = context.Request["dat2"];
            string dat4 = context.Request["dat3"];
            string dddd = context.Request["dddd"];
            string[] dat = { dat2, dat3, dat4 };
            foreach (string dat1 in dat)
            {
                int ert1 = 0;
                int ert2 = 0;
                int ert3 = 0;
                int ert4 = 0;
                int ert5 = 0;
                int yue = 0;
                int ert12 = 0;
                int lizhi = 0;
                double bili = 0;
                double yuebili = 0;
                int count = 0;
                string ziwei;
                object dw1;
                object dw;
                object dk;
                foreach (string G in GG)
                {
                    count++;
                    DataTable dm;
                    SqlParameter[] or ={
                                    new SqlParameter ("@zhongli",G),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                    if (count > 3)
                    {
                        if (count > 9)
                        {
                            dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT ziwei,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by ziwei,lichang having lichang=@dat1", CommandType.Text, or));
                        }
                        else
                        {
                            dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT ziwei,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by ziwei,BU,lichang having BU=@zhongli and lichang=@dat1", CommandType.Text, or));
                        }
                    }
                    else
                    {
                        dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT leibie,ziwei,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by leibie,ziwei,lichang having leibie=@zhongli and lichang=@dat1", CommandType.Text, or));
                    }
                    if (dm.Rows.Count > 0)
                    {
                        for (int k = 0; k < dm.Rows.Count; k++)
                        {
                            ziwei = dm.Rows[k]["ziwei"].ToString();
                            if (ziwei.IndexOf("師") > -1)
                            {
                                ert3 = ert3 + Convert.ToInt32(dm.Rows[k]["shuliang"]);
                            }
                            else
                            {
                                if (ziwei.IndexOf("1") > -1)
                                {
                                    ert1 = ert1 + Convert.ToInt32(dm.Rows[k]["shuliang"]);
                                }
                                if (ziwei.IndexOf("2") > -1 || ziwei.IndexOf("3") > -1)
                                {
                                    ert2 = ert2 + Convert.ToInt32(dm.Rows[k]["shuliang"]);
                                }
                            }
                        }
                    }
                    else
                    {
                        ert1 = 0;
                        ert2 = 0;
                        ert3 = 0;
                    }
                    SqlParameter[] ot ={
                                    new SqlParameter ("@zhongli",G),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                    if (count > 3)
                    {
                        if (count > 9)
                        {
                            dw = DBHelper.ExecuteScalar("SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi having riqi = @dat1", CommandType.Text, ot);
                            dw1 = DBHelper.ExecuteScalar("SELECT sum(convert(int,A.zongshu))/count(*) FROM (SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi having riqi like '%" + dat20 + "%')A", CommandType.Text, ot);
                        }
                        else
                        {
                            dw = DBHelper.ExecuteScalar("SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,BU having riqi = @dat1 and BU=@zhongli", CommandType.Text, ot);
                            dw1 = DBHelper.ExecuteScalar("SELECT sum(convert(int,A.zongshu))/count(*) FROM (SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,BU having riqi like '%" + dat20 + "%' and BU=@zhongli)A", CommandType.Text, ot);
                        }
                    }
                    else
                    {
                        dw = DBHelper.ExecuteScalar("SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,leibie having riqi = @dat1 and leibie=@zhongli", CommandType.Text, ot);
                        dw1 = DBHelper.ExecuteScalar("SELECT sum(convert(int,A.zongshu))/count(*) FROM (SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,leibie having riqi like '%" + dat20 + "%' and leibie=@zhongli)A", CommandType.Text, ot);
                    }
                    
                    SqlParameter[] os ={
                                    new SqlParameter ("@zhongli",G),
                                    new SqlParameter ("@dat1",dat1),
                                    new SqlParameter ("@dddd",dddd)
                                            };
                    if (count > 3)
                    {
                        if (count > 9)
                        {
                            dk = DBHelper.ExecuteScalar("SELECT COUNT(*) as zongshu  FROM FIT_LEAVE where lichang between @dddd and @dat1", CommandType.Text, os);
                        }
                        else
                        {
                            dk = DBHelper.ExecuteScalar("SELECT COUNT(*) as zongshu  FROM FIT_LEAVE where lichang between @dddd and @dat1 and BU=@zhongli", CommandType.Text, os);
                        }
                    }
                    else
                    {
                        dk = DBHelper.ExecuteScalar("SELECT COUNT(*) as zongshu  FROM FIT_LEAVE where lichang between @dddd and @dat1 and leibie=@zhongli", CommandType.Text, os);
                    }
                    
                    yue = Convert.ToInt32(dk);
                    ert4 = Convert.ToInt32(dw);
                    if (Convert.IsDBNull(dw1))
                    {
                        ert5 = 0;
                    }
                    else
                    {
                        ert5 = Convert.ToInt32(dw1);
                    }
                    ert12 = ert1 + ert2;
                    lizhi = ert12 + ert3;
                    bili = Convert.ToDouble(lizhi) / (lizhi + ert4);
                    yuebili = Convert.ToDouble(yue) / (yue + ert5);
                    sum += "{\"yuan1\":\"" + ert1 + "\",\"yuan23\":\"" + ert2 + "\",\"zhongli\":\"" + G + "\",\"dat1\":\"" + dat1 + "\",\"yuan\":\"" + ert12 + "\",\"shi\":\"" + ert3 + "\",\"lizhi\":\"" + lizhi + "\",\"bili\":\"" + Math.Round(bili, 4) * 100 + '%' + "\",\"biaozhun\":\"" + "0.15%" + "\",\"sum\":\"" + ert4 + "\",\"sum1\":\"" + ert5 + "\",\"yue\":\"" + yue + "\",\"yuebili\":\"" + Math.Round(yuebili, 4) * 100 + '%' + "\"},";
                    ert1 = 0;
                    ert2 = 0;
                    ert3 = 0;
                }
            }
            sum = sum.Substring(0, sum.Length - 1);
            sum += "]";
            mm = "[";
            mm = mm + load + "," + shuju + "," + sum;
            mm = mm + "]";
            context.Response.Write(mm);
        }
        public void admin(HttpContext context)
        {
            string mm;
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select distinct(originalFactory) as TEXT from hr_employee_fact"));
            if (dt.Rows.Count > 0)
            {
                mm = "[";
                mm = mm + DBHelper.DataTable2Json(dt);
                mm = mm + "]";
            }
            else
            {
                mm = "NG";
            }
            context.Response.Write(mm);
        }
        public void canshu(HttpContext context)
        {
            string changqu = context.Request["changqu"];
            string sbu = context.Request["sbu"];
            string lichang = context.Request["lichang"];
            string ziwei = context.Request["ziwei"];
            string mm;
            SqlParameter[] om ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@lichang",lichang),
                                    new SqlParameter ("@ziwei",ziwei)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select * from FIT_LEAVE where sbu=@sbu and changqu=@changqu and lichang=@lichang and ziwei like @ziwei", CommandType.Text, om));
            if (dt.Rows.Count > 0)
            {
                mm = "[";
                mm = mm + DBHelper.DataTable2Json(dt);
                mm = mm + "]";
            }
            else
            {
                mm = "NG";
            }
            context.Response.Write(mm);
        }
        public void queren(HttpContext context)
        {
            bool mm = false;
            string changqu = context.Request["changqu0"];
            string xingzhi = context.Request["xingzhi0"];
            string leibie = context.Request["leibie0"];
            string bu = context.Request["bu0"];
            string sbu = context.Request["sbu0"];
            string zongfuze = context.Request["zongfuze0"];
            string zongganshi = context.Request["zongganshi0"];
            string zongji = context.Request["zongji0"];
            string ganji = context.Request["ganji0"];
            SqlParameter[] om ={
                                    new SqlParameter ("@sbu",sbu)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select * from FIT_SBU where sbu=@sbu", CommandType.Text, om));
            SqlParameter[] ox ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@xingzhi",xingzhi),
                                    new SqlParameter ("@leibie",leibie),
                                    new SqlParameter ("@bu",bu),
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@zongfuze",zongfuze),
                                    new SqlParameter ("@zongganshi",zongganshi),
                                    new SqlParameter ("@zongji",zongji),
                                    new SqlParameter ("@ganji",ganji)
                                            };
            if (dt.Rows.Count > 0)
            {
                int count = DBHelper.ExecuteNonQuery("update FIT_SBU set changqu=@changqu,xingzhi=@xingzhi,leibie=@leibie,bu=@bu,zongfuze=@zongfuze,zongganshi=@zongganshi,zongji=@zongji,ganji=@ganji where sbu=@sbu", CommandType.Text, ox);
                if (count > 0)
                {
                    mm = true;
                }
            }
            else
            {
                int count = DBHelper.ExecuteNonQuery("insert into FIT_SBU values(@changqu,@xingzhi,@leibie,@bu,@sbu,@zongfuze,@zongji,@zongganshi,@ganji)", CommandType.Text, ox);
                if (count > 0)
                {
                    mm = true;
                }
            }
            if (!mm)
            {
                context.Response.Write("NG");
            }
            else
            {
                context.Response.Write("OK");
            }
        }
        public void changqu1(HttpContext context)
        {
            string mm;
            string changqu = context.Request["changqu"];
            string bu = context.Request["bu"];
            SqlParameter[] om ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@bu",bu)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select distinct(sbu) as TEXT from FIT_SBU where changqu=@changqu and bu=@bu", CommandType.Text, om));
            if (dt.Rows.Count > 0)
            {
                mm = "[";
                mm = mm + DBHelper.DataTable2Json(dt);
                mm = mm + "]";
            }
            else
            {
                mm = "NG";
            }
            context.Response.Write(mm);
        }
        public void changqu(HttpContext context)
        {
            string mm;
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select distinct(bu) as TEXT from FIT_SBU"));
            if (dt.Rows.Count > 0)
            {
                mm = "[";
                mm = mm + DBHelper.DataTable2Json(dt);
                mm = mm + "]";
            }
            else
            {
                mm = "NG";
            }
            context.Response.Write(mm);
        }
        public void sbu(HttpContext context)
        {
            string mm;
            string sbu = context.Request["sbu"];
            string changqu = context.Request["changqu"];
            SqlParameter[] om ={
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@changqu",changqu)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select * from FIT_SBU where sbu=@sbu and changqu=@changqu", CommandType.Text, om));
            if (dt.Rows.Count > 0)
            {
                mm = "[";
                mm = mm + DBHelper.DataTable2Json(dt);
                mm = mm + "]";
            }
            else 
            {
                mm = "NG";
            }
            context.Response.Write(mm);
        }
        public void tijiao(HttpContext context)
        {
            bool status = false;
            string gonghao = context.Request["gonghao"];
            string xingming = context.Request["xingming"];
            string ziwei = context.Request["ziwei"];
            string changqu = context.Request["changqu"];
            string xingzhi = context.Request["xingzhi"];
            string leibie = context.Request["leibie"];
            string bu = context.Request["bu"];
            string sbu = context.Request["sbu"];
            string ruchang = context.Request["ruchang"];
            string lichang = context.Request["lichang"];
            string yuanying = context.Request["yuanying"];
            SqlParameter[] om ={
                                    new SqlParameter ("@gonghao",gonghao)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select * from FIT_LEAVE where gonghao=@gonghao", CommandType.Text, om));
            SqlParameter[] op ={
                                    new SqlParameter ("@gonghao",gonghao),
                                    new SqlParameter ("@xingming",xingming),
                                    new SqlParameter ("@ziwei",ziwei),
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@xingzhi",xingzhi),
                                    new SqlParameter ("@leibie",leibie),
                                    new SqlParameter ("@bu",bu),
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@ruchang",ruchang),
                                    new SqlParameter ("@lichang",lichang),
                                    new SqlParameter ("@yuanying",yuanying)
                                            };
            if (dt.Rows.Count > 0)
            {
                int count = DBHelper.ExecuteNonQuery("update FIT_LEAVE set xingming=@xingming,ziwei=@ziwei,changqu=@changqu,xingzhi=@xingzhi,leibie=@leibie,bu=@bu,sbu=@sbu,ruchang=@ruchang,lichang=@lichang,yuanying=@yuanying where gonghao=@gonghao", CommandType.Text, op);
                if (count > 0)
                {
                    status = true;
                }
            }
            else
            {
                int count = DBHelper.ExecuteNonQuery("insert into FIT_LEAVE values(@gonghao,@xingming,@ziwei,@changqu,@xingzhi,@leibie,@bu,@sbu,@ruchang,@lichang,@yuanying)", CommandType.Text, op);
                if (count > 0)
                {
                    status = true;
                }
            }
            if (!status)
            {
                context.Response.Write("NG");
            }
        }
        public void emplayee(HttpContext context)
        {
            string changqu = context.Request["changqu"];
            string empno = context.Request["empno"];
            string mm;
            SqlParameter[] op ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@empno",empno)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select * from FIT_LEAVE where changqu=@changqu and gonghao=@empno", CommandType.Text, op));
            if (dt.Rows.Count > 0)
            {
                mm = "[";
                mm = mm + DBHelper.DataTable2Json(dt);
                mm = mm + "]";
            }
            else
            {
                SqlParameter[] om ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@empno",empno)
                                            };
                dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select employeeID_cpf01 as gonghao,employeeName_cpf02 as xingming,gradeCode_cpf10 as ziwei,originalFactory as changqu, '' as xingzhi,'' as leibie,BUIndustry as bu,BU_cpf63_na as sbu,inauguralDate_cpf69 as ruchang,quitDate_cpf35 as lichang,'' as yuanying from hr_employee_fact where employeeID_cpf01=@empno", CommandType.Text, om));
                string sbu = dt.Rows[0][7].ToString();
                SqlParameter[] ot ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@sbu",sbu)
                                            };
                DataTable dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select xingzhi,leibie from FIT_SBU where changqu=@changqu and sbu=@sbu", CommandType.Text, ot));
                if (dm.Rows.Count > 0)
                {
                    dt.Rows[0][4] = dm.Rows[0][0];
                    dt.Rows[0][5] = dm.Rows[0][1];
                }
                if (dt.Rows.Count > 0)
                {
                    mm = "[";
                    mm = mm + DBHelper.DataTable2Json(dt);
                    mm = mm + "]";
                }
                else
                {
                    mm = "NG";
                }
            }
            context.Response.Write(mm);
        }
        public void load(HttpContext context)
        {
            string mm;
            string changqu = context.Request["changqu"];
            SqlParameter[] op ={
                                    new SqlParameter ("@changqu",changqu)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select * from FIT_SBU where changqu=@changqu order by xingzhi desc", CommandType.Text, op));
            if (dt.Rows.Count > 0)
            {
                mm = "[";
                mm = mm + DBHelper.DataTable2Json(dt);
                mm = mm + "]";
            }
            else
            {
                mm = "NG";
            }
            context.Response.Write(mm);
        }

        public string load1(HttpContext context)
        {
            string mm;
            string changqu = context.Request["changqu"];
            SqlParameter[] op ={
                                    new SqlParameter ("@changqu",changqu)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select * from FIT_SBU where changqu=@changqu order by xingzhi desc", CommandType.Text, op));
            if (dt.Rows.Count > 0)
            {
                mm = "[";
                mm = mm + DBHelper.DataTable2Json(dt);
                mm = mm + "]";
            }
            else
            {
                mm = "NG";
            }
            return mm;
        }

        public string shuju1(HttpContext context)
        {
            string changqu = context.Request["changqu"];
            string dat2 = context.Request["dat1"];
            string dat20 = dat2.Substring(0, 7);
            string dat3 = context.Request["dat2"];
            string dat4 = context.Request["dat3"];
            string dddd = context.Request["dddd"];
            string[] dat = { dat2, dat3, dat4 };
            bool status = true;
            string datm;
            List<string> dq = new List<string>();
            List<string> da = new List<string>();

            SqlParameter[] op ={
                                    new SqlParameter ("@changqu",changqu)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select sbu,leibie,bu from FIT_SBU where changqu=@changqu order by xingzhi desc", CommandType.Text, op));
            if (dt.Rows.Count > 0)
            {
                datm = "[";
                int count = 1;
                foreach (string dat1 in dat)
                {
                    string sbu;
                    string leibie;
                    string bu;
                    string d1 = "[";
                    int ert1 = 0;
                    int ert2 = 0;
                    int ert3 = 0;
                    int ert4 = 0;
                    int ert5 = 0;
                    int yue = 0;
                    int ert12 = 0;
                    int lizhi = 0;
                    double bili = 0;
                    double yuebili = 0;
                    shumu mu = new shumu();
                    string ziwei;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sbu = dt.Rows[i][0].ToString();
                        if (status)
                        {
                            if (!dq.Contains(dt.Rows[i][1].ToString()))
                            {
                                dq.Add(dt.Rows[i][1].ToString());
                            }
                            if (!da.Contains(dt.Rows[i][2].ToString()))
                            {
                                da.Add(dt.Rows[i][2].ToString());
                            }
                        }
                        SqlParameter[] or ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        DataTable dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT sbu,ziwei,changqu,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by sbu,ziwei,changqu,lichang having changqu=@changqu and sbu=@sbu and lichang=@dat1", CommandType.Text, or));
                        if (dm.Rows.Count > 0)
                        {
                            for (int k = 0; k < dm.Rows.Count; k++)
                            {
                                ziwei = dm.Rows[k][1].ToString();
                                if (ziwei.IndexOf("師") > -1)
                                {
                                    ert3 = ert3 + Convert.ToInt32(dm.Rows[k][3]);
                                }
                                else
                                {
                                    if (ziwei.IndexOf("1") > -1)
                                    {
                                        ert1 = ert1 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                    if (ziwei.IndexOf("2") > -1 || ziwei.IndexOf("3") > -1)
                                    {
                                        ert2 = ert2 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ert1 = 0;
                            ert2 = 0;
                            ert3 = 0;
                        }
                        SqlParameter[] ot ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        object dw = DBHelper.ExecuteScalar("SELECT zai FROM FIT_DEL where riqi = @dat1 and changqu=@changqu and sbu=@sbu", CommandType.Text, ot);
                        object dw1 = DBHelper.ExecuteScalar("SELECT sum(convert(int,zai))/count(*) FROM FIT_DEL where riqi like '%" + dat20 + "%' and changqu=@changqu and sbu=@sbu", CommandType.Text, ot);
                        SqlParameter[] os ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@dat1",dat1),
                                    new SqlParameter ("@dddd",dddd)
                                            };
                        object dk = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM FIT_LEAVE where lichang between @dddd and @dat1 and changqu=@changqu and sbu=@sbu", CommandType.Text, os);
                        yue = Convert.ToInt32(dk);
                        if (dw == null)
                        {
                            ert4 = 0;
                        }
                        else
                        {
                            ert4 = Convert.ToInt32(dw);
                        }
                        if (Convert.IsDBNull(dw1))
                        {
                            ert5 = 0;
                        }
                        else
                        {
                            ert5 = Convert.ToInt32(dw1);
                        }
                        ert12 = ert1 + ert2;
                        lizhi = ert12 + ert3;
                        bili = Convert.ToDouble(lizhi) / (lizhi + ert4);
                        yuebili = Convert.ToDouble(yue) / (yue + ert5);
                        d1 += "{\"yuan1\":\"" + ert1 + "\",\"yuan23\":\"" + ert2 + "\",\"changqu\":\"" + changqu + "\",\"sbu\":\"" + sbu + "\",\"dat1\":\"" + dat1 + "\",\"yuan\":\"" + ert12 + "\",\"shi\":\"" + ert3 + "\",\"lizhi\":\"" + lizhi + "\",\"bili\":\"" + Math.Round(bili, 4) * 100 + '%' + "\",\"biaozhun\":\"" + "0.15%" + "\",\"sum\":\"" + ert4 + "\",\"sum1\":\"" + ert5 + "\",\"yue\":\"" + yue + "\",\"yuebili\":\"" + Math.Round(yuebili, 4) * 100 + '%' + "\"},";
                        ert1 = 0;
                        ert2 = 0;
                        ert3 = 0;
                    }
                    status = false;
                    foreach (string leibie9 in dq)
                    {
                        leibie = leibie9;
                        SqlParameter[] or ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@leibie",leibie),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        DataTable dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT leibie,ziwei,changqu,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by leibie,ziwei,changqu,lichang having changqu=@changqu and leibie=@leibie and lichang=@dat1", CommandType.Text, or));

                        if (dm.Rows.Count > 0)
                        {
                            for (int k = 0; k < dm.Rows.Count; k++)
                            {
                                ziwei = dm.Rows[k][1].ToString();
                                if (ziwei.IndexOf("師") > -1)
                                {
                                    ert3 = ert3 + Convert.ToInt32(dm.Rows[k][3]);
                                }
                                else
                                {
                                    if (ziwei.IndexOf("1") > -1)
                                    {
                                        ert1 = ert1 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                    if (ziwei.IndexOf("2") > -1 || ziwei.IndexOf("3") > -1)
                                    {
                                        ert2 = ert2 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ert1 = 0;
                            ert2 = 0;
                            ert3 = 0;
                        }
                        SqlParameter[] ot ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@leibie",leibie),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        object dw = DBHelper.ExecuteScalar("SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu,leibie having riqi = @dat1 and changqu=@changqu and leibie=@leibie", CommandType.Text, ot);
                        object dw1 = DBHelper.ExecuteScalar("SELECT sum(convert(int,A.zongshu))/count(*) FROM (SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu,leibie having riqi like '%" + dat20 + "%' and changqu=@changqu and leibie=@leibie)A", CommandType.Text, ot);
                        SqlParameter[] os ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@leibie",leibie),
                                    new SqlParameter ("@dat1",dat1),
                                    new SqlParameter ("@dddd",dddd)
                                            };
                        object dk = DBHelper.ExecuteScalar("SELECT COUNT(*) as zongshu  FROM FIT_LEAVE where lichang between @dddd and @dat1 and changqu=@changqu and leibie=@leibie", CommandType.Text, os);
                        yue = Convert.ToInt32(dk);
                        if (dw == null)
                        {
                            ert4 = 0;
                        }
                        else
                        {
                            ert4 = Convert.ToInt32(dw);
                        }
                        if (Convert.IsDBNull(dw1))
                        {
                            ert5 = 0;
                        }
                        else
                        {
                            ert5 = Convert.ToInt32(dw1);
                        }
                        ert12 = ert1 + ert2;
                        lizhi = ert12 + ert3;
                        bili = Convert.ToDouble(lizhi) / (lizhi + ert4);
                        yuebili = Convert.ToDouble(yue) / (yue + ert5);
                        d1 += "{\"yuan1\":\"" + ert1 + "\",\"yuan23\":\"" + ert2 + "\",\"changqu\":\"" + changqu + "\",\"leibie\":\"" + leibie + "\",\"dat1\":\"" + dat1 + "\",\"yuan\":\"" + ert12 + "\",\"shi\":\"" + ert3 + "\",\"lizhi\":\"" + lizhi + "\",\"bili\":\"" + Math.Round(bili, 4) * 100 + '%' + "\",\"biaozhun\":\"" + "0.15%" + "\",\"sum\":\"" + ert4 + "\",\"sum1\":\"" + ert5 + "\",\"yue\":\"" + yue + "\",\"yuebili\":\"" + Math.Round(yuebili, 4) * 100 + '%' + "\"},";
                        ert1 = 0;
                        ert2 = 0;
                        ert3 = 0;
                    }
                    foreach (string bu9 in da)
                    {
                        bu = bu9;
                        SqlParameter[] or ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@bu",bu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        DataTable dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT bu,ziwei,changqu,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by bu,ziwei,changqu,lichang having changqu=@changqu and bu=@bu and lichang=@dat1", CommandType.Text, or));
                        if (dm.Rows.Count > 0)
                        {
                            for (int k = 0; k < dm.Rows.Count; k++)
                            {
                                ziwei = dm.Rows[k][1].ToString();
                                if (ziwei.IndexOf("師") > -1)
                                {
                                    ert3 = ert3 + Convert.ToInt32(dm.Rows[k][3]);
                                }
                                else
                                {
                                    if (ziwei.IndexOf("1") > -1)
                                    {
                                        ert1 = ert1 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                    if (ziwei.IndexOf("2") > -1 || ziwei.IndexOf("3") > -1)
                                    {
                                        ert2 = ert2 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ert1 = 0;
                            ert2 = 0;
                            ert3 = 0;
                        }
                        SqlParameter[] ot ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@bu",bu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        object dw = DBHelper.ExecuteScalar("SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu,bu having riqi = @dat1 and changqu=@changqu and bu=@bu", CommandType.Text, ot);
                        object dw1 = DBHelper.ExecuteScalar("SELECT sum(convert(int,A.zongshu))/count(*) FROM (SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu,bu having riqi  like '%" + dat20 + "%' and changqu=@changqu and bu=@bu)A", CommandType.Text, ot);
                        SqlParameter[] os ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@bu",bu),
                                    new SqlParameter ("@dat1",dat1),
                                    new SqlParameter ("@dddd",dddd)
                                            };
                        object dk = DBHelper.ExecuteScalar("SELECT COUNT(*) as zongshu  FROM FIT_LEAVE where lichang between @dddd and @dat1 and changqu=@changqu and bu=@bu", CommandType.Text, os);
                        yue = Convert.ToInt32(dk);
                        ert4 = Convert.ToInt32(dw);
                        if (Convert.IsDBNull(dw1))
                        {
                            ert5 = 0;
                        }
                        else
                        {
                            ert5 = Convert.ToInt32(dw1);
                        }
                        ert12 = ert1 + ert2;
                        lizhi = ert12 + ert3;
                        bili = Convert.ToDouble(lizhi) / (lizhi + ert4);
                        yuebili = Convert.ToDouble(yue) / (yue + ert4);
                        d1 += "{\"yuan1\":\"" + ert1 + "\",\"yuan23\":\"" + ert2 + "\",\"changqu\":\"" + changqu + "\",\"leibie\":\"" + bu + "\",\"dat1\":\"" + dat1 + "\",\"yuan\":\"" + ert12 + "\",\"shi\":\"" + ert3 + "\",\"lizhi\":\"" + lizhi + "\",\"bili\":\"" + Math.Round(bili, 4) * 100 + '%' + "\",\"biaozhun\":\"" + "0.15%" + "\",\"sum\":\"" + ert4 + "\",\"sum1\":\"" + ert5 + "\",\"yue\":\"" + yue + "\",\"yuebili\":\"" + Math.Round(yuebili, 4) * 100 + '%' + "\"},";
                        ert1 = 0;
                        ert2 = 0;
                        ert3 = 0;
                    }
                    SqlParameter[] or9 ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                    DataTable dm9 = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT bu,ziwei,changqu,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by bu,ziwei,changqu,lichang having changqu=@changqu and lichang=@dat1", CommandType.Text, or9));
                    if (dm9.Rows.Count > 0)
                    {
                        for (int k = 0; k < dm9.Rows.Count; k++)
                        {
                            ziwei = dm9.Rows[k][1].ToString();
                            if (ziwei.IndexOf("師") > -1)
                            {
                                ert3 = ert3 + Convert.ToInt32(dm9.Rows[k][3]);
                            }
                            else
                            {
                                if (ziwei.IndexOf("1") > -1)
                                {
                                    ert1 = ert1 + Convert.ToInt32(dm9.Rows[k][3]);
                                }
                                if (ziwei.IndexOf("2") > -1 || ziwei.IndexOf("3") > -1)
                                {
                                    ert2 = ert2 + Convert.ToInt32(dm9.Rows[k][3]);
                                }
                            }
                        }
                    }
                    else
                    {
                        ert1 = 0;
                        ert2 = 0;
                        ert3 = 0;
                    }
                    SqlParameter[] ot9 ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                    object dw9 = DBHelper.ExecuteScalar("SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu having riqi = @dat1 and changqu=@changqu", CommandType.Text, ot9);
                    object dw90 = DBHelper.ExecuteScalar("SELECT sum(convert(int,A.zongshu))/count(*) FROM (SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu having riqi like '%" + dat20 + "%' and changqu=@changqu)A", CommandType.Text, ot9);
                    SqlParameter[] os9 ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@dat1",dat1),
                                    new SqlParameter ("@dddd",dddd)
                                            };
                    object dk9 = DBHelper.ExecuteScalar("SELECT COUNT(*) as zongshu  FROM FIT_LEAVE where lichang between @dddd and @dat1 and changqu=@changqu", CommandType.Text, os9);
                    yue = Convert.ToInt32(dk9);
                    if (dw9 == null)
                    {
                        ert4 = 0;
                    }
                    else
                    {
                        ert4 = Convert.ToInt32(dw9);
                    }
                    if (Convert.IsDBNull(dw90))
                    {
                        ert5 = 0;
                    }
                    else
                    {
                        ert5 = Convert.ToInt32(dw90);
                    }
                    ert12 = ert1 + ert2;
                    lizhi = ert12 + ert3;
                    bili = Convert.ToDouble(lizhi) / (lizhi + ert4);
                    yuebili = Convert.ToDouble(yue) / (yue + ert4);
                    d1 += "{\"yuan1\":\"" + ert1 + "\",\"yuan23\":\"" + ert2 + "\",\"changqu\":\"" + changqu + "\",\"leibie\":\"" + changqu + "\",\"dat1\":\"" + dat1 + "\",\"yuan\":\"" + ert12 + "\",\"shi\":\"" + ert3 + "\",\"lizhi\":\"" + lizhi + "\",\"bili\":\"" + Math.Round(bili, 4) * 100 + '%' + "\",\"biaozhun\":\"" + "0.15%" + "\",\"sum\":\"" + ert4 + "\",\"sum1\":\"" + ert5 + "\",\"yue\":\"" + yue + "\",\"yuebili\":\"" + Math.Round(yuebili, 4) * 100 + '%' + "\"}";
                    d1 += "]";
                    datm += "{\"" + 'm' + count.ToString() + "\":" + d1 + "},";
                    count++;
                }
                datm = datm.Substring(0, datm.Length - 1);
                datm += "]";
            }
            else
            {
                datm = "NG";
            }

            return datm;
        }

        public void shuju(HttpContext context)
        {
            string changqu = context.Request["changqu"];
            string dat2 = context.Request["dat1"];
            string dat20 = dat2.Substring(0, 7);
            string dat3 = context.Request["dat2"];
            string dat4 = context.Request["dat3"];
            string dddd = context.Request["dddd"];
            string[] dat = { dat2, dat3, dat4 };
            bool status = true;
            string datm;
            List<string> dq = new List<string>();
            List<string> da = new List<string>();

            SqlParameter[] op ={
                                    new SqlParameter ("@changqu",changqu)
                                            };
            DataTable dt = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("select sbu,leibie,bu from FIT_SBU where changqu=@changqu order by xingzhi desc", CommandType.Text, op));
            if (dt.Rows.Count > 0)
            {
                datm = "[";
                int count = 1;
                foreach (string dat1 in dat)
                {
                    string sbu;
                    string leibie;
                    string bu;
                    string d1 = "[";
                    int ert1 = 0;
                    int ert2 = 0;
                    int ert3 = 0;
                    int ert4 = 0;
                    int ert5 = 0;
                    int yue = 0;
                    int ert12 = 0;
                    int lizhi = 0;
                    double bili = 0;
                    double yuebili = 0;
                    shumu mu = new shumu();
                    string ziwei;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sbu = dt.Rows[i][0].ToString();
                        if (status)
                        {
                            if (!dq.Contains(dt.Rows[i][1].ToString()))
                            {
                                dq.Add(dt.Rows[i][1].ToString());
                            }
                            if (!da.Contains(dt.Rows[i][2].ToString()))
                            {
                                da.Add(dt.Rows[i][2].ToString());
                            }
                        }
                        SqlParameter[] or ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        DataTable dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT sbu,ziwei,changqu,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by sbu,ziwei,changqu,lichang having changqu=@changqu and sbu=@sbu and lichang=@dat1", CommandType.Text, or));
                        if (dm.Rows.Count > 0)
                        {
                            for (int k = 0; k < dm.Rows.Count; k++)
                            {
                                ziwei = dm.Rows[k][1].ToString();
                                if (ziwei.IndexOf("師") > -1)
                                {
                                    ert3 = ert3 + Convert.ToInt32(dm.Rows[k][3]);
                                }
                                else
                                {
                                    if (ziwei.IndexOf("1") > -1)
                                    {
                                        ert1 = ert1 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                    if (ziwei.IndexOf("2") > -1 || ziwei.IndexOf("3") > -1)
                                    {
                                        ert2 = ert2 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ert1 = 0;
                            ert2 = 0;
                            ert3 = 0;
                        }
                        SqlParameter[] ot ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        object dw = DBHelper.ExecuteScalar("SELECT zai FROM FIT_DEL where riqi = @dat1 and changqu=@changqu and sbu=@sbu", CommandType.Text, ot);
                        object dw1 = DBHelper.ExecuteScalar("SELECT sum(convert(int,zai))/count(*) FROM FIT_DEL where riqi like '%"+dat20+"%' and changqu=@changqu and sbu=@sbu", CommandType.Text, ot);
                        SqlParameter[] os ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@sbu",sbu),
                                    new SqlParameter ("@dat1",dat1),
                                    new SqlParameter ("@dddd",dddd)
                                            };
                        object dk = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM FIT_LEAVE where lichang between @dddd and @dat1 and changqu=@changqu and sbu=@sbu", CommandType.Text, os);
                        yue = Convert.ToInt32(dk);
                        if (dw == null)
                        {
                            ert4 = 0;
                        }
                        else
                        {
                            ert4 = Convert.ToInt32(dw);
                        }
                        if (Convert.IsDBNull(dw1))
                        {
                            ert5 = 0;
                        }
                        else
                        {
                            ert5 = Convert.ToInt32(dw1);
                        }
                        ert12 = ert1 + ert2;
                        lizhi = ert12 + ert3;
                        bili = Convert.ToDouble(lizhi) / (lizhi + ert4);
                        yuebili = Convert.ToDouble(yue) / (yue + ert5);
                        d1 += "{\"yuan1\":\"" + ert1 + "\",\"yuan23\":\"" + ert2 + "\",\"changqu\":\"" + changqu + "\",\"sbu\":\"" + sbu + "\",\"dat1\":\"" + dat1 + "\",\"yuan\":\"" + ert12 + "\",\"shi\":\"" + ert3 + "\",\"lizhi\":\"" + lizhi + "\",\"bili\":\"" + Math.Round(bili, 4) * 100 + '%' + "\",\"biaozhun\":\"" + "0.15%" + "\",\"sum\":\"" + ert4 + "\",\"sum1\":\"" + ert5 + "\",\"yue\":\"" + yue + "\",\"yuebili\":\"" + Math.Round(yuebili, 4) * 100 + '%' + "\"},";
                        ert1 = 0;
                        ert2 = 0;
                        ert3 = 0;
                    }
                    status = false;
                    foreach (string leibie9 in dq)
                    {
                        leibie = leibie9;
                        SqlParameter[] or ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@leibie",leibie),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        DataTable dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT leibie,ziwei,changqu,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by leibie,ziwei,changqu,lichang having changqu=@changqu and leibie=@leibie and lichang=@dat1", CommandType.Text, or));

                        if (dm.Rows.Count > 0)
                        {
                            for (int k = 0; k < dm.Rows.Count; k++)
                            {
                                ziwei = dm.Rows[k][1].ToString();
                                if (ziwei.IndexOf("師") > -1)
                                {
                                    ert3 = ert3 + Convert.ToInt32(dm.Rows[k][3]);
                                }
                                else
                                {
                                    if (ziwei.IndexOf("1") > -1)
                                    {
                                        ert1 = ert1 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                    if (ziwei.IndexOf("2") > -1 || ziwei.IndexOf("3") > -1)
                                    {
                                        ert2 = ert2 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ert1 = 0;
                            ert2 = 0;
                            ert3 = 0;
                        }
                        SqlParameter[] ot ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@leibie",leibie),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        object dw = DBHelper.ExecuteScalar("SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu,leibie having riqi = @dat1 and changqu=@changqu and leibie=@leibie", CommandType.Text, ot);
                        object dw1 = DBHelper.ExecuteScalar("SELECT sum(convert(int,A.zongshu))/count(*) FROM (SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu,leibie having riqi like '%" + dat20 + "%' and changqu=@changqu and leibie=@leibie)A", CommandType.Text, ot);
                        SqlParameter[] os ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@leibie",leibie),
                                    new SqlParameter ("@dat1",dat1),
                                    new SqlParameter ("@dddd",dddd)
                                            };
                        object dk = DBHelper.ExecuteScalar("SELECT COUNT(*) as zongshu  FROM FIT_LEAVE where lichang between @dddd and @dat1 and changqu=@changqu and leibie=@leibie", CommandType.Text, os);
                        yue = Convert.ToInt32(dk);
                        if (dw == null)
                        {
                            ert4 = 0;
                        }
                        else
                        {
                            ert4 = Convert.ToInt32(dw);
                        }
                        if (Convert.IsDBNull(dw1))
                        {
                            ert5 = 0;
                        }
                        else
                        {
                            ert5 = Convert.ToInt32(dw1);
                        }
                        ert12 = ert1 + ert2;
                        lizhi = ert12 + ert3;
                        bili = Convert.ToDouble(lizhi) / (lizhi + ert4);
                        yuebili = Convert.ToDouble(yue) / (yue + ert5);
                        d1 += "{\"yuan1\":\"" + ert1 + "\",\"yuan23\":\"" + ert2 + "\",\"changqu\":\"" + changqu + "\",\"leibie\":\"" + leibie + "\",\"dat1\":\"" + dat1 + "\",\"yuan\":\"" + ert12 + "\",\"shi\":\"" + ert3 + "\",\"lizhi\":\"" + lizhi + "\",\"bili\":\"" + Math.Round(bili, 4) * 100 + '%' + "\",\"biaozhun\":\"" + "0.15%" + "\",\"sum\":\"" + ert4 + "\",\"sum1\":\"" + ert5 + "\",\"yue\":\"" + yue + "\",\"yuebili\":\"" + Math.Round(yuebili, 4) * 100 + '%' + "\"},";
                        ert1 = 0;
                        ert2 = 0;
                        ert3 = 0;
                    }
                    foreach (string bu9 in da)
                    {
                        bu = bu9;
                        SqlParameter[] or ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@bu",bu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        DataTable dm = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT bu,ziwei,changqu,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by bu,ziwei,changqu,lichang having changqu=@changqu and bu=@bu and lichang=@dat1", CommandType.Text, or));
                        if (dm.Rows.Count > 0)
                        {
                            for (int k = 0; k < dm.Rows.Count; k++)
                            {
                                ziwei = dm.Rows[k][1].ToString();
                                if (ziwei.IndexOf("師") > -1)
                                {
                                    ert3 = ert3 + Convert.ToInt32(dm.Rows[k][3]);
                                }
                                else
                                {
                                    if (ziwei.IndexOf("1") > -1)
                                    {
                                        ert1 = ert1 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                    if (ziwei.IndexOf("2") > -1 || ziwei.IndexOf("3") > -1)
                                    {
                                        ert2 = ert2 + Convert.ToInt32(dm.Rows[k][3]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ert1 = 0;
                            ert2 = 0;
                            ert3 = 0;
                        }
                        SqlParameter[] ot ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@bu",bu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                        object dw = DBHelper.ExecuteScalar("SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu,bu having riqi = @dat1 and changqu=@changqu and bu=@bu", CommandType.Text, ot);
                        object dw1 = DBHelper.ExecuteScalar("SELECT sum(convert(int,A.zongshu))/count(*) FROM (SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu,bu having riqi  like '%" + dat20 + "%' and changqu=@changqu and bu=@bu)A", CommandType.Text, ot);
                        SqlParameter[] os ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@bu",bu),
                                    new SqlParameter ("@dat1",dat1),
                                    new SqlParameter ("@dddd",dddd)
                                            };
                        object dk = DBHelper.ExecuteScalar("SELECT COUNT(*) as zongshu  FROM FIT_LEAVE where lichang between @dddd and @dat1 and changqu=@changqu and bu=@bu", CommandType.Text, os);
                        yue = Convert.ToInt32(dk);
                        ert4 = Convert.ToInt32(dw);
                        if (Convert.IsDBNull(dw1))
                        {
                            ert5 = 0;
                        }
                        else
                        {
                            ert5 = Convert.ToInt32(dw1);
                        }
                        ert12 = ert1 + ert2;
                        lizhi = ert12 + ert3;
                        bili = Convert.ToDouble(lizhi) / (lizhi + ert4);
                        yuebili = Convert.ToDouble(yue) / (yue + ert4);
                        d1 += "{\"yuan1\":\"" + ert1 + "\",\"yuan23\":\"" + ert2 + "\",\"changqu\":\"" + changqu + "\",\"leibie\":\"" + bu + "\",\"dat1\":\"" + dat1 + "\",\"yuan\":\"" + ert12 + "\",\"shi\":\"" + ert3 + "\",\"lizhi\":\"" + lizhi + "\",\"bili\":\"" + Math.Round(bili, 4) * 100 + '%' + "\",\"biaozhun\":\"" + "0.15%" + "\",\"sum\":\"" + ert4 + "\",\"sum1\":\"" + ert5 + "\",\"yue\":\"" + yue + "\",\"yuebili\":\"" + Math.Round(yuebili, 4) * 100 + '%' + "\"},";
                        ert1 = 0;
                        ert2 = 0;
                        ert3 = 0;
                    }
                    SqlParameter[] or9 ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                    DataTable dm9 = DBHelper.ConvertDataReaderToDataTable(DBHelper.ExecuteReader("SELECT bu,ziwei,changqu,COUNT(ziwei) as shuliang FROM FIT_LEAVE group by bu,ziwei,changqu,lichang having changqu=@changqu and lichang=@dat1", CommandType.Text, or9));
                    if (dm9.Rows.Count > 0)
                    {
                        for (int k = 0; k < dm9.Rows.Count; k++)
                        {
                            ziwei = dm9.Rows[k][1].ToString();
                            if (ziwei.IndexOf("師") > -1)
                            {
                                ert3 = ert3 + Convert.ToInt32(dm9.Rows[k][3]);
                            }
                            else
                            {
                                if (ziwei.IndexOf("1") > -1)
                                {
                                    ert1 = ert1 + Convert.ToInt32(dm9.Rows[k][3]);
                                }
                                if (ziwei.IndexOf("2") > -1 || ziwei.IndexOf("3") > -1)
                                {
                                    ert2 = ert2 + Convert.ToInt32(dm9.Rows[k][3]);
                                }
                            }
                        }
                    }
                    else
                    {
                        ert1 = 0;
                        ert2 = 0;
                        ert3 = 0;
                    }
                    SqlParameter[] ot9 ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@dat1",dat1)
                                            };
                    object dw9 = DBHelper.ExecuteScalar("SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu having riqi = @dat1 and changqu=@changqu", CommandType.Text, ot9);
                    object dw90 = DBHelper.ExecuteScalar("SELECT sum(convert(int,A.zongshu))/count(*) FROM (SELECT SUM(convert(int,zai)) as zongshu  FROM FIT_DEL group by riqi,changqu having riqi like '%" + dat20 + "%' and changqu=@changqu)A", CommandType.Text, ot9);
                    SqlParameter[] os9 ={
                                    new SqlParameter ("@changqu",changqu),
                                    new SqlParameter ("@dat1",dat1),
                                    new SqlParameter ("@dddd",dddd)
                                            };
                    object dk9 = DBHelper.ExecuteScalar("SELECT COUNT(*) as zongshu  FROM FIT_LEAVE where lichang between @dddd and @dat1 and changqu=@changqu", CommandType.Text, os9);
                    yue = Convert.ToInt32(dk9);
                    if (dw9 == null)
                    {
                        ert4 = 0;
                    }
                    else
                    {
                        ert4 = Convert.ToInt32(dw9);
                    }
                    if (Convert.IsDBNull(dw90))
                    {
                        ert5 = 0;
                    }
                    else
                    {
                        ert5 = Convert.ToInt32(dw90);
                    }
                    ert12 = ert1 + ert2;
                    lizhi = ert12 + ert3;
                    bili = Convert.ToDouble(lizhi) / (lizhi + ert4);
                    yuebili = Convert.ToDouble(yue) / (yue + ert4);
                    d1 += "{\"yuan1\":\"" + ert1 + "\",\"yuan23\":\"" + ert2 + "\",\"changqu\":\"" + changqu + "\",\"leibie\":\"" + changqu + "\",\"dat1\":\"" + dat1 + "\",\"yuan\":\"" + ert12 + "\",\"shi\":\"" + ert3 + "\",\"lizhi\":\"" + lizhi + "\",\"bili\":\"" + Math.Round(bili, 4) * 100 + '%' + "\",\"biaozhun\":\"" + "0.15%" + "\",\"sum\":\"" + ert4 + "\",\"sum1\":\"" + ert5 + "\",\"yue\":\"" + yue + "\",\"yuebili\":\"" + Math.Round(yuebili, 4) * 100 + '%' + "\"}";
                    d1 += "]";
                    datm += "{\"" + 'm' + count.ToString() + "\":" + d1 + "},";
                    count++;
                }
                datm = datm.Substring(0, datm.Length - 1);
                datm += "]";
            }
            else
            {
                datm = "NG";
            }
            
            context.Response.Write(datm);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class shumu
    {
        public string yuan1;
        public string yuan2;
        public string yuan3;
        public string shi;
        public string heji;
        public string bili;
        public string biaozhun;
        public string sum;
    }
}