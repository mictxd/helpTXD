using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using System.Linq;

namespace TXD.CF
{
    /// <summary>
    /// 窗体编辑状态
    /// </summary>
    public enum FormDBMode
    {
        fmBrowse,
        fmNew,
        fmEdit,
        fmDelete,
        fmCancel,
        fmSave,
        fmLoading
    };

    public static class PC操作标记
    {
        /// <summary>
        /// 通用的，必须一个字符.
        /// </summary>
        public static char char分隔符 = '|';

        /// <summary>
        /// 通用的，必须一个字符.
        /// </summary>
        public static char char分隔符_次级 = ';';

        /// <summary>
        /// 通用的，必须一个字符.
        /// </summary>
        public static char char分隔符_显示 = ',';

        /// <summary>
        /// 通用的，必须一个字符.
        /// </summary>
        public static char char分隔符_制表符 = '\t';

        public static string MSG_保存成功 = "保存成功"; //执行成功里的文字  必须包含==>保存成功的提示信息内容文字.一字不落的包含.
        public static string MSG_流程执行成功 = "保存成功。" + Environment.NewLine + "部分操作未处理,系统仍然认为执行成功.如果执行刷新操作,未处理内容可能会还原显示.";
        public static string MSG_流程失败 = "未知错误";
        public static string MSG_修改数据过期 = "数据过期,无权处理.  请联系管理员设置过期数据权限.    ";
        public static string MSG_未发现该记录 = "未发现该记录,无法处理.    ";
        public static string MSG_删除提交 = "直接提交服务器." + Environment.NewLine + "确认删除?";
    }

    public static class EmpConst
    {
        public const string UserIDCreate = "FUserIDCreate";
        public const string UserIDModify = "FUserIDModify";
        public const string DateCreate = "FDateCreate";
        public const string FDateModify = "FDateModify";
    }

    public class SynBool
    {
        private bool SynObj = false;

        public bool isTure
        {
            get
            {
                lock (this)
                {
                    return SynObj;
                }
            }
            set
            {
                lock (this)
                {
                    SynObj = value;
                }
            }
        }
    }

    public static class DataFormatTypeTXD
    {
        public static string 计数器 = "计数器";
        public static string 计量数量 = "计量数量";
        public static string 字符 = "字符";
        public static string 段落 = "段落";
        public static string 价格 = "价格";
        public static string 金额 = "金额";
        public static string 日期 = "日期";
        public static string 时钟 = "时钟";
        public static string 长时间 = "长时钟";
    }


    public static partial class HelpTXD
    {
        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (null == convertibleValue)
            {
                return default(T);
            }

