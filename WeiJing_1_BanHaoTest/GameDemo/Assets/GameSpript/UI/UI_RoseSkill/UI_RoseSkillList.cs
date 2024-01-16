using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RoseSkillList : MonoBehaviour {

	public string SkillID;
    public string SkillAddID;
	public GameObject Obj_SkillIcon;
	public GameObject Obj_SkillName;
	public GameObject Obj_SkillDes;
    public GameObject Obj_SkillLv;
    public GameObject Obj_SkillSP;
    public GameObject Obj_SkillUp;
    public GameObject Obj_SkillUpText;
	private int ItmePrice;
	private GameObject obj_SkillTips;
    private GameObject moveIconObj;
    private string skillIcon;               //技能Icon
    private Sprite skillIconSpr;            //技能图标精灵
    private string skillLv;
    private string learnSkillLv;            //技能学习需要等级
    public GameObject Obj_SkillParent;          //父级的Obj

	// Use this for initialization
	void Start () {
        
        //Debug.Log("SkillID = " + SkillID);
        showSkillListData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //初始化技能数据
    void showSkillListData() {

        //获取的Icon
        skillIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillIcon", "ID", SkillID, "Skill_Template");
        string SkillName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillName", "ID", SkillID, "Skill_Template");
        string skillDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillDescribe", "ID", SkillID, "Skill_Template");
        skillLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLv", "ID", SkillID, "Skill_Template");
        learnSkillLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnRoseLv", "ID", SkillID, "Skill_Template"); 
        string nextSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextSkillID", "ID", SkillID, "Skill_Template");
        string skillSP = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostSPValue", "ID", SkillID, "Skill_Template");

        int lvChazhi = 0;
        if (SkillAddID != "" && SkillAddID != "0")
        {
            skillDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillDescribe", "ID", SkillAddID, "Skill_Template");
            skillIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillIcon", "ID", SkillAddID, "Skill_Template");
            SkillName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillName", "ID", SkillAddID, "Skill_Template");
            string addskillLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLv", "ID", SkillAddID, "Skill_Template");
            lvChazhi = int.Parse(addskillLv) - int.Parse(skillLv);
            skillLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLv", "ID", SkillID, "Skill_Template");
        }

        //显示Icon
        object obj = Resources.Load("SkillIcon/" + skillIcon, typeof(Sprite));
        skillIconSpr = obj as Sprite;
        Obj_SkillIcon.GetComponent<Image>().sprite = skillIconSpr;
        object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
        Material huiMaterial = huiObj as Material;

        if (skillLv != "0")
        {
            Obj_SkillLv.GetComponent<Text>().text = "等级：" + skillLv;
            Obj_SkillIcon.GetComponent<Image>().material = null;

        }
        else {

            Obj_SkillIcon.GetComponent<Image>().material = huiMaterial;
            Obj_SkillLv.GetComponent<Text>().text = "";
        }
        
        //获取角色等级
        int lv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
        //判定当前学习技能等级是否满足
        if (int.Parse(learnSkillLv) > lv)
        {
            Obj_SkillUp.GetComponent<Image>().material = huiMaterial;
            Obj_SkillUpText.GetComponent<Text>().text = learnSkillLv + "级学习";
            //获得技能学习需要角色等级
            //Obj_SkillLv.GetComponent<Text>().text = "";
        }
        

        //显示技能描述和名称
        Obj_SkillName.GetComponent<Text>().text = SkillName;
        Obj_SkillDes.GetComponent<Text>().text = skillDes;
        if (lvChazhi > 0)
        {
            Obj_SkillName.GetComponent<Text>().text = SkillName + "<color=#00ff00ff>(技能+" + lvChazhi + ")</color>";
        }

        
        //下一级技能为空时,隐藏技能学习按钮和SP值
        if (nextSkillID != "" && nextSkillID == "0")
        {
            Obj_SkillSP.SetActive(false);
            Obj_SkillUp.SetActive(false);
        }
        else {
            Obj_SkillSP.GetComponent<Text>().text = "需要技能点：" + skillSP + "点";
        }
    }

	
	//鼠标按下 显示Tips
	public void Mouse_Down(){
        //判定当前技能是否为0级
        if (skillLv == "0")
        {
            return;
        }
		//调用方法显示UI的Tips
		if (obj_SkillTips == null) {
            obj_SkillTips = Game_PublicClassVar.Get_function_UI.UI_SkillTips(SkillID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
            //Debug.Log("A");
        }
	}
	//鼠标松开注销Tips
	public void Mouse_Up(){
		if (obj_SkillTips != null) {
			Destroy(obj_SkillTips);
            //Debug.Log("B");
		}
	}

    //拖动技能
    public void MouseDrag_Start() {

        //判定当前技能是否为0级
        if (skillLv == "0") {
            return;
        }

        //获取技能是否为主动技能
        bool dragStatus = false;
        string skillType = "";
        if (SkillAddID != "" && SkillAddID != "0"){
            skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", SkillAddID, "Skill_Template");
        }
        else {
            skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", SkillID, "Skill_Template");
        }
        
        if (skillType == "1") {
            dragStatus = true;

            //循环获取快捷栏内的技能检测是否有CD

        }
        //开启技能拖拽状态
        if (dragStatus)
        {
            //拖拽时注销Tips
            if (moveIconObj != null)
            {
                Destroy(moveIconObj);
            }
            //实例化技能图标
            if (skillIcon != "0")
            {
                moveIconObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UIMoveIcon);
                moveIconObj.GetComponent<UI_MoveItemIcon>().itemIconSprite = skillIconSpr;      //传入图标精灵
                moveIconObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet.transform);
                moveIconObj.transform.localScale = new Vector3(1, 1, 1);
            }
            //开启技能移动
            Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus = true;

            if (SkillAddID != "" && SkillAddID != "0")
            {
                //if (SkillAddID>=)
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = SkillAddID;

            }
            else
            {
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = SkillID;
            }
            
        }
        else {
            //Debug.Log("被动技能不能被拖拽");
            Game_PublicClassVar.Get_function_UI.GameHint("被动技能不能被拖拽");
        }

        //注销Tips
        if (obj_SkillTips!=null) {
            Destroy(obj_SkillTips);
        }
    }

    //结束拖拽技能
    public void MouseDrag_End() {
        //判定当前技能是否为0级
        if (skillLv == "0")
        {
            return;
        }
        //执行技能拖拽
        Game_PublicClassVar.Get_function_UI.UI_MoveToMainSkill("1");
        //注销移动的Obj
        if (moveIconObj != null) {
            Destroy(moveIconObj);
        }
    }

    //点击技能升级按钮
    public void Btn_SkillUp() {

        //获取角色等级
        int lv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        //判定当前学习技能等级是否满足
        if (int.Parse(learnSkillLv) > lv)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("升级技能需要玩家等级达到" + learnSkillLv + "级");
            return;
        }

       bool skillupStatus = Game_PublicClassVar.Get_function_Skill.SkillUp(SkillID, "1");
        //bool skillupStatus = true;
       if (skillupStatus)
       {
           Debug.Log("技能升级成功");
           Game_PublicClassVar.Get_function_UI.GameHint("技能升级成功");
           //获取技能下一级ID,并更新相应的UI
           string nextSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextSkillID", "ID", SkillID, "Skill_Template");
           if (nextSkillID!="0") {
               //Debug.Log("SkillID = " + SkillID + "  nextSkillID = " + nextSkillID);
               Game_PublicClassVar.Get_function_Skill.UpdataMainUISkillID(SkillID, nextSkillID);        //替换主界面图标
               SkillID = nextSkillID;
               //SkillAddID = SkillID;
               //更新装备技能（放在这里需要上面先写入套装技能ID后再进行更新）
               Game_PublicClassVar.Get_function_Skill.UpdataEquipSkillID();
               //更新SkillAddID
               string[] SkillItemList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
               string[] AddSkillItemList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
               for (int i = 0; i <= SkillItemList.Length - 1; i++) {
                   if (SkillItemList[i] == SkillID) {
                       SkillAddID = AddSkillItemList[i];
                   }
               }
               //更新技能信息
               showSkillListData();
           }

        //更新主界面SP值
        string spvalue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_SkillParent.GetComponent<UI_RoseSkill>().Obj_SpValue.GetComponent<Text>().text = "技能点：" + spvalue;
       }
       else {
           Debug.Log("技能升级失败");
       }
    }
}
