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

    //树形菜单当前刷新队列的元素最大层级
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
        //删除可能已经存在的树形菜单元素
        if (_treeViewItems != null)
        {
            for (int i = 0; i < _treeViewItems.Count; i++)
            {
                Destroy(_treeViewItems[i]);
            }
            _treeViewItems.Clear();
        }
        //重新创建树形菜单元素
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
        //上一轮刷新还未结束
        if (_isRefreshing)
        {
            return;
        }

        _isRefreshing = true;

        //复制一份菜单
        _treeViewItemsClone = new List<GameObject>(_treeViewItems);

        //用复制的菜单进行刷新计算
        for (int i = 0; i < _treeViewItemsClone.Count; i++)
        {
            //已经计算过或者不需要计算位置的元素
            if (_treeViewItemsClone[i] == null || !_treeViewItemsClone[i].activeSelf)   // activeSelf 物体的active状态
            {
                continue;
            }

            TreeViewItem tvi = _treeViewItemsClone[i].GetComponent<TreeViewItem>();

            if (tvi.GetHierarchy() > _hierarchy)
            {
                _hierarchy = tvi.GetHierarchy();
            }

            //如果子元素是展开的，继续向下刷新
            if (tvi.IsExpanding)
            {
                RefreshTreeViewChild(tvi);
            }

            _treeViewItemsClone[i] = null;
        }

        //清空复制的菜单
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

            //如果子元素是展开的，继续向下刷新
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
