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
    float planeHandleHalfSize = 0.05F;
    [SerializeField]
    Transform handleRoot;
    [SerializeField]
    BuildItemTransformToolHandle xAxis, yAxis, zAxis, xPlane, yPlane, zPlane, magnetCube;
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

    Vector3 lastMouseDownToolPosition;
    Plane lastMouseDownVirtualPlane;
    Vector3 lastMouseDownVirtualPlaneStartPos;
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
        if(ControllingBuildItems.Count > 0)
        {
            if (IsMovingModel)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (GameManager.SceneView.MouseRaycastingObject != ControllingAxis)
                        ControllingAxis.Lit(false);
                    ControllingAxis = null;
                }
                else
                {
                    if (ControllingAxis == xAxis || ControllingAxis == yAxis || ControllingAxis == zAxis)
                    {
                        Vector3 handleForward = ControllingAxis.transform.forward;
                        transform.position = lastMouseDownToolPosition + Vector3.Project(GetRaycastPoint(lastMouseDownVirtualPlane, GameManager.SceneView.MouseRay) - lastMouseDownVirtualPlaneStartPos, handleForward);
                    }
                    else if (ControllingAxis == xPlane || ControllingAxis == yPlane || ControllingAxis == zPlane)
                    {
                        transform.position = lastMouseDownToolPosition + GetRaycastPoint(lastMouseDownVirtualPlane, GameManager.SceneView.MouseRay) - lastMouseDownVirtualPlaneStartPos;
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
            Vector3 cameraPosition = GameManager.SceneCamera.transform.position;
            Vector3 toolLocalPosFromCamera = GameManager.SceneCamera.transform.InverseTransformPoint(transform.position);
            handleRoot.position = GameManager.SceneCamera.transform.TransformPoint(toolLocalPosFromCamera * axisDisplayDistance / toolLocalPosFromCamera.z);
            int xSign = cameraPosition.x < transform.position.x ? 1 : -1; 
            int ySign = cameraPosition.y < transform.position.y ? 1 : -1; 
            int zSign = cameraPosition.z < transform.position.z ? 1 : -1;
            xPlane.transform.localPosition = new Vector3(0, -ySign, zSign) * planeHandleHalfSize;
            yPlane.transform.localPosition = new Vector3(xSign, 0, zSign) * planeHandleHalfSize;
            zPlane.transform.localPosition = new Vector3(xSign, -ySign, 0) * planeHandleHalfSize;
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
        lastMouseDownToolPosition = transform.position;

        if (ControllingAxis == xAxis || ControllingAxis == yAxis || ControllingAxis == zAxis)
        {
            Vector3 handleForward = axis.transform.forward;
            lastMouseDownVirtualPlane = GetAxisVirtualPlane(handleForward);
            lastMouseDownVirtualPlaneStartPos = GetRaycastPoint(lastMouseDownVirtualPlane, GameManager.SceneView.MouseRay);
        }
        else if (ControllingAxis == xPlane || ControllingAxis == yPlane || ControllingAxis == zPlane)
        {
            Vector3 planeNormal = axis.transform.up;
            lastMouseDownVirtualPlane = new Plane(planeNormal, transform.position);
            lastMouseDownVirtualPlaneStartPos = GetRaycastPoint(lastMouseDownVirtualPlane, GameManager.SceneView.MouseRay);
        }
    }
    private Plane GetAxisVirtualPlane(Vector3 axisDirection)
    {
        return new Plane(Vector3.Cross(axisDirection, Vector3.Cross(GameManager.SceneCamera.transform.forward, axisDirection)), transform.position);
    }
    private Vector3 GetRaycastPoint(Plane plane, Ray ray)
    {
        plane.Raycast(ray, out float enter);
        return ray.GetPoint(enter);
    }
}