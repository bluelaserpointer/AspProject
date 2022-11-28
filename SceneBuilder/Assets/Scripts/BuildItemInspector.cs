using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class BuildItemInspector : MonoBehaviour
{
    [SerializeField]
    Text _multiSelectNotifyText;
    [SerializeField]
    InputField _nameInputField;
    [SerializeField]
    InputField _positionXInputField, _positionYInputField, _positionZInputField;

    BuildItem HighlightedBuildItem => GameManager.HighlightedBuildItem;
    List<BuildItem> SelectedBuildItems => GameManager.SelectedBuildItems;
    BuildItemTransformController BuildItemTransformController => GameManager.BuildItemTransformController;

    private void Start()
    {
        _multiSelectNotifyText.gameObject.SetActive(false);
        _nameInputField.onSubmit.AddListener(str =>
        {
            foreach (var item in SelectedBuildItems)
            {
                item.name = str;
                item.UITag.UpdateName();
            }
        });
        _positionXInputField.onSubmit.AddListener(str => {
            Transform referencingTransform = GetReferencingTransform();
            Vector3 pos = referencingTransform.position;
            pos.x = float.Parse(str);
            referencingTransform.position = pos;
        });
        _positionYInputField.onSubmit.AddListener(str => {
            Transform referencingTransform = GetReferencingTransform();
            Vector3 pos = referencingTransform.position;
            pos.y = float.Parse(str);
            referencingTransform.position = pos;
        });
        _positionZInputField.onSubmit.AddListener(str => {
            Transform referencingTransform = GetReferencingTransform();
            Vector3 pos = referencingTransform.position;
            pos.z = float.Parse(str);
            referencingTransform.position = pos;
        });
    }
    public void UpdateInspector()
    {
        if (SelectedBuildItems.Count == 0)
            return;
        if (SelectedBuildItems.Count == 1)
        {
            if (HighlightedBuildItem == null)
                return;
            _multiSelectNotifyText.gameObject.SetActive(false);
            _nameInputField.text = HighlightedBuildItem.name;
        }
        else
        {
            _multiSelectNotifyText.gameObject.SetActive(true);
            _multiSelectNotifyText.text = "选中了" + SelectedBuildItems.Count + "个物体";
            string name = SelectedBuildItems[0].name;
            bool nameIsSame = true;
            for(int i = 1; i < SelectedBuildItems.Count;i++)
            {
                if (SelectedBuildItems[i].name != name)
                {
                    nameIsSame = false;
                    break;
                }
            }
            _nameInputField.text = nameIsSame ? name : "--";
        }
        Transform referencingTransform = GetReferencingTransform();
        _positionXInputField.text = referencingTransform.position.x.ToString();
        _positionYInputField.text = referencingTransform.position.y.ToString();
        _positionZInputField.text = referencingTransform.position.z.ToString();
    }
    private Transform GetReferencingTransform()
    {
        return SelectedBuildItems.Count == 1 ? HighlightedBuildItem.transform : BuildItemTransformController.transform;
    }
}
