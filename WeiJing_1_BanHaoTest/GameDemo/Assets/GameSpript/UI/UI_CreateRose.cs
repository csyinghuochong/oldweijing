using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//创建角色调用此脚本
public class UI_CreateRose : MonoBehaviour {

    //public bool ifGetData;
    public GameObject UI_NameObj;
    public GameObject UI_OccObj;


    public bool MoveStatus;                 //选中移动状态
    public bool MoveStatus_One;     
    public bool ReturnMoveStatus;           //取消选中回去的状态
    public bool ReturnMoveStatus_One;

    public GameObject UI_SetObj_MoFaShi;                //选中魔法师
    public GameObject UI_SetObj_ZhanShi;                //选中战士
    public GameObject UI_NameObj_MoFaShi;               //选中魔法师名字
    public GameObject UI_NameObj_ZhanShi;               //选中战士名字
    public GameObject UI_SetObj_Sclect;                 //选中名称
    public GameObject UI_SetObj_SclectShowName;         //展示角色名称
    public GameObject UI_SetObj_SclectCreateName;       //创建角色名称
    public GameObject UI_SetObj_SclectCreateNameIput;       //创建角色名称
    public GameObject UI_InputName;                     //输入的角色名称


    public GameObject Obj_MoFaShi;
    public GameObject Obj_ZhanShi;
    public GameObject Obj_MoFaShiStartPosi; //战士出生点位
    public GameObject Obj_ZhanShiStartPosi; //法师出生点位
    public GameObject Obj_MovePosi;     //选择英雄后移动的位置
    public GameObject Obj_ReturnMovePosi; //取消影响选择返回的位置
    public string sclectOcctype;       //当前选择职业移动  类型  0：什么都没选 1:战士  2：法师
    public string returnOcctype;       //取消选择职业返回  类型  0：什么都没选 1:战士  2：法师
    private GameObject sclectRoseObj;       //当前选中的职业模型
    private GameObject returnRoseObj;       //取消选择的职业模型
    public GameObject Obj_EffectSclect;     //选中特效源文件
    public GameObject Obj_EffectSclectLoop; //选中时循环播放的特效
    private GameObject effectSclectLoopObj;
    public GameObject Obj_EffectSclectZhanShi;      //选中特效播放坐标点：战士
    public GameObject Obj_EffectSclectFaShi;        //选中特效播放坐标点：法师
    private GameObject Obj_EffectSclectObj;         //当前选择的坐标点

    public GameObject UI_JiaZaiObj;
    private string returnNoRoseStr;           //没有角色返回通用字符

	// Use this for initialization
	void Start () {

        returnNoRoseStr = "点击创建角色";

        string occValue = PlayerPrefs.GetString("OccType");
        if (occValue == "") { 
            //表示第一次进入游戏默认选个值
            PlayerPrefs.SetString("OccType", "1");
        }

        //Debug.Log("occValue = " + occValue);
        sclectOcctype = PlayerPrefs.GetString("OccType");            //默认值
        returnOcctype = "0";
        CreateRose(sclectOcctype,true);          
        updateNameShow();       //更新名称显示

        

	}
	
