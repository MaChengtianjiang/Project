/** 
 *Copyright(C) 2018 by DefaultCompany 
 *All rights reserved. 
 *FileName:     ScenarioWindow.cs 
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
using UnityEngine.UI;

public class ScenarioWindow : ScenarioImage {

    //主文本框
    [SerializeField] private Text MainTextBox;

    //文本播放速度
    private float PlaySpeed = 0.1f;

    //文本当前位置
    private int CurrentTextIndex = 0;
    List<string> CurrentTextList = new List<string> {
        "123",
        "456"
    };

    private string currentMsg;

    //光标
    [SerializeField] private Image Cursor = null;

    public void init (string msg) {
        currentMsg = msg;
        Play(currentMsg);
    }

    public void Play (string message) {
        StartCoroutine (RunPlayMessage (message, message.Length, true));
    }

    private IEnumerator RunPlayMessage (string message, int length, bool isEnd) {

        Cursor.gameObject.SetActive (false);

        while (CurrentTextIndex <= length) {

            MainTextBox.text = message.Substring (0, CurrentTextIndex);;
            CurrentTextIndex++;
            yield return new WaitForSeconds (PlaySpeed);

        }

        Cursor.gameObject.SetActive (isEnd);

        yield break;
    }

    public IEnumerator StartAddImageAlpha (bool isFade = false) {

        float currentAlpha = isFade ? 1 : 0;
        if (!isFade) { //出现
            while (currentAlpha < 1) {
                MainTextBox.color = new Color (0, 0, 0, 0.03f);
                currentAlpha += 0.03f;
                yield return null;
            }
        } else { //消失
            while (currentAlpha > 1) {
                MainTextBox.color = new Color (0, 0, 0, -0.03f);
                currentAlpha += -0.03f;
                yield return null;
            }
        }
        yield break;

    }
}
