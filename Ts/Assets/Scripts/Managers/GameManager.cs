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
		Dictionary<int, DataTable> charaTable = CsvLoader.LoadDataBaseCsv("CharaDatas");
		var x = charaTable[5];
		
		Debug.Log("id:" + charaTable[5].GetIntValue("ID") + "的Value:" + x.GetSrtingValue("Job"));
		SceneManager.Instance.transform.SetParent(this.transform);
	}



}
