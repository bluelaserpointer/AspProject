using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class TreeView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Transform _treeItemRoot;
    [SerializeField]
    GameObject _treeItemPrefab;
    [SerializeField]
    GameObject _menuItem;


    private List<ResourceItem> _resourceItems => GameManager.ResourceItems;
    private List<GameObject> _treeViewItems = null;
    private List<GameObject> _treeViewItemsClone;

    private bool _isRefreshing = false;

    public UnityEvent leftClick;

    //树形菜单当前刷新队列的元素最大层级
    private int _hierarchy = 0;

    private void Start()
    {
        GenerateTreeView();

        leftClick.AddListener(new UnityAction(ButtonLeftClick));

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
        // RefreshTreeView();
        // GenerateTreeView();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClick.Invoke();
        }
    }

    private void ButtonLeftClick()
    {
        //this.HideMenu();
    }

    public void CreateNewFolder()
    {
        ResourceItem item = new ResourceItem();
        item.Name = "Folder";
        item.isFolder = true;
        GameManager.ResourceItems.Add(item);
    }

    public void GenerateTreeView()
    {
        if (_treeViewItems != null)
        {
            for (int i = 0; i < _treeViewItems.Count; i++)
            {
                Destroy(_treeViewItems[i]);
            }
            _treeViewItems.Clear();
        }

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
        if (_isRefreshing)
        {
            return;
        }

        _isRefreshing = true;

        _treeViewItemsClone = new List<GameObject>(_treeViewItems);


        for (int i = 0; i < _treeViewItemsClone.Count; i++)
        {
            if (_treeViewItemsClone[i] == null || !_treeViewItemsClone[i].activeSelf)   
            {
                continue;
            }

            TreeViewItem tvi = _treeViewItemsClone[i].GetComponent<TreeViewItem>();
            if (tvi.GetHierarchy() > _hierarchy)
            {
                _hierarchy = tvi.GetHierarchy();    // 更新一个最大hierarchy?
            }

            if (tvi.IsExpanding)
            {
                RefreshTreeViewChild(tvi);
            }

            _treeViewItemsClone[i] = null;
        }

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

    public void HideMenu()
    {
        for (int i = 0; i < _treeViewItems.Count; i++)
        {
            TreeViewItem tvi = _treeViewItems[i].GetComponent<TreeViewItem>();
            tvi.HideMenu();
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
