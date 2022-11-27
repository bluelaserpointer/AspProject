using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// Panel面板控制器
/// </summary>
public class PanelViewControl : MonoBehaviour
{
    /// <summary>
    /// 当前Panel面板的数据源
    /// </summary>
    [HideInInspector]
    public List<PanelViewData> Data = null;
    /// <summary>
    /// Panel面板中元素的模板
    /// </summary>
    public GameObject Template;
    /// <summary>
    /// Panel面板中元素的根物体 - 对象的位置
    /// </summary>
    public Transform PanelItems;
    /// <summary>
    /// Panel面板中元素的宽度
    /// </summary>
    public int ItemWidth = 230;
    /// <summary>
    /// Panel面板中元素的高度
    /// </summary>
    public int ItemHeight = 35;
    /// <summary>
    /// 所有子元素的鼠标点击回调事件 - delegate 委托：对方法的引用
    /// </summary>
    public delegate void ClickItemdelegate(GameObject item);
    public event ClickItemdelegate ClickItemEvent;

    //当前Panel面板中的所有元素
    private List<GameObject> _panelViewItems;
    //当前Panel面板中的所有元素克隆体（刷新Panel面板时用于过滤计算）？
    private List<GameObject> _panelViewItemsClone;
    //Panel面板当前刷新队列的元素位置索引？
    private int _yIndex = 0;
    //正在进行刷新
    private bool _isRefreshing = false;

    void Awake()
    {
        ClickItemEvent += ClickItemTemplate;
    }
    /// <summary>
    /// 鼠标点击子元素事件
    /// </summary>
    public void ClickItem(GameObject item)
    {
        ClickItemEvent(item);
    }
    void ClickItemTemplate(GameObject item)
    {
        //空的事件，不这样做的话ClickItemEvent会引发空引用异常
    }

    /// <summary>
    /// 生成Panel面板
    /// </summary>
    public void GeneratePanelView()
    {
        //删除可能已经存在的Panel面板元素
        if (_panelViewItems != null)
        {
            for (int i = 0; i < _panelViewItems.Count; i++)
            {
                Destroy(_panelViewItems[i]);
            }
            _panelViewItems.Clear();
        }
        //重新创建Panel面板元素 - 将PanelViewItem转为GameObject?
        _panelViewItems = new List<GameObject>();
        for (int i = 0; i < Data.Count; i++)
        {
            GameObject item = Instantiate(Template);    // 找到prefab中的Template组件

            item.transform.name = "PanelViewItem";
            item.transform.Find("ItemTitle").GetComponent<Text>().text = Data[i].Name;
            //item.transform.SetParent(PanelItems);   // unity方法 设置parent
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.Euler(Vector3.zero);
            item.SetActive(true);   // 激活

            _panelViewItems.Add(item);
        }
    }

    /// <summary>
    /// 刷新Panel面板
    /// </summary>
    public void RefreshPanelView()
    {
        //上一轮刷新还未结束
        if (_isRefreshing)
        {
            return;
        }

        _isRefreshing = true;
        //_yIndex = 0;

        //复制一份菜单 - 为什么要复制？
        _panelViewItemsClone = new List<GameObject>(_panelViewItems);

        //用复制的菜单进行刷新计算
        for (int i = 0; i < _panelViewItemsClone.Count; i++)
        {
            //已经计算过或者不需要计算位置的元素
            if (_panelViewItemsClone[i] == null || !_panelViewItemsClone[i].activeSelf) // activeSelf - GameObject是否active
            {
                continue;
            }

            PanelViewItem pvi = _panelViewItemsClone[i].GetComponent<PanelViewItem>();

            // 父组件已经设置 grid 子组件还要设置location吗
            //_panelViewItemsClone[i].GetComponent<RectTransform>().localPosition = new Vector3(pvi.GetHierarchy() * HorizontalItemSpace, _yIndex,0);
            //_yIndex += (-(ItemHeight + VerticalItemSpace));


            _panelViewItemsClone[i] = null;
        }

        //重新计算滚动视野的区域
        //float x = _hierarchy * HorizontalItemSpace + ItemWidth;
        //float y = Mathf.Abs(_yIndex);
        //transform.GetComponent<ScrollRect>().content.sizeDelta = new Vector2(x, y);

        //清空复制的菜单
        _panelViewItemsClone.Clear();

        _isRefreshing = false;
    }
    
}
