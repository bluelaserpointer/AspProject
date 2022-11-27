using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 通过拖动小坐标轴 移动模型
/// </summary>
public class BuildItemMoveController : MonoBehaviour
{
    [SerializeField]
    Transform axisRoot;
    [SerializeField]
    float speed = 25F;
    [SerializeField]
    float axisDisplayDistance = 0.25F;

    #region 字段
    /// <summary>
    /// 控制中的物体
    /// </summary>
    List<BuildItem> ControllingBuildItems => GameManager.SelectedBuildItems;
    /// <summary>
    /// 坐标轴颜色 分别对应x、y、z、选中轴
    /// </summary>
    Color[] m_axisColors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow };
    /// <summary>
    /// 是否正在移动物体
    /// </summary>
    public bool IsMovingModel { get; private set; }
    //上一帧鼠标位置
    Vector3 m_lastMouseWorldPos;
    //当前选中坐标轴
    AxisState m_axisState = AxisState.Idle;
    //坐标轴的三个轴
    Transform[] m_xyz = new Transform[3];
    #endregion

    #region unity回调
    void Start()
    {
        Initialized();
    }

    void Update()
    {
        if(IsMovingModel)
        {
            if (Input.GetMouseButtonUp(0))
            {
                MoveComplete();
            }
            else
            {
                MovingModel();
            }
        }
        //update axis display
        if(ControllingBuildItems.Count == 0)
        {
            axisRoot.gameObject.SetActive(false);
        }
        else
        {
            Vector3 controlCentre = Vector3.zero;
            foreach(var item in ControllingBuildItems)
            {
                controlCentre += item.transform.position;
            }
            controlCentre /= ControllingBuildItems.Count;
            axisRoot.gameObject.SetActive(true);
            Vector3 cameraPosition = GameManager.SceneCamera.transform.position;
            axisRoot.position = cameraPosition + (controlCentre - cameraPosition).normalized * axisDisplayDistance;
            m_lastMouseWorldPos = GameManager.SceneView.MouseWorldPos;
        }
    }
    #endregion

    #region 方法
    //初始化
    void Initialized()
    {
        for (int i = 0; i < axisRoot.childCount; i++)
        {
            m_xyz[i] = axisRoot.GetChild(i);
        }
        //坐标轴颜色初始化
        m_xyz[0].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[0]);
        m_xyz[1].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[1]);
        m_xyz[2].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[2]);
    }
    //移动
    void MovingModel()
    {
        Vector3 currentMouseWorldPos = GameManager.SceneView.MouseWorldPos;
        Vector3 mouseWorldDir = currentMouseWorldPos - m_lastMouseWorldPos;

        Vector3 similarVec = Vector3.zero;
        Vector3 axisStart = Camera.main.WorldToScreenPoint(axisRoot.position);
        switch (m_axisState)
        {
            case AxisState.X:
                Transform x = axisRoot.Find("X");
                //Vector3 screenDir = Camera.main.WorldToScreenPoint(x.forward);
                float similar = Vector3.Dot(mouseWorldDir, x.forward);
                similarVec = new Vector3(similar, 0, 0);

                break;
            case AxisState.Y:
                Transform y = axisRoot.Find("Y");
                //screenDir = Camera.main.WorldToScreenPoint(y.forward);
                similar = Vector3.Dot(mouseWorldDir, y.forward);
                similarVec = new Vector3(0, similar, 0);
                break;
            case AxisState.Z:
                Transform z = axisRoot.Find("Z");
                //screenDir = Camera.main.WorldToScreenPoint(z.forward);
                similar = Vector3.Dot(mouseWorldDir, z.forward);
                similarVec = new Vector3(0, 0, similar);
                break;
            default: break;
        }
        Vector3 offset = speed * similarVec * Time.deltaTime;
        foreach(var item in ControllingBuildItems)
            item.transform.position += offset;
        GameManager.InspectorView.UpdateInspector();
    }

    //完成本次移动
    void MoveComplete()
    {
        IsMovingModel = false;

        switch (m_axisState)
        {
            case AxisState.X:
                m_xyz[0].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[0]);
                break;
            case AxisState.Y:
                m_xyz[1].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[1]);
                break;
            case AxisState.Z:
                m_xyz[2].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[2]);
                break;
        }
    }

    #endregion

    #region 事件回调
    //鼠标悬浮坐标轴 黄色材质
    void MoseHoverEnter(string axisName)
    {
        if (!IsMovingModel)
        {
            switch (axisName)
            {
                case "X":
                    m_xyz[0].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[3]);
                    break;
                case "Y":
                    m_xyz[1].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[3]);
                    break;
                case "Z":
                    m_xyz[2].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[3]);
                    break;
            }
        }
    }

    void MouseHoverExis(string axisName)
    {
        if (!IsMovingModel)
        {
            switch (axisName)
            {
                case "X":
                    m_xyz[0].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[0]);
                    break;
                case "Y":
                    m_xyz[1].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[1]);
                    break;
                case "Z":
                    m_xyz[2].GetComponent<MeshRenderer>().material.SetColor("_Color", m_axisColors[2]);
                    break;
            }
        }
    }

    //选中坐标轴
    void MouseDown(string axisName)
    {
        IsMovingModel = true;
        m_lastMouseWorldPos = GameManager.SceneView.MouseWorldPos;
        switch (axisName)
        {
            case "X":
                m_axisState = AxisState.X;
                break;
            case "Y":
                m_axisState = AxisState.Y;
                break;
            case "Z":
                m_axisState = AxisState.Z;
                break;
            default:
                m_axisState = AxisState.Idle;
                break;
        }
    }
    #endregion


    enum AxisState
    {
        X,
        Y,
        Z,
        Idle
    }
}