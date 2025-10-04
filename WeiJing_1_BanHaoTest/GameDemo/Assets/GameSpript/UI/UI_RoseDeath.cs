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
        Debug.Log("nowScenceID222:" + nowScenceID);
        string SceneTransferID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HomeTransferID", "ID", nowScenceID, "Scene_Template");
        string ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", SceneTransferID, "SceneTransfer_Template");
            
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        Game_PublicClassVar.Get_function_UI.PlaySource("10003", "1");


        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;

        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = SceneTransferID;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();
        
        //Btn_ClickDeath();
    }


    public void Btn_RetuenBuilding()
    {
        
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", "EnterGame", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().roseObjUpdataStatus = true;
        //EnterGameObj.GetComponent<UI_EnterGame>().EnterBuildGame();
        //EnterGameObj.GetComponent<UI_EnterGame>().KeepObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Keep;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterBuildGame();
        //Application.LoadLevel("EnterGame");
        //DestroyKeepObj = true;
        
    }

}
