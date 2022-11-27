using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// 树形菜单元素
/// </summary>
public class PanelViewItem : MonoBehaviour
{
    /// <summary>
    /// 树形菜单控制器
    /// </summary>
    public PanelViewControl Controler;

    //正在进行刷新
    private bool _isRefreshing = false;

    void Awake()
    {
        //上下文按钮点击回调 抓取元件 为元件添加监听
        transform.Find("ItemPreview").GetComponent<Button>().onClick.AddListener(ItemClick); // 选中
/*        transform.Find("PanelViewButton").GetComponent<Button>().onClick.AddListener(delegate () {
            Controler.ClickItem(gameObject);
        });*/
    }
    /// <summary>
    /// 点击上下文菜单按钮，元素的子元素改变显示状态
    /// </summary>
    void ItemClick()
    {
        // TO DO - 调用drag接口
    }
    
    
    /// <summary>
    /// 改变某一元素所有子元素的显示状态
    /// </summary>
/*    void ChangeChildren(PanelViewItem tvi, bool value)
    {
        for (int i = 0; i < tvi.GetChildrenNumber(); i++)
        {
            tvi.GetChildrenByIndex(i).gameObject.SetActive(value);
            if (tvi.GetChildrenByIndex(i).IsExpanding)
            {
                ChangeChildren(tvi.GetChildrenByIndex(i), value);
            }
        }
    }*/

    #region 属性访问 GetSet方法

    #endregion
}
