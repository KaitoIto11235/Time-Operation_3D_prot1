using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Text;

public class AutoCondition : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] private GameObject guidance, user;
    [SerializeField] string readFileName = "default";
    [SerializeField] string writeFileName = "default";
    [SerializeField] GameObject startPoint, endPoint;
    [SerializeField] int readFileRowCount = 1000;
    FileOperation autoFile;
    AutoPlay autoGuidance;
    [SerializeField] bool Recording = false;
    [SerializeField] Material[] materialArray = new Material[3];
    int commaPlaySpeed = 10; // 10が等速再生
    //[SerializeField, Range(1, 20)] int commaPlaySpeed = 10;

    [SerializeField] GameObject wristR;

    void Start()
    {
        if(Recording)
        {
            autoFile = new FileOperation(readFileName, readFileRowCount, writeFileName, user, startPoint, endPoint);
            autoFile.WriteOpenData();
        }
        else
        {
            autoFile = new FileOperation(readFileName, readFileRowCount,startPoint, endPoint);
        }
        autoGuidance = new AutoPlay(guidance, user, readFileRowCount, autoFile.modelPositions, autoFile.modelQuaternions,
         commaPlaySpeed, materialArray, wristR);
        autoFile.ReadOpenData();

        
        autoFile.FileSettingCheck();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        autoGuidance.GuidanceUpdate();
        
        // 0.5秒に1度、効果音を鳴らす
        if(autoGuidance.GuidanceTime % 45 == 0)
        {
            audioSource.Play();
        }

        if(Recording)
        {
            autoFile.RecordingUpdate();
        }
    }
}


