using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// 脚本转型器
namespace Scenario {

    public class ScenarioLoader {

        //循环次数
        private static int LoopCount = 0;

        //解析中的文字列
        private static string CurrentReadingLine = null;

        public static readonly char TagStart = '[';

        //解析文字列成命令
        public static List<ScenarioCommand> ParseScriptListToCommand (List<string> scriptTextList) {

            //结果命令列表
            var result = new List<ScenarioCommand> ();

            //结果命令标签列表
            Dictionary<string, List<ScenarioCommand>> labelActions = new Dictionary<string, List<ScenarioCommand>> ();

            var lineCount = 0;
            while (true) {

                string line = scriptTextList[lineCount];
                CurrentReadingLine = line;

                if (line[0] == TagStart) {

                    // Tag的场合.

                    // 变换这一行.
                    Dictionary<string, string> lineDictionary = ConvertLine (line);
                    string actionName = lineDictionary["actionName"];
                    ScenarioCommand action = null;

                    // 可以用Enum.IsDefined(typeof(ScriptCommandConst.SoundType), actionName)来进行判定、
                    // 但是速度慢，维持现状

                    switch (actionName) {
                        case "OpenCharaSeirfWindow":
                            action = new OpenCharaSeirfWindow ();
                            Debug.Log ("OpenCharaSeirfWindow.读取成功");
                            break;
                    }

                    // if (actionName.StartsWith ("OpenCharaSeirfWindow")) {
                    //     // シナリオタグで始まる場合.
                    //     action = new OpenCharaSeirfWindow ();

                    //     Debug.Log ("OpenCharaSeirfWindow.读取成功");
                    // }

                }
                lineCount++;

                if (lineCount >= scriptTextList.Count) {
                    // 读完了 处理结束.
                    break;
                }
            }
            return result;
        }

        // 分割指定行的标签和属性.
        private static Dictionary<string, string> ConvertLine (string line) {
            Dictionary<string, string> lineDictionary = new Dictionary<string, string> ();

#if UNITY_EDITOR
            // 检查脚本构成.
            int equqlCount = line.ToList ().Where (c => c.Equals ('=')).Count ();
            int doubleQuoteCount = line.ToList ().Where (c => c.Equals ('"')).Count ();
            if ((equqlCount != 0) && ((equqlCount * 2) != doubleQuoteCount)) {
                LineError ("=或者\"不够.");
            }
            int startPutCount = line.ToList ().Where (c => c.Equals ('[')).Count ();
            int endPutCount = line.ToList ().Where (c => c.Equals (']')).Count ();
            if (startPutCount != endPutCount) {
                LineError ("没有[ ]的闭合.");
            }
#endif
            List<String> splitLine = new List<String> ();
            char[] splitChar;

            string tmpLine = line;
            int spaceCount = tmpLine.Length - tmpLine.Replace (" ".ToString (), "").Length;
            int doubleQuartCount = tmpLine.Length - tmpLine.Replace ("\"", "").Length;
            spaceCount -= doubleQuartCount / 2;
            if ((0 < spaceCount) && !tmpLine.StartsWith ("*")) {
                Debug.LogWarning ("存在违反规则的半角空格:" + CurrentReadingLine + "空格处:" + spaceCount + ".");

                // 分割文字列.
                int firstSpaceIndex = line.IndexOf (' ');
                splitLine.Add (line.Substring (1, firstSpaceIndex - 1));

                splitChar = new char[] { '[', ']', ' ', '=', '"', '*' };
                line = line.Substring (firstSpaceIndex);
                List<String> addLineSplit = line.Split (splitChar, StringSplitOptions.RemoveEmptyEntries).ToList ();
                for (int i = 0; i < addLineSplit.Count; ++i) {
                    int firstIndex = addLineSplit[i].IndexOf (' ');
                    int lastIndex = addLineSplit[i].LastIndexOf (' ');
                    if (firstIndex != -1) {
                        addLineSplit[i] = addLineSplit[i].Substring (1);
                    }
                    if ((lastIndex != -1) && (lastIndex != firstIndex)) {
                        addLineSplit[i] = addLineSplit[i].Substring (0, addLineSplit[i].Length);
                    }
                    splitLine.Add (addLineSplit[i]);
                }
            } else {
                // 分割文字列.
                splitChar = new char[] { '[', ']', ' ', '=', '"', '*' };
                // 不允许有empty
                splitLine = line.Split (splitChar, StringSplitOptions.RemoveEmptyEntries).ToList ();
            }

#if false
            for (int i = 0; i < splitLine.Count; ++i) {
                Debug.LogWarning (i + " : " + splitLine[i]);
            }
#endif

            // 用大写字母封装.
            lineDictionary.Add ("actionName", splitLine[0]);

            if (splitLine.Count > 1) {
                for (int i = 1; i < splitLine.Count; i += 2) {

                    lineDictionary.Add (splitLine[i], splitLine[i + 1]);
                }
            }
            return lineDictionary;
        }

#if UNITY_EDITOR
        // 因为解析中的行已经发生报错。所以前一行就已经发生错误.
        public static void LineError (string alert) {
            Debug.LogError ("error发生行 = " + CurrentReadingLine + "\n" + alert);
        }
#endif
    }

}
