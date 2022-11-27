using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform _buildItemRoot;
    [SerializeField]
    SceneView _sceneView;
    [SerializeField]
    SceneCameraManager _sceneCameraManager;
    [SerializeField]
    BuildItemMoveController _itemMoveController;
    
    public static GameManager Instance { get; private set; }
    public static Transform BuildItemRoot => Instance._buildItemRoot;
    public static SceneView SceneView => Instance._sceneView;
    public static SceneCameraManager SceneCameraManager => Instance._sceneCameraManager;
    public static BuildItemMoveController ControlAxis => Instance._itemMoveController;
    public static Camera SceneCamera => SceneCameraManager.Camera;
    public static List<BuildItem> SelectedBuildItems => Instance._selectedBuildItems;
    public static BuildItem HighlightedBuildItem
    {
        get => Instance._highlightedBuildItem;
        set
        {
            if(Instance._highlightedBuildItem != value)
            {
                BuildItem old = Instance._highlightedBuildItem;
                Instance._highlightedBuildItem = value;
                OnBuildItemHighlightChange.Invoke(old, value);
            }
        }
    }
    public static UnityEvent<BuildItem, BuildItem> OnBuildItemHighlightChange => Instance._onBuildItemHighlightChange;
    public static UnityEvent<BuildItem, bool> OnBuildItemSelect => Instance._onBuildItemSelect;

    BuildItem _highlightedBuildItem;
    readonly UnityEvent<BuildItem, BuildItem> _onBuildItemHighlightChange = new UnityEvent<BuildItem, BuildItem>();
    readonly UnityEvent<BuildItem, bool> _onBuildItemSelect = new UnityEvent<BuildItem, bool>();
    readonly List<BuildItem> _selectedBuildItems = new List<BuildItem>();
    private void Awake()
    {
        Instance = this;
    }
    public static void SelectBuildItem(BuildItem buildItem)
    {
        if(!SelectedBuildItems.Contains(buildItem))
        {
            SelectedBuildItems.Add(buildItem);
            OnBuildItemSelect.Invoke(buildItem, true);
        }
    }
    public static bool DeselectBuildItem(BuildItem buildItem)
    {
        if(SelectedBuildItems.Remove(buildItem))
        {
            OnBuildItemSelect.Invoke(buildItem, false);
            return true;
        }
        return false;
    }
    public static void DeselectAllBuildItems()
    {
        foreach (var item in SelectedBuildItems)
            OnBuildItemSelect.Invoke(item, false);
        SelectedBuildItems.Clear();
    }
}