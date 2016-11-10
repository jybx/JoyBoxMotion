using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 

public struct BoneRecord
{
    public Vector3 localPos;
    public Quaternion localRot;
};
public class KeyFrame
{
    public BoneRecord[] records;
    public KeyFrame(int len)
    {
        records = new BoneRecord[len];
    }
};


[RequireComponent(typeof(KinectManager))]
public class RecordAvatar : MonoBehaviour
{
    public int maxFrames = 1024;

	Transform[] skeleton;
    Queue<KeyFrame> history;
    KeyFrame[] framesForPlayback;

    KeyFrame initialState=null;

    KinectManager mgr;

    public enum Mode { Inactive, Recording, Playback };
    public Mode mode = Mode.Inactive;
    public int PlaybackFrame = 0;
    public void Awake()
    {
        mgr = GetComponent<KinectManager>(); 
        Reset();

    }

    public KinectManager GetMgr()
    {
        return mgr;
    }

    public void Reset()
    {
        skeleton = GetComponentsInChildren<Transform>();
        history = new Queue<KeyFrame>();
        if (initialState==null || initialState.records.Length!=skeleton.Length)
        {
            initialState = new KeyFrame(skeleton.Length);
            for (int i = 0; i < skeleton.Length; i++)
            {
                initialState.records[i].localPos = skeleton[i].localPosition;
                initialState.records[i].localRot = skeleton[i].localRotation;
            }
        }
        else
        {
            for (int i = 0; i < skeleton.Length; i++)
            {
                skeleton[i].localPosition = initialState.records[i].localPos;
                skeleton[i].localRotation = initialState.records[i].localRot;
            }
        }
    }

    void Record()
    {
        if (framesForPlayback != null)
        {
            framesForPlayback = null;

        }
        KeyFrame frame = new KeyFrame(skeleton.Length);
        for(int i=0;i<skeleton.Length;i++)
        {
            frame.records[i].localPos = skeleton[i].localPosition;
            frame.records[i].localRot = skeleton[i].localRotation;
        }
        if (history.Count == maxFrames)
            history.Dequeue();

        history.Enqueue(frame);
    }
    void Playback(int n)
    {
        if(framesForPlayback==null)
        {
            framesForPlayback=new KeyFrame[history.Count];
            history.CopyTo(framesForPlayback, 0);

        }

        KeyFrame frame = framesForPlayback[n];
        for (int i = 0; i < skeleton.Length; i++)
        {
            skeleton[i].localPosition = frame.records[i].localPos;
            skeleton[i].localRotation = frame.records[i].localRot;
        }
    }
	// Update the avatar each frame.
    public void FixedUpdate()
    {	
	    if(mode==Mode.Recording)
        {
            Record();
        }
        else if(mode==Mode.Playback)
        {
            Playback(PlaybackFrame % history.Count);
        }
		
	}
}

