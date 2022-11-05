using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TreeViewTest : MonoBehaviour 
{
    public TreeViewControl treeView;    // 显示在unity中的参数

    
    // TreeViewControl - 不用考虑层级关系 & ns 直接引用
    // Awake: 初始化生命周期
    void Awake()
	{
        //生成数据 TreeViewDada from where
        List<TreeViewData> datas = new List<TreeViewData>();

        TreeViewData data = new TreeViewData();
        data.Name = "Root Folder";
        data.ParentID = -1;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "Level 1 Folder";
        data.ParentID = 0;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "Level 1 Folder";
        data.ParentID = 0;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "Level 2 Folder";
        data.ParentID = 1;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "Level 2 Folder";
        data.ParentID = 2;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "Level 2 Folder";
        data.ParentID = 1;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "Level 3 Folder";
        data.ParentID = 3;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "Level 3 Folder";
        data.ParentID = 3;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "Level 4 Folder";
        data.ParentID = 7;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "Level 4 Folder";
        data.ParentID = 7;
        datas.Add(data);

        data = new TreeViewData();
        data.Name = "File";
        data.ParentID = 8;
        datas.Add(data);

        //指定数据源
        treeView.Data = datas;
        //重新生成树形菜单
        treeView.GenerateTreeView();
        //刷新树形菜单
        treeView.RefreshTreeView();
        //注册子元素的鼠标点击事件
        treeView.ClickItemEvent += CallBack;
    }

    void Update()
    {
        //判断树形菜单中名为“ Root Folder ”的元素是否被勾选
        if (Input.GetKeyDown(KeyCode.A))    // 用A判断？
        {
            bool isCheck = treeView.ItemIsCheck("Root Folder");
            Debug.Log("当前树形菜单中的元素 Root Folder " + (isCheck?"已被选中！":"未被选中！"));
        }
        //获取树形菜单中所有被勾选的元素
        if (Input.GetKeyDown(KeyCode.S))
        {
            List<string> items = treeView.ItemsIsCheck();
            for (int i = 0; i < items.Count; i++)
            {
                Debug.Log("当前树形菜单中被选中的元素有：" + items[i]);
            }
        }
    }

    void CallBack(GameObject item)
    {
        Debug.Log("点击了 " + item.transform.Find("TreeViewText").GetComponent<Text>().text);
    }
}
