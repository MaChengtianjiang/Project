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
using UnityEngine.UI;


namespace Scenario
{
    public class ScenarioController : MonoBehaviour
    {

        public const string PrefabPath = "Prefab/Scenario/ScenarioController";

        [SerializeField] private Bg MainBg = null;
        [SerializeField] private List<CharaImage> CharaImageList = null;
        [SerializeField] private ScenarioTextBox TextBox = null;

        private float IntervalTime = 0.1f;

        private bool IsShow = false;

        [SerializeField] private ScenarioController Instance = null;

        private ScenarioCommand[] CurrentScenarioCommands = null;

        public static void StartScenarion()
        {
            GameObject go = Instantiate(Resources.Load(PrefabPath)) as GameObject;
            go.transform.SetParent(GameObject.Find(ObjectDefine.GameManager).transform);
        }

        public void SetCurrentScenarioCommands(String filePath) {
            CurrentScenarioCommands = null;

            Resources.Load(filePath);
            
        }

        public IEnumerator Init(string bgName, string[] charaName, string[] charaFace)
        {
            IsShow = false;
            MainBg.SetBg(bgName);   //加载背景

            for (int i = 0; i < CharaImageList.Count; i++)
            {
                if (charaName[i] != string.Empty)
                {
                    CharaImageList[i].SetChara(charaName[i], charaFace[i]);  //加载角色
                }
            }

            //间隔 0.1秒
            var interval = new WaitForSeconds(IntervalTime);

            ShowBg();

            yield return interval;
            ShowChara(charaName, charaFace);

            yield return interval;

            ShowTextBox();

            var waitTime = new WaitForSeconds(0.3f);

            yield return waitTime;

            IsShow = true;
            yield break;
        }

        public IEnumerator Quit()
        {

            //间隔 0.1秒
            var interval = new WaitForSeconds(IntervalTime);
            ShowTextBox(true);

            yield return interval;
            ShowChara(null, null, true);

            yield return interval;
            ShowBg(true);
            var waitTime = new WaitForSeconds(0.3f);

            yield return waitTime;
            IsShow = false;

            yield break;
        }

        private void ShowBg(bool isFade = false)
        {
            StartCoroutine(MainBg.StartAddImageAlpha(isFade));    //开始显示背景
        }

        private void ShowChara(string[] charaName, string[] charaFace, bool isFade = false)
        {

            for (int i = 0; i < CharaImageList.Count; i++)
            {
                if (charaName != null)
                {
                    if (charaName[i] != string.Empty)
                    {
                        StartCoroutine(CharaImageList[i].StartAddImageAlpha(isFade)); //显示角色
                    }
                }
                else
                {
                    StartCoroutine(CharaImageList[i].StartAddImageAlpha(isFade)); //显示角色
                }
            }
        }

        private void ShowTextBox(bool isFade = false)
        {
            StartCoroutine(TextBox.StartAddImageAlpha(isFade));
        }


        public void SetScenarioText(string scenarioFileName)
        {

            if (!IsShow)
            {
                return;
            }


        }
    }

    public class ScenarioImage : MonoBehaviour
    {
        [SerializeField] protected Image MainImage;
        bool IsShow = false;

        public IEnumerator StartAddImageAlpha(bool isFade = false)
        {

            float currentAlpha = isFade ? 1 : 0;
            if (!isFade)
            {  //出现
                while (currentAlpha < 1)
                {
                    MainImage.color = new Color(0, 0, 0, 0.03f);
                    currentAlpha += 0.03f;
                    yield return null;
                }
            }
            else
            {    //消失
                while (currentAlpha > 1)
                {
                    MainImage.color = new Color(0, 0, 0, -0.03f);
                    currentAlpha += -0.03f;
                    yield return null;
                }
            }
            yield break;

        }
    }

    public class Bg : ScenarioImage
    {
        private string BgImagePath = "";
        public void SetBg(string fileName)
        {
            Sprite image = Resources.Load(BgImagePath + "¥" + fileName) as Sprite;
            MainImage.sprite = image;
        }
    }

    public class CharaImage : ScenarioImage
    {
        private string CharaImagePath = "";
        public void SetChara(string charaName, string charaFace)
        {
            Sprite image = Resources.Load(CharaImagePath + "¥" + charaName + "¥" + charaFace) as Sprite;
            MainImage.sprite = image;
        }
    }

}