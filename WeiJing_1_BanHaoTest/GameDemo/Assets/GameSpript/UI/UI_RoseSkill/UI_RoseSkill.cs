using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RoseSkill : MonoBehaviour {

	public string NpcID;					//NpcID
	public GameObject Obj_SkillList;		//商品的Obj
	public GameObject Obj_SkillListNum;		//商品页数的Obj
	public Transform Tra_SkillListSet;
	private string SkillItemListText;		//商品ID的整个Text
    private string AddSkillItemListText;	//商品ID的整个Text,有装备给此技能+等级的
	private string[] SkillItemList;			//商品ID数组
    private string[] AddSkillItemList;			//商品ID数组
	private float SkillListPosition_X;		//商品显示的X坐标
	private float SkillListPosition_Y;		//商品显示的Y坐标
	private int nowSkillListNum;			//当前商品页数
	private int sumSkillListNum;			//商品总页数
    public GameObject Obj_SpValue;          //SP的值

	private int creatNum;					//单页里面存在几个技能
	
	// Use this for initialization
	void Start () {
		creatNum = 5;
        UpdataSkillData();  //更新技能数据


		//创建第1页显示商品
		nowSkillListNum = 1;
		createSkillList (nowSkillListNum);
		//显示商品页数
		if (SkillItemList.Length <= creatNum) {
			sumSkillListNum = (int)SkillItemList.Length / creatNum;
		} else {
			sumSkillListNum = (int)SkillItemList.Length / creatNum+1;
		}
		
		Obj_SkillListNum.GetComponent<Text> ().text = "1/" + sumSkillListNum.ToString();
	}
	
	// Update is called once per frame
	void Update () {

	}
	//循环创建对应页数的子商品
	void createSkillList(int storeNum){

        UpdataSkillData();  //更新技能数据
        //显示当前SP值
        string spvalue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_SpValue.GetComponent<Text>().text = "技能点：" + spvalue;

		//循环删除子控件
		for(int i = 0;i<Tra_SkillListSet.transform.childCount;i++)
		{
			GameObject go = Tra_SkillListSet.transform.GetChild(i).gameObject;
			Destroy(go);
		}
		
		int rowSum = 0;
		int createSum = 0;
		SkillListPosition_X = -180.0f;
		SkillListPosition_Y = 220.0f;
		//循环创建
		for(int i = (storeNum-1)*creatNum;i<=SkillItemList.Length-1;i++){
			if(SkillItemList[i]!=""){
				rowSum = rowSum +1;
				
				//实例化技能栏
				GameObject obj_skillList = (GameObject)MonoBehaviour.Instantiate(Obj_SkillList);
				obj_skillList.transform.SetParent(Tra_SkillListSet);
				obj_skillList.transform.localPosition = new Vector3(SkillListPosition_X, SkillListPosition_Y, 0);
				obj_skillList.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				obj_skillList.GetComponent<UI_RoseSkillList>().SkillID = SkillItemList[i];
                obj_skillList.GetComponent<UI_RoseSkillList>().SkillAddID = AddSkillItemList[i];
                obj_skillList.GetComponent<UI_RoseSkillList>().Obj_SkillParent = this.gameObject;
				SkillListPosition_Y = SkillListPosition_Y -100.0f;
				SkillListPosition_X = -180.0f;
			}
			
			//循环创建12个，满足12个立即跳出当前循环
			createSum = createSum + 1;
			if(createSum>=creatNum){
				i = SkillItemList.Length;	//立即跳出循环
            }
		}
	}
	
	//显示上一页
	public void SkillList_Up(){
		if (nowSkillListNum > 1) {
			nowSkillListNum = nowSkillListNum - 1;
			createSkillList(nowSkillListNum);
			Obj_SkillListNum.GetComponent<Text> ().text = nowSkillListNum + "/" + sumSkillListNum.ToString();
		}
	}
	
	//显示下一页
	public void SkillList_Down(){
		
		if (nowSkillListNum < sumSkillListNum){
			nowSkillListNum = nowSkillListNum + 1;
			createSkillList(nowSkillListNum);
			Obj_SkillListNum.GetComponent<Text> ().text = nowSkillListNum + "/" + sumSkillListNum.ToString();
		}
	}

    //更新技能数据
    public void UpdataSkillData() {
        //获取本身技能和装备技能合并
        SkillItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        AddSkillItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //获取技能
        string addList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (addList != "")
        {
            SkillItemListText = SkillItemListText + "," + addList;
            AddSkillItemListText = AddSkillItemListText + "," + addList;
        }
        //如果没有自身技能就只获取装备技能
        if (SkillItemListText == "")
        {
            SkillItemListText = addList;
            AddSkillItemListText = addList;
        }

        SkillItemList = SkillItemListText.Split(',');
        AddSkillItemList = AddSkillItemListText.Split(',');
    }

    public void Btn_SkillReSPCommont() {
        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否花费800钻石重置技能点数？", Btn_SkillReSP, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
    }

    //技能重置
    public void Btn_SkillReSP() {

        Debug.Log("我点击了技能重置按钮");
        
        //消耗800钻石
        if (Game_PublicClassVar.Get_function_Rose.CostReward("2", "800"))
        {
            //重置SP
            Game_PublicClassVar.Get_function_Skill.RoseReSkillSP();
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("钻石不足!");
        }
    }

	public void CloseUI(){
		Destroy (this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseSkill_Status = false;
	}
}
