using UnityEngine;
using System.Collections;

//主要用于打开GameObje的技能开关和传递对应参数,和SkillObject的区别是绑定在技能开始脚本和触发技能脚本
public class SkillObjBase : MonoBehaviour {

    public bool SkillOpen;                      //当此开关打开后，执行对应的绑定技能
    public MonoBehaviour Obj_FirstSkill;        //第一个执行的技能脚本
    public string SkillID;                      //当前Skill的ID
    public Vector3 SkillTargetPoint;            //技能释放目标点
    public GameObject SkillTargetObj;           //技能释放的目标Obj  （对目标单体伤害时用）
    public GameObject MonsterSkillObj;          //技能释放技能时,才会用到此字段,表示此技能是哪个怪物释放的
    //public Vector3 SkillTar
    private int skillUseNum;                    //当前技能使用次数
    public float DelaySkillTime;                //延迟技能释放时间
    private float delaySkillTimeSum;            //延迟技能释放时间累计值
    private GameObject HintSkillEffect;         //提示技能特效
    private bool hintSkillEffectStatus;         //提示技能状态

    private float SkillRigidity;               //怪物静止状态时间
    private float skillRigiditySum;

    //施法中吟唱
    public bool IfSkillSingStatus;              //技能施法中吟唱状态
    private float skillSingTime;                //技能施法中的吟唱时间
    
     
    //private float ;

	// Use this for initialization
    void Start()
    {

        //初始化技能使用次数
        skillUseNum = 0;
        //Debug.Log("当前触发技能：" + SkillID);
        DelaySkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterDelayTime", "ID", SkillID, "Skill_Template"));
        SkillRigidity = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillRigidity", "ID", SkillID, "Skill_Template"));

        if (MonsterSkillObj != null)
        {
            MonsterSkillObj.GetComponent<AI_1>().AIStopMoveStatus = true;
            MonsterSkillObj.GetComponent<AI_1>().StopMoveTime = SkillRigidity;
            MonsterSkillObj.GetComponent<AI_1>().StopMoveTimeSum = 0;
        }
        //设置怪物是否可以移动
        //MonsterSkillObj.GetComponent<AI_Property>().AI_MoveSpeed = 0;
        //设置技能范围
        Game_PublicClassVar.Get_function_Skill.AddSkillRange(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject);

        string ifLookAtTarget = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfLookAtTarget", "ID", SkillID, "Skill_Template");

        if (ifLookAtTarget == "1")
        {
            if (MonsterSkillObj != null)
            {
                this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetStatus = true;
                string ifLookAtTatgetTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfLookAtTatgetTime", "ID", SkillID, "Skill_Template");
                this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetTime = float.Parse(ifLookAtTatgetTime);
                this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetTimeSum = 0;
            }
            else
            {

            }
        }

        if (SkillID == "62003004")
        {
            //提示
            Game_PublicClassVar.Get_function_UI.GameGirdHint(DelaySkillTime + "秒后开启狂暴状态,请尽快击败!");
        }

        //技能施法中是否需要持续吟唱
        //Debug.Log("SkillID = " + SkillID);
        skillSingTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillSingTime", "ID", SkillID, "Skill_Template"));
        if (skillSingTime > 0) {
            IfSkillSingStatus = true;
            //Debug.Log("开启施法吟唱!");
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("aaaaaa = " + this.gameObject.transform.position);
        //判定是否执行技能
        if (SkillOpen)
        {
            delaySkillTimeSum = delaySkillTimeSum + Time.deltaTime;
            if (delaySkillTimeSum >= DelaySkillTime)
            {
                Obj_FirstSkill.enabled = true;
                skillUseNum = skillUseNum + 1;
                SkillOpen = false;      //保证只执行一次

                //删除技能提示特效
                if (HintSkillEffect != null) {
                    Destroy(HintSkillEffect);
                }
                delaySkillTimeSum = 0;

                //获取该技能有没有附加的技能
                string addSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddSkillID", "ID", SkillID, "Skill_Template");
                if (addSkillID != "0"&&addSkillID !="") {
                    Game_PublicClassVar.Get_function_MonsterSkill.AddSkillID(addSkillID, MonsterSkillObj);
                }

                //技能施法中是否需要持续吟唱
                if (IfSkillSingStatus)
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingStatus = true;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingTime = skillSingTime;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingSkillObj = this.gameObject;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingType = "2";
                }
                
            }
            else {
                if (!hintSkillEffectStatus) {
                    
                    //实例化一个提示特效
                    GameObject obj = (GameObject)Resources.Load("Effect/Rose/Rose_SelectRange_Monster", typeof(GameObject));        //实例化范围特效
                    HintSkillEffect = (GameObject)Instantiate(obj);
                    HintSkillEffect.transform.position = SkillTargetPoint;
                    hintSkillEffectStatus = true;
                    //Debug.Log("实例化了一个提示特效" + HintSkillEffect.name);
                    hintSkillEffectStatus = true;
                    //获取技能范围大小
                    float rangeSize = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillRangeSize", "ID", SkillID, "Skill_Template"));
                    HintSkillEffect.GetComponent<Rose_SelectRange>().RangeSize = rangeSize;
                }
            }
        }

        //如果父级为返回或死亡状态,则删除自己
        if (MonsterSkillObj != null)
        {
            string ai_statusValue = MonsterSkillObj.GetComponent<AI_1>().AI_Status;
            if (ai_statusValue == "1" || ai_statusValue == "4" || ai_statusValue == "5")
            {
                Destroy(this.gameObject);
            }
        }
        //Debug.Log("aaaaaa11 = " + this.gameObject.transform.position);
	}

    //销毁时调用
    void OnDestroy() {
        //this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetStatus = false;
    }
} 
