using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;


[RequireComponent(typeof(RecordAvatar))]
[RequireComponent(typeof(MadelineUIStyles))]
public class MadelineRecordUI : MonoBehaviour
{
    public enum Mode { WaitingForDetails, DelayedStart, Recording, Playback };

    public float recordingTime = 10.0f;

    bool uiActive = false;

    RecordAvatar recorder;
    float timer;

    public MadelineUIStyles styles;
    void Awake()
    {
        styles = GetComponent<MadelineUIStyles>();
        recorder = GetComponent<RecordAvatar>(); 
    }


    void FinishRecording()
    {
        recorder.mode = RecordAvatar.Mode.Inactive;
        gameObject.SendMessage("StateChanged", "GetDetails");
    }

    void SetRecordingTime(float val)
    {
        recordingTime = val;
    }
    void StateChanged(String mode )
    {
        if (mode.Equals("Recording"))
        {
            if (recorder)
            {
                recorder.Reset();
                recorder.mode = RecordAvatar.Mode.Recording;
            }
            timer = Time.time + recordingTime;
            uiActive = true;
        }
        else
        {
            uiActive = false;
        }
    }

    void Update()
    {
        if (uiActive)
        {
            if (recordingTime > 0 && Time.time > timer)
            {
                FinishRecording();
            }
        }
    }

    void OnGUI()
    {

            if (uiActive)
            {


                GUILayout.BeginArea(new Rect(Screen.width * 0.02f, Screen.height * 0.02f, Screen.width * 0.3f, Screen.height * 0.1f), styles.areaStyle);
                if (recordingTime>0.0f)
                {


                    float timeRemaining = timer - Time.time;

                    GUILayout.Label("Stopping in " + timeRemaining + "s", styles.textStyle);
                }
                else
                {
                    if (GUILayout.Button("Stop"))
                    {
                        FinishRecording();
                    }

                }


                GUILayout.EndArea();
            }
    }
}