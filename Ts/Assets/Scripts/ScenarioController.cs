/** 
 *Copyright(C) 2018 by DefaultCompany 
 *All rights reserved. 
 *FileName:     ScenarioManager.cs 
 *Author:       Passion 
 *Version:      1.0 
 *UnityVersion：2017.3.1f1 
 *Date:         2018-03-12 
 *Description:    
 *History: 
*/  

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour {


    public const string PrefabPath = "Prefab/Scenario/ScenarioController";

    [SerializeField]
    private ScenarioController Instance = null;

    public static void StartScenarion(){
        GameObject go = Instantiate(Resources.Load(PrefabPath)) as GameObject;
        go.transform.SetParent(GameObject.Find(ObjectDefine.GameManager).transform);
    }
}
