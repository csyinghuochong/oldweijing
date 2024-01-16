using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ChouKa : MonoBehaviour
{
    private string takeCard_IDStr;
    private string[] takeCard_ID;
    private string countryLv;
    private string zuanShiNum;
    private string zuanShiNum_Ten;
    private string dropID;
    public GameObject ChouKaItemSet;
    public GameObject ChouKaItemObj;
    public GameObject ChouKaTitleObj;
    public GameObject ChouKaImgObj;
    public GameObject Obj_ChouKaTime_One;
    public GameObject Obj_ChouKaTime_Ten;
    private float updataTimeSum;
    private float chouKaTime_One;
    private float chouKaTime_Ten;
    public bool chouKaTime_OneStatus;
    public bool chouKaTime_TenStatus;

    public GameObject chouKaShowItemSet;
    public string nowChouKaID;

    // Use this for initialization
    void Start()
    {

        //初始位置
        this.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        this.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        this.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);

        //
        chouKaTime_OneStatus = false;
        chouKaTime_TenStatus = false;
        //打开界面初始化抽取数据
        Game_PublicClassVar.Get_game_PositionVar.ChouKaStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.ChouKaStr = "";
        Game_PublicClassVar.Get_game_PositionVar.ChouKaUIOpenStatus = true;

        countryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //抽卡字符串
        takeCard_IDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "TakeCard_ID", "GameMainValue");
        takeCard_ID = takeCard_IDStr.Split(';');


        dropID = "0";
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        //循环判定掉落
        for (int i = 0; i <= takeCard_ID.Length - 1; i++)
        {
            if (takeCard_ID[i] != "")
            {
                string roseLvLimit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseLvLimit", "ID", takeCard_ID[i], "TakeCard_Template");
                if (roseLv >= int.Parse(roseLvLimit))
                {
                    dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", takeCard_ID[i], "TakeCard_Template");
                    zuanShiNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuanShiNum", "ID", takeCard_ID[i], "TakeCard_Template");
                    zuanShiNum_Ten = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuanShiNum_Ten", "ID", takeCard_ID[i], "TakeCard_Template");
                    nowChouKaID = takeCard_ID[i];
                }
            }
        }

        //循环展示掉落
        string[] dropItemShowList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropShow", "ID", nowChouKaID, "TakeCard_Template").Split(';');
        for (int i = 0; i <= dropItemShowList.Length - 1; i++)
        {
            string[] chouKaItem = dropItemShowList[i].Split(',');
            GameObject chouKaItemObj = (GameObject)Instantiate(ChouKaItemObj);
            chouKaItemObj.transform.SetParent(chouKaShowItemSet.transform);
            chouKaItemObj.transform.localPosition = new Vector3(i*100, 0, 0);
            chouKaItemObj.transform.localScale = new Vector3(1, 1, 1);
            chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemID = chouKaItem[0];
            chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = chouKaItem[1];
        }
    }

    // Update is called once per frame
    void Update()
    {

        //每秒更新一次时间
        updataTimeSum = updataTimeSum + Time.deltaTime;
        if (updataTimeSum >= 1) {
            updataTimeSum = 0;
            showOtherTime();
        }


        if (Game_PublicClassVar.Get_game_PositionVar.ChouKaStatus) {
            Game_PublicClassVar.Get_game_PositionVar.ChouKaStatus = false;
            //Debug.Log("Game_PublicClassVar.Get_game_PositionVar.ChouKaStr = " + Game_PublicClassVar.Get_game_PositionVar.ChouKaStr);
            string[] dropList = Game_PublicClassVar.Get_game_PositionVar.ChouKaStr.Split(';');
            if (dropList.Length >= 10)
            {
                //设置单抽标题
                ChouKaImgObj.SetActive(false);
                ChouKaTitleObj.SetActive(true);
                chouKaShowItemSet.SetActive(false);
                ChouKaTitleObj.transform.localPosition = new Vector3(0,200,0);
                float show_X = -300;
                float show_Y = 50;
                int showNumHang = 0;

                //10连抽
                for (int i = 0; i <= dropList.Length - 1; i++)
                {
                    if (dropList[i] != "") {
                        showNumHang = showNumHang + 1;

                        //设置十连抽位置
                        string dropItemID = dropList[i].Split(',')[0];
                        string dropItemNum = dropList[i].Split(',')[1];
                        string hideID = dropList[i].Split(',')[2];

                        //实例化道具
                        GameObject chouKaItemObj = (GameObject)Instantiate(ChouKaItemObj);
                        chouKaItemObj.transform.SetParent(ChouKaItemSet.transform);
                        chouKaItemObj.transform.localPosition = new Vector3(show_X, show_Y, 0);
                        chouKaItemObj.transform.localScale = new Vector3(1, 1, 1);
                        chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemID = dropItemID;
                        chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = dropItemNum;
                        chouKaItemObj.GetComponent<UI_ChouKaItemObj>().HindID = hideID;
                        show_X = show_X + 150;

                        //每5个切一行
                        if (showNumHang >= 5)
                        {
                            show_X = -300;
                            showNumHang = 0;
                            show_Y = show_Y - 150;
                        }
                    }
                }
            }
            else { 

                //设置单抽
                ChouKaImgObj.SetActive(false);
                ChouKaTitleObj.SetActive(true);
                chouKaShowItemSet.SetActive(false);
                ChouKaTitleObj.transform.localPosition = new Vector3(0,150,0);
                //单抽
                string dropItemID = dropList[0].Split(',')[0];
                string dropItemNum = dropList[0].Split(',')[1];
                string hideID = dropList[0].Split(',')[2];

                GameObject chouKaItemObj = (GameObject)Instantiate(ChouKaItemObj);
                chouKaItemObj.transform.SetParent(ChouKaItemSet.transform);
                chouKaItemObj.transform.localPosition = new Vector3(0, 0, 0);
                chouKaItemObj.transform.localScale = new Vector3(1, 1, 1);
                chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemID = dropItemID;
                chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = dropItemNum;
                chouKaItemObj.GetComponent<UI_ChouKaItemObj>().HindID = hideID;
            }
        }
        //监测抽卡状态开启,读取抽卡字符串进行显示
    }

    void OnDestroy() {
        Game_PublicClassVar.Get_game_PositionVar.ChouKaUIOpenStatus = false;
    }

    void showOtherTime() {
        chouKaTime_One = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_One", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
        chouKaTime_Ten = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_Ten", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));

        chouKaTime_One = 86400 - chouKaTime_One;
        chouKaTime_Ten = 259200 - chouKaTime_Ten;

        if (chouKaTime_One >= 1)
        {
            //显示倒计时
            int hour = (int)(chouKaTime_One / 3600);
            int hour_value = (int)(chouKaTime_One % 3600);
            int minute = (int)(hour_value / 60);
            int second = (int)(hour_value % 60);
            Obj_ChouKaTime_One.GetComponent<Text>().text = hour + ":" + minute + ":" + second + "秒后免费领取";
            chouKaTime_OneStatus = false;
        }
        else { 
            //显示领取
            Obj_ChouKaTime_One.GetComponent<Text>().text = "当前可免费领取";
            chouKaTime_OneStatus = true;
        }

        if (chouKaTime_Ten >= 1)
        {
            //显示倒计时
            int hour = (int)(chouKaTime_Ten / 3600);
            int hour_value = (int)(chouKaTime_Ten % 3600);
            int minute = (int)(hour_value / 60);
            int second = (int)(hour_value % 60);
            Obj_ChouKaTime_Ten.GetComponent<Text>().text = hour + ":" + minute + ":" + second + "秒后免费领取";
            chouKaTime_TenStatus = false;
        }
        else
        {
            //显示领取
            Obj_ChouKaTime_Ten.GetComponent<Text>().text = "当前可免费领取";
            chouKaTime_TenStatus = true;
        }

    }

    public void Btn_ChouKa() {

        //监测背包
        int bagNum = Game_PublicClassVar.Get_function_UI.BagSpaceNullNum();
        if (bagNum < 1) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包已满,请预留至少1个位置！");
            return;
        }

        //开启Var内的抽卡状态
        //Debug.Log("点击抽卡按钮");
        Game_PublicClassVar.Get_game_PositionVar.ChouKaStr = "";
        bool chouKaStatus = false;
        bool chouKaStatus_MianFei = false;
        //监测免费次数
        if (chouKaTime_OneStatus)
        {
            chouKaStatus = true;
            chouKaTime_OneStatus = false;
            //清空倒计时
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
            chouKaStatus_MianFei = true;
        }

        //监测钻石
        if (!chouKaStatus) {
            int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
            if (roseRmb >= int.Parse(zuanShiNum))
            {
                chouKaStatus = true;
            }
            else {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足！");
            }
        }

        if (chouKaStatus) {
            //扣除钻石_免费不扣钻石
            if (!chouKaStatus_MianFei) {
                Game_PublicClassVar.Get_function_Rose.CostReward("2", zuanShiNum);
            }
            
            //抽卡
            chouKa(1);
            //清空显示
            for (int i = 0; i < ChouKaItemSet.transform.childCount; i++)
            {
                GameObject go = ChouKaItemSet.transform.GetChild(i).gameObject;
                Destroy(go);
            }
        }

    }

    public void Btn_ChouKaTen()
    {
        //监测背包
        int bagNum = Game_PublicClassVar.Get_function_UI.BagSpaceNullNum();
        if (bagNum < 10)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包已满,请预留至少10个位置！");
            return;
        }

        //开启Var内的抽卡状态
        //Debug.Log("点击抽卡按钮");
        Game_PublicClassVar.Get_game_PositionVar.ChouKaStr = "";

        bool chouKaStatus = false;
        bool chouKaStatus_MianFei = false;
        //监测免费次数
        if (chouKaTime_TenStatus)
        {
            chouKaStatus = true;
            chouKaTime_TenStatus = false;
            //清空倒计时
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
            chouKaStatus_MianFei = true;
        }

        //监测钻石
        if (!chouKaStatus) {
            int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
            if (roseRmb >= int.Parse(zuanShiNum_Ten))
            {
                chouKaStatus = true;
            }
            else
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足！");
            }
        }

        if (chouKaStatus) {
            //扣除钻石_免费不扣钻石
            if (!chouKaStatus_MianFei) {
                Game_PublicClassVar.Get_function_Rose.CostReward("2", zuanShiNum_Ten);
            }
            //抽卡
            chouKa(10);
            //清空显示
            for (int i = 0; i < ChouKaItemSet.transform.childCount; i++)
            {
                GameObject go = ChouKaItemSet.transform.GetChild(i).gameObject;
                Destroy(go);
            }
        }
    }

    void chouKa(int chouKaNum) {
        //掉落ID
        for (int i = 1; i <= chouKaNum; i++)
        {
            Game_PublicClassVar.Get_function_AI.DropIDToDropItem(dropID, Vector3.zero,"79999999");
        }
        //写入每日任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "1", chouKaNum.ToString());
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("3", "3", chouKaNum.ToString());
    }

    public void Btn_CloseUI(){
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Destroy(this.gameObject);
    }
}