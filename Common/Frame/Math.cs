using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TXD.CF.DB;

namespace TXD.CF
{
    /*
     int[] IntArr = new int[] { 1, 2, 3, 4, 5 }; //整型数组
    List<int[]> ListCombination = PermutationAndCombination<int>.GetPermutation(IntArr, 3); //求全部的5取3排列
    foreach(int[] arr in ListCombination)
    {
        foreach(int item in arr)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine("");
    }
         */

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PermutationAndCombination<T> where T : new()
    {
        /// <summary>
        /// 交换两个变量
        /// </summary>
        /// <param name="a">变量1</param>
        /// <param name="b">变量2</param>
        public static void Swap(List<T> t, int st, int end)
        {
            T temp = t[st];
            t[st] = t[end];
            t[end] = temp;
        }

        /// <summary>
        /// 递归算法求数组的组合(私有成员)
        /// </summary>
        /// <param name="list">返回的范型</param>
        /// <param name="t">所求数组</param>
        /// <param name="n">辅助变量</param>
        /// <param name="m">辅助变量</param>
        /// <param name="b">辅助数组</param>
        /// <param name="M">辅助变量M</param>
        private static void GetCombination(List<T> t, int n, int m, List<int> b, int M, PC_CombTable 一个缓冲)
        {
            for (int i = n; i >= m; i--)
            {
                b[m - 1] = i - 1;
                if (m > 1)
                {
                    GetCombination(t, i - 1, m - 1, b, M, 一个缓冲);
                }
                else
                {
                    //if (list == null)
                    //{
                    //    list = new List<List<T>>();
                    //}
                    List<T> temp = new List<T>();
                    for (int j = 0; j < b.Count; j++)
                    {
                        temp.Add(t[b[j]]);
                    }
                    if (typeof(T) == typeof(int))
                    {
                        List<int> ttt = new List<int>();
                        foreach (T lvt in temp)
                        {
                            ttt.Add(Convert.ToInt32(lvt));
                        }
                        while (!一个缓冲.Enque(ttt))
                        {
                            Thread.Sleep(200);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 递归算法求排列(私有成员)
        /// </summary>
        /// <param name="list">返回的列表</param>
        /// <param name="t">所求数组</param>
        /// <param name="startIndex">起始标号</param>
        /// <param name="endIndex">结束标号</param>
        private static void GetPermutationpailei(ref List<List<T>> list, List<T> t, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
            {
                if (list == null)
                {
                    list = new List<List<T>>();
                }
                List<T> temp = new List<T>();
                foreach (T lv他 in t)
                {
                    T lvnew = new T();
                    temp.Add(lv他);
                }
                //t.CopyTo(temp, 0);
                list.Add(temp);
            }
            else
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    T t1 = t[startIndex];
                    T t2 = t[i];
                    Swap(t, startIndex, i);
                    GetPermutationpailei(ref list, t, startIndex + 1, endIndex);
                    Swap(t, startIndex, i);
                }
            }
        }

        ///// <summary>
        ///// 求从起始标号到结束标号的排列，其余元素不变
        ///// </summary>
        ///// <param name="t">所求数组</param>
        ///// <param name="startIndex">起始标号</param>
        ///// <param name="endIndex">结束标号</param>
        ///// <returns>从起始标号到结束标号排列的范型</returns>
        //public static List<List<T>> GetPermutation(List<T> t, int startIndex, int endIndex)
        //{
        //    if (startIndex < 0 || endIndex > t.Count - 1)
        //    {
        //        return null;
        //    }
        //    List<List<T>> list = new List<List<T>>();
        //    GetPermutation(ref list, t, startIndex, endIndex);
        //    return list;
        //}

        ///// <summary>
        ///// 返回数组所有元素的全排列
        ///// </summary>
        ///// <param name="t">所求数组</param>
        ///// <returns>全排列的范型</returns>
        //public static List<List<T>> GetPermutationPailie(List<T> t) //where T : new()
        //{
        //    return GetPermutation(t, 0, t.Count - 1);
        //}

        ///// <summary>
        ///// 求数组中n个元素的排列
        ///// </summary>
        ///// <param name="t">所求数组</param>
        ///// <param name="n">元素个数</param>
        ///// <returns>数组中n个元素的排列</returns>
        //public static List<List<T>> GetPermutation(List<T> t, int n)
        //{
        //    if (n > t.Count)
        //    {
        //        return null;
        //    }
        //    List<List<T>> list = new List<List<T>>();
        //    List<List<T>> c = GetCombination(t, n, 一个缓冲);
        //    for (int i = 0; i < c.Count; i++)
        //    {
        //        List<List<T>> l = new List<List<T>>();
        //        GetPermutation(ref l, c[i], 0, n - 1);
        //        list.AddRange(l);
        //    }
        //    return list;
        //}

        /// <summary>
        /// 求数组中n个元素的组合
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="n">元素个数</param>
        /// <returns>数组中n个元素的组合的范型</returns>
        public static void GetCombination(List<T> t, int n, PC_CombTable 一个缓冲)
        {
            if (t.Count < n)
            {
                return;
            }
            List<int> temp = new List<int>();
            for (int lvii = 0; lvii < n; lvii++)
            {
                temp.Add(0);
            }
            //List<List<T>> list = new List<List<T>>();
            GetCombination(t, t.Count, n, temp, n, 一个缓冲);
            //return list;
        }
    }

    //调不出来，先建表
    /// <summary>
    /// 将数据缓存发到数据库
    /// </summary>
    public class PC_CombTable
    {
        public const int wm用户自定义消息起始段 = 0x500;
        public const int wm组合数计算开始 = wm用户自定义消息起始段 + 1;
        public const int wm组合数计算结束 = wm用户自定义消息起始段 + 2;

        public IntPtr PV_MsgForm = IntPtr.Zero;
        public SynBool CmdOn = new SynBool();
        public SynBool AutoClose = new SynBool();
        Thread m_DataReceiveProcThread;
        private IDataAccess pv_Dac = null;
        private Queue<List<int>> pv_保存组合数的队列 = new Queue<List<int>>();

        /// <summary>
        /// 同步对象
        /// </summary>
        private object pv_Syn = new object();

        public PC_CombTable(IDataAccess iDac)
        {
            CmdOn.isTure = true;
            AutoClose.isTure = false;
            pv_Dac = iDac;
            //ThreadStart lvTS = new ThreadStart(pt_线程执行);
            m_DataReceiveProcThread = new Thread(new ThreadStart(pt_线程执行));
            m_DataReceiveProcThread.Start();
        }

        private void pt_线程执行()
        {
            if (PV_MsgForm != IntPtr.Zero)
            {
                Tool.PostMessage(PV_MsgForm, wm组合数计算开始, 0, IntPtr.Zero);
            }
            string lvSql = @"truncate TABLE  TB_PECP_QMAIN;
 
";
            pv_Dac.ExecuteNonQuery(lvSql, null);
            lvSql = @" 
truncate TABLE  TB_PECP_QSUB;
";

            pv_Dac.ExecuteNonQuery(lvSql, null);

            int trytime = 0;
            int lvCombIDUnique = 1;
            List<List<int>> lv组合数暂存 = new List<List<int>>();
            while (CmdOn.isTure)
            {
                lock (pv_Syn)
                {
                    while (pv_保存组合数的队列.Count > 0)
                    {
                        lv组合数暂存.Add(pv_保存组合数的队列.Dequeue());
                    }
                }
                if (lv组合数暂存.Count <= 0)
                {
                    trytime++;
                }
                else
                {
                    trytime = 0;
                }
                if (AutoClose.isTure && (lv组合数暂存.Count <= 0) && trytime > 10)
                {
                    break;
                }
                int lv最大数 = -1;
                foreach (List<int> lv当前组合 in lv组合数暂存)
                {
                    lv最大数 = -1;
                    lvSql = @"
LOCK TABLES TB_PECP_QSUB WRITE;
REPLACE INTO TB_PECP_QSUB (COMB_QMAIN_ID, NUM_PART) VALUES";
                    for (int lvd = 1; lvd <= lv当前组合.Count; lvd++)
                    {
                        if (lv最大数 < lv当前组合[lvd - 1])
                        {
                            lv最大数 = lv当前组合[lvd - 1];
                        }
                        lvSql += "(" + lvCombIDUnique + ", " + lv当前组合[lvd - 1].ToString() + "),";
                    }
                    lvSql = lvSql.Substring(0, lvSql.Length - 1) + @";
UNLOCK TABLES;
";
                    lvSql += @"
insert into TB_PECP_QMAIN (NUM_MAX,  NUM_MIN, COMB_QMAIN_ID)
VALUES (  " + +lv最大数 + @", " + lv当前组合.Count.ToString() + @", " + lvCombIDUnique + @");
    
";
                    pv_Dac.ExecuteNonQuery(lvSql, null);
                    lvCombIDUnique++;
                }
                lv组合数暂存.Clear();
                Thread.Sleep(1);
            }
            if (PV_MsgForm != null)
            {
                Tool.PostMessage(PV_MsgForm, wm组合数计算结束, 0, IntPtr.Zero);
            }
        }

        public bool Enque(List<int> a组合数)
        {
            bool lvRet = false;
            lock (pv_Syn)
            {
                if (pv_保存组合数的队列.Count < 10000)
                {
                    pv_保存组合数的队列.Enqueue(a组合数);
                    lvRet = true;
                }
            }
            return lvRet;
        }

        public static string pt序号转字母(int aSN)
        {
            char a = Convert.ToChar(aSN + 64);
            return Convert.ToString(a);
        }


        //public static string get归集sql()
        //{
        //    string lvSql = "";
        //}
        public static string get文章组合池_创建SQL()
        {
            string lvSql = "";
            string lvSN = "";
            string lvOne = "";
            //string lvIndex = "";
            for (int lvi = 1; lvi <= 40; lvi++)
            {
                lvSN = lvi.ToString("d2");
                lvOne = @"
DROP TABLE IF EXISTS TB_PECP_ARTI_" + lvSN + @";

CREATE TABLE TB_PECP_ARTI_" + lvSN + @"
(
    COMB_ARTI_ID         INT NOT NULL AUTO_INCREMENT COMMENT '组合ID。',
    BLOCK_ID             INT NOT NULL COMMENT '抽卷，题目要求。要记录。',
    IS_DELETED           BOOL NOT NULL DEFAULT FALSE COMMENT '删除标记',";
                //lvIndex = "";
                for (int lvj = 1; lvj <= lvi; lvj++)
                {
                    string lvQRCSN = lvj.ToString("d2");
                    lvOne += @"
    INST_ID_SN" + lvQRCSN + "      INT NOT NULL DEFAULT 0 COMMENT 'BLOCK第" + lvQRCSN + @"个文章。',";
                }

                lvOne += @"
    PRIMARY KEY(COMB_ARTI_ID)
);
ALTER TABLE TB_PECP_ARTI_" + lvSN + "  COMMENT '组合文章清单。选" + lvi.ToString() + @"个文章的。'; 

CREATE INDEX TB_PECP_ARTI_" + lvSN + @"_BLOCK_ID ON TB_PECP_ARTI_" + lvSN + @"
(
   BLOCK_ID
);
                ";
                lvSql += lvOne;
            }
            return lvSql;
        }

        public static string getCheckTableSql()
        {
            string lvSqlAll = "";

            //int MaxNum = 10;
            int MinNum = 20;
            for (int lv当前组合取数 = 3; lv当前组合取数 <= MinNum; lv当前组合取数++)
            {
                string tableName = "TB_PECP_QMAIN_" + lv当前组合取数.ToString("d2");

                string fieldInser = "";
                string field = "";

                for (int lvd = 1; lvd <= lv当前组合取数; lvd++)
                {
                    fieldInser = fieldInser + "    NUM" + lvd.ToString("d2") + ",";
                    field = field + pt序号转字母(lvd) + ".UID,";
                }

                lvSqlAll += @"
select count(*) from " + tableName + @";
select max(NUM01) from " + tableName + @";

select distinct " + fieldInser + @"0
from " + tableName + @"
group by  " + fieldInser.Substring(0, fieldInser.Length - 1) + "; " + Environment.NewLine;
            }
            for (int lv当前组合取数 = 2; lv当前组合取数 <= MinNum; lv当前组合取数++)
            {
                string tableName = "TB_PECP_QMAIN_" + lv当前组合取数.ToString("d2");


                lvSqlAll += @"
update   " + tableName + @" set num_max = NUM" + lv当前组合取数.ToString("d2") + ";";
            }

            return lvSqlAll;
        }

        public static string get题干组合数建表sql()
        {
            string lvSqlAll = "";


            for (int lvc = 1; lvc < 21; lvc++)
            {
                string tableName = "TB_PECP_QMAIN_" + lvc.ToString("d2");
                string field = "";
                for (int lvd = 1; lvd <= lvc; lvd++)
                {
                    field = field + "    NUM" + lvd.ToString("d2") + " INT NOT NULL DEFAULT 0," + Environment.NewLine;
                }
                lvSqlAll +=
                    @"
DROP TABLE IF EXISTS " + tableName + @";
CREATE TABLE " + tableName + @"
(
    COMB_QMAIN_ID INT NOT NULL DEFAULT 0,
" + field + @"
    UID INT NOT NULL AUTO_INCREMENT COMMENT '不能做关联。',
    NUM_MAX INT NOT NULL DEFAULT 0 COMMENT '一个从N取M组合里，M个数中最大的记录到这。以后X取M时，肯定包含这个组合。所以，只要计算一次N取M。N到M之间的X取M数的组合都在这里了。',
    PRIMARY KEY (UID)
);
ALTER TABLE " + tableName + @" COMMENT '缩写：paper english Combination Pool;题干组合池。文章题干按自然数序号。组合选取N选M。可以初始化维护这里。';
CREATE INDEX TB_PECP_QMAIN_" + lvc.ToString("d2") + "_COMB_QMAIN_ID ON TB_PECP_QMAIN_" + lvc.ToString("d2") + @"
(
   COMB_QMAIN_ID
);

" + Environment.NewLine;
            }


            return lvSqlAll;
        }

        public static string get删表sql()
        {
            string lvSqlAll = "";

            List<string> lv表名前List = new List<string>();
            lv表名前List.Add("tb_pe_comb_pool_arti_");
            lv表名前List.Add("tb_com_arti_");
            lv表名前List.Add("TB_PECP_QMAIN_");
            lv表名前List.Add("tb_pecp_aq_");

            foreach (string tbn in lv表名前List)
            {
                for (int lvc = 1; lvc < 21; lvc++)
                {
                    lvSqlAll +=
                        @"
DROP TABLE IF EXISTS " + tbn + lvc.ToString("d2") + ";" + Environment.NewLine;
                }
            }


            return lvSqlAll;
        }


        public static string get题干对文章_组合数_建表sql()
        {
            string lvSqlAll = "";


            for (int lvc = 1; lvc < 21; lvc++)
            {
                string tableName = "TB_PECP_AQ_" + lvc.ToString("d2");
                string field = "";
                for (int lvd = 1; lvd <= lvc; lvd++)
                {
                    field = field + " COMB_QMAIN_ID" + lvd.ToString("d2") + " INT NOT NULL DEFAULT 0," + Environment.NewLine;
                }
                lvSqlAll +=
                    @"
DROP TABLE IF EXISTS " + tableName + @";
CREATE TABLE " + tableName + @"
(
    COMB_AQ_ID             INT NOT NULL AUTO_INCREMENT COMMENT '文章结合题干的一个组合，针对BLOCK的一个组合。',
    BLOCK_ID        INT NOT NULL COMMENT '抽卷，题目要求。要记录。',
    IS_DELETED      BOOL NOT NULL DEFAULT FALSE COMMENT '删除标记',
    COMB_ARTI_ID    INT NOT NULL DEFAULT 0 COMMENT '文章的一个组合，行列转换时对照',
" + field + @"
    PRIMARY KEY (COMB_AQ_ID)
); 
ALTER TABLE " + tableName + @" COMMENT '缩写：paper english Combination Pool;文章+题干组合池。为了判定BLOCK的要求。这表是中间表';

CREATE INDEX TB_PECP_AQ_" + lvc.ToString("d2") + "_COMB_AQ_ID ON TB_PECP_AQ_" + lvc.ToString("d2") + @"
(
   COMB_AQ_ID
);

CREATE INDEX TB_PECP_AQ_" + lvc.ToString("d2") + "_BLOCK_ID ON TB_PECP_AQ_" + lvc.ToString("d2") + @"
(
   BLOCK_ID
);

CREATE INDEX TB_PECP_AQ_" + lvc.ToString("d2") + "_COMB_ARTI_ID ON TB_PECP_AQ_" + lvc.ToString("d2") + @"
(
   COMB_ARTI_ID
);
" + Environment.NewLine;
            }


            return lvSqlAll;
        }
    }
}