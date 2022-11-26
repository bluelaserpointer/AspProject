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
    public static BuildItem HighlightedBuildItem
    {
        get => Instance._highlightedBuildItem;
        set
        {
            if(Instance._highlightedBuildItem != value)
            {
                BuildItem old = Instance._highlightedBuildItem;
                Instance._highlightedBuildItem = value;
                OnHighlightChange.Invoke(old, value);
            }
        }
    }
    public static UnityEvent<BuildItem, BuildItem> OnHighlightChange => Instance.onHighlightChange;

    BuildItem _highlightedBuildItem;
    readonly UnityEvent<BuildItem, BuildItem> onHighlightChange = new UnityEvent<BuildItem, BuildItem>();
    private void Awake()
    {
        Instance = this;
    }
}
