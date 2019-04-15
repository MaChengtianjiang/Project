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

        //解析文字列成命令
        public List<ScenarioCommand> ParseScriptListToCommand (List<string> scriptTextList) {

            //结果命令列表
            var result = new List<ScenarioCommand> ();
            return result;
        }

        Dictionary<string, List<ScenarioCommand>> actionMap = new Dictionary<string, List<ScenarioCommand>> ();

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
                Debug.LogWarning ("存在违反规则的半角空格:" + spaceCount + " .");

                // 分割文字列.
                int firstSpaceIndex = line.IndexOf (' ');
                splitLine.Add (line.Substring (1, firstSpaceIndex - 1));

                splitChar = new char[] { '[', ']', ' ', '=', '"', '*' };
                line = line.Substring (firstSpaceIndex);
                List<String> addLineSplit = line.Split (splitChar, StringSplitOptions.RemoveEmptyEntries).ToList ();
                for (int ii = 0; ii < addLineSplit.Count; ++ii) {
                    int firstIndex = addLineSplit[ii].IndexOf (' ');
                    int lastIndex = addLineSplit[ii].LastIndexOf (' ');
                    if (firstIndex != -1) {
                        addLineSplit[ii] = addLineSplit[ii].Substring (1);
                    }
                    if ((lastIndex != -1) && (lastIndex != firstIndex)) {
                        addLineSplit[ii] = addLineSplit[ii].Substring (0, addLineSplit[ii].Length);
                    }
                    splitLine.Add (addLineSplit[ii]);
                }
            } else {
                // 文字列の分割.
                splitChar = new char[] { '[', ']', ' ', '=', '"', '*' };
                // emptyを許容しない.
                splitLine = line.Split (splitChar, StringSplitOptions.RemoveEmptyEntries).ToList ();
            }

#if false
            for (int ii = 0; ii < splitLine.Count; ++ii) {
                Debug.LogWarning (ii + " : " + splitLine[ii]);
            }
#endif

            // 大文字で格納.
            lineDictionary.Add ("actionName", splitLine[0].ToUpper ());

            for (int ii = 1; ii < splitLine.Count; ii += 2) {
                lineDictionary.Add (splitLine[ii], splitLine[ii + 1]);
            }

            return lineDictionary;
        }

#if UNITY_EDITOR
        // 解析中の行数に対しエラーを出す。なので１行上のログがエラー発生行になります.
        public static void LineError (string alert) {
            Debug.LogError ("エラー発生行 = " + CurrentReadingLine + "\n" + alert);
        }
#endif
    }

}