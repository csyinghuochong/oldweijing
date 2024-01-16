using UnityEngine;
using System.Collections;

public class UI_EnterPVESet : MonoBehaviour {

    public GameObject UI_ZhangJieObj;
    public GameObject UI_ZhangJieSet;
    public int ChapterNum;
    public bool UpdataZhangJieStatus;
    public GameObject UI_ChapterSonSet;
    public string UIPVEStatus;                  //0:章节界面  1：关卡界面
    private string[] roseData_PveChapter;       //玩家关卡保存数据

	// Use this for initialization
	void Start () {
        UIPVEStatus = "0";
        updataZhangJie();
	}
	
	// Update is called once per frame
	void Update () {

        if (UpdataZhangJieStatus) {
            updataZhangJie();
            UpdataZhangJieStatus = false;
        }
	}

    void updataZhangJie() {

        roseData_PveChapter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PVEChapter", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');

        //判定删除子集下的物体
        for (int i = 0; i < UI_ZhangJieSet.transform.childCount; i++)
        {
            GameObject go = UI_ZhangJieSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //循环实例化
        for (int i = 1; i <= ChapterNum; i++)
        {
            //实例化章节OBJ
            GameObject obj = (GameObject)Instantiate(UI_ZhangJieObj);
            obj.transform.SetParent(UI_ZhangJieSet.transform);
            obj.GetComponent<UI_ZhangJie>().ZhangJiePosition = new Vector3(0, 125.0f + (i - 1) * -125.0f, 0);
            obj.GetComponent<UI_ZhangJie>().WaitTime = 0.125f * i;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_ZhangJie>().ZhangJieID = i.ToString();
            obj.GetComponent<UI_ZhangJie>().ZhangJieSet = UI_ZhangJieSet;
            obj.GetComponent<UI_ZhangJie>().Obj_ChapterSonSet = UI_ChapterSonSet;
            obj.GetComponent<UI_ZhangJie>().Obj_EnterPveSet = this.gameObject;
            //判定玩家当前存档是否打到此关卡
            if (int.Parse(roseData_PveChapter[0]) >= i)
            {
                obj.GetComponent<UI_ZhangJie>().IfOpen = true;
            }
            else {
                obj.GetComponent<UI_ZhangJie>().IfOpen = false;
            }
        }
    }

    public void Btn_Close() {
        //隐藏怪物模型
        Game_PublicClassVar.Get_game_PositionVar.Obj_MonsterModelSheXiangJi.SetActive(true);
        switch (UIPVEStatus) { 
            case "0":
                this.gameObject.SetActive(false);
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().ifHideMainUI = false;
                //判定删除子集下的物体
                for (int i = 0; i < UI_ZhangJieSet.transform.childCount; i++)
                {
                    GameObject go = UI_ZhangJieSet.transform.GetChild(i).gameObject;
                    Destroy(go);
                }
                break;

            case "1":
                UpdataZhangJieStatus = true;
                //判定删除子集下的物体
                for (int i = 0; i < UI_ChapterSonSet.GetComponent<UI_ChapterSonSet>().Obj_EnterPveSet.transform.childCount; i++)
                {
                    GameObject go = UI_ChapterSonSet.GetComponent<UI_ChapterSonSet>().Obj_EnterPveSet.transform.GetChild(i).gameObject;
                    Destroy(go);
                }
                UIPVEStatus = "0";
                UI_ChapterSonSet.SetActive(false);
                break;
        }
    }
}
