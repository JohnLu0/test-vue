using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;
using System.Text;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.HSSF.Util;
using NPOI.XSSF.UserModel;
using System.Drawing;

namespace TSCSFC.DB
{
    public class NPOIhelper
    {
        #region 由DataTable导出Excel
        /// <summary>
        /// 由DataSet导出Excel,被外界调用的方法
        /// NPOIHelper.ExportMutilDataTableToExcel(new DataTable[] { dt, dt1 }, fileName, new string[] { "工作表1", "工作表2" }, "1");
        /// </summary>   
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fileName">指定Excel工作表名称</param>
        /// <param name="fileName">strType=0:普通格式 1有格式化的形式</param>
        /// <returns>Excel工作表</returns>   
        public static void ExportMutilDatatableToExcel(DataTable[] sourcetable, string filename, string[] sheetname, string strtype)
        {
            MemoryStream ms = null;
            if (strtype == "0")
            {
                ms = ExportMutilDatatableToBasicExcel(sourcetable, sheetname) as MemoryStream;
            }
            else
                ms = ExportMutilDatatableToFormatExcel(sourcetable, sheetname) as MemoryStream;
            SaveToFile(ms, filename);
        }
        /// <summary>
        /// 由DataTable导出Excel(基本形式)
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>    
        /// <param name="sheetName">工作表名称</param>
        /// <returns>Excel工作表</returns> 
        private static Stream ExportMutilDatatableToBasicExcel(DataTable[] sourcetable, string[] sheetname)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < sheetname.Length; i++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sheetname[i]);

                int rowindex = 0;
                int sheetnum = 1;
                //create tablehead
                foreach (DataRow row in sourcetable[i].Rows)
                {
                    if (rowindex == 65535 || rowindex == 0)
                    {
                        if (rowindex != 0)
                        {
                            sheetnum++;
                            sheet = (HSSFSheet)workbook.CreateSheet(sheetname[i]);
                        }
                        var headerrow = sheet.CreateRow(0);
                        var headstyle = workbook.CreateCellStyle();
                        headstyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headstyle.SetFont(font);
                        foreach (DataColumn column in sourcetable[i].Columns)
                        {
                            headerrow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        }
                        rowindex = 1;
                    }
                    //create tableinner
                    HSSFRow datarow = (HSSFRow)sheet.CreateRow(rowindex);
                    foreach (DataColumn column in sourcetable[i].Columns)
                    {
                        datarow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowindex++;
                }

            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            workbook = null;
            return ms;
        }
        /// <summary>
        /// 由DataTable导出Excel(带有格式)
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>    
        /// <param name="sheetName">工作表名称</param>
        /// <returns>Excel工作表</returns>
        private static Stream ExportMutilDatatableToFormatExcel(DataTable[] sourcetable, string[] sheetname)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();

