using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform _importedItemRoot;
    [SerializeField]
    TreeView _treeView;
    [SerializeField]
    PanelView _panelView;

    public static GameManager Instance { get; private set; }
    public static TreeView TreeView => Instance._treeView;
    public static PanelView PanelView => Instance._panelView;
    public static List<ResourceItem> ResourceItems => Instance._resourceItems;

    public static ResourceItem HighlightedResourceItem
    {
        get => Instance._highlightedResourceItem;
        set
        {
            if (Instance._highlightedResourceItem != value)
            {
                ResourceItem old = Instance._highlightedResourceItem;
                Instance._highlightedResourceItem = value;
                OnResourceItemHighlightChange.Invoke(old, value);
            }
        }
    }

    public static UnityEvent<ResourceItem, ResourceItem> OnResourceItemHighlightChange => Instance._onResourceItemHighlightChange;
    public static UnityEvent<ResourceItem, bool> OnResourceItemSelect => Instance._onResourceItemSelect;

    ResourceItem _highlightedResourceItem;

    readonly UnityEvent<ResourceItem, ResourceItem> _onResourceItemHighlightChange = new UnityEvent<ResourceItem, ResourceItem>();
    readonly UnityEvent<ResourceItem, bool> _onResourceItemSelect = new UnityEvent<ResourceItem, bool>();
    readonly List<ResourceItem> _resourceItems = new List<ResourceItem>();


    private void Awake()
    {
        Instance = this;

        ResourceItem item = new ResourceItem();
        item.Name = "Asserts";
        item.isFolder = true;
        item.ID = 0;
        item.Hierarchy = 0;
        Instance._resourceItems.Add(item);

        item = new ResourceItem();
        item.Name = "test 1";
        item.isFolder = false;
        item.ID = 1;
        item.ParentID = 0;
        item.Hierarchy = 1;
        Instance._resourceItems.Add(item);
    }

    private void Update()
    {
        // ResourceItem有成功更新
        //foreach (var item in ResourceItems)
        //{
        //    Debug.Log(item.Name);
        //}
    }

    public static void SelectResourceItem(ResourceItem resourceItem)
    {
        //if (!SelectedResourceItems.Contains(resourceItem))
        //{
        //    SelectedResourceItems.Add(resourceItem);
        //    OnResourceItemSelect.Invoke(resourceItem, true);
        //}
    }

}
