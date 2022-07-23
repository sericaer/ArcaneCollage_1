using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Text>().text = $"{Global.session.time.year}年{Global.session.time.month}月{Global.session.time.day}日 {Global.session.time.hour}时";
    }
}
