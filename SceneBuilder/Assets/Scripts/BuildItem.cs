using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BuildItem : SceneObject
{
    public string modelPath;

    public void Highlight()
    {
        GameManager.HighlightedBuildItem = this;
    }
    public override void OnSceneMouseClick()
    {
        Highlight();
    }

    public override void OnSceneMouseDown()
    {
    }

    public override void OnSceneMouseEnter()
    {
    }

    public override void OnSceneMouseExit()
    {
    }

    public override void OnSceneMouseUp()
    {
    }
}
