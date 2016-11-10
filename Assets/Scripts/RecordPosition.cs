using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 

public class RecordPosition : MonoBehaviour
{	
	Transform[] history;

	bool calibrated=false;
	
	public Transform marker;
	public float interval=0.1f;
	public int count = 10;
	public float offset = 0.5f;
	float timer=0.0f;
	int idx=0;
	int numRecords=0;
	
	public void Calibrated(uint userId)
	{
		calibrated=true;
	}
	public void Awake()
    {	
		history=new Transform[count];
		for(int i=0;i<count;i++)
		{
			history[i] = Instantiate(marker);
			history[i].gameObject.SetActive(false);
		}
	}
	
	// Update the avatar each frame.
    public void Update()
    {	
		if(!calibrated)
			return;
			
		if(timer<=Time.time)
		{
			timer= Time.time+interval;
			
			if(numRecords<count)
				numRecords++;
			for(int i=numRecords-1;i>0;i--)
			{
				Vector3 lastPos = history[i-1].position;
				lastPos.z+=offset;
				history[i].position=lastPos;
				history[i].gameObject.SetActive(true);
			}
			history[0].position=transform.position;
			history[0].gameObject.SetActive(true);
		}
		
	}
}

