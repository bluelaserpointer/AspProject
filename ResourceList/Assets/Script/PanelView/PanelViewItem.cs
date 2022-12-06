using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class PanelViewItem : MonoBehaviour
{

    [SerializeField]
    Image selectImage;
    [SerializeField]
    Image highlightImage;
    [SerializeField]
    Text itemTitle;

    public string path { get; private set; }

    void Awake()
    {
        //transform.Find("ItemPreview").GetComponent<Button>().onClick.AddListener(ItemClick); // 选中
    }

    public void SetSelectState(bool cond)
    {
        selectImage.enabled = cond;
    }

    public void SetHighlightState(bool cond)
    {
        highlightImage.enabled = cond;
    }

    public void SetItem(ResourceItem resourceItem) { 
        path = resourceItem.Path; 
        itemTitle.text = path;
    }

    void ItemClick()
    {
        // TO DO - 调用drag接口
    }
}
