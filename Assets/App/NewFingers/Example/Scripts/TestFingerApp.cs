using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFingerApp : NewFingerApp
{
    public override void StartApp(NewFingerView.NewFingerType type)
    {
        Debug.Log("start App : " + type);
    }
    
}
