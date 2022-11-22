using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SceneSerializer : MonoBehaviour
{
    [Header("Debug")]
    [TextArea]
    public string testJson;

    [System.Serializable]
    public struct SceneData
    {
        [SerializeField]
        public List<ObjData> objDatas;
        [SerializeField]
        public List<ModelData> modelDatas;
    }
    [System.Serializable]
    public struct ObjData
    {
        public string name;
        public JsonVector3 position;
        public JsonVector3 eulerAngles;
        public JsonVector3 localScale;
        public int modelID;
    }
    [System.Serializable]
    public struct ModelData
    {
        public string resourcePath;
    }
    [System.Serializable]
    public struct JsonVector3
    {
        public string x, y, z;
        public void Set(Vector3 vector3)
        {
            x = Format(vector3.x);
            y = Format(vector3.y);
            z = Format(vector3.z);
        }
        public Vector3 Get()
        {
            return new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
        }
        private string Format(float value)
        {
            return string.Format("{0:F4}", value.ToString());
        }
    }
    public void DoSerialize()
    {
        List<ObjData> objDatas = new List<ObjData>();
        foreach (Transform childTf in transform)
        {
            ObjData data = new ObjData();
            data.name = childTf.name;
            data.position.Set(childTf.position);
            data.eulerAngles.Set(childTf.eulerAngles);
            data.localScale.Set(childTf.localScale);
            objDatas.Add(data);
        }
        SceneData sceneData = new SceneData();
        sceneData.objDatas = objDatas;
        Debug.Log(JsonUtility.ToJson(sceneData));
    }
    public void DoDeserialize()
    {
        foreach (Transform childTf in transform)
        {
            Destroy(childTf.gameObject);
        }
        SceneData sceneData = (SceneData)JsonUtility.FromJson(testJson, typeof(SceneData));
        foreach(ObjData objData in sceneData.objDatas)
        {
            GameObject obj = new GameObject(objData.name);
            obj.transform.parent = transform;
            obj.transform.position = objData.position.Get();
            obj.transform.eulerAngles = objData.eulerAngles.Get();
            obj.transform.localScale = objData.localScale.Get();
        }
    }
}
