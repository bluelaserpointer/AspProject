using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMouseEvent : MonoBehaviour
{
    Transform m_gameManager;
    void Start()
    {
        m_gameManager = GameObject.Find("GameManager").transform;
    }

     void OnMouseEnter()
    {
        m_gameManager.SendMessage("MoseHoverEnter",gameObject.name);
    }

     void OnMouseDown()
    {
        m_gameManager.SendMessage("MouseDown", gameObject.name);
    }

     void OnMouseExit()
    {
        m_gameManager.SendMessage("MouseHoverExis", gameObject.name);
    }

}
