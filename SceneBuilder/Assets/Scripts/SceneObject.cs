using UnityEngine;
/// <summary>
/// �ܽ�������¼��ĳ������塣������ͨ�ĳ��������⣬�����������������
/// </summary>
public abstract class SceneObject : MonoBehaviour
{
    public abstract void OnSceneMouseEnter();
    public abstract void OnSceneMouseExit();
    public abstract void OnSceneMouseClick();
    public abstract void OnSceneMouseDown();
    public abstract void OnSceneMouseUp();
}
