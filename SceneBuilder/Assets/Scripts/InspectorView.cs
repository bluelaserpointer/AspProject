using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class InspectorView : MonoBehaviour
{
    [SerializeField]
    BuildItemInspector _buildItemInspector;

    public BuildItemInspector BuildItemInspector => _buildItemInspector;
    private void Start()
    {
        _buildItemInspector.gameObject.SetActive(false);
        GameManager.OnBuildItemHighlightChange.AddListener((oldItem, newItem) => {
            if(newItem == null)
            {
                _buildItemInspector.gameObject.SetActive(false);
            }
            else
            {
                _buildItemInspector.gameObject.SetActive(true);
                _buildItemInspector.UpdateInspector();
            }
        });
        GameManager.OnBuildItemSelect.AddListener((item, cond) => _buildItemInspector.UpdateInspector());
    }
}