            if (!typeof(T).IsGenericType)
            {
                return (T) Convert.ChangeType(convertibleValue, typeof(T));
            }
            else
            {
                Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return (T) Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(typeof(T)));
                }
            }
            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, typeof(T).FullName));
        }

        public static object SD_ChanageType(this object value, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类,
            if (convertsionType.IsGenericType &&
                //判断convertsionType是否为nullable泛型类
                convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }

                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(convertsionType);
                //将convertsionType转换为nullable对的基础基元类型
                convertsionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, convertsionType);
        }

        public static void CheckMax(object obj2report, int maxProLen)
        {
            StringBuilder lvsb = new StringBuilder();
            Type type = obj2report.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                if ((i.PropertyType == typeof(string)) || (i.PropertyType == typeof(String)))
                {
                    if (i.GetValue(obj2report, null).ToString().Length > maxProLen)
                    {
                        throw new Exception("超出安全长度:" + i.Name);
                    }
                }
            }
        }

        public static string Report对象所有属性值(object obj2report)
        {
            StringBuilder lvsb = new StringBuilder();
            Type type = obj2report.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                lvsb.AppendLine(i.Name + " -- " + i.GetValue(obj2report, null));
            }
            return lvsb.ToString();
        }

        /// <summary>
        /// 对象复制所有属性。不能克隆，指向对象的属性。会出问题。这里不是递归方式的克隆。可以为两种类型。内部是按照名称来对应的。
        /// </summary>
        /// <param name="obj2Src"></param>
        /// <param name="obj2Dest"></param>
        public static void CopyPropertyValues(object obj2Src, object obj2Dest)
        {
            Type lvTypeSrc = obj2Src.GetType();
            System.Reflection.PropertyInfo[] ps = lvTypeSrc.GetProperties();
            Type lvTypeDesd = obj2Dest.GetType();
            System.Reflection.PropertyInfo[] pd = lvTypeDesd.GetProperties();
            List<int> lv来源属性可用的 = new List<int>();
            List<int> lv目标属性可用的 = new List<int>();
            for (int lvs = 0; lvs < ps.Length; lvs++)
            {
                for (int lvd = 0; lvd < pd.Length; lvd++)
                {
                    if (ps[lvs].Name == pd[lvd].Name)
                    {
                        lv来源属性可用的.Add(lvs);
                        lv目标属性可用的.Add(lvd);
                        break;
                    }
                }
            }

            foreach (int lvi in lv来源属性可用的) //PropertyInfo i in )
            {
                //try
                //{
                pd[lv目标属性可用的[lvi]].SetValue(obj2Dest, ps[lv来源属性可用的[lvi]].GetValue(obj2Src, null), null);
                //}
                //catch
                //{
                //}
            }
        }

        public static int pt_DayOfWeekToInt(DayOfWeek aDW)
        {
            if (aDW == DayOfWeek.Monday)
            {
                return 1;
            }
            else if (aDW == DayOfWeek.Tuesday)
            {
                return 2;
            }
            else if (aDW == DayOfWeek.Wednesday)
            {
                return 3;
            }
            else if (aDW == DayOfWeek.Thursday)
            {
                return 4;
            }
            else if (aDW == DayOfWeek.Friday)
            {
                return 5;
            }
            else if (aDW == DayOfWeek.Saturday)
            {
                return 6;
            }
            else if (aDW == DayOfWeek.Sunday)
            {
                return 7;
            }
            else
            {
                return 0;
            }
        }

        public static List<int> TyrParseToID(string IDs)
        {
            List<int> lvRet = new List<int>();
            List<string> lvSplited = IDs.Split(new char[] {','}).ToList();
            foreach (string lvStrID in lvSplited)
            {
                if (string.IsNullOrEmpty(lvStrID))
                {
                    continue;
                }
                int lvID = -1;
                if (int.TryParse(lvStrID, out lvID))
                {
                    lvRet.Add(lvID);
                }
            }
            return lvRet;
        }

        public static string pt_TyrParseToString(List<int> IDList)
        {
            string lvRet = "";
            foreach (int lvi in IDList)
            {
                lvRet += lvi + ",";
            }
            if (lvRet.Length > 0)
            {
                lvRet = lvRet.Substring(0, lvRet.Length - 1);
            }
            return lvRet;
        }
    }

    /// <summary>
    /// 各种系统代表逻辑值的规范。一般采用固定的某个方式。
    /// </summary>
    public static class PC_逻辑值
    {
        public static int TrueNotZero = 1;

        public static int FalseEqZero = 0;
        //public static string Y = "Y";
        //public static string N = "N";
        //public static string 是 = "是";
        //public static string 否 = "否";

        //public static string correctYN(string aBoolValueString)
        //{
        //    return ToValueStringYN(isTrue(aBoolValueString));
        //}

        //public static string ToValueStringYN(bool aBoolValue)
        //{
        //    if (aBoolValue)
        //    {
        //        return Y;
        //    }
        //    else
        //    {
        //        return N;
        //    }
        //}
        //public static string ToValueString是否(bool aBoolValue)
        //{
        //    if (aBoolValue)
        //    {
        //        return 是;
        //    }
        //    else
        //    {
        //        return 否;
        //    }
        //}

        //public static bool isTrue(string exp)
        //{
        //    if (string.IsNullOrEmpty(exp))
        //    {
        //        return false;
        //    }
        //    return ((exp.ToUpper() == Y.ToUpper()) || (exp == 是));
        //}
    }


    /// <summary>
    /// 明显能解决的错误。不需要运营时记录，等待解决。为了少写return发明的。
    /// </summary>
    [Serializable]
    public class JustAlertException : Exception
    {
        public JustAlertException(string ShowMessage)
            : base(ShowMessage)
        {
        }
        public JustAlertException(string ShowMessage,Exception innerex)
            : base(ShowMessage, innerex)
        {
        }
    }


    /// <summary>
    /// Class used to hold sort information
    /// </summary>
    public class SortClass
    {
        private string _sortProperty;

        public string SortProperty
        {
            get { return _sortProperty; }
            set { _sortProperty = value; }
        }

        private ListSortDirection _sortDirection;

        public ListSortDirection SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        public SortClass(string sortProperty, ListSortDirection sortDirection)
        {
            _sortProperty = sortProperty;
            _sortDirection = sortDirection;
        }
    }

    /// <summary>
    /// Implementation of IComparer
    /// </summary>
    public class Comparer<T> : IComparer<T>
    {
        private List<SortClass> _sortClasses;

        /// <summary>
        /// Collection of sorting criteria
        /// </summary>
        public List<SortClass> SortClasses
        {
            get { return _sortClasses; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Comparer()
        {
            _sortClasses = new List<SortClass>();
        }

        /// <summary>
        /// Constructor that takes a sorting class collection as param
        /// </summary>
        /// <param name="sortClass">
        /// Collection of sorting criteria 
        ///</param>
        public Comparer(List<SortClass> sortClass)
        {
            _sortClasses = sortClass;
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="sortProperty">Property to sort on</param>
        /// <param name="sortDirection">Direction to sort</param>
        public Comparer(string sortProperty, ListSortDirection sortDirection)
        {
            _sortClasses = new List<SortClass>();
            _sortClasses.Add(new SortClass(sortProperty, sortDirection));
        }

        /// <summary>
        /// Implementation of IComparer interface to compare to object
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            if (SortClasses.Count == 0)
            {
                return 0;
            }
            return CheckSort(0, x, y);
        }

        /// <summary>
        /// Recursive function to do sorting
        /// </summary>
        /// <param name="sortLevel">Current level sorting at</param>
        /// <param name="myObject1"></param>
        /// <param name="myObject2"></param>
        /// <returns></returns>
        private int CheckSort(int sortLevel, T myObject1, T myObject2)
        {
            int returnVal = 0;
            if (SortClasses.Count - 1 >= sortLevel)
            {
                object valueOf1 = myObject1.GetType().GetProperty(SortClasses[sortLevel].SortProperty).GetValue(myObject1, null);
                object valueOf2 = myObject2.GetType().GetProperty(SortClasses[sortLevel].SortProperty).GetValue(myObject2, null);
                if (SortClasses[sortLevel].SortDirection == ListSortDirection.Ascending)
                {
                    returnVal = ((IComparable) valueOf1).CompareTo((IComparable) valueOf2);
                }
                else
                {
                    returnVal = ((IComparable) valueOf2).CompareTo((IComparable) valueOf1);
                }

                if (returnVal == 0)
                {
                    returnVal = CheckSort(sortLevel + 1, myObject1, myObject2);
                }
            }


            return returnVal;
        }
    }

    /// <summary>
    /// 使用举例 List<Project> projectList=...;
    /// List<Project> sortedProject = ListSorter.SortList(projectList, "Name", SortDirection.Ascending);
    /// </summary>
    public class ListSorter
    {
        public static List<T> SortList<T>(List<T> listToSort, List<string> sortExpression, List<ListSortDirection> sortDirection)
        {
            //check parameters           
            if (sortExpression.Count != sortDirection.Count || sortExpression.Count == 0 || sortDirection.Count == 0)
            {
                throw new Exception("Invalid sort arguments!");
            }

            //get myComparer
            Comparer<T> myComparer = new Comparer<T>();
            for (int i = 0; i < sortExpression.Count; i++)
            {
                SortClass sortClass = new SortClass(sortExpression[i], sortDirection[i]);
                myComparer.SortClasses.Add(sortClass);
            }
            listToSort.Sort(myComparer);
            return listToSort;
        }

        public static List<T> SortList<T>(List<T> listToSort, string sortExpression, ListSortDirection sortDirection)
        {
            //check parameters
            if (sortExpression == null || sortExpression == string.Empty)
            {
                return listToSort;
            }

            Comparer<T> myComparer = new Comparer<T>();
            myComparer.SortClasses.Add(new SortClass(sortExpression, sortDirection));
            listToSort.Sort(myComparer);
            return listToSort;
        }
    }

    /// <summary>
    /// 对MODEL属性，区分是否 其他MODEL引用其值的。通常是本MODEL的主键，唯一一个property上设定。简化一些工作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DocRefKey : Attribute
    {
        // private bool pvIsRefKey=false;
        public DocRefKey()
        {
            this.isRefKey = true;
        }

        public bool isRefKey { get; set; }
    }

    public class pc_临时ID共享器
    {
        private int _pv临时ID = -1;
        public   int Next临时Id
        {
            get
            {
                lock (this)
                {
                    _pv临时ID--;
                return _pv临时ID;
                }
            }
        }
    }

    public class pc_整数线程队列
    {
        private Queue<int> pv_队列 = new Queue<int>();

        public void Enqueue(int value)
        {
            lock (this)
            {
                pv_队列.Enqueue(value);
            }
        }

        public void Enqueue(List<int> value)
        {
            lock (this)
            {
                foreach (int lvi in value)
                {
                    pv_队列.Enqueue(lvi);
                }
            }
        }

        public int pt_deque()
        {
            lock (this)
            {
                if (pv_队列.Count > 0)
                {
                    return pv_队列.Dequeue();
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}