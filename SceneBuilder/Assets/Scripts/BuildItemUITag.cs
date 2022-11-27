using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class BuildItemUITag : MonoBehaviour
{
    [SerializeField]
    Image selectImage;
    [SerializeField]
    Image highlightImage;
    [SerializeField]
    Text itemNameText;

    public BuildItem BuildItem { get; private set; }
    public void SetSelectState(bool cond)
    {
        selectImage.enabled = cond;
    }
    public void SetHighlightState(bool cond)
    {
        highlightImage.enabled = cond;
    }
    public void SetItem(BuildItem buildItem)
    {
        BuildItem = buildItem;
        itemNameText.text = buildItem.name;
    }
    public void OnClick()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            //multi-select
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            //TODO: multi-select in list
        }
        else
        {
            GameManager.DeselectAllBuildItems();
        }
        BuildItem.SelectAndHighlight();
    }
}
