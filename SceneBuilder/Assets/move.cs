using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 0.022f;
    public float speed1 = 2f;
    public float y;
    // Start is called before the first frame update
    void Start()
    {
      
        y = transform.rotation.y;
    }

    // Update is called once per frame

    void Update()
    {
    //获取当前位置
    Vector3 position = transform.position;
        
        if (Input.GetKey("d"))
        {
            position.x += speed;
        }
        if (Input.GetKey("a"))
        {
            position.x -= speed;
        }
        if (Input.GetKey("w"))
        {
            position.z += speed;
        }
        if (Input.GetKey("s"))
        {
            position.z -= speed;
        }
        transform.position = position;
        if (Input.GetKey("q"))
        {
            y -= speed1;

        }
        if (Input.GetKey("e"))
        {
            y += speed1;
  
        }
        transform.localRotation= Quaternion.Euler(0, y, 0);
    }
}
