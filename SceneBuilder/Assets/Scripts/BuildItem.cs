using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BuildItem : SceneObject
{
    public string modelPath;

    public BuildItemUITag UITag => GameManager.HierarchyView.FindBuildItemUITag(this); //TODO: bind ui tag on initialize 
    public void SelectAndHighlight()
    {
        Select();
        GameManager.HighlightedBuildItem = this;
    }
    public void Select()
    {
        GameManager.SelectBuildItem(this);
    }
    public void Deselect()
    {
        GameManager.DeselectBuildItem(this);
    }
    public override void OnSceneMouseClick()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            //multi-select
        }
        else
        {
            GameManager.DeselectAllBuildItems();
        }
        SelectAndHighlight();
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
