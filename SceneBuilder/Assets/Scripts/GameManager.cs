using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    SceneView _sceneView;
    [SerializeField]
    SceneCameraManager _sceneCameraManager;
    [SerializeField]
    ControlAxisManager _controlAxis;

    public static SceneView SceneView => Instance._sceneView;
    public static SceneCameraManager SceneCameraManager => Instance._sceneCameraManager;
    public static ControlAxisManager ControlAxis => Instance._controlAxis;
    public static Camera SceneCamera => SceneCameraManager.Camera;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
}
