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
            saveFileDialog1.Filter = "Excel�ļ�|*.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                HelpTXD.DataGridViewtoExcel(tmpDataTable, saveFileDialog1.FileName);
                HelpTXD.ShowInfo("������ɡ�" + saveFileDialog1.FileName);
            }
        }

        public static void DataGridViewtoExcel(DataGridView tmpDataTable, String strFileName)
        {
            if (tmpDataTable == null)
                return;


            string lvssss = Application.StartupPath + "\\Microsoft.Office.Interop.Excel.dll";
            Assembly ass;

            object obj;
            //��ȡ������DLL����еĳ���
            ass = Assembly.LoadFile(lvssss);

            //��ȡ������ͣ�����ʹ�����ƿռ�+������
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


                int lv������ = tmpDataTable.Rows.Count;
                int lv������ = tmpDataTable.Columns.Count;



                object[,] objData = new Object[1, lv������];
                int lv��_��ˮ = 0;
                for (int i = 0; i < tmpDataTable.Columns.Count; i++)
                {
                    DataGridViewColumn dc = tmpDataTable.Columns[i];
                    if (dc.Visible == true && dc.GetType().Name != "DataGridViewButtonColumn")
                    {
                        objData[0, lv��_��ˮ] = dc.HeaderText;
                        lv��_��ˮ++;
                    }
                }
                xlSheet.Range["A1"].Select();
                Microsoft.Office.Interop.Excel.Range m_objRange = null;

                m_objRange = xlSheet.get_Range("A1", m_objOpt);
                m_objRange = m_objRange.get_Resize(1, lv������);
                m_objRange.Value = objData;

                string lvTmpvalue;
                decimal lviTmpValue;
                int lv����_ÿ����������� = 2000;
                int lv�����ܴ��� = lv������ / lv����_ÿ����������� + 1;
                int lv�����_��������� = 0; //���������
                lv��_��ˮ = 0;
                int lv���θ��ƿ�ʼ�� = 0;
                int lv���οɸ������� = 0;
                while (lv�����_��������� < lv������)
                {
                    lv���θ��ƿ�ʼ�� = lv�����_���������;

                    //���ʣ������ ���� �̶��������� �򱾴� ���� �̶�����
                    lv���οɸ������� = lv������ - lv�����_���������;
                    if (lv���οɸ������� > lv����_ÿ�����������)
                    { lv���οɸ������� = lv����_ÿ�����������; }
                    int lv��_���θ�����ˮ = 0;

                    objData = new Object[lv���οɸ�������, lv������];
                    while (lv��_���θ�����ˮ < lv���οɸ�������)
                    {
                        lv���θ��ƿ�ʼ�� = lv�����_���������;
                        lv��_��ˮ = 0;
                        for (int j = 0; j < lv������; j++)
                        {
                            if (tmpDataTable.Columns[j].Visible == true && tmpDataTable.Columns[j].GetType().Name != "DataGridViewButtonColumn")
                            {
                                if (tmpDataTable.Rows[lv���θ��ƿ�ʼ��+lv��_���θ�����ˮ].Cells[j].Value == null)
                                {
                                    objData[lv��_���θ�����ˮ , lv��_��ˮ] = "";
                                }
                                else
                                {
                                    lvTmpvalue = tmpDataTable.Rows[lv���θ��ƿ�ʼ�� + lv��_���θ�����ˮ].Cells[j].Value.ToString().Trim();
                                    if (decimal.TryParse(lvTmpvalue, out lviTmpValue))
                                    {
                                        if (lviTmpValue == 0)
                                        { lvTmpvalue = ""; }
                                    }
                                    objData[lv��_���θ�����ˮ, lv��_��ˮ] = lvTmpvalue;
                                }
                                lv��_��ˮ++;
                            }
                        }
                        lv��_���θ�����ˮ++;
                    }
                    lv�����_��������� = lv�����_��������� + lv���οɸ�������;
                    m_objRange = xlSheet.get_Range(xlSheet.Cells[lv���θ��ƿ�ʼ��+1, 1], xlSheet.Cells[lv���θ��ƿ�ʼ�� + lv���οɸ�������, lv������]);
                    //m_objRange = m_objRange.get_Resize(lv��������������, lv������);
                    m_objRange.Value = objData;
                }



                xlBook.SaveCopyAs(System.Web.HttpUtility.UrlDecode(strFileName, System.Text.Encoding.UTF8));
                xlBook.Saved = true;
                xlApp.Quit();
            }
            catch// (Exception ex)
            { xlApp.Quit(); throw; }
            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application(); //����Excel����
            //Microsoft.Office.Interop.Excel.Workbook book = excel.Workbooks.Add(strFileName); //����Excel������
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
            saveFileDialog1.Filter = "Excel�ļ�|*.xls";
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
                //д��excel
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
                HelpTXD.ShowInfo("������ɡ�" + saveFileDialog1.FileName);
            }
            catch //(Exception ex)
            {
                xlBook.Saved = true;
                xlApp.Quit();
                throw;
            }
        }

        public static void PrintWordDot(string lvģ���ļ���,List<string> lvģ��ʻ�, List<string> lv�滻�ʻ�)
        {

            object path;//�ļ�·��
            string strContent;//�ļ�����
            string lvssss = Application.StartupPath + "\\Microsoft.Office.Interop.Word.dll";
            Assembly ass;
            string lvģ���ļ�����·�� = Application.StartupPath + "\\"+lvģ���ļ���;
            Object oMissing = System.Reflection.Missing.Value;
            object objģ�� = oMissing;
            object obj;
            //��ȡ������DLL����еĳ���
            ass = Assembly.LoadFile(lvssss);

            obj = ass.CreateInstance("Microsoft.Office.Interop.Word.ApplicationClass");
            //��ȡ������ͣ�����ʹ�����ƿռ�+������
            Microsoft.Office.Interop.Word._Application WordApp = obj as Microsoft.Office.Interop.Word._Application;
            WordApp.Visible = true;
            try
            {
                Microsoft.Office.Interop.Word._Document WordDoc = WordApp.Documents.Add(ref objģ��, ref oMissing, ref oMissing, ref oMissing);
                object FindText, ReplaceWith, Replace;
                for (int lvi = 0; lvi < lvģ��ʻ�.Count; lvi++)
                {
                    WordDoc.Content.Find.Text = lvģ��ʻ�[lvi];
                    //Ҫ���ҵ��ı� 
                    FindText = lvģ��ʻ�[lvi];
                    //�滻�ı� 
                    //ReplaceWith = strNewText;
                    ReplaceWith = lv�滻�ʻ�[lvi];

                    //wdReplaceAll - �滻�ҵ�������� 
                    //wdReplaceNone - ���滻�ҵ����κ�� 
                    //wdReplaceOne - �滻�ҵ��ĵ�һ� 
                    Replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;


                    //�Ƴ�Find�������ı��Ͷ����ʽ���� 
                    WordDoc.Content.Find.ClearFormatting();
                    WordDoc.Content.Find.Execute(ref FindText, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref ReplaceWith, ref Replace, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                }
                WordDoc.PrintPreview();
                WordDoc.Saved = true;
                //�ر�wordDoc�ĵ�
                WordDoc.Close(ref oMissing, ref oMissing, ref oMissing);
                //�ر�wordApp�������
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