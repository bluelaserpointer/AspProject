using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]
public class BuildItemTransformToolHandle : SceneObject
{
    [SerializeField]
    Color normalColor, litColor = Color.yellow;

    Renderer _renderer;
    BuildItemTransformTool _transformController;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _transformController = GetComponentInParent<BuildItemTransformTool>();
        Lit(false);
    }
    public override void OnSceneMouseEnter()
    {
        if(_transformController.ControllingAxis == null)
            Lit(true);
    }
    public override void OnSceneMouseExit()
    {
        if (_transformController.ControllingAxis == null)
            Lit(false);
    }
    public override void OnSceneMouseDown()
    {
        _transformController.OnMouseDownControlAxis(this);
    }
    public override void OnSceneMouseClick()
    {
    }
    public override void OnSceneMouseUp()
    {
    }
    public void Lit(bool cond)
    {
        _renderer.material.SetColor("_Color", cond ? litColor : normalColor);
    }
}
