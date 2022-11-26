using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class SceneCameraManager : MonoBehaviour
{
    //inspector
    [Header("Camera Control")]
    [SerializeField]
    [Tooltip("旋转视角时相机x轴转速")]
    float xRotateSpeed = 250.0f;
    [SerializeField]
    [Tooltip("旋转视角时相机y轴转速")]
    float yRotateSpeed = 250.0f;

    //[Header("References")]
    //[Tooltip("相机跟随的目标物体，一般是一个空物体")]
    //public Transform target;

    public Camera Camera { get; private set; }

    /*
    private float Distance = 5; //相机和target之间的距离，因为相机的Z轴总是指向target，也就是相机z轴方向上的距离

    private int MouseWheelSensitivity = 1; //滚轮灵敏度设置
    private int MouseZoomMin = 1; //相机距离最小值
    private int MouseZoomMax = 20; //相机距离最大值

    private float moveSpeed = 10; //相机跟随速度（中键平移时），采用平滑模式时起作用，越大则运动越平滑

    private Vector3 targetOnScreenPosition; //目标的屏幕坐标，第三个值为z轴距离
    private Vector3 CameraTargetPosition; //target的位置
    private Vector3 initPosition; //平移时用于存储平移的起点位置

    private Vector3 initScreenPos; //中键刚按下时鼠标的屏幕坐标（第三个值其实没什么用）
    private Vector3 curScreenPos; //当前鼠标的屏幕坐标（第三个值其实没什么用）
    */
    private Vector2 cameraEular;
    private void Awake()
    {
        Camera = GetComponent<Camera>();
    }
    void Start()
    {
        //这里就是设置一下初始的相机视角以及一些其他变量，这里的x和y。。。是和下面getAxis的mouse x与mouse y对应
        var angles = transform.eulerAngles;
        cameraEular.x = angles.y;
        cameraEular.y = angles.x;
        //CameraTargetPosition = target.position;
        //transform.rotation = Quaternion.Euler(cameraEular.y + 60, cameraEular.x, 0); //设置相机姿态
        //Vector3 position = transform.rotation * new Vector3(0.0F, 0.0F, -Distance) + CameraTargetPosition; //四元数表示一个旋转，四元数乘以向量相当于把向量旋转对应角度，然后加上目标物体的位置就是相机位置了
        //transform.position = transform.rotation * new Vector3(0, 0, -Distance) + CameraTargetPosition; //设置相机位置
    }

    void Update()
    {
        if(!GameManager.ControlAxis.IsMovingModel)
        {
            //平移
            if (Input.GetMouseButton(2))
            {
                Camera.main.transform.position -= Camera.main.transform.TransformVector(new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0)) * Time.deltaTime;
            }
            //旋转
            if (Input.GetMouseButton(1))
            {
                cameraEular.x += Input.GetAxis("Mouse X") * xRotateSpeed * Time.deltaTime;
                cameraEular.y = ClampAngle(cameraEular.y - Input.GetAxis("Mouse Y") * yRotateSpeed * Time.deltaTime, -360, 360);

                transform.rotation = Quaternion.Euler(cameraEular.y, cameraEular.x, 0);
                //var position = transform.rotation * new Vector3(0.0f, 0.0f, -Distance) + CameraTargetPosition;
                //transform.position = position;
            }
            //缩放（FoV）
            float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (mouseWheelInput < 0)
            {
                if (Camera.main.fieldOfView <= 100)
                    Camera.main.fieldOfView += 2;
                if (Camera.main.orthographicSize <= 20)
                    Camera.main.orthographicSize += 0.5F;
            }
            else if (mouseWheelInput > 0)
            {
                if (Camera.main.fieldOfView > 2)
                    Camera.main.fieldOfView -= 2;
                if (Camera.main.orthographicSize >= 1)
                    Camera.main.orthographicSize -= 0.5F;
            }
            //else if (mouseWheelInput != 0) //靠近（与缩放取舍）
            //{
            //    transform.position = CameraTargetPosition;
            //    if (Distance >= MouseZoomMin && Distance <= MouseZoomMax)
            //    {
            //        Distance -= Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;
            //    }
            //    if (Distance < MouseZoomMin)
            //    {
            //        Distance = MouseZoomMin;
            //    }
            //    if (Distance > MouseZoomMax)
            //    {
            //        Distance = MouseZoomMax;
            //    }
            //    var rotation = transform.rotation;

            //    transform.position = transform.rotation * new Vector3(0.0F, 0.0F, -Distance) + CameraTargetPosition;
            //}

            //鼠标中键平移
            //if (Input.GetMouseButtonDown(0))
            //{
            //    initScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetOnScreenPosition.z);
            //    Debug.Log("downOnce");

            //    //targetOnScreenPosition.z为目标物体到相机xmidbuttonDownPositiony平面的法线距离
            //    targetOnScreenPosition = Camera.main.WorldToScreenPoint(CameraTargetPosition);
            //    initPosition = CameraTargetPosition;
            //}
            //if (Input.GetMouseButtonUp(0))
            //{
            //    Debug.Log("upOnce");
            //    //平移结束把cameraTargetPosition的位置更新一下，不然会影响缩放与旋转功能
            //    CameraTargetPosition = target.position;
            //}
        }
    }
    //将angle限制在min~max之间
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
