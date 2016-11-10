using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

[RequireComponent(typeof(MadelineUIStyles))]
public class MadelineDelayUI : MonoBehaviour
{
    public enum Mode { WaitingForDetails, DelayedStart, Recording, Playback };

    public float delayTime = 5.0f;

    
    bool uiActive = false;

    float timer;

    public MadelineUIStyles styles;
    void Awake()
    {
        styles = GetComponent<MadelineUIStyles>();
    }


    void SetDelayTime(float val)
    {
        delayTime = val;
    }
    void StateChanged(String mode )
    {
        if (mode.Equals("WaitingForRecording"))
        {
            uiActive = true;
            timer = Time.time + delayTime;
        }
        else
        {
            uiActive = false;
        }
    }

    void Update()
    {
        if(uiActive)
        {
            if(Time.time>timer)
            {
                gameObject.SendMessage("StateChanged", "Recording");
            }
        }
    }

    void OnGUI()
    {

            if (uiActive)
            {

                float timeRemaining = timer - Time.time;
                GUILayout.BeginArea(new Rect(Screen.width * 0.02f, Screen.height * 0.02f, Screen.width * 0.5f, Screen.height * 0.1f), styles.areaStyle);

                GUILayout.Label("Starting in " + timeRemaining + "s", styles.textStyle);


                GUILayout.EndArea();
                
            }
    }
}