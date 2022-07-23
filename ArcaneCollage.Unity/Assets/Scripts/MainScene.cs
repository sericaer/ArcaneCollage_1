using ArcaneCollage.Sessions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public GameObject defaultBuilding;

    // Start is called before the first frame update
    void Start()
    {
        Global.session = Session.Builder.Build();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTimeLapse()
    {
        Global.session.OnTimeLapse();
    }
}
