using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class BuildItemUITag : MonoBehaviour
{
    [SerializeField]
    Image highlightImage;
    [SerializeField]
    Text itemNameText;

    public BuildItem BuildItem { get; private set; }
    public void SetHighlight(bool cond)
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
        BuildItem.Highlight();
    }
}
