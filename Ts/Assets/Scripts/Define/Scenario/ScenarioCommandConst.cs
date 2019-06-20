/** 
 *Copyright(C) 2018 by DefaultCompany 
 *All rights reserved. 
 *FileName:     ScriptCommandConst.cs 
 *Author:       Passion 
 *Version:      1.0 
 *UnityVersion：2017.1.1f1 
 *Date:         2018-03-17 
 *Description:    
 *History: 
 */

using System.Collections.Generic;
// Layer上角色的显示位置.

public enum CharacterPosition {
    left,
    center,
    right
}

//
public enum ScenarioCommandType {
    //打开角色对话框
    OPENCHARASEIRFWINDOW,
    //关闭对话框
    CLOSECHARASEIRFWINDOW,
    //显示角色
    SHOWCHARA,
    //名字 台词
    CHARASEIRFWINDOW,
    //分支选择框
    OPENSELECTBRANCHWINDOW,
    //分歧内容
    IFTYPE1,
    IFTYPE2,
    IFTYPE3,
    ENDIF,

}

public class ScenarioCommandConst {

    public static readonly Dictionary<string, List<string>> CommandRequiredParamKeyList =
        new Dictionary<string, List<string>> ( ) { { ScenarioCommandType.OPENCHARASEIRFWINDOW.ToString ( ), new List<string> ( ) {} }, { ScenarioCommandType.CLOSECHARASEIRFWINDOW.ToString ( ), new List<string> ( ) {} }, { ScenarioCommandType.SHOWCHARA.ToString ( ), new List<string> ( ) { "Image", " Face" } }, { ScenarioCommandType.CHARASEIRFWINDOW.ToString ( ), new List<string> ( ) { "Name", "Serif" } }, { ScenarioCommandType.OPENSELECTBRANCHWINDOW.ToString ( ), new List<string> ( ) { "Branch", "BranchSerif" } }, { ScenarioCommandType.IFTYPE1.ToString ( ), new List<string> ( ) {} }, { ScenarioCommandType.IFTYPE2.ToString ( ), new List<string> ( ) {} }, { ScenarioCommandType.IFTYPE3.ToString ( ), new List<string> ( ) {} }, { ScenarioCommandType.ENDIF.ToString ( ), new List<string> ( ) {} },

        };
}
