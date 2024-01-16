using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;

public class UI_StoreHouse : MonoBehaviour {

    public GameObject Obj_BagSpace;                 //动态创建的格子
    public Transform Tra_BagSpaceList;              //动态创建格子的绑点
	public GameObject Obj_BagGoldNum;				//背包金币
	public GameObject Obj_BagFuzhong;				//背包当前负重
    private float bagSpacePosition_X;               //动态创建格子的X轴位置
    private float bagSpacePosition_Y;               //动态创建格子的Y轴位置
    private int creatSpaceNum;                      //动态创建的格子数
	// Use this for initialization
	void Start () {

        //缓存一下背包数据
        /*
        DataTable dataTable;
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseBag.xml", "RoseBag");
        Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables.Remove("RoseBag");   //先将背包数据移除在进行添加
        Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables.Add(dataTable);
        */
		//更新背包货币数据
        //this.gameObject.active = false;
		UpdataBagMoney ();
       
        //动态创建格子行
        for (int z = 1; z <= 8; z++) {

            //动态创建每行中的格子
            for (int i = 1; i <= 8; i++)
            {
                //开始创建背包格子
                GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                bagSpace.transform.SetParent(Tra_BagSpaceList);
				bagSpace.transform.localScale = new Vector3(1,1,1);
                bagSpace.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(bagSpacePosition_X, bagSpacePosition_Y, 0);
                bagSpacePosition_X = bagSpacePosition_X + 59.0f;
                creatSpaceNum = creatSpaceNum + 1;
                bagSpace.GetComponent<UI_StoreHouseSpace>().BagPosition = creatSpaceNum.ToString();
                bagSpace.GetComponent<UI_StoreHouseSpace>().SpaceType = "3";   //设置格子为背包属性 
            }

            //每行创建完数据清0
            bagSpacePosition_Y = bagSpacePosition_Y - 59.0f;
            bagSpacePosition_X = 0.0f;
        
        }

        //打开背包
        if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status == false)
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
        }

        
	}
	
	// Update is called once per frame
	void Update () {
        //this.gameObject.active = true;
		if (Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll) {
			//更新背包货币数据
			UpdataBagMoney ();
		}


        //触发移动注销此界面
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus != "1"){
            Destroy(this.gameObject);
            //关闭仓库状态
            Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus = false;

            //关闭背包
            if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status == true)
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
            }
        }
	}

    //被销毁时调用
    void OnDisable() {
        //此处会有一个错误,当开启背包UI，强制关闭游戏时,此处会报错，因为找不到对象
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose != null) {
            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_UIStatus>().RoseBag_Status = false;
        }
    }

	public void CloseUI(){
		Destroy (this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status = false;
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //关闭仓库状态
        Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus = false;
	}

	public void UpdataBagMoney(){
		//获取当前金币
		string gold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Obj_BagGoldNum.GetComponent<Text>().text = gold;
	}

    public void CloseItemTips() {
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
    }
}
