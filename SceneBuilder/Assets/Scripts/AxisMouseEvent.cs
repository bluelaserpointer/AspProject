using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMouseEvent : SceneObject
{
    public override void OnSceneMouseEnter()
    {
        transform.parent.parent.SendMessage("MoseHoverEnter",gameObject.name);
    }
    public override void OnSceneMouseExit()
    {
        transform.parent.parent.SendMessage("MouseHoverExis", gameObject.name);
    }
    public override void OnSceneMouseDown()
    {
        transform.parent.parent.SendMessage("MouseDown", gameObject.name);
    }
    public override void OnSceneMouseClick()
    {
    }
    public override void OnSceneMouseUp()
    {
    }
}
