using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RenderFileList : MonoBehaviour
{
    public static RenderFileList renderFileList;

    private List<string> fileList;

    public TreeViewControl treeView;    // ��ʾ��unity�еĲ���

    // Start is called before the first frame update
    void Start()
    {
            
    }

    void Awake()
    {
        renderFileList = new RenderFileList();

        fileList = GameObject.Find("fileList").GetComponent<List<string>>();

        //�������� TreeViewDada from where
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

        //ָ������Դ
        treeView.Data = datas;
        //�����������β˵�
        treeView.GenerateTreeView();
        //ˢ�����β˵�
        treeView.RefreshTreeView();
        //ע����Ԫ�ص�������¼�
        treeView.ClickItemEvent += CallBack;
    }


    // Update is called once per frame
    void Update()
    {
        // ����д�����
    }

    void CallBack(GameObject item)
    {
        Debug.Log("����� " + item.transform.Find("TreeViewText").GetComponent<Text>().text);
    }

    #region ���Է��� GetSet����
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
