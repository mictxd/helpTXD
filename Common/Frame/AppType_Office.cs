using System;
using System.Collections.Generic;

using System.Data;
using System.Collections;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using TXD.CF.DB;
using System.Drawing;
using System.Configuration;
using System.Reflection;
using System.Text;
namespace TXD.CF
{


    public static partial class HelpTXD
    {
       

        public static void DataGridViewtoExcel_withAsk(DataGridView tmpDataTable, String strDefaultFileName)
        {
            SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.FileName = strDefaultFileName;
            saveFileDialog1.DefaultExt = "*.xls";
            saveFileDialog1.Filter = "Excel文件|*.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                HelpTXD.DataGridViewtoExcel(tmpDataTable, saveFileDialog1.FileName);
                HelpTXD.ShowInfo("导出完成。" + saveFileDialog1.FileName);
            }
        }

        public static void DataGridViewtoExcel(DataGridView tmpDataTable, String strFileName)
        {
            if (tmpDataTable == null)
                return;


            string lvssss = Application.StartupPath + "\\Microsoft.Office.Interop.Excel.dll";
            Assembly ass;

            object obj;
            //获取并加载DLL类库中的程序集
            ass = Assembly.LoadFile(lvssss);

            //获取类的类型：必须使用名称空间+类名称
            obj = ass.CreateInstance("Microsoft.Office.Interop.Excel.ApplicationClass");
            Microsoft.Office.Interop.Excel.Application xlApp = obj as Microsoft.Office.Interop.Excel.Application;
            try
            {
                xlApp.Visible = true;
                xlApp.DefaultFilePath = "";

                xlApp.DisplayAlerts = true;

                xlApp.SheetsInNewWorkbook = 1;

                Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);

                Microsoft.Office.Interop.Excel._Worksheet xlSheet = xlBook.ActiveSheet as Microsoft.Office.Interop.Excel._Worksheet;
                object m_objOpt = System.Reflection.Missing.Value;


                int lv总行数 = tmpDataTable.Rows.Count;
                int lv总列数 = tmpDataTable.Columns.Count;



                object[,] objData = new Object[1, lv总列数];
                int lv列_流水 = 0;
                for (int i = 0; i < tmpDataTable.Columns.Count; i++)
                {
                    DataGridViewColumn dc = tmpDataTable.Columns[i];
                    if (dc.Visible == true && dc.GetType().Name != "DataGridViewButtonColumn")
                    {
                        objData[0, lv列_流水] = dc.HeaderText;
                        lv列_流水++;
                    }
                }
                xlSheet.Range["A1"].Select();
                Microsoft.Office.Interop.Excel.Range m_objRange = null;

                m_objRange = xlSheet.get_Range("A1", m_objOpt);
                m_objRange = m_objRange.get_Resize(1, lv总列数);
                m_objRange.Value = objData;

                string lvTmpvalue;
                decimal lviTmpValue;
                int lv行数_每批导出最大行 = 2000;
                int lv复制总次数 = lv总行数 / lv行数_每批导出最大行 + 1;
                int lv行序号_相对总行数 = 0; //相对总行数
                lv列_流水 = 0;
                int lv本次复制开始行 = 0;
                int lv本次可复制行数 = 0;
                while (lv行序号_相对总行数 < lv总行数)
                {
                    lv本次复制开始行 = lv行序号_相对总行数;

                    //如果剩余行数 大于 固定导出行数 则本次 导出 固定行数
                    lv本次可复制行数 = lv总行数 - lv行序号_相对总行数;
                    if (lv本次可复制行数 > lv行数_每批导出最大行)
                    { lv本次可复制行数 = lv行数_每批导出最大行; }
                    int lv行_本次复制流水 = 0;

                    objData = new Object[lv本次可复制行数, lv总列数];
                    while (lv行_本次复制流水 < lv本次可复制行数)
                    {
                        lv本次复制开始行 = lv行序号_相对总行数;
                        lv列_流水 = 0;
                        for (int j = 0; j < lv总列数; j++)
                        {
                            if (tmpDataTable.Columns[j].Visible == true && tmpDataTable.Columns[j].GetType().Name != "DataGridViewButtonColumn")
                            {
                                if (tmpDataTable.Rows[lv本次复制开始行+lv行_本次复制流水].Cells[j].Value == null)
                                {
                                    objData[lv行_本次复制流水 , lv列_流水] = "";
                                }
                                else
                                {
                                    lvTmpvalue = tmpDataTable.Rows[lv本次复制开始行 + lv行_本次复制流水].Cells[j].Value.ToString().Trim();
                                    if (decimal.TryParse(lvTmpvalue, out lviTmpValue))
                                    {
                                        if (lviTmpValue == 0)
                                        { lvTmpvalue = ""; }
                                    }
                                    objData[lv行_本次复制流水, lv列_流水] = lvTmpvalue;
                                }
                                lv列_流水++;
                            }
                        }
                        lv行_本次复制流水++;
                    }
                    lv行序号_相对总行数 = lv行序号_相对总行数 + lv本次可复制行数;
                    m_objRange = xlSheet.get_Range(xlSheet.Cells[lv本次复制开始行+1, 1], xlSheet.Cells[lv本次复制开始行 + lv本次可复制行数, lv总列数]);
                    //m_objRange = m_objRange.get_Resize(lv包含标题总行数, lv总列数);
                    m_objRange.Value = objData;
                }



                xlBook.SaveCopyAs(System.Web.HttpUtility.UrlDecode(strFileName, System.Text.Encoding.UTF8));
                xlBook.Saved = true;
                xlApp.Quit();
            }
            catch// (Exception ex)
            { xlApp.Quit(); throw; }
            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application(); //引用Excel对象
            //Microsoft.Office.Interop.Excel.Workbook book = excel.Workbooks.Add(strFileName); //引用Excel工作簿
            //excel.Visible = true; 
            //xlBook.SaveAs(strFileName,System.Text.Encoding.UTF8);

        }

        public static void DataGridViewtoExcel(List<DataGridView> Grids4Excel, List<string> GridText, string strDefaultFileName)
        {
            if (Grids4Excel == null)
            { return; }
            if (Grids4Excel.Count == 0)
            { return; }

            SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.FileName = strDefaultFileName;
            saveFileDialog1.DefaultExt = "*.xls";
            saveFileDialog1.Filter = "Excel文件|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }


            int rowNum;
            int columnNum;
            int columnIndex = 1;
            int maxLineCount;


            string lvssss = Application.StartupPath + "\\Microsoft.Office.Interop.Excel.dll";
            Assembly ass;
            object obj;
            ass = Assembly.LoadFile(lvssss);
            obj = ass.CreateInstance("Microsoft.Office.Interop.Excel.ApplicationClass");
            Microsoft.Office.Interop.Excel.Application xlApp = obj as Microsoft.Office.Interop.Excel.Application;
            xlApp.Visible = true;

            xlApp.DefaultFilePath = "";
            xlApp.DisplayAlerts = true;
            xlApp.SheetsInNewWorkbook = Grids4Excel.Count;
            object m_objOpt = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);

            DataGridView lvGrid;

            for (int lvSheetNo = 0; lvSheetNo < Grids4Excel.Count; lvSheetNo++)
            {
                lvGrid = Grids4Excel[lvSheetNo];
                rowNum = lvGrid.Rows.Count;
                columnNum = lvGrid.Columns.Count;
                columnIndex = 1;
                maxLineCount = rowNum + 1;
                object[,] objData = new Object[maxLineCount, columnNum];
                columnIndex = 0;
                for (int i = 0; i < lvGrid.Columns.Count; i++)
                {
                    DataGridViewColumn dc = lvGrid.Columns[i];
                    if (dc.Visible == true && dc.GetType().Name != "DataGridViewButtonColumn")
                    {
                        objData[0, columnIndex] = dc.HeaderText;
                        columnIndex++;
                    }
                }
                string lvTmpvalue;
                decimal lviTmpValue;
                for (int r = 0; r < rowNum; r++)
                {
                    columnIndex = 0;
                    for (int j = 0; j < columnNum; j++)
                    {
                        if (lvGrid.Columns[j].Visible == true && lvGrid.Columns[j].GetType().Name != "DataGridViewButtonColumn")
                        {
                            if (lvGrid.Rows[r].Cells[j].Value == null)
                            {
                                objData[r + 1, columnIndex] = "";
                            }
                            else
                            {
                                lvTmpvalue = lvGrid.Rows[r].Cells[j].Value.ToString().Trim();
                                if (decimal.TryParse(lvTmpvalue, out lviTmpValue))
                                {
                                    if (lviTmpValue == 0)
                                    { lvTmpvalue = ""; }
                                }
                                objData[r + 1, columnIndex] = lvTmpvalue;
                            }
                            columnIndex++;
                        }
                    }
                }
                //写入excel
                if (xlBook.Sheets.Count < lvSheetNo + 1)
                { xlBook.Sheets.Add(m_objOpt, m_objOpt, m_objOpt, m_objOpt); }
                //xlBook.Sheets.get_Item(lvSheetNo + 1);
                Microsoft.Office.Interop.Excel._Worksheet xlSheet = (Microsoft.Office.Interop.Excel._Worksheet)(xlBook.Sheets.get_Item(1));
                //xlSheet.Range["A1"].Select();
                xlSheet.Activate();
                xlSheet.Name = GridText[lvSheetNo];
                Microsoft.Office.Interop.Excel.Range m_objRange = null;
                m_objRange = xlSheet.get_Range("A1", m_objOpt);
                m_objRange = m_objRange.get_Resize(maxLineCount, columnNum);
                m_objRange.Value = objData;
            }

            try
            {
                xlBook.SaveCopyAs(System.Web.HttpUtility.UrlDecode(saveFileDialog1.FileName, System.Text.Encoding.UTF8));
                xlBook.Saved = true;
                xlApp.Quit();
                HelpTXD.ShowInfo("导出完成。" + saveFileDialog1.FileName);
            }
            catch //(Exception ex)
            {
                xlBook.Saved = true;
                xlApp.Quit();
                throw;
            }
        }

        public static void PrintWordDot(string lv模板文件名,List<string> lv模板词汇, List<string> lv替换词汇)
        {

            object path;//文件路径
            string strContent;//文件内容
            string lvssss = Application.StartupPath + "\\Microsoft.Office.Interop.Word.dll";
            Assembly ass;
            string lv模板文件名含路径 = Application.StartupPath + "\\"+lv模板文件名;
            Object oMissing = System.Reflection.Missing.Value;
            object obj模板 = oMissing;
            object obj;
            //获取并加载DLL类库中的程序集
            ass = Assembly.LoadFile(lvssss);

            obj = ass.CreateInstance("Microsoft.Office.Interop.Word.ApplicationClass");
            //获取类的类型：必须使用名称空间+类名称
            Microsoft.Office.Interop.Word._Application WordApp = obj as Microsoft.Office.Interop.Word._Application;
            WordApp.Visible = true;
            try
            {
                Microsoft.Office.Interop.Word._Document WordDoc = WordApp.Documents.Add(ref obj模板, ref oMissing, ref oMissing, ref oMissing);
                object FindText, ReplaceWith, Replace;
                for (int lvi = 0; lvi < lv模板词汇.Count; lvi++)
                {
                    WordDoc.Content.Find.Text = lv模板词汇[lvi];
                    //要查找的文本 
                    FindText = lv模板词汇[lvi];
                    //替换文本 
                    //ReplaceWith = strNewText;
                    ReplaceWith = lv替换词汇[lvi];

                    //wdReplaceAll - 替换找到的所有项。 
                    //wdReplaceNone - 不替换找到的任何项。 
                    //wdReplaceOne - 替换找到的第一项。 
                    Replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;


                    //移除Find的搜索文本和段落格式设置 
                    WordDoc.Content.Find.ClearFormatting();
                    WordDoc.Content.Find.Execute(ref FindText, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref ReplaceWith, ref Replace, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                }
                WordDoc.PrintPreview();
                WordDoc.Saved = true;
                //关闭wordDoc文档
                WordDoc.Close(ref oMissing, ref oMissing, ref oMissing);
                //关闭wordApp组件对象
                WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
            }
            catch //(Exception ex)
            {
                WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
                throw;
            }
        }
       
    }


}