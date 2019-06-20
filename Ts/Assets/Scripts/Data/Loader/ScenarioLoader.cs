using System;
using System.Collections.Generic;
using System.Linq;

using Scenario;

using UnityEngine;

public class ScenarioLoader {

    //循环次数
    private static int LoopCount = 0;

    //解析中的文字列
    private static string CurrentReadingLine = null;

    public static readonly char TagStart = '[';

    //解析文字列成命令
    public static List<ScenarioCommandAction> ParseScriptListToCommand (List<string> scriptTextList) {

        //结果命令列表
        var result = new List<ScenarioCommandAction> ( );

        //结果命令标签列表
        Dictionary<string, List<ScenarioCommandAction>> labelActions = new Dictionary<string, List<ScenarioCommandAction>> ( );

        int lineCount = 0;
        while (true) {

            string line = scriptTextList [lineCount];
            CurrentReadingLine = line;

            if (line [0] == TagStart) {

                // Tag的场合.

                // 变换这一行.
                Dictionary<string, string> lineDictionary = ConvertLine (line);
                string actionName = lineDictionary ["actionName"];
                ScenarioCommandAction action = null;

                // 可以用Enum.IsDefined(typeof(ScriptCommandConst.SoundType), actionName)来进行判定、
                // 但是速度慢，维持现状
                ScenarioCommandType commandType = (ScenarioCommandType) Enum.Parse (typeof (ScenarioCommandType), actionName);

                //检查参数
                CheckRequiredParamKey (lineDictionary ["actionName"], lineDictionary);

                switch (commandType) {
                case ScenarioCommandType.OPENCHARASEIRFWINDOW:
                    action = new OpenCharaSeirfWindow (commandType, lineDictionary);
                    //Debug.Log ("OpenCharaSeirfWindow.读取成功");
                    break;

                case ScenarioCommandType.CHARASEIRFWINDOW:
                    action = new CharaSeirfWindow (commandType, lineDictionary);
                    break;
                default:
                    action = new NullAction (commandType, lineDictionary);
                    break;

                }
                // if (actionName.StartsWith ("OpenCharaSeirfWindow")) {
                //     // シナリオタグで始まる場合.
                //     action = new OpenCharaSeirfWindow ();

                //     Debug.Log ("OpenCharaSeirfWindow.读取成功");
                // }

                result.Add (action);
            }
            lineCount++;

            if (lineCount >= scriptTextList.Count) {
                // 读完了 处理结束.
                break;
            }

        }

        lineCount--;

        while (lineCount > 1) {
            //加载下一条命令
            //Debug.Log ("lineCount:" + lineCount);
            result [lineCount - 2].SetNextAction (result [lineCount - 1]);
            lineCount--;
        }

        return result;
    }

    // 分割指定行的标签和属性.
    private static Dictionary<string, string> ConvertLine (string line) {
        Dictionary<string, string> lineDictionary = new Dictionary<string, string> ( );

#if UNITY_EDITOR
        // 检查脚本构成.
        int equqlCount = line.ToList ( ).Where (c => c.Equals ('=')).Count ( );
        int doubleQuoteCount = line.ToList ( ).Where (c => c.Equals ('"')).Count ( );
        if ((equqlCount != 0) && ((equqlCount * 2) != doubleQuoteCount)) {
            LineError ("=或者\"不够.");
        }
        int startPutCount = line.ToList ( ).Where (c => c.Equals ('[')).Count ( );
        int endPutCount = line.ToList ( ).Where (c => c.Equals (']')).Count ( );
        if (startPutCount != endPutCount) {
            LineError ("没有[ ]的闭合.");
        }
#endif
        List<String> splitLine = new List<String> ( );
        char [ ] splitChar;

        string tmpLine = line;
        int spaceCount = tmpLine.Length - tmpLine.Replace (" ".ToString ( ), "").Length;
        int doubleQuartCount = tmpLine.Length - tmpLine.Replace ("\"", "").Length;
        spaceCount -= doubleQuartCount / 2;
        if ((0 < spaceCount) && !tmpLine.StartsWith ("*")) {
            Debug.LogWarning ("存在违反规则的半角空格:" + CurrentReadingLine + "空格处:" + spaceCount + ".");

            // 分割文字列.
            int firstSpaceIndex = line.IndexOf (' ');
            splitLine.Add (line.Substring (1, firstSpaceIndex - 1));

            splitChar = new char [ ] { '[', ']', ' ', '=', '"', '*' };
            line = line.Substring (firstSpaceIndex);
            List<String> addLineSplit = line.Split (splitChar, StringSplitOptions.RemoveEmptyEntries).ToList ( );
            for (int i = 0; i < addLineSplit.Count; ++i) {
                int firstIndex = addLineSplit [i].IndexOf (' ');
                int lastIndex = addLineSplit [i].LastIndexOf (' ');
                if (firstIndex != -1) {
                    addLineSplit [i] = addLineSplit [i].Substring (1);
                }
                if ((lastIndex != -1) && (lastIndex != firstIndex)) {
                    addLineSplit [i] = addLineSplit [i].Substring (0, addLineSplit [i].Length);
                }
                splitLine.Add (addLineSplit [i]);
            }
        } else {
            // 分割文字列.
            splitChar = new char [ ] { '[', ']', ' ', '=', '"', '*' };
            // 不允许有empty
            splitLine = line.Split (splitChar, StringSplitOptions.RemoveEmptyEntries).ToList ( );
        }

#if false
        for (int i = 0; i < splitLine.Count; ++i) {
            Debug.LogWarning (i + " : " + splitLine [i]);
        }
#endif

        // 用大写字母封装.
        lineDictionary.Add ("actionName", splitLine [0].ToUpper ( ));

        if (splitLine.Count > 1) {
            for (int i = 1; i < splitLine.Count; i += 2) {

                lineDictionary.Add (splitLine [i], splitLine [i + 1]);
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

    public static void CheckRequiredParamKey (string type, Dictionary<string, string> dictionary) {
        // 现在检查的命令中有必须参数么
        List<string> debugLogs = new List<string> ( );
        CheckRequiredParamKey (type, dictionary, ref debugLogs);
        for (int ii = 0; ii < debugLogs.Count; ++ii) {
            LineError (debugLogs [ii]);
        }
    }

    public static List<string> CheckRequiredParamKey (string type, Dictionary<string, string> dictionary, ref List<string> debugLogs) {
        List<string> requireParams = null;
        if (ScenarioCommandConst.CommandRequiredParamKeyList.TryGetValue (type, out requireParams)) {
            // 从dictionary中检查必须有的参数.
            int paramNum = requireParams.Count;
            for (int ii = 0; ii < paramNum; ++ii) {
                string paramName = requireParams [ii];
                if (!dictionary.ContainsKey (paramName)) {
                    // 没有必须的参数
                    debugLogs.Add (string.Format ("[脚本错误] <b>{0}</b>命令中<b>没有发现{1}</b>参数.", type.ToString ( ), paramName));
                }
            }
        }
        return debugLogs;
    }
}
