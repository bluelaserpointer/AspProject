using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 通过拖动小坐标轴 移动模型
/// </summary>
public class BuildItemTransformTool : MonoBehaviour
{
    /// <summary>
    /// 坐标轴颜色 分别对应x、y、z、高亮色
    /// </summary>
    [SerializeField]
    Transform handleRoot;
    [SerializeField]
    BuildItemTransformToolHandle xAxis, yAxis, zAxis, xyPlane, yzPlane, zxPlane, magnetCube;
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
    /// 当前选中坐标轴
    /// </summary>
    [HideInInspector]
    public BuildItemTransformToolHandle ControllingAxis { get; private set; }
    /// <summary>
    /// 是否正在移动物体
    /// </summary>
    public bool IsMovingModel => ControllingAxis != null;

    //上一帧鼠标位置
    Vector3 m_lastMouseWorldPos;
    #endregion

    #region unity回调
    void Start()
    {
        handleRoot.gameObject.SetActive(false);
        GameManager.OnBuildItemSelect.AddListener((item, cond) =>
        {
            if(!cond)
                item.transform.SetParent(GameManager.BuildItemRoot);
            if (ControllingBuildItems.Count == 0)
            {
                handleRoot.gameObject.SetActive(false);
            }
            else
            {
                handleRoot.gameObject.SetActive(true);
                //TODO: improvable
                Vector3 sumPos = Vector3.zero;
                ControllingBuildItems.ForEach(eachItem =>
                {
                    eachItem.transform.SetParent(GameManager.BuildItemRoot);
                    sumPos += eachItem.transform.position;
                });
                transform.position = sumPos / ControllingBuildItems.Count;
                ControllingBuildItems.ForEach(eachItem => eachItem.transform.SetParent(transform));
            }
        });
    }

    void Update()
    {
        if(IsMovingModel)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if(GameManager.SceneView.MouseRaycastingObject != ControllingAxis)
                    ControllingAxis.Lit(false);
                ControllingAxis = null;
            }
            else
            {
                Vector3 mouseWorldDir = GameManager.SceneView.MouseWorldPos - m_lastMouseWorldPos;
                if (ControllingAxis == xAxis)
                {
                    transform.position += Vector3.right * Vector3.Dot(mouseWorldDir, handleRoot.Find("X").forward) * speed * Time.deltaTime;
                }
                else if (ControllingAxis == yAxis)
                {
                    transform.position += Vector3.up * Vector3.Dot(mouseWorldDir, handleRoot.Find("Y").forward) * speed * Time.deltaTime;
                }
                else if (ControllingAxis == zAxis)
                {
                    transform.position += Vector3.forward * Vector3.Dot(mouseWorldDir, handleRoot.Find("Z").forward) * speed * Time.deltaTime;
                }
                else if (ControllingAxis == magnetCube)
                {
                    BuildItem closestHitItem = null;
                    float closestHitItemDistance = float.MaxValue;
                    foreach (RaycastHit hitInfo in Physics.RaycastAll(GameManager.SceneView.MouseRay))
                    {
                        BuildItem hitItem = hitInfo.collider.GetComponent<BuildItem>();
                        if (hitItem != null && !ControllingBuildItems.Contains(hitItem) && hitInfo.distance < closestHitItemDistance)
                        {
                            closestHitItemDistance = hitInfo.distance;
                            closestHitItem = hitItem;
                        }
                    }
                    if (closestHitItem != null)
                    {
                        Collider collider = closestHitItem.GetComponent<Collider>();
                        //collider
                    }
                }
                GameManager.InspectorView.BuildItemInspector.UpdateInspector();
            }
        }
        //update axis display
        if(ControllingBuildItems.Count > 0)
        {
            Vector3 cameraPosition = GameManager.SceneCamera.transform.position;
            handleRoot.position = cameraPosition + (transform.position - cameraPosition).normalized * axisDisplayDistance;
            m_lastMouseWorldPos = GameManager.SceneView.MouseWorldPos;
        }
    }
    #endregion

    /// <summary>
    /// 选中坐标轴
    /// </summary>
    /// <param name="axis"></param>
    public void OnMouseDownControlAxis(BuildItemTransformToolHandle axis)
    {
        ControllingAxis = axis;
        m_lastMouseWorldPos = GameManager.SceneView.MouseWorldPos;
    }
}