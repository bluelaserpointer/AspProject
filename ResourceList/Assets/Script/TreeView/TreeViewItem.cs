using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TreeViewItem : MonoBehaviour
{

    [SerializeField]
    GameObject _menuItem;

    public bool IsExpanding = false;

    private int _hierarchy = 0;

    private TreeViewItem _parent;

    private List<TreeViewItem> _children;

    private bool _isRefreshing = false;



    //public UnityEvent leftClick;
    //public UnityEvent rightClick;

    private void Start()
    {
        //leftClick.AddListener(new UnityAction(ButtonLeftClick));
        //rightClick.AddListener(new UnityAction(ButtonRightClick));
    }

    private void Update()
    {

    }

    public void ContextButtonClick()
    {
        //上一轮刷新还未结束
        if (_isRefreshing)
        {
            return;
        }

        _isRefreshing = true;

        Debug.Log("click icon, isExpanding:" + IsExpanding);
        if (IsExpanding)
        {
            transform.Find("ContextButton").GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 90);
            IsExpanding = false;
            ChangeChildren(this, false);
        }
        else
        {
            transform.Find("ContextButton").GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            IsExpanding = true;
            ChangeChildren(this, true);
        }

        //刷新树形菜单
        //GameManager.TreeView.RefreshTreeView();

        _isRefreshing = false;
    }
    
    public void ChangeChildren(TreeViewItem tvi, bool value)
    {
        for (int i = 0; i < tvi.GetChildrenNumber(); i++)
        {
            tvi.GetChildrenByIndex(i).gameObject.SetActive(value);
            if (tvi.GetChildrenByIndex(i).IsExpanding)
            {
                ChangeChildren(tvi.GetChildrenByIndex(i), value);
            }
        }
    }

    public void OnClick()
    {
        Debug.Log("Click");
        if (Input.GetMouseButtonDown(0))
        {
            // select
            _menuItem.SetActive(false);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // open menu
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            float z = Input.mousePosition.z;
            _menuItem.transform.position = new Vector3(x, y, z);
            _menuItem.SetActive(true);
        }
    }

    #region 属性访问 GetSet方法
    public int GetHierarchy()
    {
        return _hierarchy;
    }
    public void SetHierarchy(int hierarchy)
    {
        _hierarchy = hierarchy;
    }
    public TreeViewItem GetParent()
    {
        return _parent;
    }
    public void SetParent(TreeViewItem parent)
    {
        _parent = parent;
    }
    public void AddChildren(TreeViewItem children)
    {
        if (_children == null)
        {
            _children = new List<TreeViewItem>();
        }
        _children.Add(children);
    }
    public void RemoveChildren(TreeViewItem children)
    {
        if (_children == null)
        {
            return;
        }
        _children.Remove(children);
    }
    public void RemoveChildren(int index)
    {
        if (_children == null || index < 0 || index >= _children.Count)
        {
            return;
        }
        _children.RemoveAt(index);
    }
    public int GetChildrenNumber()
    {
        if (_children == null)
        {
            return 0;
        }
        return _children.Count;
    }
    public TreeViewItem GetChildrenByIndex(int index)
    {
        if (index >= _children.Count)
        {
            return null;
        }
        return _children[index];
    }
    #endregion
}
