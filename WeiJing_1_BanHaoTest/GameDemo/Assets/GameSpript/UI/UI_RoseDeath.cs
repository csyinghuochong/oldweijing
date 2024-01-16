using UnityEngine;
using System.Collections;

public class UI_RoseDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //点击死亡按钮
    public void Btn_ClickDeath() {

        //Debug.Log("我点击了死亡按钮");
        Destroy(this.gameObject);
        //设置角色血量
        int rose_Hp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().rose_LastHp = rose_Hp;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow = rose_Hp;
        //设置角色状态
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatusOnce = false;
        string nowScenceID = Application.loadedLevelName;
        
        string SceneTransferID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HomeTransferID", "ID", nowScenceID, "Scene_Template");
        string ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", SceneTransferID, "SceneTransfer_Template");

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        Game_PublicClassVar.Get_function_UI.PlaySource("10003", "1");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = SceneTransferID;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();
        //Obj_UIEnterPveSet


    }

}
