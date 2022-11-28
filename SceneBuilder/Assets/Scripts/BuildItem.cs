using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[DisallowMultipleComponent]
public class BuildItem : SceneObject
{
    public string modelPath;

    public BuildItemUITag UITag => GameManager.HierarchyView.FindBuildItemUITag(this); //TODO: bind ui tag on initialize
    private void Awake()
    {
        gameObject.tag = "BuildItem";
    }
    public void SelectAndHighlight()
    {
        Select();
        GameManager.HighlightedBuildItem = this;
    }
    public bool Select()
    {
        if (!GameManager.SelectedBuildItems.Contains(this))
        {
            GameManager.SelectedBuildItems.Add(this);
            GameManager.OnBuildItemSelect.Invoke(this, true);
            return true;
        }
        return false;
    }
    public bool Deselect()
    {
        if (GameManager.SelectedBuildItems.Remove(this))
        {
            GameManager.OnBuildItemSelect.Invoke(this, false);
            return true;
        }
        return false;
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