            var headstyle = workbook.CreateCellStyle();
            headstyle.Alignment = HorizontalAlignment.Center;
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headstyle.SetFont(font);
            headstyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            headstyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;

            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < sheetname.Length; i++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sheetname[i]);

                var datestyle = workbook.CreateCellStyle();
                var format = workbook.CreateDataFormat();
                datestyle.DataFormat = format.GetFormat("yyyy-mm-dd");

                //columnswidth
                int[] arrcolwidth = new int[sourcetable[i].Columns.Count];
                foreach (DataColumn item in sourcetable[i].Columns)
                {
                    arrcolwidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                }
                for (int k = 0; k < sourcetable[i].Rows.Count; k++)
                {
                    for (int j = 0; j < sourcetable[i].Columns.Count; j++)
                    {
                        int inttemp = Encoding.GetEncoding(936).GetBytes(sourcetable[i].Rows[k][j].ToString()).Length;
                        if (inttemp > arrcolwidth[j])
                        {
                            arrcolwidth[j] = inttemp;
                        }
                    }
                }
                int rowindex = 0;
                int sheetnum = 1;

                foreach (DataRow row in sourcetable[i].Rows)
                {
                    //create tablehead
                    if (rowindex == 65535 || rowindex == 0)
                    {
                        if (rowindex != 0)
                        {
                            sheetnum++;
                            sheet = (HSSFSheet)workbook.CreateSheet(sheetname[i]);
                        }
                        var headerrow = sheet.CreateRow(0);
                        foreach (DataColumn column in sourcetable[i].Columns)
                        {
                            headerrow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerrow.GetCell(column.Ordinal).CellStyle = headstyle;
                            //set columnswidth
                            sheet.SetColumnWidth(column.Ordinal, (arrcolwidth[column.Ordinal] + 1) * 256);
                        }
                        rowindex = 1;
                    }
                    //create tableinner
                    HSSFRow datarow = (HSSFRow)sheet.CreateRow(rowindex);
                    foreach (DataColumn column in sourcetable[i].Columns)
                    {
                        var newcell = datarow.CreateCell(column.Ordinal);

                        string drvalue = row[column].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型  
                                newcell.SetCellValue(drvalue);
                                break;
                            case "System.DateTime"://日期类型

                            case "MySql.Data.Types.MySqlDateTime": //MySql类型
                                if (drvalue == "0000/0/0 0:00:00" || String.IsNullOrEmpty(drvalue))
                                {
                                    //当时间为空，防止生成的execl 中是一串“#######”号，所有赋值为空字符串
                                    newcell.SetCellValue("");
                                }
                                else
                                {
                                    DateTime dateV;
                                    DateTime.TryParse(drvalue, out dateV);
                                    newcell.SetCellValue(dateV);
                                    newcell.CellStyle = datestyle;//格式化显示  
                                }
                                break;
                            case "System.Boolean"://布尔型  
                                bool boolV = false;
                                bool.TryParse(drvalue, out boolV);
                                newcell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型  
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drvalue, out intV);
                                newcell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型  
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drvalue, out doubV);
                                newcell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理  
                                newcell.SetCellValue("");
                                break;
                            default:
                                newcell.SetCellValue("");
                                break;
                        }
                    }
                    rowindex++;
                }

            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            workbook = null;
            return ms;
        }
        /// <summary>
        /// 由DataTable导出Excel,基本方法
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param> 
        /// <returns>Excel工作表</returns>   
        private static Stream ExportDataTableToBasicExcel(DataTable sourcetable, string sheetname)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            var sheet = workbook.CreateSheet(sheetname);

            int rowindex = 0;
            int sheetnum = 1;
            foreach (DataRow row in sourcetable.Rows)
            {
                if (rowindex == 65535 || rowindex == 0)
                {
                    if (rowindex != 0)
                    {
                        sheetnum++;
                        sheet = workbook.CreateSheet(sheetname + "-" + sheetnum.ToString());
                    }
                    var headerrow = sheet.CreateRow(0);
                    var headstyle = workbook.CreateCellStyle();
                    headstyle.Alignment = HorizontalAlignment.Center;
                    var font = workbook.CreateFont();
                    font.FontHeightInPoints = 10;
                    font.Boldweight = 700;
                    headstyle.SetFont(font);
                    foreach (DataColumn column in sourcetable.Columns)
                    {
                        headerrow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                    }
                    rowindex = 1;
                }

                HSSFRow datarow = (HSSFRow)sheet.CreateRow(rowindex);
                foreach (DataColumn column in sourcetable.Columns)
                {
                    datarow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowindex++;
            }
            for (int i = 0; i <= sourcetable.Rows.Count; i++)
                sheet.AutoSizeColumn(i);

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            workbook = null;
            return ms;
        }
        /// <summary>
        /// 带格式化的
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="sheetName">创建的Sheet名称</param>
        /// <returns></returns>
        private static Stream ExportDataTableToFormatExcel(DataTable sourcetable, string sheetname)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            var sheet = workbook.CreateSheet(sheetname);

            var datestyle = workbook.CreateCellStyle();
            var format = workbook.CreateDataFormat();
            datestyle.DataFormat = format.GetFormat("yyyy-mm-dd:hh:mm:ss");

            //columnswidth
            int[] arrcolwidth = new int[sourcetable.Columns.Count];
            foreach (DataColumn item in sourcetable.Columns)
            {
                arrcolwidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }

            for (int i = 0; i < sourcetable.Rows.Count; i++)
            {
                for (int j = 0; j < sourcetable.Columns.Count; j++)
                {
                    int inttemp = Encoding.GetEncoding(936).GetBytes(sourcetable.Rows[i][j].ToString()).Length;
                    if (inttemp > arrcolwidth[j])
                    {
                        arrcolwidth[j] = inttemp;
                    }
                }
            }

            int rowindex = 0;
            int sheetnum = 1;

            var headstyle = workbook.CreateCellStyle();
            headstyle.Alignment = HorizontalAlignment.Center;
            headstyle.VerticalAlignment = VerticalAlignment.Center;
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headstyle.SetFont(font);
            headstyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            headstyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;

            ICellStyle contentstyle;
            foreach (DataRow row in sourcetable.Rows)
            {
                if (rowindex == 65535 || rowindex == 0)
                {
                    if (rowindex != 0)
                    {
                        sheetnum++;
                        sheet = workbook.CreateSheet(sheetname + "-" + sheetnum.ToString());
                        contentstyle = workbook.CreateCellStyle();
                        contentstyle.Alignment = HorizontalAlignment.Center;
                        contentstyle.VerticalAlignment = VerticalAlignment.Center;
                    }
                    var headerrow = sheet.CreateRow(0);
                    foreach (DataColumn column in sourcetable.Columns)
                    {
                        headerrow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        headerrow.GetCell(column.Ordinal).CellStyle = headstyle;
                        sheet.SetColumnWidth(column.Ordinal, (arrcolwidth[column.Ordinal] + 1) * 256);
                    }
                    rowindex = 1;
                }

                HSSFRow datarow = (HSSFRow)sheet.CreateRow(rowindex);

                foreach (DataColumn column in sourcetable.Columns)
                {
                    var newcell = datarow.CreateCell(column.Ordinal);

                    string drvalue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型  
                            newcell.SetCellValue(drvalue);
                            break;
                        case "System.DateTime"://日期类型  
                        case "MySql.Data.Types.MySqlDateTime": //MySql类型
                            if (drvalue == "0000/0/0 0:00:00" || String.IsNullOrEmpty(drvalue))
                            {
                                //当时间为空，防止生成的execl 中是一串“#######”号，所有赋值为空字符串
                                newcell.SetCellValue("");
                            }
                            else
                            {
                                DateTime dateV;
                                DateTime.TryParse(drvalue, out dateV);
                                newcell.SetCellValue(dateV);
                                newcell.CellStyle = datestyle;//格式化显示  
                            }
                            break;
                        case "System.Boolean"://布尔型  
                            bool boolV = false;
                            bool.TryParse(drvalue, out boolV);
                            newcell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型  
                        case "System.Int32":
                        case "System.Int64":
                            Int64 int64V = 0;
                            Int64.TryParse(drvalue, out int64V);
                            newcell.SetCellValue(int64V);
                            break;
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drvalue, out intV);
                            newcell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型  
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drvalue, out doubV);
                            newcell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理  
                            newcell.SetCellValue("");
                            break;
                        default:
                            newcell.SetCellValue("");
                            break;
                    }

                }
                rowindex++;
            }
            //for (int i = 0; i <= sourcetable.Rows.Count; i++)
            //    sheet.AutoSizeColumn(i);

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }
        /// <summary>
        /// 由DataTable导出Excel，外部調用的方法
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fileName">指定Excel工作表名称</param>
        /// <param name="sheetName">指定Sheet名称</param>
        /// <param name="strType">strType=0:基本的，1：带有格式的</param>
        /// <returns>Excel工作表</returns>
        public static string ExportDataTableToExcel(DataTable sourceTable, string fileName, string sheetName, string strType)
        {
            MemoryStream ms = null;
            string url;
            if (strType == "0")
            {
                ms = ExportDataTableToBasicExcel(sourceTable, sheetName) as MemoryStream;
            }
            else
            {
                ms = ExportDataTableToFormatExcel(sourceTable, sheetName) as MemoryStream;
            }
            url = SaveToFile(ms, fileName);
            return url;
            //HttpContext curContext = HttpContext.Current;
            //RenderToBrowser(ms, curContext, fileName);
        }


        /// <summary>
        /// 由DataTable导出Excel(适应于基本的模版导出，且不超过65535条)
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="modelpath">模版文件实体路径</param>
        /// <param name="modelName">模版文件名称</param>
        /// <param name="fileName">指定Excel工作表名称</param>
        /// <param name="sheetName">作为模型的Excel</param>
        /// <param name="rowindex">从第几行开始写入数据(此为行索引，若为1则从第2行开始写入数据)</param>
        /// <returns>Excel工作表</returns>
        public static void ExportDataTableToExcelModel(DataTable sourceTable, string modelpath, string modelName, string fileName, string sheetName, int rowIndex)
        {
            int colIndex = 0;
            FileStream file = new FileStream(modelpath + "/" + modelName, FileMode.Open, FileAccess.Read);//读入excel模板
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.GetSheet(sheetName);
            if (sourceTable.Rows.Count + rowIndex > 65535)
            {
                throw new ArgumentException("数据太多，系统尚不支持，请缩小查询范围!");
            }

            foreach (DataRow row in sourceTable.Rows)
            {   //双循环写入sourceTable中的数据
                colIndex = 0;
                HSSFRow xlsrow = (HSSFRow)sheet1.CreateRow(rowIndex);
                foreach (DataColumn col in sourceTable.Columns)
                {
                    xlsrow.CreateCell(colIndex).SetCellValue(row[col.ColumnName].ToString());
                    colIndex++;
                }
                rowIndex++;
            }
            sheet1.ForceFormulaRecalculation = true;

            //CS项目适用胡方法
            //FileStream fileS = new FileStream(modelpath + fileName + ".xls", FileMode.Create);//保存
            //hssfworkbook.Write(fileS);
            //fileS.Close();
            MemoryStream ms = new MemoryStream();
            hssfworkbook.Write(ms);

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + ".xls");
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }
        #endregion

        #region 从Excel中读数据到DataTable
        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="extension">Excel文件的扩展名</param>
        /// <param name="SheetName">要获取数据的工作表名称</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string strFileName, string extension, string SheetName, int HeaderRowIndex)
        {
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = null;
                if (extension.Equals(".xls") || extension.Equals(".XLS"))
                {
                    workbook = new HSSFWorkbook(file);
                }
                else
                {
                    workbook = new XSSFWorkbook(file);
                }
                return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
            }
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="extension">Excel文件的扩展名</param>
        /// <param name="SheetIndex">要获取数据的工作表序号(从0开始)</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string strFileName, string extension, int SheetIndex, int HeaderRowIndex)
        {
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = null;
                if (extension.Equals(".xls") || extension.Equals(".XLS"))
                {
                    workbook = new HSSFWorkbook(file);
                }
                else
                {
                    workbook = new XSSFWorkbook(file);
                }

                string SheetName = workbook.GetSheetName(SheetIndex);
                return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
            }
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="ExcelFileStream">Excel文件流</param>
        /// <param name="SheetName">要获取数据的工作表名称</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
        {
            IWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ExcelFileStream.Close();
            return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="ExcelFileStream">Excel文件流</param>
        /// <param name="SheetIndex">要获取数据的工作表序号(从0开始)</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
        {
            IWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ExcelFileStream.Close();
            string SheetName = workbook.GetSheetName(SheetIndex);
            return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="workbook">要处理的工作薄</param>
        /// <param name="SheetName">要获取数据的工作表名称</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(IWorkbook workbook, string SheetName, int HeaderRowIndex)
        {
            ISheet sheet = workbook.GetSheet(SheetName);
            DataTable table = new DataTable();
            try
            {
                IRow headerRow = sheet.GetRow(HeaderRowIndex);
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;

                #region 循环各行各列,写入数据到DataTable
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell == null)
                        {
                            dataRow[j] = null;
                        }
                        else
                        {
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    dataRow[j] = null;
                                    break;
                                case CellType.Boolean:
                                    dataRow[j] = cell.BooleanCellValue;
                                    break;
                                case CellType.Numeric:
                                    dataRow[j] = cell.ToString();
                                    break;
                                case CellType.String:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                                case CellType.Error:
                                    dataRow[j] = cell.ErrorCellValue;
                                    break;
                                case CellType.Formula:
                                default:
                                    dataRow[j] = "=" + cell.CellFormula;
                                    break;
                            }
                        }
                    }
                    table.Rows.Add(dataRow);
                    //dataRow[j] = row.GetCell(j).ToString();
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                table.Clear();
                table.Columns.Clear();
                table.Columns.Add("出错了");
                DataRow dr = table.NewRow();
                dr[0] = ex.Message;
                table.Rows.Add(dr);
                return table;
            }
            finally
            {
                //sheet.Dispose();
                workbook = null;
                sheet = null;
            }
            #region 清除最后的空行
            for (int i = table.Rows.Count - 1; i > 0; i--)
            {
                bool isnull = true;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Rows[i][j] != null)
                    {
                        if (table.Rows[i][j].ToString() != "")
                        {
                            isnull = false;
                            break;
                        }
                    }
                }
                if (isnull)
                {
                    table.Rows[i].Delete();
                }
            }
            #endregion
            return table;
        }
        #endregion

        public static MemoryStream RenderToExcel(IDataReader reader)
        {
            MemoryStream ms = new MemoryStream();

            using (reader)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);
                int cellCount = reader.FieldCount;

                // handling header.
                for (int i = 0; i < cellCount; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(reader.GetName(i));
                }

                // handling value.
                int rowIndex = 1;
                while (reader.Read())
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    for (int i = 0; i < cellCount; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(reader[i].ToString());
                    }

                    rowIndex++;
                }

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }
        //将内存流保存到文件
        public static string SaveToFile(MemoryStream ms, string fileName)
        {
            //将EXCEL保存到服务器端指定文件夹+ "Excel/"
            string urlpath = "Excel/" + fileName;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + urlpath.Replace("\"", "");

            string directoryName = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            //if (System.IO.File.Exists(filepath))
            //{
            //    File.Delete(fileName);
            //}

            using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
            ms.Close();
            ms = null;
            return urlpath;
        }

        //public static string GetAllUsersDesktopFolderPath()
        //{
        //    RegistryKey folders;
        //    folders = OpenRegistryPath(Registry.CurrentUser, @"/software/microsoft/windows/currentversion/explorer/shell folders");
        //    // Windows用户桌面路径
        //    string desktopPath = folders.GetValue("Desktop").ToString();
        //    return desktopPath;
        //}
        //private static RegistryKey OpenRegistryPath(RegistryKey root, string s)
        //{
        //    s = s.Remove(0, 1) + @"/";
        //    while (s.IndexOf(@"/") != -1)
        //    {
        //        root = root.OpenSubKey(s.Substring(0, s.IndexOf(@"/")));
        //        s = s.Remove(0, s.IndexOf(@"/") + 1);
        //    }
        //    return root;
        //}

        //将内存流输出为下载文件
        public static void RenderToBrowser(MemoryStream ms, HttpContext context, string fileName)
        {
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            context.Response.BinaryWrite(ms.ToArray());
            context.Response.Flush();
            context.Response.End();
        }
    }
}