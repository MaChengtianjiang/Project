/** 
 *Copyright(C) 2018 by #COMPANY# 
 *All rights reserved. 
 *FileName:     #SCRIPTFULLNAME# 
 *Author:       #AUTHOR# 
 *Version:      #VERSION# 
 *UnityVersion：#UNITYVERSION# 
 *Date:         #DATE# 
 *Description:    
 *History: 
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	void Awake() {
		DontDestroyOnLoad(this);
		Dictionary<int, DataTable> charaTable = CsvLoader.LoadDataBaseCsv("level");
		var x = charaTable[50];
		
		Debug.Log("id:" + charaTable[50].GetIntValue("Level") + "的Value:" + x.GetFloatValue("PlayerExp"));
		SceneManager.Instance.transform.SetParent(this.transform);

		//ScenarioController.StartScenarion();

		var text = TxtLoder.Load("Scenario1");

		
	}



}
