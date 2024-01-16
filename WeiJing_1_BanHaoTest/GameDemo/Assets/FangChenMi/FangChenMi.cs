using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FangChenMi : MonoBehaviour {
    public GameObject Obj_Name;
    public GameObject Obj_ShenFenID;
    public GameObject Obj_HintText;

	// Use this for initialization
	void Start () {
        
        /*
        PlayerPrefs.SetInt("FangChenMi_Type", 0);
        PlayerPrefs.SetString("FangChenMi_Name", "");
        PlayerPrefs.SetString("FangChenMi_ID", "");
        */
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void YanZheng() {

        if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type != 0) {
            Obj_HintText.GetComponent<Text>().text = "您的身份已经认证,无法再次认证！";
            return;
        }

        string roseName = Obj_Name.GetComponent<InputField>().text;
        string roseShenFenID = Obj_ShenFenID.GetComponent<InputField>().text;

        //验证名称
        bool yanZhengName = false;
        if (roseName == "") {
            Debug.Log("名字为空!");
            return;
        }
        //CheckIDCard(roseShenFenID)
        //验证身份证号
        if (this.GetComponent<YanZheng2>().YanZhengShenFen(roseName, roseShenFenID)) {

            Debug.Log("身份验证通过！");
            PlayerPrefs.SetInt("FangChenMi_Type", 1);
            PlayerPrefs.SetString("FangChenMi_Name", roseName);
            PlayerPrefs.SetString("FangChenMi_ID", roseShenFenID);
            

            //获取身份证年龄
            int year = int.Parse(roseShenFenID.Substring(6, 4));
            int month = int.Parse(roseShenFenID.Substring(10, 2));
            int day = int.Parse(roseShenFenID.Substring(12, 2));
            //身份证为15的开启验证
            if (roseShenFenID.Length == 15) {
                year = 18;
            }

            DateTime t1 = new DateTime(year, month, day);
            DateTime t2 = DateTime.Now;
            /*
            if (Game_PublicClassVar.gameLinkServer.ServerLinkTimeStamp != "") {
                t2 = Game_PublicClassVar.Get_wwwSet.GetTime(Game_PublicClassVar.gameLinkServer.ServerLinkTimeStamp);
            }
            */
            
            if (Game_PublicClassVar.Get_wwwSet.DataTime != null)
            {
                t2 = Game_PublicClassVar.Get_wwwSet.DataTime;
            }
            

            TimeSpan chaSpan = t2 - t1;
            int chayear = (int)(chaSpan.Days / 365);

            PlayerPrefs.SetInt("FangChenMi_Year", chayear);
            Game_PublicClassVar.Get_wwwSet.FangChengMi_Type = 1;
            Game_PublicClassVar.Get_wwwSet.FangChengMi_Year = chayear;
            if (chayear >= 18) {
                Game_PublicClassVar.Get_wwwSet.FangChengMi_Type = 2;
                PlayerPrefs.SetInt("FangChenMi_Type", 2);
            }

            CloseUI();
        }
        else
        {
            Debug.Log("身份证错误!");
            Obj_HintText.GetComponent<Text>().text = "身份证信息验证失败,请重新确认！";
        }
    }

    public void CloseUI() {

        this.gameObject.SetActive(false);

        if (Application.loadedLevelName != "StartGame") {

            if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 0)
            {
                //验证防沉迷
                string nowTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_10", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                nowTime = PlayerPrefs.GetInt("FangChenMi_Time").ToString();
                if (nowTime == "")
                {
                    nowTime = "0";
                }
                int nowTimeSum = int.Parse(nowTime);
                if (nowTimeSum >= 60)
                {
                    Application.Quit();
                }
            }

            if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 1) {
                //验证防沉迷
                string nowTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_10", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                nowTime = PlayerPrefs.GetInt("FangChenMi_Time").ToString();
                if (nowTime == "")
                {
                    nowTime = "0";
                }
                int nowTimeSum = int.Parse(nowTime);
                if (nowTimeSum >= 90)
                {
                    Application.Quit();
                }
            }

            //关闭此界面
            Destroy(this.gameObject);
        }
    }

    public bool CheckIDCard(string Id)
    {
        if (Id.Length == 18)
        {
            bool check = CheckIDCard18(Id);
            return check;
        }
        else if (Id.Length == 15)
        {
            bool check = CheckIDCard15(Id);
            return check;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIDCard18(string Id)
    {
        long n = 0;
        if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
        {
            return false;//数字验证
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(Id.Remove(2)) == -1)
        {
            return false;//省份验证
        }

        string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
        {
            return false;//生日验证
        }

        string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
        string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
        char[] Ai = Id.Remove(17).ToCharArray();
        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
        }
        int y = -1;
        DivRem(sum, 11, out y);
        if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
        {
            return false;//校验码验证
        }
        return true;//符合GB11643-1999标准
    }

    public int DivRem(int a, int b, out int result)
    {
        result = a % b;
        return (a / b);
    }

    public bool CheckIDCard15(string Id)
    {
        long n = 0;
        if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
        {
            return false;//数字验证
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(Id.Remove(2)) == -1)
        {
            return false;//省份验证
        }
        string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
        {
            return false;//生日验证
        }
        return true;//符合15位身份证标准
    }
}
