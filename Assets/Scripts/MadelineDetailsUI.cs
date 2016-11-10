using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

[RequireComponent(typeof(MadelineUIStyles))]
public class MadelineDetailsUI : MonoBehaviour
{
    public enum Mode { WaitingForDetails, DelayedStart, Recording, Playback };

    public float delayTime = 5.0f;
    public float recordTime = 10.0f;

  

    String currentUser = "";
    String currentPhrase = "";

    bool uiActive = true;
    public MadelineUIStyles styles;
    void Awake()
    {
        styles = GetComponent<MadelineUIStyles>();

    }


    void StateChanged(String mode)
    {
        if (mode.Equals("GetDetails"))
        {
            uiActive = true;
        }
        else
        {
            uiActive = false;
        }
    }
    void OnGUI()
    {

        if (uiActive)
        {


            GUILayout.BeginArea(new Rect(Screen.width * 0.02f, Screen.height * 0.02f, Screen.width * 0.5f, Screen.height * 0.2f), styles.areaStyle);

            GUILayout.FlexibleSpace();
            // Make a background box

            GUILayout.BeginHorizontal();
            GUILayout.Label("Enter dancer's name:", styles.textStyle);
            currentUser = GUILayout.TextField(currentUser);
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Enter name for the phrase:", styles.textStyle);
            currentPhrase = GUILayout.TextField(currentPhrase);
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Start immediately"))
            {
                gameObject.SendMessage("SetRecordingTime", -1.0f);
                gameObject.SendMessage("StateChanged", "Recording");
            }

            if (GUILayout.Button("Start timed"))
            {
                gameObject.SendMessage("SetRecordingTime", 10.0f);
                gameObject.SendMessage("SetDelayTime", 5.0f);
                gameObject.SendMessage("StateChanged", "WaitingForRecording");
            }
            GUILayout.EndHorizontal();

            // End the group we started above. This is very important to remember!
            GUILayout.FlexibleSpace();

            GUILayout.EndArea();

        }
    }
}