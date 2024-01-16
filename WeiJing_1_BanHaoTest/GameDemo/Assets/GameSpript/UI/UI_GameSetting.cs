using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_GameSetting : MonoBehaviour {

    public GameObject Obj_RoseName;
    public GameObject Obj_RoseNameText;
    public GameObject Obj_RoseMusicText;
    public GameObject Obj_Bgm;
    public GameObject Obj_ZhangHaoIDStr;
    public GameObject Obj_YaoGanBtnText;
    public GameObject Obj_ShiJianChuo;
    public GameObject Obj_BanBenText;
    public GameObject Obj_RmbValue;
    public GameObject Obj_ServerGameVersion;


	// Use this for initialization
	void Start () {
	    //获取游戏名称并显示
        string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_RoseNameText.GetComponent<Text>().text = roseName;
        //显示声音状态
        if (Game_PublicClassVar.Get_game_PositionVar.GameSourceValue == 1)
        {
            Obj_RoseMusicText.GetComponent<Text>().text = "关";
        }
        else {
            Obj_RoseMusicText.GetComponent<Text>().text = "开";
        }

        //摇杆状态  1：表示开  0:表示关
        /*
        string yaoganStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YaoGanStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (yaoganStatus == "1")
        {
            Obj_YaoGanBtnText.GetComponent<Text>().text = "关";
        }
        else
        {
            Obj_YaoGanBtnText.GetComponent<Text>().text = "开";
        }
        */
        //显示账号ID
        ZhangHaoID_Show();

        //显示时间
        //string shiJianChuo = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffGameTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //DateTime DataTime = Game_PublicClassVar.Get_wwwSet.GetTime(shiJianChuo);

        //记录时间LastOffGameTime
        if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
        {
            Obj_ShiJianChuo.GetComponent<Text>().text = "上次离线时间:" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Month + "月" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Day + "日" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Hour + "时" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Minute + "分" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Second + "秒";
        }
        else {
            Obj_ShiJianChuo.GetComponent<Text>().text = "未连接网络！";
        }

        //显示版本号
        Obj_BanBenText.GetComponent<Text>().text = "当前游戏版本:" + Application.version;
        if (Game_PublicClassVar.Get_wwwSet.GameServerVersionStr != "")
        {
            if (Application.version != Game_PublicClassVar.Get_wwwSet.GameServerVersionStr)
            {
                Obj_ServerGameVersion.GetComponent<Text>().text = "最新游戏版本:" + Game_PublicClassVar.Get_wwwSet.GameServerVersionStr;
            }
            else {
                Obj_ServerGameVersion.GetComponent<Text>().text = "当前版本为最新版本";
            }

        }
        else {
            Obj_ServerGameVersion.GetComponent<Text>().text = "未连接服务器获取最新版本";
        }

        string payValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_RmbValue.GetComponent<Text>().text = payValue;
        
	}
	
	// Update is called once per frame
	void Update () {
	    //this.GetComponent<>
        
	}

    //开关音乐
    public void Btn_GameMusic() {
        if (Game_PublicClassVar.Get_game_PositionVar.GameSourceValue == 1)
        {
            Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 0;
            Obj_RoseMusicText.GetComponent<Text>().text = "开";
            //设置背景音乐音量为空
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingBGM.GetComponent<AudioSource>().volume = 0;
            //存储声音状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SourceSize", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
        else {
            Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
            Obj_RoseMusicText.GetComponent<Text>().text = "关";
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingBGM.GetComponent<AudioSource>().volume = Game_PublicClassVar.Get_game_PositionVar.GameSourceValue;
            //存储声音状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SourceSize", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
    }

    //开关摇杆
    public void Btn_YaoGanStatus()
    {
        //摇杆状态  1：表示开  0:表示关
        string yaoganStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YaoGanStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (yaoganStatus == "1")
        {
            //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 0;
            Obj_YaoGanBtnText.GetComponent<Text>().text = "开";
            Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = false;
            //存储状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            Game_PublicClassVar.Get_function_UI.GameHint("摇杆操作已关闭");
        }
        else
        {
            //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
            Obj_YaoGanBtnText.GetComponent<Text>().text = "关";
            Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(true);
            Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
            //存储状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            Game_PublicClassVar.Get_function_UI.GameHint("摇杆操作已开启");
        }
    }


    //开关摇杆
    public void Btn_YaoGanStatus_0()
    {
        //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
        Obj_YaoGanBtnText.GetComponent<Text>().text = "开";
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = false;
        //存储状态
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        Game_PublicClassVar.Get_function_UI.GameHint("点击移动模式开启");
    }

    //开关摇杆
    public void Btn_YaoGanStatus_1()
    {
        //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
        Obj_YaoGanBtnText.GetComponent<Text>().text = "关";
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
        //存储状态
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        Game_PublicClassVar.Get_function_UI.GameHint("摇杆(移动)操作已开启");
    }

    //开关摇杆
    public void Btn_YaoGanStatus_2()
    {
        //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
        Obj_YaoGanBtnText.GetComponent<Text>().text = "关";
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
        //存储状态
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        Game_PublicClassVar.Get_function_UI.GameHint("摇杆(固定)操作已开启");
    }


    public void Btn_GameName() {
        string roseName = Obj_RoseName.GetComponent<InputField>().text;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Name",roseName, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        //修改主界面显示的名称
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Obj_RoseName.GetComponent<Text>().text = roseName;
        Game_PublicClassVar.Get_function_UI.GameHint("你的昵称已修改为：" + roseName);
        //存储角色通用数据
        Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Name", roseName);
    }

    /*
    //复制账号ID
    public void ZhangHaoID_Copy() {
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_ZhangHaoIDStr.GetComponent<Text>().text = zhanghaoID;

    }
    */
    //显示账号ID
    public void ZhangHaoID_Show()
    {
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_ZhangHaoIDStr.GetComponent<Text>().text = zhanghaoID;
    }

    public void Btn_CloseUI() {
        Destroy(this.gameObject);
    }

    //切换游戏难度（1.普通  2.挑战 3.地狱）
    public void ChangGameNanDu(string nanduType) {

        switch (nanduType) { 
            //切换普通
            case "1":
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_4", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_UI.GameGirdHint("切换普通模式成功!");
                Game_PublicClassVar.Get_game_PositionVar.GameNanduValue = "1";
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                break;
            //切换挑战
            case "2":
                if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= 15)
                {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_4", "2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint("切换挑战模式成功！Boss属性增强,更有大概率掉落道具！");
                    Game_PublicClassVar.Get_game_PositionVar.GameNanduValue = "2";
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }
                else {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint("等级提升至15级后开启挑战模式！");
                }

                break;
            //切换地狱
            case "3":
                if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= 25)
                {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_4", "3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint("切换地狱模式成功！Boss属性大大增强,更有超大概率掉落道具！");
                    Game_PublicClassVar.Get_game_PositionVar.GameNanduValue = "3";
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }
                else {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint("等级提升至25级后开启地狱模式！");
                }
                break;
        }

        

    }

    

    //关闭游戏
    public void Btn_CloseGame() {
        Application.Quit();
    }


    //返回角色
    public void Btn_ReturnRose() {

        //PlayerPrefs.SetInt("ifReturnRoseStatus", 1);

        for (int i = 0; i < Game_PublicClassVar.Get_game_PositionVar.Obj_Keep.Length; i++) {
            Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Keep[i]);
        }
        Destroy(Game_PublicClassVar.Get_wwwSet.gameObject);
        SceneManager.LoadScene("StartGame");        //加载场景


    }

    public void Btn_UpdateGame() {
    
#if UNITY_ANDROID
        Application.OpenURL("http://l.taptap.com/Hys7qS2P");
#endif
#if UNITY_IPHONE
        Application.OpenURL("https://itunes.apple.com/cn/app/id1273563125");
#endif

    }
}
