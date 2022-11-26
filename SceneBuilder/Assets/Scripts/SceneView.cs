using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(RawImage))]
public class SceneView : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public RawImage RawImage { get; private set; }
    public RectTransform RectTransform => RawImage.rectTransform;
    public bool IsMouseEnter { get; private set; }

    public Vector2 MouseScreenPos { get; private set; }
    public Vector3 MouseWorldPos { get; private set; }
    public SceneObject MouseRaycastingObject
    {
        get => _mouseRaycastingObject;
        set
        {
            if(_mouseRaycastingObject != value)
            {
                _mouseRaycastingObject?.OnSceneMouseExit();
                (_mouseRaycastingObject = value)?.OnSceneMouseEnter();
            }
        }
    }
    SceneObject _mouseRaycastingObject;

    private void Awake()
    {
        RawImage = GetComponent<RawImage>();
    }
    private void Update()
    {
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        Vector2 outMouseSceneScreenPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, null, out outMouseSceneScreenPos);
        outMouseSceneScreenPos += RectTransform.rect.size / 2;
        MouseScreenPos = outMouseSceneScreenPos;
        MouseWorldPos = GameManager.SceneCamera.ScreenToWorldPoint(new Vector3(MouseScreenPos.x, MouseScreenPos.y, 5));

        Ray sceneMouseRay = GameManager.SceneCamera.ScreenPointToRay(MouseScreenPos);
        bool foundRaycastHit = false;
        foreach (Collider collider in GameManager.ControlAxis.GetComponentsInChildren<Collider>())
        {
            SceneObject sceneObject = collider.GetComponent<SceneObject>();
            if (sceneObject != null)
            {
                RaycastHit hitInfo;
                if (collider.Raycast(sceneMouseRay, out hitInfo, float.MaxValue))
                {
                    MouseRaycastingObject = sceneObject;
                    foundRaycastHit = true;
                    break;
                }
            }
        }
        if(!foundRaycastHit)
        {
            MouseRaycastingObject = null;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        MouseRaycastingObject?.OnSceneMouseClick();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        MouseRaycastingObject?.OnSceneMouseDown();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        MouseRaycastingObject?.OnSceneMouseUp();
    }
}
