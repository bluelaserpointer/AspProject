using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TreeViewItem : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    GameObject _menuPrefab;

    public bool IsExpanding = false;

    private int _hierarchy = 0;

    private TreeViewItem _parent;

    private List<TreeViewItem> _children;

    private bool _isRefreshing = false;

    public UnityEvent leftClick;
    public UnityEvent rightClick;

    GameObject _menuItem = null;

    private void Start()
    {

        _menuItem = Instantiate(_menuPrefab, this.transform);
        leftClick.AddListener(new UnityAction(ButtonLeftClick));
        rightClick.AddListener(new UnityAction(ButtonRightClick));
    }

    public void ContextButtonClick()
    {
        if (_isRefreshing)
        {
            return;
        }

        _isRefreshing = true;

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
        GameManager.TreeView.RefreshTreeView();

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClick.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClick.Invoke();
        }
    }

    private void ButtonLeftClick()
    {
        //GameManager.TreeView.HideMenu();
    }

    private void ButtonRightClick()
    {
        // Set other menu inactive
        GameManager.TreeView.HideMenu();
        // open menu
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        float z = Input.mousePosition.z;
        _menuItem.transform.position = new Vector3(x, y, z);
        _menuItem.SetActive(true);
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
    public void HideMenu()
    {
        _menuItem?.SetActive(false);
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
