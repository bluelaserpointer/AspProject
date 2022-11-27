using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class InspectorView : MonoBehaviour
{
    [SerializeField]
    GameObject _buildItemInspectorRoot;
    [SerializeField]
    InputField _nameInputField;
    [SerializeField]
    InputField _positionXInputField, _positionYInputField, _positionZInputField;

    BuildItem HighlightedBuildItem => GameManager.HighlightedBuildItem;
    private void Start()
    {
        _buildItemInspectorRoot.SetActive(false);
        GameManager.OnBuildItemHighlightChange.AddListener((oldItem, newItem) =>
        {
            if(newItem == null)
            {
                _buildItemInspectorRoot.SetActive(false);
            }
            else
            {
                _buildItemInspectorRoot.SetActive(true);
                _nameInputField.text = newItem.name;
                _positionXInputField.text = newItem.transform.position.x.ToString();
                _positionYInputField.text = newItem.transform.position.y.ToString();
                _positionZInputField.text = newItem.transform.position.z.ToString();
            }
        });
        _nameInputField.onSubmit.AddListener(str =>
        {
            HighlightedBuildItem.name = str;
            HighlightedBuildItem.UITag.UpdateName();
        });
        _positionXInputField.onSubmit.AddListener(str => {
            Vector3 pos = HighlightedBuildItem.transform.position;
            pos.x = float.Parse(str);
            HighlightedBuildItem.transform.position = pos;
        });
        _positionYInputField.onSubmit.AddListener(str => {
            Vector3 pos = HighlightedBuildItem.transform.position;
            pos.y = float.Parse(str);
            HighlightedBuildItem.transform.position = pos;
        });
        _positionZInputField.onSubmit.AddListener(str => {
            Vector3 pos = HighlightedBuildItem.transform.position;
            pos.z = float.Parse(str);
            HighlightedBuildItem.transform.position = pos;
        });
    }
    public void UpdateInspector()
    {
        if (HighlightedBuildItem == null)
            return;
        _positionXInputField.text = HighlightedBuildItem.transform.position.x.ToString();
        _positionYInputField.text = HighlightedBuildItem.transform.position.y.ToString();
        _positionZInputField.text = HighlightedBuildItem.transform.position.z.ToString();
    }
}
