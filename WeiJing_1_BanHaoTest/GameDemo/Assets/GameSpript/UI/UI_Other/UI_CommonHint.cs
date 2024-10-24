﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_CommonHint : MonoBehaviour {

    //public GameObject Obj_HintParent;

    public GameObject Obj_HintTitleName;
    public GameObject Obj_HintText;
    public GameObject Btn_HintTrueText;
    public GameObject Btn_HintFalseText;

    /*
    public string HintTitleName;
    public string HintText;
    public string HintTrueText;
    public string HintFalseText;
    */
    //定义一个委托
    public delegate void DelegateCommon();   // 定义一个委托
    //public bool testStatus;
    private DelegateCommon commonDel_True;
    private DelegateCommon commonDel_False;
	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*
	    //Obj_HintParent.GetComponent<HintTitleName>()
        if (testStatus) {
            Debug.Log("调用委托成功1111;");
            testStatus = false;
            commonDel_True();
        }
         */
	}

    //参数（提示内容,方法1,方法2,提示框名称,按钮名称_是,按钮名称_否）
    public void Btn_CommonHint(string hintStr, DelegateCommon dt_true, DelegateCommon dt_false,string hintTitle = "系统提示",string hint_TrueStr = "确定",string hint_FalseStr = "取消")
    {
        //显示提示信息
        Obj_HintTitleName.GetComponent<Text>().text = hintTitle;
        Obj_HintText.GetComponent<Text>().text = hintStr;
        Btn_HintTrueText.GetComponent<Text>().text = hint_TrueStr;
        Btn_HintFalseText.GetComponent<Text>().text = hint_FalseStr;
        
        //存储方法
        commonDel_True = dt_true;
        commonDel_False = dt_false;
    }

    //点击是的按钮
    public void BtnTrue() {

        //执行功能
        if (commonDel_True != null) {
            commonDel_True();
        }
        
        //销毁此窗口
        Destroy(this.gameObject);

    }

    //点击否的按钮
    public void BtnFalse() {

        //执行功能
        if (commonDel_False != null) {
            commonDel_False();
        }
        //销毁此窗口
        Destroy(this.gameObject);
    }

    //关闭界面
    public void Btn_Close() {
        Destroy(this.gameObject);
    }
}
