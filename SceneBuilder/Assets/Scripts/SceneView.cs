using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SceneView : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    RawImage _sceneRenderImage;
    public RectTransform RectTransform => _sceneRenderImage.rectTransform;
    public bool IsMouseEnter { get; private set; }

    public Vector2 MouseScreenPos { get; private set; }
    public Ray MouseRay { get; private set; }
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

        MouseRay = GameManager.SceneCamera.ScreenPointToRay(MouseScreenPos);
        bool foundRaycastHit = false;
        //Check item controller before items
        foreach (Collider collider in GameManager.BuildItemTransformController.GetComponentsInChildren<Collider>())
        {
            SceneObject sceneObject = collider.GetComponent<SceneObject>();
            if (sceneObject != null)
            {
                RaycastHit hitInfo;
                if (collider.Raycast(MouseRay, out hitInfo, float.MaxValue))
                {
                    MouseRaycastingObject = sceneObject;
                    foundRaycastHit = true;
                    break;
                }
            }
        }
        if (!foundRaycastHit)
        {
            float closestDistance = float.MaxValue;
            SceneObject closestObject = null;
            foreach (RaycastHit hitInfo in Physics.RaycastAll(MouseRay))
            {
                SceneObject sceneObject = hitInfo.collider.GetComponent<SceneObject>();
                if (sceneObject != null && hitInfo.distance < closestDistance)
                {
                    closestDistance = hitInfo.distance;
                    closestObject = sceneObject;
                    break;
                }
            }
            if(closestObject != null)
            {
                MouseRaycastingObject = closestObject;
                foundRaycastHit = true;
            }
        }
        if (!foundRaycastHit)
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
        if (MouseRaycastingObject != null)
        {
            MouseRaycastingObject.OnSceneMouseDown();
        }
        else
        {
            GameManager.HighlightedBuildItem = null;
            GameManager.DeselectAllBuildItems();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        MouseRaycastingObject?.OnSceneMouseUp();
    }
}
