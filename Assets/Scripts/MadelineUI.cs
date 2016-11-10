using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

[RequireComponent(typeof(KinectManager))]
public class MadelineUI : MonoBehaviour
{
    public enum Mode { WaitingForDetails, DelayedStart, Recording, Playback };

    public bool timed = true;
    public float delayTime = 5.0f;
    public float recordTime = 10.0f;
    float timer; 

    public Mode mode = Mode.WaitingForDetails;
    public GUIStyle areaStyle;
    public GUIStyle textStyle;

    KinectManager mgr;

    public RecordAvatar recorder;

    String currentUser = "";
    String currentPhrase = "";

    void Awake()
    {
        mgr = GetComponent<KinectManager>(); 
    }

    void OnGUI()
    {
        if (true)//Event.current.type == EventType.Repaint)
        {

            if (mode == Mode.WaitingForDetails)
            {

                if (mgr != null)
                    mgr.Enabled = false;

                GUILayout.BeginArea(new Rect(Screen.width * 0.02f, Screen.height * 0.02f, Screen.width * 0.5f, Screen.height * 0.1f), areaStyle);

                GUILayout.FlexibleSpace();
                // Make a background box

                GUILayout.BeginHorizontal();
                GUILayout.Label("Enter dancer's name:", textStyle);
                currentUser = GUILayout.TextField(currentUser);
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Enter name for the phrase:", textStyle);
                currentPhrase = GUILayout.TextField(currentPhrase);
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Go"))
                {
                    if (timed)
                    {
                        timer = Time.time + delayTime;
                        mode = Mode.DelayedStart;
                    }
                    else
                    {
                        mode = Mode.Recording;
                        if (recorder)
                        {
                            recorder.Reset();
                            recorder.mode = RecordAvatar.Mode.Recording;
                        }
                    }
                }
                
                // End the group we started above. This is very important to remember!
                GUILayout.FlexibleSpace();

                GUILayout.EndArea();
                
            }
            else if (mode == Mode.DelayedStart)
            {



                if (Time.time > timer)
                {
                    timer = Time.time + recordTime;
                    mode = Mode.Recording;
                    if (recorder)
                    {
                        recorder.Reset();
                        recorder.mode = RecordAvatar.Mode.Recording;
                    }
                }
                else
                {

                    float timeRemaining = timer - Time.time;
                    GUILayout.BeginArea(new Rect(Screen.width * 0.02f, Screen.height * 0.02f, Screen.width * 0.5f, Screen.height * 0.1f), areaStyle);

                    GUILayout.Label("Starting in " + timeRemaining + "s", textStyle);


                    GUILayout.EndArea();

                }
            }
            else if (mode == Mode.Recording)
            {

                if (mgr != null)
                    mgr.Enabled = true;

                GUILayout.BeginArea(new Rect(Screen.width * 0.02f, Screen.height * 0.02f, Screen.width * 0.3f, Screen.height * 0.1f), areaStyle);
                if (timed)
                {

                    if (Time.time > timer)
                    {
                        mode = Mode.WaitingForDetails;
                    }
                    float timeRemaining = timer - Time.time;

                    GUILayout.Label("Stopping in " + timeRemaining + "s", textStyle);

                }
                else
                {
                    if (GUILayout.Button("Stop"))
                    {
                        mode = Mode.WaitingForDetails;
                    }

                }


                GUILayout.EndArea();
            }
        }
    }
}