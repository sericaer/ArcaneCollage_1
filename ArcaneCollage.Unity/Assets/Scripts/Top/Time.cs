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
        GetComponent<Text>().text = $"{Global.session.time.year}��{Global.session.time.month}��{Global.session.time.day}�� {Global.session.time.hour}ʱ";
    }
}
