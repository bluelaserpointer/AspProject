using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RenderFileList : MonoBehaviour
{
    public static RenderFileList renderFileList;

    private List<string> fileList;

    public TreeViewControl treeView;    // 显示在unity中的参数

    // Start is called before the first frame update
    void Start()
    {
            
    }

    void Awake()
    {
        renderFileList = new RenderFileList();

        fileList = GameObject.Find("fileList").GetComponent<List<string>>();

        //生成数据 TreeViewDada from where
        List<TreeViewData> datas = new List<TreeViewData>();

        TreeViewData data = new TreeViewData();
        data.Name = "Resource";
        data.ParentID = -1;
        datas.Add(data);

        foreach (var file in fileList)
        {
            data = new TreeViewData();
            data.Name = file;
            data.ParentID = 0;
            datas.Add(data);
        }

        //指定数据源
        treeView.Data = datas;
        //重新生成树形菜单
        treeView.GenerateTreeView();
        //刷新树形菜单
        treeView.RefreshTreeView();
        //注册子元素的鼠标点击事件
        treeView.ClickItemEvent += CallBack;
    }


    // Update is called once per frame
    void Update()
    {
        // 更新写在这里？
    }

    void CallBack(GameObject item)
    {
        Debug.Log("点击了 " + item.transform.Find("TreeViewText").GetComponent<Text>().text);
    }

    #region 属性访问 GetSet方法
    public List<string> GetFileList()
    {
        return _fileList;
    }
    public void AddFile(string file)
    {
        _fileList.Add(file);
    }
    #endregion
}
