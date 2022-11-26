using UnityEngine;
/// <summary>
/// 能接受鼠标事件的场景物体。除了普通的场景物体外，还包括物体控制器。
/// </summary>
public abstract class SceneObject : MonoBehaviour
{
    public abstract void OnSceneMouseEnter();
    public abstract void OnSceneMouseExit();
    public abstract void OnSceneMouseClick();
    public abstract void OnSceneMouseDown();
    public abstract void OnSceneMouseUp();
}
