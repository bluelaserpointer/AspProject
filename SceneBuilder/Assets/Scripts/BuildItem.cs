using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BuildItem : SceneObject
{
    public string modelPath;

    public override void OnSceneMouseClick()
    {
        GameManager.HighlightedBuildItem = this;
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
