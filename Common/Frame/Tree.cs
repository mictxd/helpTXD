using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MultTreeNode
{
    #region members

    private int parentId;
    private int selfId;
    protected String nodeName;
    protected Object obj;
    protected MultTreeNode parentNode;
    protected List<MultTreeNode> childList;

    #endregion members

    #region contrustion

    public MultTreeNode()
    {
        init();
    }

    public MultTreeNode(MultTreeNode parentNode)
    {
        init(parentNode);
    }

    public void init(MultTreeNode pNode = null)
    {
        if (childList == null)
            childList = new List<MultTreeNode>();
        parentId = 0;
        selfId = 0;
        nodeName = "";
        obj = null;

        if (pNode != null)
        {
            nodeName = pNode.getNodeName();
            if (pNode.getObj() != null)
                obj = new Hashtable((Hashtable) pNode.getObj());
            parentNode = pNode.getParentNode();
            if (pNode.getChildList() != null)
                childList = new List<MultTreeNode>(pNode.getChildList());
        }

    }

    #endregion contrustion

    #region check node

    public bool isLeaf()
    {
        return childList == null || childList.Count == 0;
    }

    public bool isRoot()
    {
        return parentNode == null;
    }

    /// <summary>
    /// check exist of child node.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool ExistChildNode(string name) //,bool IgnoreCase = false
    {
        bool bRet = false;
        foreach (MultTreeNode node in childList)
        {
            if (node.nodeName == name)
            {
                bRet = true;
                break;
            }
        }
        return bRet;
    }

    #endregion check node

    #region insert

    /// <summary>
    /// insert child node to one node(this).
    /// </summary>
    /// <param name="name">new child node name.</param>
    /// <param name="path"></param>
    /// <param name="newnode">can be null. no use I think.</param>
    /// <returns></returns>
    public virtual MultTreeNode InsertNode(string name, string path = "", MultTreeNode newnode = null)
    {
        MultTreeNode pntnode = this;
        if (newnode == null) newnode = new MultTreeNode();
        if (path.Length > 0)
        {
            pntnode = findNodeByPath(path);
        }
        if (pntnode == null) return null;
        // check if has same node with same name.
        // can not insert two same name node under one parent node.
        // 2016-03-04 09:38:33
        if (pntnode.ExistChildNode(name))
            return pntnode;
        newnode.setNodeName(name);
        newnode.parentNode = pntnode;
        pntnode.InsertChildNode(newnode);
        return newnode;
    }

    /** 插入一个child节点到当前节点中 */

    public virtual void InsertChildNode(MultTreeNode treeNode)
    {
        if (childList == null)
            childList = new List<MultTreeNode>();
        treeNode.setParentNode(this);
        childList.Add(treeNode);
    }

    #endregion insert

    #region get

    /// <summary>
    /// get current node 's brother node count.
    /// = parent node 's childlist size.
    /// </summary>
    /// <returns>current node 's brother node count</returns>
    public int GetBrotherNodeCount()
    {
        if (parentNode != null)
        {
            return parentNode.getChildList() != null ? parentNode.getChildList().Count : 1;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// get current node 's childlist count.
    /// </summary>
    /// <returns></returns>
    public int GetChildNodeCount()
    {
        return childList == null ? 0 : childList.Count;

    }

    /** 找到一颗树中某个节点 按节点ID */

    public MultTreeNode findNodeById(int id)
    {
        if (this.selfId == id)
            return this;
        if (isLeaf())
        {
            return null;
        }
        else
        {
            int childNumber = childList.Count;
            for (int i = 0; i < childNumber; i++)
            {
                MultTreeNode child = childList.ElementAt(i);
                MultTreeNode resultNode = child.findNodeById(id);
                if (resultNode != null)
                {
                    return resultNode;
                }
            }
            return null;
        }
    }


    /// <summary>
    /// find one note in tree.
    /// 2016-03-04 16:21:34
    /// modify when empty path .return this instead of null.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public MultTreeNode findNodeByPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return this;
        //path = StrUtils.FormatPath(path);
        //path = StrUtils.GetSDirString(path);
        string[] pathArr = path.Split(new char[] {'\\'}); //StrUtils.Split(path, "\\"); //path.Split('\\');
        if (pathArr[0] == null) pathArr[0] = path;

        // make sure this is root path.
        if (nodeName != pathArr[0]) return null;

        MultTreeNode node = this;
        for (int i = 0; i < pathArr.Count(); i++)
        {
            // 说明：注释。不知道为什么要加这句 可能会影响代码生成。需要注意
            // 修改为 break; => continue; \ 查找具有 相同节点名称的 子节点 逻辑
            // 短暂测试是正常的。
            // 修改日期：2014-8-5 14:20:28
            if (string.IsNullOrEmpty(pathArr[i]))
            {
                List<MultTreeNode> list = node.getChildList();
                if (i < pathArr.Count() - 1)
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j].getNodeName() == pathArr[i + 1])
                        {
                            node = list[j];
                        }
                    }
                continue;
            }
            // use path to avoid error like 0\0\0
            // 2016-07-11 21:34:54
            string nodePath = node.getPath();
            string testPath = StrUtils.FromArr(ArrayUtils.SplitArray<string>(pathArr, 0, i - 1), "\\");

            if (node.getNodeName() == pathArr[i] && nodePath == testPath)
            {
                List<MultTreeNode> list = node.getChildList();
                for (int j = 0; j < list.Count; j++)
                {
                    if (i + 1 < pathArr.Length && list[j].getNodeName() == pathArr[i + 1])
                    {
                        node = list[j];
                    }
                }
            }
            else
            {
                node = null;
                break;
            }
        }
        return node;
    }

    /// <summary>
    /// build one path by node name
    /// </summary>
    /// <returns>path like directory</returns>
    public string getPath()
    {
        List<MultTreeNode> list = getElders();
        string path = "";
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                if (path.Length > 0) path += "\\";
                path += list[i].getNodeName();
            }
        }
        return path;
    }

    /** 返回当前节点的父辈节点集合 形成路径使用 */

    public List<MultTreeNode> getElders()
    {
        ListExtA<MultTreeNode> elderList = new ListExtA<MultTreeNode>();
        MultTreeNode parentNode = this.getParentNode();
        if (parentNode == null)
        {
            return elderList;
        }
        else
        {
            elderList.AddAll(parentNode.getElders());
            elderList.Add(parentNode);
            return elderList;
        }
    }

    /** 返回当前节点的晚辈集合 */

    public List<MultTreeNode> getJuniors()
    {
        ListExtA<MultTreeNode> juniorList = new ListExtA<MultTreeNode>();
        List<MultTreeNode> childList = this.getChildList();
        if (childList == null)
        {
            return juniorList;
        }
        else
        {
            int childNumber = childList.Count;
            for (int i = 0; i < childNumber; i++)
            {
                MultTreeNode junior = childList.ElementAt(i);
                juniorList.Add(junior);
                juniorList.AddAll(junior.getJuniors());
            }
            return juniorList;
        }
    }


    /** 返回当前节点的孩子集合 */

    public List<MultTreeNode> getChildList()
    {
        return childList;
    }

    #endregion get

    #region delete

    /** 删除节点和它下面的晚辈 */

    public void deleteNode()
    {
        MultTreeNode parentNode = this.getParentNode();
        int id = this.getSelfId();

        if (parentNode != null)
        {
            parentNode.deleteChildNode(id);
        }
    }

    /** 删除当前节点的某个子节点
     *  childId=0时删除所有子节点
     */

    public void deleteChildNode(int childId = 0)
    {
        List<MultTreeNode> childList = this.getChildList();
        int childNumber = childList.Count;
        for (int i = 0; i < childNumber; i++)
        {
            MultTreeNode child = childList.ElementAt(i);
            if (child.getSelfId() == childId || childId == 0)
            {
                childList.RemoveAt(i);
                return;
            }
        }
    }

    #endregion delete

    #region loop

    public delegate int OneStepTravers(MultTreeNode node);

    /** 遍历一棵树，深度遍历 */

    public void depthtraverse(OneStepTravers stepbef, OneStepTravers stepafter)
    {
        if (stepbef != null)
            stepbef(this);

        if (childList != null)
        {
            int childNumber = childList.Count;
            for (int i = 0; i < childNumber; i++)
            {
                MultTreeNode child = childList.ElementAt(i);
                child.depthtraverse(stepbef, stepafter);
            }
        }
        if (stepafter != null)
            stepafter(this);
    }

    #endregion loop

    #region node test

    public void print(String content)
    {
        System.Diagnostics.Debug.WriteLine(content);
    }

    public void print(int content)
    {
        System.Diagnostics.Debug.WriteLine(content.ToString());
    }

    #endregion node test

    #region attribute


    public void setChildList(List<MultTreeNode> childList)
    {
        this.childList = childList;
    }

    public int getParentId()
    {
        return parentId;
    }

    public void setParentId(int parentId)
    {
        this.parentId = parentId;
    }

    public int getSelfId()
    {
        return selfId;
    }

    public void setSelfId(int selfId)
    {
        this.selfId = selfId;
    }

    public MultTreeNode getParentNode()
    {
        return parentNode;
    }

    public void setParentNode(MultTreeNode parentNode)
    {
        this.parentNode = parentNode;
    }

    public String getNodeName()
    {
        return nodeName;
    }

    public void setNodeName(String nodeName)
    {
        this.nodeName = nodeName;
    }

    public Object getObj()
    {
        return obj;
    }

    public void setObj(Object obj)
    {
        this.obj = obj;
    }

    #endregion attribute
    public class ListExtA<T> : System.Collections.Generic.List<T>
    {
        public void AddAll(List<T> addlist)
        {
            for (int i = 0; i < addlist.Count; i++)
                Add(addlist.ElementAt(i));
        }

        public static T Get(List<T> list, int nIndex = 0)
        {
            if (nIndex >= list.Count || nIndex < 0) return default(T);
            return list.ElementAt(nIndex);
        }
    }
}

