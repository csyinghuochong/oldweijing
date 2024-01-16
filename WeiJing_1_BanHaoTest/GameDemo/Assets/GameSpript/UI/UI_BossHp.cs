using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_BossHp : MonoBehaviour {

	private string monsterID;
	public int MonsterNowHp;
	private int MonsterHp;
	public GameObject Obj_MonsterName;
	public GameObject Obj_MonsterLv;
	public GameObject Obj_MonsterHp;
	public GameObject Obj_BossHpSet;
	public bool UpdataMonster;
	private GameObject obj_RoseActTarget;
    public bool CloseStatus;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (UpdataMonster) {
			UpdataMonster = false;
			obj_RoseActTarget = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
			monsterID = obj_RoseActTarget.GetComponent<AI_1>().AI_ID.ToString();
			string monsterType = "2";
			if(monsterType =="2"){
				Obj_BossHpSet.SetActive(true);

				string monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", monsterID, "Monster_Template");

                //怪物难度设定
                string monsterTypeBoss = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", monsterID, "Monster_Template");
                if (monsterTypeBoss == "3")
                {
                    switch (Game_PublicClassVar.Get_game_PositionVar.GameNanduValue)
                    {
                        case "1":
                            break;
                        case "2":
                            monsterName = monsterName + "（挑战）";
                            break;
                        case "3":
                            monsterName = monsterName + "（地狱）";
                            break;
                    }
                }

				string monsterLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", monsterID, "Monster_Template");
				MonsterHp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Hp", "ID", monsterID, "Monster_Template"));
				Obj_MonsterName.GetComponent<Text>().text = monsterName;
				Obj_MonsterLv.GetComponent<Text>().text = monsterLv;

			}else{
				Obj_BossHpSet.SetActive(false);
			}

		}
		if (obj_RoseActTarget != null) {
			MonsterNowHp = obj_RoseActTarget.GetComponent<AI_Property>().AI_Hp;
			float value = (float)MonsterNowHp/(float)MonsterHp ;
			Obj_MonsterHp.GetComponent<Image>().fillAmount = value;
		}

        //目标为空关闭UI显示
		if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status> ().Obj_ActTarget == null) {
			Obj_BossHpSet.SetActive(false);
		}

        //开启关闭状态关闭UI显示
        if (CloseStatus) {
            CloseStatus = false;
            Obj_BossHpSet.SetActive(false);
        }
	}
}
