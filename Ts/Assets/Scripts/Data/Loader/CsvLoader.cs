/** 
 *Copyright(C) 2018 by #COMPANY# 
 *All rights reserved. 
 *FileName:     CsvLoader
 *Author:       Passion 
 *Version:      1.0 
 *UnityVersion：2017.1.1f1 
 *Date:         2018-01-23 
 *Description:    
 *History: 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvLoader
{

    enum LoadType
    {
        LoadKey,
        LoadValueType,
        LoadValue
    }
    private const string CsvPathRoot = "Csv/";

    public static Dictionary<int, DataTable> LoadDataBaseCsv(string path)
    {
        int index = 1;
        Dictionary<int, DataTable> csvTable = new Dictionary<int, DataTable>();
        var asset = Resources.Load(CsvPathRoot + path, typeof(TextAsset)) as TextAsset;

        string value = asset.text;

        //分割行
        string[] strLine = value.Split('\n');
        List<string> dataKeyList = new List<string>();
        Dictionary<string, string> dataTypeTable = new Dictionary<string, string>();

        LoadType currentLoadType = LoadType.LoadKey;
        for (int i = 0; i < strLine.Length -1  ; i++)
        {
            string currentStrLine = strLine[i];
            // 忽略行 忽略符号 #
            if (currentStrLine.Substring(0, 1) == "#")
            {
                continue;
            }
            DataTable currentData = new DataTable();
            string[] currentDatas = currentStrLine.Split(',');
            // 读取数据
            for (int j = 0; j < currentDatas.Length; j++)
            {
                if (currentLoadType == LoadType.LoadKey)
                {
                    //  读取数据Key
                    dataKeyList.Add(currentDatas[j]);

                }
                else if (currentLoadType == LoadType.LoadValueType)
                {
                    //  读取数据类型ß
                    dataTypeTable.Add(dataKeyList[j], currentDatas[j]);
                }
                else
                {
                    string typeSrt = dataTypeTable[dataKeyList[j]];
                    Debug.Log("Key:" + dataKeyList[j] + ",Value:" + currentDatas[j]);
                    // 登录本行数据
                    if (typeSrt == "string")
                    {
                        currentData.AddSrtingValue(dataKeyList[j], currentDatas[j]);
                    }
                    else if (typeSrt == "int")
                    {
                        currentData.AddIntValue(dataKeyList[j], int.Parse(currentDatas[j]));
                    }
                    else if (typeSrt == "float")
                    {
                        currentData.AddFloatValue(dataKeyList[j], float.Parse(currentDatas[j]));
                    }
                }

            }
            // 读取下一行
            if (currentLoadType == LoadType.LoadKey)
            {
                currentLoadType = LoadType.LoadValueType;
            }
            else if (currentLoadType == LoadType.LoadValueType)
            {
                currentLoadType = LoadType.LoadValue;
            }
            else
            {
                csvTable.Add(index, currentData);
                index++;
            }

        }


        return csvTable;
    }


}
