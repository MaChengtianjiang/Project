/** 
 *Copyright(C) 2018 by DefaultCompany 
 *All rights reserved. 
 *FileName:     TxtLoder.cs 
 *Author:       Passion 
 *Version:      1.0 
 *UnityVersion：2017.1.1f1 
 *Date:         2018-03-17 
 *Description:    
 *History: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TxtLoder : MonoBehaviour {

    private static readonly string PathRoot = "Txt/";

    public static List<string> Load (string path) {

        TextAsset loadTextAsset = Resources.Load (PathRoot + path, typeof (TextAsset)) as TextAsset;

        string allText = loadTextAsset.text;

        string[] textList = allText.Split (';');

        List<string> resultList = new List<string> ();

        for (int i = 0; i < textList.Length - 1; i++) {
            if (textList[i].Substring (0, 2).Contains ("\n")) {
                //删除掉换行符
                textList[i] = textList[i].Substring (1);
            }
            // 忽略行 忽略符号 #
            if (textList[i].Substring (0, 1) == "#") {
                continue;
            }

            resultList.Add (textList[i]);

        }

        return resultList;
    }

}
