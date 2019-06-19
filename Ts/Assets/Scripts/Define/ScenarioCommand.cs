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

        public void SetActionType (ScenarioCommandType commandType) {
            CommandType = commandType;
        }
        protected string LabelName { get; private set; }
        // 标签名
        public void SetLabel (string labelName) {
            LabelName = labelName;
        }

    }

    public class OpenCharaSeirfWindow : ScenarioCommand {

        public OpenCharaSeirfWindow () {
            this.SetActionType (ScenarioCommandType.OpenCharaSeirfWindow);
            this.SetLabel("打开对话框");
        }
    }
}
