using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HierarchyView : MonoBehaviour
{
    [SerializeField]
    Transform _buildItemUITagRoot;
    [SerializeField]
    BuildItemUITag _buildItemUITagPrefab;

    private void Start()
    {
        foreach (Transform buildItemTransform in GameManager.BuildItemRoot)
        {
            BuildItemUITag tag = Instantiate(_buildItemUITagPrefab, _buildItemUITagRoot);
            tag.SetItem(buildItemTransform.GetComponent<BuildItem>());
        }
        GameManager.OnBuildItemHighlightChange.AddListener((oldItem, newItem) =>
        {
            FindBuildItemUITag(oldItem)?.SetHighlightState(false);
            FindBuildItemUITag(newItem)?.SetHighlightState(true);
        });
        GameManager.OnBuildItemSelect.AddListener((item, cond) =>
        {
            FindBuildItemUITag(item).SetSelectState(cond);
        });
    }
    public BuildItemUITag FindBuildItemUITag(BuildItem buildItem)
    {
        if(buildItem == null)
            return null;
        foreach (Transform tagTransform in _buildItemUITagRoot)
        {
            BuildItemUITag tag = tagTransform.GetComponent<BuildItemUITag>();
            if (tag.BuildItem == buildItem)
            {
                return tag;
            }
        }
        return null;
    }
}
