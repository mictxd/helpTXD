using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
//using System.Net.Mail;
////using MySql.Data.MySqlClient;
//using System.Data;
//using System.Threading;
using System.Runtime.InteropServices;
namespace Txd.CommonFrame
{

    public static class PersonIDCard
    {
        #region API声明
        [DllImport("sdtapi.dll", CallingConvention = CallingConvention.StdCall)]
        static extern int SDT_StartFindIDCard(int iPort, byte[] pucManaInfo, int iIfOpen);
        [DllImport("sdtapi.dll", CallingConvention = CallingConvention.StdCall)]
        static extern int SDT_SelectIDCard(int iPort, byte[] pucManaMsg, int iIfOpen);
        [DllImport("sdtapi.dll", CallingConvention = CallingConvention.StdCall)]
        static extern int SDT_ReadBaseMsg(int iPort, byte[] pucCHMsg, ref UInt32 puiCHMsgLen, byte[] pucPHMsg, ref UInt32 puiPHMsgLen, int iIfOpen);
        #endregion
        public static bool Read(ref int lastport, ref string PName, ref string PID, ref string PAddr, ref string PSex, ref string PNation)
        {
            bool lvrresult = false;
            //变量声明
            byte[] CardPUCIIN = new byte[255];
            byte[] pucManaMsg = new byte[255];
            byte[] pucCHMsg = new byte[255];
            byte[] pucPHMsg = new byte[3024];
            UInt32 puiCHMsgLen = 0;
            UInt32 puiPHMsgLen = 0;
            //int iPort = 1001;
            string IDContent = "";
            int st = 0;
            if ((lastport < 1001) || (lastport > 1016))
            {
                for (int lvp = 1001; lvp < 1017; lvp++)
                {
                    st = SDT_StartFindIDCard(lvp, CardPUCIIN, 1);
                    if (st == 0x9f)
                    {
                        st = SDT_SelectIDCard(lvp, pucManaMsg, 1);
                        if (st == 0x90)
                        {
                            st = SDT_ReadBaseMsg(lvp, pucCHMsg, ref puiCHMsgLen, pucPHMsg, ref puiPHMsgLen, 1);
                            if (st == 0x90)
                            {
                                //显示结果
                                 IDContent = System.Text.ASCIIEncoding.Unicode.GetString(pucCHMsg);
                                 lastport = lvp;
                            }
                        }
                    }
                }
            }
            else
            {
                //读卡操作
                st = SDT_StartFindIDCard(lastport, CardPUCIIN, 1);
                if (st == 0x9f)
                {
                    st = SDT_SelectIDCard(lastport, pucManaMsg, 1);
                    if (st == 0x90)
                    {
                        st = SDT_ReadBaseMsg(lastport, pucCHMsg, ref puiCHMsgLen, pucPHMsg, ref puiPHMsgLen, 1);
                        if (st == 0x90)
                        {
                            //显示结果
                             IDContent = System.Text.ASCIIEncoding.Unicode.GetString(pucCHMsg);
                        }
                    }

                }
            }
            if (IDContent != "")
            {PSex = FSex.男;
                string SexTag = IDContent.Substring(15, 1).Trim();

                if (SexTag != "1")
                { PSex = FSex.女; }
                PNation = IDContent.Substring(16, 2).Trim();
                PName = IDContent.Substring(0, 15).Trim();
                PID = IDContent.Substring(61, 18);
                PAddr = IDContent.Substring(26, 35).Trim();
                lvrresult = true;
               
            }
            return lvrresult;
            ////显示结果
            //IDContent = System.Text.ASCIIEncoding.Unicode.GetString(pucCHMsg);
            //return lvrresult;
        }
        public static bool Read(ref int lastport, ref string RawStr)
        {
            bool lvrresult = false;
            //变量声明
            byte[] CardPUCIIN = new byte[255];
            byte[] pucManaMsg = new byte[255];
            byte[] pucCHMsg = new byte[255];
            byte[] pucPHMsg = new byte[3024];
            UInt32 puiCHMsgLen = 0;
            UInt32 puiPHMsgLen = 0;
            //int iPort = 1001;
            int st = 0;
            if ((lastport < 1001) || (lastport > 1016))
            {
                for (int lvp = 1001; lvp < 1017; lvp++)
                {
                    st = SDT_StartFindIDCard(lvp, CardPUCIIN, 1);
                    if (st == 0x9f)
                    {
                        st = SDT_SelectIDCard(lvp, pucManaMsg, 1);
                        if (st == 0x90)
                        {
                            st = SDT_ReadBaseMsg(lvp, pucCHMsg, ref puiCHMsgLen, pucPHMsg, ref puiPHMsgLen, 1);
                            if (st == 0x90)
                            {
                                //显示结果
                                RawStr = System.Text.ASCIIEncoding.Unicode.GetString(pucCHMsg);
                             
                                lvrresult = true;
                                lastport = lvp;
                                return lvrresult;
                            }
                        }
                    }
                }
            }
            else
            {
                //读卡操作
                st = SDT_StartFindIDCard(lastport, CardPUCIIN, 1);
                if (st == 0x9f)
                {
                    st = SDT_SelectIDCard(lastport, pucManaMsg, 1);
                    if (st == 0x90)
                    {
                        st = SDT_ReadBaseMsg(lastport, pucCHMsg, ref puiCHMsgLen, pucPHMsg, ref puiPHMsgLen, 1);
                        if (st == 0x90)
                        {
                            //显示结果
                            RawStr = System.Text.ASCIIEncoding.Unicode.GetString(pucCHMsg);
                           
                            lvrresult = true;
                        }
                    }

                }
            }
            ////显示结果
            //IDContent = System.Text.ASCIIEncoding.Unicode.GetString(pucCHMsg);
            return lvrresult;
        }
    }
}
