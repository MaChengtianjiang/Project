/** 
 *Copyright(C) 2018 by DefaultCompany 
 *All rights reserved. 
 *FileName:     ScenarioCommand.cs 
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


/**
* 剧情指令集
 */
public abstract class ScenarioCommandAction {

    public ScenarioCommandAction (ScenarioCommandType scenarioCommandType, Dictionary<string, string> dictionary) {
        this.SetActionType (scenarioCommandType);
        this.SetLabel (scenarioCommandType.ToString ( ));
        this.Parse (dictionary);
    }
    public ScenarioCommandType CommandType;

    public void SetActionType (ScenarioCommandType commandType) {
        CommandType = commandType;
    }
    protected string LabelName { get; private set; }
    // 标签名
    public void SetLabel (string labelName) {
        LabelName = labelName;
    }

    // 下一条指令.
    public ScenarioCommandAction NextAction { get; private set; }
    // 安置下一条指令.
    public void SetNextAction (ScenarioCommandAction action) {
        NextAction = action;
    }

    protected abstract void Parse (Dictionary<string, string> dictionary);

}

public class NullAction : ScenarioCommandAction {

    public NullAction (ScenarioCommandType scenarioCommandType, Dictionary<string, string> dictionary) : base (scenarioCommandType, dictionary) {}
    protected override void Parse (Dictionary<string, string> dictionary) {

    }
}

public class OpenCharaSeirfWindow : ScenarioCommandAction {

    public OpenCharaSeirfWindow (ScenarioCommandType scenarioCommandType, Dictionary<string, string> dictionary) : base (scenarioCommandType, dictionary) {}
    protected override void Parse (Dictionary<string, string> dictionary) {

    }
}

/**
    文字信息相关
 */
public class ScenarioCommandMessageAction : ScenarioCommandAction {

    public string Name;
    public string Serif;
    public ScenarioCommandMessageAction (ScenarioCommandType scenarioCommandType, Dictionary<string, string> dictionary) : base (scenarioCommandType, dictionary) {

    }

    protected override void Parse (Dictionary<string, string> dictionary) {

    }

    protected void ParseCommon (Dictionary<string, string> dictionary) {

        string name;
        Name = (dictionary.TryGetValue ("Name", out name)) ? name : null;

        string serif;
        Serif = (dictionary.TryGetValue ("Serif", out serif)) ? serif : null;
    }

}

public class CharaSeirfWindow : ScenarioCommandMessageAction {
    public CharaSeirfWindow (ScenarioCommandType scenarioCommandType, Dictionary<string, string> dictionary) : base (scenarioCommandType, dictionary) {

    }

    protected override void Parse (Dictionary<string, string> dictionary) {
        this.ParseCommon (dictionary);
    }

}
