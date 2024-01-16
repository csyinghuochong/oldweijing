using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_StartGame : MonoBehaviour {
    public bool DestroyKeepObj;                     //当次开关打开是注销Keep保存的Obj,此处用在战斗界面切换建筑界面中
    public GameObject EnterGameObj;
    public GameObject Obj_ReturnBuilding;
    public GameObject Obj_StartBGM;                 //开始界面的BGM      //只在初始界面配置播放即可
    public bool EnterGameStatus;                    //进入游戏状态
	// Use this for initialization
	void Start () {
        if (Obj_StartBGM != null) {
            float sourceSize = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SourceSize", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig"));
            Obj_StartBGM.GetComponent<AudioSource>().volume = sourceSize;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatusOnce)
        {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
            {
                this.transform.parent.transform.gameObject.SetActive(false);        //隐藏创建角色界面
                Game_PublicClassVar.Get_wwwSet.DataUpdataStatusOnce = true;
                Debug.Log("创建角色进入场景!");
                try
                {
                    Btn_EnterGame();
                }
                catch {
                    Debug.Log("报错！！！进入场景");
                }
            }
        }
	}

    public void Btn_EnterGame() {

        //Application.LoadLevel("EnterGame");

        //写入场景
        //ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", SceneTransferID, "SceneTransfer_Template");
        //获取当前血量
        if (Application.loadedLevelName != "StartGame")
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", "EnterGame", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        //GameObject.Find("DoorWay").gameObject.GetComponent<DoorWay>().DoorWayID = "1";
        //GameObject.Find("DoorWay").gameObject.GetComponent<DoorWay>().EnterGame();
        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = SceneTransferID;
        EnterGameObj.GetComponent<UI_EnterGame>().roseObjUpdataStatus = true;
        EnterGameObj.GetComponent<UI_EnterGame>().EnterGame();
        
    }

    public void Btn_RetuenBuilding() {

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", "EnterGame", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        EnterGameObj.GetComponent<UI_EnterGame>().roseObjUpdataStatus = true;
        //EnterGameObj.GetComponent<UI_EnterGame>().EnterBuildGame();
        //EnterGameObj.GetComponent<UI_EnterGame>().KeepObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Keep;
        EnterGameObj.GetComponent<UI_EnterGame>().EnterBuildGame();
        //Application.LoadLevel("EnterGame");
        //DestroyKeepObj = true;
    }

    public void Btn_ReturnBuildingUI() {
        GameObject obj = (GameObject)Instantiate(Obj_ReturnBuilding);
        obj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        obj.GetComponent<UI_ReturnBuilding>().ReturnBuildingObj = this.gameObject;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);
    }

    void LateUpdate()
    {
        if (DestroyKeepObj) {
            DestroyKeepObj = false;
            //保证切换场景以下预设体不消失
            for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.Obj_Keep.Length; i++)
            {
                Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Keep[i - 1]);
            }
        }
    }

    public void Btn_CreateRose() {
        //Debug.Log("Btn_CreateRose");
        
        if (this.gameObject.transform.parent.gameObject.GetComponent<UI_CreateRose>().sclectOcctype == "0") {
            //Debug.Log("请先选择一个角色！");
            return;
        }
        
        GameObject createRoseObj = this.gameObject.transform.parent.gameObject;
        string createName = createRoseObj.GetComponent<UI_CreateRose>().UI_InputName.GetComponent<InputField>().text;
        //Debug.Log("createName = " + createName);
        //if (createName != "") { 
            //修改角色名称
            string roseID = "10001";
            switch (createRoseObj.GetComponent<UI_CreateRose>().sclectOcctype)
            {
                case "1":
                    roseID = "10001";
                break;

                case "2":
                    roseID = "10002";
                break;
            }

            /*
            string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + roseID + "/";
            if (Game_PublicClassVar.Get_xmlScript.Xml_SetDate("Name", createName, "ID", roseID, set_XmlPath + "RoseData.xml")) {
                Debug.Log("创建角色:" + createName);
            }
            */

        //}
            //Debug.Log("roseID = " + roseID);
        //更新创建角色的名称
        Game_PublicClassVar.Get_wwwSet.CreateRoseStatus = true;
        if (createName == "")
        {
            string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + roseID + "/";
            createName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Name", "ID", roseID, set_XmlPath + "GameConfig.xml");
        }
        //Debug.Log("createName = " + createName);
        Game_PublicClassVar.Get_wwwSet.CreateRoseNameStr = createName;
        
        Game_PublicClassVar.Get_wwwSet.IfChangeOcc = true;
        this.gameObject.transform.position = new Vector3(10000, 0, 0);  //移除屏幕外  UI_InputName
    
    }


}