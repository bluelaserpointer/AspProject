using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMouseEvent : SceneObject
{
    public override void OnSceneMouseEnter()
    {
        transform.parent.SendMessage("MoseHoverEnter",gameObject.name);
    }
    public override void OnSceneMouseExit()
    {
        transform.parent.SendMessage("MouseHoverExis", gameObject.name);
    }
    public override void OnSceneMouseDown()
    {
        transform.parent.SendMessage("MouseDown", gameObject.name);
    }
    public override void OnSceneMouseClick()
    {
    }
    public override void OnSceneMouseUp()
    {
    }
}
