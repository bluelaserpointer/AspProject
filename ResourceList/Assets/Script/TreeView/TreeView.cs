using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class TreeView : MonoBehaviour
{
    [SerializeField]
    Transform _treeItemRoot;
    [SerializeField]
    GameObject _treeItemPrefab;
    

    private List<ResourceItem> _resourceItems => GameManager.ResourceItems;
    private List<GameObject> _treeViewItems = null;
    private List<GameObject> _treeViewItemsClone;

    private bool _isRefreshing = false;

    //public UnityEvent rightClick;

    //���β˵���ǰˢ�¶��е�Ԫ�����㼶
    private int _hierarchy = 0;

    private void Start()
    {
        GenerateTreeView();

        //rightClick.AddListener(new UnityAction(ButtonRightClick));

        //GameManager.OnResourceItemHighlightChange.AddListener((oldItem, newItem) =>
        //{
        //    FindResourceItemUITag(oldItem)?.SetHighlightState(false);
        //    FindResourceItemUITag(newItem)?.SetHighlightState(true);
        //});
        //GameManager.OnResourceItemSelect.AddListener((item, cond) =>
        //{
        //    FindResourceItemUITag(item).SetSelectState(cond);
        //});
    }

    private void Update()
    {
        RefreshTreeView();
        //GenerateTreeView();
        
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (eventData.button == PointerEventData.InputButton.Right)
    //    {
    //        rightClick.Invoke();
    //    }
    //}

    //private void ButtonRightClick()
    //{
    //    Debug.Log("Button Right Click");
    //    // OPEN MENU
    //}

    private void CreateNewFolder()
    {
        
    }

    public void GenerateTreeView()
    {
        //ɾ�������Ѿ����ڵ����β˵�Ԫ��
        if (_treeViewItems != null)
        {
            for (int i = 0; i < _treeViewItems.Count; i++)
            {
                Destroy(_treeViewItems[i]);
            }
            _treeViewItems.Clear();
        }
        //���´������β˵�Ԫ��
        _treeViewItems = new List<GameObject>();
        List<ResourceItem> rootItems = _resourceItems.FindAll(item => item.Hierarchy == 0);

        for (int i = 0; i < _resourceItems.Count; i++)
        {
            GameObject treeItem = Instantiate(_treeItemPrefab, _treeItemRoot);

            if (_resourceItems[i].Hierarchy == 0)
            {
                treeItem.GetComponent<TreeViewItem>().SetHierarchy(0);
                treeItem.GetComponent<TreeViewItem>().SetParent(null);
            }
            else
            {
                TreeViewItem tvi = _treeViewItems[_resourceItems[i].ParentID].GetComponent<TreeViewItem>();
                treeItem.GetComponent<TreeViewItem>().SetHierarchy(tvi.GetHierarchy() + 1);
                treeItem.GetComponent<TreeViewItem>().SetParent(tvi);
                tvi.AddChildren(treeItem.GetComponent<TreeViewItem>());
            }

            treeItem.transform.name = "TreeViewItem";
            treeItem.transform.Find("TreeViewText").GetComponent<Text>().text = _resourceItems[i].Name;
            treeItem.SetActive(true);

            _treeViewItems.Add(treeItem);
        }
    }

    public void RefreshTreeView()
    {
        //��һ��ˢ�»�δ����
        if (_isRefreshing)
        {
            return;
        }

        _isRefreshing = true;

        //����һ�ݲ˵�
        _treeViewItemsClone = new List<GameObject>(_treeViewItems);

        //�ø��ƵĲ˵�����ˢ�¼���
        for (int i = 0; i < _treeViewItemsClone.Count; i++)
        {
            //�Ѿ���������߲���Ҫ����λ�õ�Ԫ��
            if (_treeViewItemsClone[i] == null || !_treeViewItemsClone[i].activeSelf)   // activeSelf �����active״̬
            {
                continue;
            }

            TreeViewItem tvi = _treeViewItemsClone[i].GetComponent<TreeViewItem>();

            if (tvi.GetHierarchy() > _hierarchy)
            {
                _hierarchy = tvi.GetHierarchy();
            }

            //�����Ԫ����չ���ģ���������ˢ��
            if (tvi.IsExpanding)
            {
                RefreshTreeViewChild(tvi);
            }

            _treeViewItemsClone[i] = null;
        }

        //��ո��ƵĲ˵�
        _treeViewItemsClone.Clear();

        _isRefreshing = false;
    }

    private void RefreshTreeViewChild(TreeViewItem tvi)
    {
        for (int i = 0; i < tvi.GetChildrenNumber(); i++)
        {
            if (tvi.GetChildrenByIndex(i).GetHierarchy() > _hierarchy)
            {
                _hierarchy = tvi.GetChildrenByIndex(i).GetHierarchy();
            }

            //�����Ԫ����չ���ģ���������ˢ��
            if (tvi.GetChildrenByIndex(i).IsExpanding)
            {
                RefreshTreeViewChild(tvi.GetChildrenByIndex(i));
            }

            int index = _treeViewItemsClone.IndexOf(tvi.GetChildrenByIndex(i).gameObject);
            if (index >= 0)
            {
                _treeViewItemsClone[index] = null;
            }
        }
    }


    //public TreeViewItem FindTreeViewItem(ResourceItem resourceItem)
    //{
    //    if (resourceItem == null)
    //        return null;
    //    foreach (Transform tagTransform in _resourceItemUITagRoot)
    //    {
    //        ResourceItemUITag tag = tagTransform.GetComponent<ResourceItemUITag>();
    //        if (tag.ResourceItem == resourceItem)
    //        {
    //            return tag;
    //        }
    //    }
    //    return null;
    //}
}
