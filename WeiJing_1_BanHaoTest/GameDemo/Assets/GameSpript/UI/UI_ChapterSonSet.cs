using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ChapterSonSet : MonoBehaviour {

    public GameObject Obj_EnterPve;
    public GameObject Obj_EnterPveSet;
    public string ChapterSonStr;    
    private int ChapterSonNum;
    public bool UpdataZhangJieStatus;
    private string[] chapterSonArrary;
    public int RoseData_ChapterSon;             //玩家子章节存储
    public bool AllChapterSonStatus;

    public string ChapterNum;
    public GameObject ShowMonsterNameObj;
    public GameObject ShowMonsterImageObj;
    public string ShowMonsterName;

    //public GameObject Obj_UIEnterPveSet;

	// Use this for initialization
	void Start () {

        chapterSonArrary = ChapterSonStr.Split(';');
        ChapterSonNum = chapterSonArrary.Length;
        updataEnterPveSet();
	}
	
	// Update is called once per frame
	void Update () {
        if (UpdataZhangJieStatus) {
            UpdataZhangJieStatus = false;
            updataEnterPveSet();
        }
	}

    //更新
    void updataEnterPveSet() {

        chapterSonArrary = ChapterSonStr.Split(';');
        ChapterSonNum = chapterSonArrary.Length;

        //当记录当前关卡时后面不予显示
        if (AllChapterSonStatus)
        {
            ChapterSonNum = RoseData_ChapterSon;
        }

        //判定删除子集下的物体
        for (int i = 0; i < Obj_EnterPveSet.transform.childCount; i++)
        {
            GameObject go = Obj_EnterPveSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //循环实例化
        for (int i = 0; i <= ChapterSonNum-1; i++)
        {
            //实例化章节OBJ
            GameObject obj = (GameObject)Instantiate(Obj_EnterPve);
            obj.transform.SetParent(Obj_EnterPveSet.transform);
            //obj.transform.localPosition = new Vector3(0f, 9999f, 0f);
            obj.GetComponent<UI_EnterPVE>().ChapterSonPosition = new Vector3(330, 200.0f + i* -105.0f, 0);
            obj.GetComponent<UI_EnterPVE>().WaitTime = 0.125f * i;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_EnterPVE>().ChapterSonID = chapterSonArrary[i];
            obj.GetComponent<UI_EnterPVE>().IfOpen = true;
            //Debug.Log("RoseData_ChapterSon = " + RoseData_ChapterSon + ", i = "+i);

        }

        //显示怪物模型信息
        switch (ChapterNum)
        {
            case "1":
                ShowMonsterName = "骷髅王-卡德拉";
                break;
            case "2":
                ShowMonsterName = "地狱领主-里奥迪亚";
                break;
            case "3":
                ShowMonsterName = "冰封魔王-阿兹里斯";
                break;
            case "4":
                ShowMonsterName = "裂石领主-艾力克斯";
                break;
            case "5":
                ShowMonsterName = "黑暗魔王-卡利兹";
                break;
        }
        //Debug.Log("ShowMonsterName = " + ShowMonsterName);
        ShowMonsterNameObj.GetComponent<Text>().text = ShowMonsterName;
        object obj2 = Resources.Load("CameraText/MonsterMolde_" + ChapterNum, typeof(Texture));
        Texture monsterImage = obj2 as Texture;
        //Obj_EquipQuality.GetComponent<Image>().sprite = itemQuality;
        ShowMonsterImageObj.GetComponent<RawImage>().texture = monsterImage;
    }
}
