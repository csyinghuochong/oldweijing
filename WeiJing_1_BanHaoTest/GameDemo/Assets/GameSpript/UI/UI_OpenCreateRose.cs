using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_OpenCreateRose : MonoBehaviour {

    public GameObject Obj_CreateRose;
    public GameObject Obj_LianJieObj;
    public GameObject Obj_FangChenMi;
    public GameObject Obj_FangChenMiHint;
    // Use this for initialization
    void Start () {

        int ifreturn = PlayerPrefs.GetInt("ifReturnRoseStatus");
        if (ifreturn == 1) {
            Open_CreateRose();
            PlayerPrefs.SetInt("ifReturnRoseStatus", 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Game_PublicClassVar.Get_wwwSet.TryWorldTimeStatus)
        {
            Obj_LianJieObj.GetComponent<Text>().text = "请点击开启冒险！";
            //Obj_LianJieObj.SetActive(false);
        }

        if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 0)
        {
            Obj_FangChenMiHint.SetActive(true);
        }
        /*
        else
        {

            Obj_FangChenMiHint.SetActive(false);
        }
        */
        //认证结果
        if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 1)
        {
            Obj_FangChenMiHint.GetComponent<Text>().text = "认证结果:" + "未成年,限制游戏";
        }
        if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 2)
        {
            Obj_FangChenMiHint.GetComponent<Text>().text = "认证结果:" + "已成年,可正常游戏";
        }
    }

    public void Open_CreateRose() {

        //判断当前是否进行防沉迷验证
        if (Game_PublicClassVar.Get_wwwSet.AgeRange < 12) {

            //禁止登录游戏
            Hint2("您目前为未成年人账号，已被纳入防沉迷系统。根据适龄提示，此时段本游戏将无法为未成年人用户提供任何形式的游戏服务。");
            return;

        }

        //判断当前是否进行防沉迷验证
        if (Game_PublicClassVar.Get_wwwSet.AgeRange <= 17)
        {
            if (Game_PublicClassVar.Get_wwwSet.RemainingTime <= 1) {
                Hint2("您目前为未成年人账号，已被纳入防沉迷系统。根据国家新闻出版署《关于进一步严格管理切实防止未成年人沉迷网络游戏的通知》，仅每周五、周六、周日和法定节假日每日20时至21时提供1小时网络游戏服务。"); 
            }
            else {
                int time = (int)(Game_PublicClassVar.Get_wwwSet.RemainingTime / 60);
                Hint("您目前为未成年人账号，已被纳入防沉迷系统。根据国家新闻出版署《关于进一步严格管理切实防止未成年人沉迷网络游戏的通知》，仅每周五、周六、周日和法定节假日每日20时至21时提供1小时网络游戏服务。您今日游戏剩余时间"+ time.ToString()+ "分钟。");
            }
            return;
        }



        //判定当前读取时间是否正常
        if (!Game_PublicClassVar.Get_wwwSet.TryWorldTimeStatus) {
            Debug.Log("请稍后再试");
            return;
        }


        Obj_CreateRose.SetActive(true);
        GameObject.Find("Canvas/XieYiText").gameObject.SetActive(false);

        /*

        if (Game_PublicClassVar.Get_wwwSet.CreateRoseDataNum < 2)
        {
            Debug.Log("角色GameConfig未生成完毕,请稍后再次点击");
        }
        else {
            if (Obj_CreateRose != null)
            {

                if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 0)
                {

                    string hintText = "您当前未完成防沉迷验证！\n如果您点击进入游戏则角色进行防沉迷限定,游戏时间不得超过1小时,且不能进行任何付费行为！";
                    GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint_2);
                    uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(hintText, entergame, FangChenMi, "实名认证提示", "进入游戏", "开始认证");
                    uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
                    uiCommonHint.transform.localPosition = Vector3.zero;
                    uiCommonHint.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    return;
                }

                Obj_CreateRose.SetActive(true);
                GameObject.Find("Canvas/XieYiText").gameObject.SetActive(false);
            }
        }
        */
        
    }


    public void Hint(string textcont) {

        string hintText = textcont;
        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint_2);
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(hintText, entergame, entergame, "提示信息", "进入游戏", "进入游戏");
        uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
        uiCommonHint.transform.localPosition = Vector3.zero; 
        uiCommonHint.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

    }

    public void Hint2(string textcont)
    {

        string hintText = textcont;
        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint_2);
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(hintText, null, null, "提示信息", "我已了解", "我已了解");
        uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

    }



    public void entergame()
    {
        Obj_CreateRose.SetActive(true);
        GameObject.Find("Canvas/XieYiText").gameObject.SetActive(false);
    }

    public void FangChenMi()
    {
        Obj_FangChenMi.SetActive(true);
    }
}
