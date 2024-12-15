using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Text;

public class AdaptCondition : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] private GameObject guidance, user;
    [SerializeField] string readFileName = "default";
    [SerializeField] string writeFileName = "default";
    [SerializeField] [Range(1, 6)] int experiment4_condition = 7;
    [SerializeField] GameObject startPoint, endPoint;

    [SerializeField] int readFileRowCount = 1000;
    FileOperation adaptFile;
    AdaptPlay adaptGuidance;
    [SerializeField] bool Recording = false;

    [SerializeField] Material[] materialArray = new Material[3];
    // User停止時に手の上に表示されるオブジェクト
    [SerializeField] GameObject stopUser, stopGuidance;
    [SerializeField] GameObject wristR;

    
    void Start()
    {
        if(Recording)
        {
            adaptFile = new FileOperation(readFileName, readFileRowCount, writeFileName, user, startPoint, endPoint);
            adaptFile.WriteOpenData();
        }
        else
        {
            adaptFile = new FileOperation(readFileName, readFileRowCount, startPoint, endPoint);
        }
        adaptGuidance = new AdaptPlay(guidance, user, readFileRowCount, adaptFile.modelPositions, adaptFile.modelQuaternions, materialArray,
         experiment4_condition, stopUser, stopGuidance, wristR);
        adaptFile.ReadOpenData();

        adaptFile.FileSettingCheck();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        adaptGuidance.GuidanceUpdate();

        // 0.5秒に1度、効果音を鳴らす
        if(adaptGuidance.GuidanceTime % 45 == 0)
        {
            audioSource.Play();
        }

        if(Recording)
        {
            adaptFile.RecordingUpdate(adaptGuidance.DistToFile, adaptGuidance.UserLevel, adaptGuidance.TrialOffset, adaptGuidance.LevelOffset);
        }
    }
}
