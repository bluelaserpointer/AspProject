using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PanelView : MonoBehaviour
{
    [SerializeField]
    Transform _panelItemRoot;
    [SerializeField]
    GameObject _panelItemPrefab;

    ResourceItem HighlightedResourceItem => GameManager.HighlightedResourceItem;

    private List<ResourceItem> _resourceItems => GameManager.ResourceItems;
    private List<GameObject> _panelViewItems;
    // Start is called before the first frame update
    void Start()
    {
        //_panelItemRoot.SetActive(false);
        //childResourceItems = new List<ResourceItem>();
        //GameManager.OnResourceItemHighlightChange.AddListener((oldItem, newItem) =>
        //{
        //    if (newItem == null)
        //    {
        //        //_panelItemRoot.SetActive(false);
        //    }
        //    else
        //    {
        //        //_panelItemRoot.SetActive(true);
        //        foreach (ResourceItem childResourceItem in GameManager
        //        .ResourceItems.FindAll(_item => _item.ParentID == newItem.ID))
        //        {
        //            _panelViewItems.Add(childResourceItem);
        //            PanelViewItem panelItem = Instantiate(_panelItemPrefab, _panelItemRoot);
        //            //panelItem.setItem(childResourceItem);
        //        }

        //    }
        //});
    }

    // Update is called once per frame
    void Update()
    {
        GeneratePanelView();
    }

    public void GeneratePanelView()
    {
        if (_panelViewItems != null)
        {
            for (int i = 0; i < _panelViewItems.Count; i++)
            {
                Destroy(_panelViewItems[i]);
            }
            _panelViewItems.Clear();
        }

        _panelViewItems = new List<GameObject>();
        List<ResourceItem> selectItems = _resourceItems.FindAll(item => item.ParentID != 0);

        for (int i = 0; i < selectItems.Count; i++)
        {
            GameObject panelItem = Instantiate(_panelItemPrefab, _panelItemRoot);

            panelItem.transform.name = "PanelViewItem";
            panelItem.transform.Find("ItemTitle").GetComponent<Text>().text = _resourceItems[i].Name;
            panelItem.SetActive(true);

            _panelViewItems.Add(panelItem);
        }
    }

}
