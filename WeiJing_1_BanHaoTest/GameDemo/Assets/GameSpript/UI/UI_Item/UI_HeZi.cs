using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HeZi : MonoBehaviour {

    public string ItemID;
    public string ItemSubType;
    public GameObject Obj_ItemNum;
    public GameObject Obj_ItemIcon;
    public GameObject Obj_ItemQuality;
    public GameObject Obj_BtnZuanshiText;
    public GameObject Obj_Title1;
    public GameObject Obj_Title2;
    public GameObject Obj_Title3;
    private int openZuanShi;
    //经验用的
    private int roseExp;
    private int addRoseExp;

    //金币用的
    private int roseGold;
    private int addRoseGold;
    //private int openZuanShi_Gold;

	// Use this for initialization
	void Start () {
        ItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");

        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        Obj_ItemNum.GetComponent<Text>().text = "数量:" + itemNum;

        //显示道具图标
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        //显示道具Icon
        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;
        //显示品质
        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
        Sprite itemQuality = obj2 as Sprite;
        Obj_ItemQuality.GetComponent<Image>().sprite = itemQuality;
        string makeItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        Obj_Title3.GetComponent<Text>().text = makeItemName;

        switch (ItemSubType) { 
            case "2":
                Obj_Title1.GetComponent<Text>().text = "开启经验木桩可获得大量经验！";
                Obj_Title2.GetComponent<Text>().text = "提示：钻石开启获得更多经验,更有概率触发经验暴击！";
                //显示开启钻石
                openZuanShi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "5", "GameMainValue"));
                Obj_BtnZuanshiText.GetComponent<Text>().text = openZuanShi + "钻开启";
                //更新显示数据
                updataRoseExp();
                break;
            case "3":
                Obj_Title1.GetComponent<Text>().text = "开启金币袋子可获得大量金币！";
                Obj_Title2.GetComponent<Text>().text = "提示：钻石开启获得更多金币,更有概率触发金币暴击！";
                //显示开启钻石
                openZuanShi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "10", "GameMainValue"));
                Obj_BtnZuanshiText.GetComponent<Text>().text = openZuanShi + "钻开启";
                //更新显示数据
                updataRoseGold();
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    //免费开启按钮
    public void Btn_MianFei()
    {

        //监测背包是否有道具
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        if (itemNum <= 0)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("道具不足");
            return;
        }
        else {
            Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
            updataBagItemNum();
        }

        switch (ItemSubType) {
            case "2":
                updataRoseExp();    //更新获得经验
                addRoseExp = (int)(addRoseExp);      //特殊处理,因为改配置麻烦T.T
                Game_PublicClassVar.Get_function_Rose.AddExp(addRoseExp,"1");
                Game_PublicClassVar.Get_function_UI.GameHint("免费开启,获得经验：" + addRoseExp);
            break;

            case "3":
                updataRoseGold();    //更新获得金币
                Game_PublicClassVar.Get_function_Rose.SendReward("1", addRoseGold.ToString());
                Game_PublicClassVar.Get_function_UI.GameHint("免费开启,获得金币：" + addRoseGold);
            break;
        }



    }

    //钻石开启
    public void Btn_ZuanShi()
    {
        //监测背包是否有道具
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        if (itemNum <= 0)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("道具不足");
            return;
        }

        int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        if (roseRmb < openZuanShi)
        {
            Game_PublicClassVar.Get_function_UI.AddRMBHint();   //钻石不足统一提示
            return;
        }
        else
        {
            //扣除钻石
            Game_PublicClassVar.Get_function_Rose.CostReward("2", openZuanShi.ToString());
            //销毁道具
            Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
            updataBagItemNum();         //监测道具是否存在
        }

        switch (ItemSubType) {
            //经验盒子
            case "2":
                updataRoseExp();    //更新获得经验
                float expPro_Zuanshi = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "4", "GameMainValue"));
                //监测是否触发经验暴击
                float expPro_Cir = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "3", "GameMainValue"));
                if (Random.value <= expPro_Cir)
                {
                    addRoseExp = (int)(addRoseExp * expPro_Zuanshi * 2);
                    Game_PublicClassVar.Get_function_UI.GameHint("触发经验暴击！获得大量经验：" + addRoseExp);
                }
                else {
                    addRoseExp = (int)(addRoseExp * expPro_Zuanshi);
                    Game_PublicClassVar.Get_function_UI.GameHint("钻石开启！获得大量经验：" + addRoseExp);
                }

                Game_PublicClassVar.Get_function_Rose.AddExp((int)(addRoseExp),"1");

            break;
            //金币盒子
            case "3":
                updataRoseGold();   //更新获得金币
                float goldPro_Zuanshi = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "9", "GameMainValue"));
                //监测是否触发经验暴击
                float goldPro_Cir = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "8", "GameMainValue"));
                if (Random.value <= goldPro_Cir)
                {
                    addRoseGold = (int)(addRoseGold * goldPro_Zuanshi * 2);
                    Game_PublicClassVar.Get_function_UI.GameHint("触发金币暴击！获得大量金币：" + addRoseGold);
                }
                else {
                    addRoseGold = (int)(addRoseGold * goldPro_Zuanshi);
                    Game_PublicClassVar.Get_function_UI.GameHint("钻石开启！获得大量金币：" + addRoseGold);
                }

                Game_PublicClassVar.Get_function_Rose.SendReward("1", addRoseGold.ToString());

            break;
        }


    }

    //更新背包此道具数据,为0时关闭界面
    public void updataBagItemNum() {
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        if (itemNum <= 0)
        {
            Btn_Close();
        }
        else {
            Obj_ItemNum.GetComponent<Text>().text = "数量:" + itemNum;        //更新数量
        }
    }

    //更新每次获得的经验
    void updataRoseExp() {
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        roseExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpPro", "RoseLv", roseLv.ToString(), "RoseExp_Template"));
        float expPro_Min = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "1", "GameMainValue"));
        float expPro_Max = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "2", "GameMainValue"));
        addRoseExp = (int)(((expPro_Max - expPro_Min) * Random.value + expPro_Min) * roseExp);
    }

    //更新每次获得的金币
    void updataRoseGold()
    {
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        roseGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseGoldPro", "RoseLv", roseLv.ToString(), "RoseExp_Template"));
        float expPro_Min = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "6", "GameMainValue"));
        float expPro_Max = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "7", "GameMainValue"));
        addRoseGold = (int)(((expPro_Max - expPro_Min) * Random.value + expPro_Min) * roseGold);
    }

    public void Btn_Close() {
        Destroy(this.gameObject);
    }


}
