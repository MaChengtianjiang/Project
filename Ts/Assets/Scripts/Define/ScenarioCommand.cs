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

namespace Scenario {
    public enum ScenarioCommandType {
        //打开角色对话框
        OpenCharaSeirfWindow,
        //关闭对话框
        CloseCharaSeirfWindow,
        //显示角色
        ShowChara,
        //名字 台词
        CharaSeirfWindow,
    }

    public abstract class ScenarioCommand {
        public ScenarioCommandType CommandType;

        public void SetActionName (ScenarioCommandType commandType) {
            CommandType = commandType;
        }
        protected string LabelName { get; private set; }
        // ラベル名のセット.
        public void SetLabel (string labelName) {
            LabelName = labelName;
        }

    }

    public class OpenCharaSeirfWindow : ScenarioCommand {

    }
}