	// Update is called once per frame
	void Update () {

        //选中移动角色
        if (MoveStatus) {
            //设置移动的模型
            if (!MoveStatus_One) {
                MoveStatus_One = true;
                switch (sclectOcctype)
                {
                    case "1":
                        sclectRoseObj = Obj_ZhanShi;
                        Obj_EffectSclectObj = Obj_EffectSclectZhanShi;
                        //显示UI
                        UI_NameObj.GetComponent<Text>().text = getRoseData(sclectOcctype);
                        UI_OccObj.GetComponent<Text>().text = "战士";
                        
                        UI_SetObj_ZhanShi.SetActive(false);
                        
                        break;
                    case "2":
                        sclectRoseObj = Obj_MoFaShi;
                        Obj_EffectSclectObj = Obj_EffectSclectFaShi;
                        //显示UI
                        UI_NameObj.GetComponent<Text>().text = getRoseData(sclectOcctype);
                        UI_OccObj.GetComponent<Text>().text = "魔法师";
                        UI_SetObj_MoFaShi.SetActive(false);
                        break;
                }

                //设置移动方向
                sclectRoseObj.transform.LookAt(Obj_MovePosi.transform.position);
                Animator roseAnimator = sclectRoseObj.GetComponent<Animator>();
                roseAnimator.SetBool("Run", true);
                roseAnimator.SetBool("Idle", false);

                //播放特效
                GameObject effectObject = (GameObject)Instantiate(Obj_EffectSclect);
                effectObject.transform.SetParent(Obj_EffectSclectObj.transform);
                effectObject.transform.localPosition = Vector3.zero;
                effectObject.transform.localScale = new Vector3(1, 1, 1);

                UI_SetObj_Sclect.SetActive(false);

                PlayerPrefs.SetString("OccType", sclectOcctype);

                //选中的角色是否现实名称个创建角色名字的UI
                if (getRoseData(sclectOcctype) == returnNoRoseStr)
                {
                    //创建游戏名称
                    UI_SetObj_SclectShowName.SetActive(false);
                    UI_SetObj_SclectCreateName.SetActive(true);
                    UI_SetObj_SclectCreateNameIput.SetActive(true);
                }
                else {
                    //显示已有的角色名称
                    UI_SetObj_SclectShowName.SetActive(true);
                    UI_SetObj_SclectCreateName.SetActive(false);
                    UI_SetObj_SclectCreateNameIput.SetActive(false);
                }

            }

            //持续移动
            sclectRoseObj.transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
            if (Vector3.Distance(sclectRoseObj.transform.position, Obj_MovePosi.transform.position) < 0.1f)
            {
                MoveStatus = false;
                MoveStatus_One = false;
                Animator roseAnimator = sclectRoseObj.GetComponent<Animator>();
                roseAnimator.SetBool("Run", false);
                roseAnimator.SetBool("Idle", true);
                sclectRoseObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                //更新UI
                updateNameShow();

                //播放持续特效
                effectSclectLoopObj = (GameObject)Instantiate(Obj_EffectSclectLoop);
                effectSclectLoopObj.transform.SetParent(Obj_MovePosi.transform);
                effectSclectLoopObj.transform.localPosition = new Vector3(0,0.1f,-0.3f);
                effectSclectLoopObj.transform.localScale = new Vector3(1, 1, 1);

                //单独修正战士位置
                if (sclectOcctype == "1") {
                    sclectRoseObj.transform.localPosition = new Vector3(sclectRoseObj.transform.localPosition.x - 0.1f, sclectRoseObj.transform.localPosition.y, sclectRoseObj.transform.localPosition.z);
                }
            }
        }

        //返回角色
        if (ReturnMoveStatus)
        {
            //设置移动的模型
            if (!ReturnMoveStatus_One)
            {
                ReturnMoveStatus_One = true;
                //Debug.Log("返回:" + returnOcctype);
                switch (returnOcctype)
                {
                    case "1":
                        returnRoseObj = Obj_ZhanShi;
                        Obj_ReturnMovePosi = Obj_ZhanShiStartPosi;
                        break;
                    case "2":
                        returnRoseObj = Obj_MoFaShi;
                        Obj_ReturnMovePosi = Obj_MoFaShiStartPosi;
                        break;
                }

                //设置移动方向
                returnRoseObj.transform.LookAt(Obj_ReturnMovePosi.transform.position);
                Animator roseAnimator = returnRoseObj.GetComponent<Animator>();
                roseAnimator.SetBool("Run", true);
                roseAnimator.SetBool("Idle", false);
            }

            //持续移动
            returnRoseObj.transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
            if (Vector3.Distance(returnRoseObj.transform.position, Obj_ReturnMovePosi.transform.position) < 0.1f)
            {
                ReturnMoveStatus = false;
                ReturnMoveStatus_One = false;
                Animator roseAnimator = returnRoseObj.GetComponent<Animator>();
                roseAnimator.SetBool("Run", false);
                roseAnimator.SetBool("Idle", true);
                returnRoseObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }

        //加载数据
        if (Game_PublicClassVar.Get_wwwSet.updataNumSum > 0) {
            UI_JiaZaiObj.SetActive(true);

            UI_JiaZaiObj.GetComponent<Text>().text = "加载游戏数据中……  " + Game_PublicClassVar.Get_wwwSet.updataNumSum + "/" + (Game_PublicClassVar.Get_wwwSet.updataNum - 1).ToString(); 
        }

	}


    public void btn_createZhanShi() {
        if (Game_PublicClassVar.Get_wwwSet.CreateRoseStatus) {
            return;
        }

        CreateRose("1");
    }

    public void btn_createFaShi()
    {
        if (Game_PublicClassVar.Get_wwwSet.CreateRoseStatus)
        {
            return;
        }

        CreateRose("2");
    }




    //创建角色
    //创建类型（1,战士  2,法师）
    public void CreateRose(string createType,bool firstGame = false) {

        Debug.Log("createType = " + createType);

        if (!firstGame)
        {
            //如果当前选中的职业和已经选的职业相同,则不触发任何操作
            if (createType == sclectOcctype)
            {
                return;
            }
        }

        //如果当前移动则点击无效
        if (MoveStatus) {
            return;
        }

        if (ReturnMoveStatus) {
            return;
        }

        //开启移动状态
        MoveStatus = true;

        //更改进入游戏的角色
        switch (createType)
        {
            case "1":
                Game_PublicClassVar.Get_wwwSet.RoseID = "10001";
                break;
            case "2":
                Game_PublicClassVar.Get_wwwSet.RoseID = "10002";
                break;
        }

        //删除中心特效
        if (effectSclectLoopObj != null) {
            Destroy(effectSclectLoopObj);
        }


        //职业往回跑为0时,表示没有职业需要王辉跑
        if (returnOcctype == "0")
        {
            //职业往回跑
            returnOcctype = sclectOcctype;
            return;
        }

        //取消职业往回跑
        returnOcctype = sclectOcctype;
        if (sclectOcctype != "0")
        {
            ReturnMoveStatus = true;
        }

        //更新当前选中角色
        sclectOcctype = createType;



    }

    //获取当前角色数据
    private string getRoseData(string occ) {
        //显示UI
        string roseID = "10001";
        switch (occ) { 
            case "1":
                roseID = "10001";
            break;
            case "2":
                roseID = "10002";
            break;
        }

        string roseName = "";
        string roseLv = "";
        string firstGame = "";

        try
        {
            //此处加密以后不能读取此数据想一下怎么处理此处
            string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + roseID + "/";
            roseName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Name", "ID", roseID, set_XmlPath + "GameConfig.xml");
            roseLv = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Lv", "ID", roseID, set_XmlPath + "GameConfig.xml");
            firstGame = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("FirstGame", "ID", roseID, set_XmlPath + "GameConfig.xml");
            
        }
        catch {
            Debug.Log("报错了");
            roseName = "角色数据错误,点击进入游戏修复！";
        }

        if (firstGame != "0")
        {
            string value = "等级：" + roseLv + "  " + roseName;
            return value;
        }
        else {
            string value = returnNoRoseStr;
            return value;
        }

        
        
    }

    void updateNameShow() { 
        //更新名称显示
        UI_NameObj_ZhanShi.GetComponent<Text>().text = getRoseData("1");
        UI_NameObj_MoFaShi.GetComponent<Text>().text = getRoseData("2");

        switch (sclectOcctype) { 
            case "0":
                    UI_SetObj_MoFaShi.SetActive(false);
                    UI_SetObj_ZhanShi.SetActive(true);
                    UI_SetObj_Sclect.SetActive(false);
                break;

            case "1":

                    UI_SetObj_MoFaShi.SetActive(false);
                    UI_SetObj_ZhanShi.SetActive(false);
                    UI_SetObj_Sclect.SetActive(true);

                break;

            case "2":
                    
                    UI_SetObj_MoFaShi.SetActive(false);
                    UI_SetObj_ZhanShi.SetActive(true);
                    UI_SetObj_Sclect.SetActive(true);

                break;
        }

        /*
        //修正物体在屏幕中的位置
        vec3_CreateZhanShi = Camera.main.WorldToViewportPoint(Obj_EffectSclectZhanShi.transform.position);
        Debug.Log("vec3_CreateZhanShi = " + vec3_CreateZhanShi);
        vec3_CreateZhanShi = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_CreateZhanShi);
        Debug.Log("vec3_CreateZhanShi = " + vec3_CreateZhanShi);
        Btn_CreateZhanShi.transform.position = vec3_CreateZhanShi;

        //修正物体在屏幕中的位置
        vec3_CreateMoFaShi = Camera.main.WorldToViewportPoint(Obj_EffectSclectFaShi.transform.position);
        Debug.Log("vec3_CreateMoFaShi = " + vec3_CreateMoFaShi);
        vec3_CreateMoFaShi = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_CreateZhanShi);
        Debug.Log("vec3_CreateMoFaShi = " + vec3_CreateMoFaShi);
        Btn_CreateMoFaShi.transform.position = vec3_CreateMoFaShi;
        */
    }
}
