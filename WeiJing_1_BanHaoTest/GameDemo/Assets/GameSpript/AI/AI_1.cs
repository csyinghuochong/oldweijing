﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//此脚本用于怪物本身的智能
public class AI_1 : MonoBehaviour {
	
	//定义AI的属性
    public string AI_ID_Only;                       //世界生成的AI的唯一ID序号
    public long AI_ID;                              //世界生成的AI的ID序号
	public bool AI_Selected_Status;                 //是否被选中
    public string AI_Status;                        //AI状态                      0：巡逻  1：发现目标（但不追击）2：追击 3：攻击 4：返回（攻击目标太远） 5：死亡 6：释放技能 7：跟随主角模式 8:眩晕
    public GameObject AI_Target;                    //当前怪物目前注视的目标
    public GameObject Obj_AIModel;                  //AI模型
    public SkinnedMeshRenderer ModelMesh;           //AI材质
    private float hitMeshTime;                      //受击播放材质时间
    private bool hitMeshStatus;                     //受击播放材质状态
    public bool IfRosePet;                          //当前AI是否为玩家的召唤的宠物
    public GameObject MonsterCreateObj;             //怪物召唤的父级
    public bool AIStopMoveStatus;                   //怪物移动状态
    public bool AIStopLookTargetStatus;             //AI停止看目标状态
    public bool IfDeathDestoryModel;                //怪物死亡时是否立即销毁尸体

    //AI巡逻
    private bool ai_PatrolStatus;                   //AI是否在巡逻
    private bool ai_FindNextPatrol;                 //AI是否寻找下一个巡逻点
    private float ai_WalkSpeed;                     //AI走路速度（用于巡逻）
    private float ai_PatrolRange;                   //AI巡逻范围
    private float ai_PatrolRestTime;                //AI巡逻到达指定地点休息时间
    private float ai_PatrolGuardTime;               //AI巡逻保护时间
    private float ai_actRunRange;                   //怪物攻击范围
    public Vector3 ai_StarPosition;                //AI出生坐标点
    public UnityEngine.AI.NavMeshAgent ai_NavMesh;                //AI自动寻路控制器
    private Vector3 targetPosition;                 //目标的坐标点
    private Vector3 walkPosition;                   //巡逻移动的目标点
    public float ai_chaseRange;                    //AI追击范围
    //private float ai_PartolTime;                  //AI两次寻路间隔时间;
	
	
	//AI属性声明
	private string AI_Name;
	private int  AI_Hp_Max;                         //AI血量上限
    public float AI_MoveSpeed;                     //怪物移动速度
    private bool AI_IfWalk;                         //怪物是否来回走
    private bool ai_IfReturn;                       //开启后表示怪事是在超出自身距离后返回
    private bool ai_IfChase;                        //当前ai是否处于追击状态
    private float ai_ActDistance;                   //当前ai攻击距离
    private float actSpeed;                         //攻击速度
    private float actSpped_Sum;                     //攻击速度统计值（累加值,用来判定当前时间是否满足下次攻击）
    private bool NextActStatus;                     //下一次攻击状态,如果为true表示可以立即进行下一次攻击
    private bool ActStatus;                         //攻击状态
    public float monsterRebirthTime;               //怪物复活时间（单位/秒）
    public float monsterReirthTimeSum;             //怪物复活时间累计值 
    public float monsterOffLineTime;
    private bool ai_IfDeath;                        //怪物是否死亡
    private bool ai_IfDeathTimeStatus;              //怪物是为死亡复活状态
    public float monsterDestoryTime;               //怪物销毁模型时间
    //private float ai_DeathTimeSum;                //怪物死亡复活时间累计
    private string[] skillID;                        //怪物自身的技能
    private string[] passiveSkillType;               //技能触发类型
    private string[] passiveSkillPro;                //技能触发类型参数
    private string[] passiveSkillTriggerOnce;        //技能是否只触发一次参数
    private string[] passiveSkillTriggerTime;        //技能触发时间类型
    private string[] ifSkillTrigger;                //技能是否触发,当次字段的数组为1是,对应的技能将不能再次释放
    private string[] passiveSkillTriggerTimeCD;     //技能CD
    private string[] passiveSkillTriggerTimeCDSum;  //技能CD计时
    private bool triggerSkillStatus;                //怪物触发技能状态
    private string triggerSkillID;                  //怪物触发技能ID
    //private bool actStatus;                         //每次攻击打开此状态判定是否释放技能
    private float passiveSkillTriggerTimeSum;        //在Updata里每隔多少时间监测一次是否触发被动

    public bool AI_UpdateProStatus;


	//当前AI变量
	public float AI_Distance;                       //AI距离角色的距离
	private bool AI_Hp_Status;                      //如果当前AI显示血条，则为True
    private bool AI_MonsterDeathUIStatus;           //怪物死亡复活状态
    private bool AI_MonsterDeathStatus;             //怪物死亡复活状态
	public GameObject Selected_Effect;              //选中特效
	public Transform Selected_Effect_Position;      //选中特效坐标点
	private bool Selected_Effect_Status;            //特效是否已经开启
	private GameObject effect;                      //当前实例化的光圈特效
	public Transform Ai_HpShowPosition;             //当前显示血条的坐标点
    private Transform ai_DeathTimePosition;         //怪物复活UI坐标点
	public GameObject UI_GameObject;                //UI的GameObject
    private GameObject UI_Hp;                       //AI血条的UI位置
    private GameObject monsterDeathTimeObj;         //AI死亡复活时间显示UI
	private AI_Property ai_property;                //AI当前属性
	private bool MaxHp_Status;                      //是否取到最大生命值


	public bool HitStatus;                          //是否受到伤害
	public int HitDamge;                            //受到的伤害值
	public GameObject HitObject;                    //伤害飘字的显示UI
    public bool HitCriStatus;                       //是否受到暴击伤害
	private int LastHp;                             //上一次的生命值,飘字用的
    public bool IfHitEffect;                        //是否播放受伤特效
    public GameObject HitEffect;                    //受击特效
    public GameObject HitEffectt_Position;          //受击特效播放点
    public GameObject BoneSet;                      //骨骼绑点
    private AI_Status ai_status;                    //AI状态脚本
    private Rose_Status rose_status;
    private Game_PositionVar game_positionVar;
    private GameObject obj_Rose;                    //主角的Obj
    private bool SkillStatus;
    //public bool petMoveStatus;                    //宠物跟随主角移动状态
    public float StopMoveTime;
    public float StopMoveTimeSum;
    public float AIStopLookTargetTime;
    public float AIStopLookTargetTimeSum;

    //public float AIStop

    public bool XuanYunStatus;                      //眩晕状态
    public float XuanYunTime;                       //眩晕时间
    public float xuanYunTimeSum;                    //眩晕时间累计
    private bool XuanYunFlyTextStatus;              //眩晕飘字状态
    //private Animator animator;

    //为了无限刷怪的BUG,修正掉落
    public bool dropStatus;
    private float dropNum;
    private float dropNumSum;

	// Use this for initialization
	void Start () {

        //获取怪物出现概率
        float monsterShowPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterShowPro", "ID", AI_ID.ToString(), "Monster_Template"));
        //float monsterShowPro = 0f;
        if (Random.value > monsterShowPro) {
            //Debug.Log("删除了怪");
            Destroy(this.gameObject);
        }

        //初始化AI的UI显示
        ai_property = this.gameObject.GetComponent<AI_Property>();
        LastHp = ai_property.AI_Hp;             //显示AI血量

        //初始化AI巡逻机制
        ai_PatrolStatus = true;     //开启AI巡逻
        ai_FindNextPatrol = true;   //开启AI下一次攻击
        ai_StarPosition = this.gameObject.transform.position;       //获取AI的出生点
        ai_NavMesh = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        ai_status = this.gameObject.GetComponent<AI_Status>();
        ai_status.AI_StatusValue = 0;

        //初始化怪物属性
        AI_MoveSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MoveSpeed", "ID", AI_ID.ToString(), "Monster_Template"));              
        //怪物走路速度
        ai_WalkSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("WalkSpeed", "ID", AI_ID.ToString(), "Monster_Template"));             
        //设置攻击速度
        actSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActSpeed", "ID", AI_ID.ToString(), "Monster_Template"));
        //设置攻击距离  （远程单位不要超过10.0f,如果想改成超过10.0f需要手动修改追击10.0f的值才会触发）
        ai_ActDistance = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDistance", "ID", AI_ID.ToString(), "Monster_Template"));
        //设置追击范围
        ai_chaseRange = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChaseRange", "ID", AI_ID.ToString(), "Monster_Template"));
        //ai_chaseRange = 5.0f;
        ai_actRunRange = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActRunRange", "ID", AI_ID.ToString(), "Monster_Template"));
        //赋值巡逻范围
        ai_PatrolRange = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PatrolRange", "ID", AI_ID.ToString(), "Monster_Template"));
             
        //设定怪物复活时间
        monsterRebirthTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ReviveTime", "ID", AI_ID.ToString(), "Monster_Template"));
        monsterOffLineTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffLineReviveTime", "ID", AI_ID.ToString(), "Monster_Template"));

        //设置尸体消失时间
        monsterDestoryTime = 0.3f;   

        //monsterReirthTimeSum = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(AI_ID_Only);

        //防止无限刷怪BUG,强制两次掉落时间
        dropStatus = false;
        dropNum = 1800;     //为防止下面执行错误等于0
        dropNum = monsterRebirthTime/2;

        if (AI_ID_Only != "" && AI_ID_Only != "0") {
            //更新在线刷新时间
            float rebirthTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(AI_ID_Only);
            if (rebirthTime != 0) {
                monsterRebirthTime = rebirthTime;
            }
            //更新离线刷新时间
            float offLineTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathOffLineTime(AI_ID_Only);
            if (offLineTime != 0) {
                monsterOffLineTime = offLineTime;
            }
        }
        //monsterRebirthTime = 60.0f;
        //获取怪物技能
        skillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", AI_ID.ToString(), "Monster_Template").Split(';');

        //获取技能的其他参数
        if (skillID[0] != "0")
        {
            string PassiveSkillTypeStr = "";        //技能类型参数
            string PassiveSkillProStr = "";         //技能参数
            string PassiveSkillTriggerOnceStr = "";  //技能只执行一次参数
            string PassiveSkillTriggerTimeStr = "";     //技能触发时机
            string IfPassiveSkillTrigger = "";          //技能触发状态
            string passiveSkillTriggerTimeCDStr = "";          //技能触发状态
            string passiveSkillTriggerTimeCDSumStr = "";          //技能触发状态
            for (int i = 0; i <= skillID.Length - 1; i++)
            {
                //Debug.Log("skillID" + i + " = " +skillID[i]);
                //循环获取技能数据
                string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillType", "ID", skillID[i], "Skill_Template");
                //Debug.Log("skillType = " + skillType + "skillID[i] = " + skillID[i]);
                string skillPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillPro", "ID", skillID[i], "Skill_Template");
                string skillTriggerOnce = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerOnce", "ID", skillID[i], "Skill_Template");
                string skillTriggerTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerTime", "ID", skillID[i], "Skill_Template");
                string skillTriggerTimeCD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", skillID[i], "Skill_Template");
                PassiveSkillTypeStr = PassiveSkillTypeStr + skillType + ";";
                PassiveSkillProStr = PassiveSkillProStr + skillPro + ";";
                PassiveSkillTriggerOnceStr = PassiveSkillTriggerOnceStr + skillTriggerOnce + ";";
                PassiveSkillTriggerTimeStr = PassiveSkillTriggerTimeStr + skillTriggerTime + ";";
                IfPassiveSkillTrigger = IfPassiveSkillTrigger + "0;";
                passiveSkillTriggerTimeCDStr = passiveSkillTriggerTimeCDStr + skillTriggerTimeCD + ";";
                passiveSkillTriggerTimeCDSumStr = passiveSkillTriggerTimeCDSumStr + "0;";
            }

            //Debug.Log("技能类型"+PassiveSkillTypeStr);

            passiveSkillType = PassiveSkillTypeStr.Split(';');
            passiveSkillPro = PassiveSkillProStr.Split(';');
            passiveSkillTriggerOnce = PassiveSkillTriggerOnceStr.Split(';');
            passiveSkillTriggerTime = PassiveSkillTriggerTimeStr.Split(';');
            ifSkillTrigger = IfPassiveSkillTrigger.Split(';');
            passiveSkillTriggerTimeCD = passiveSkillTriggerTimeCDStr.Split(';');
            passiveSkillTriggerTimeCDSum = passiveSkillTriggerTimeCDSumStr.Split(';');

        }
        //初始化怪物默认目标
        //animator = GetComponent<Animator>();
        if (AI_Target == null) {
            AI_Target = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
        }

        rose_status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        game_positionVar = Game_PublicClassVar.Get_game_PositionVar;
        obj_Rose = game_positionVar.Obj_Rose;

        //怪物复活时间存储
        float deathTimeNow = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(AI_ID_Only);
        if (deathTimeNow != 0) {
            //this.gameObject.SetActive(false);
            this.GetComponent<AI_Property>().AI_Hp = 0;
            AI_MonsterDeathStatus = true;
            //AI_MonsterDeathUIStatus = true;
            //ai_IfDeath = true;
            //showMonsterDeathTimeUI();       //怪物复活时间显示
        }

        //ai_DeathTimePosition = BoneSet.transform.Find("Center").transform;

        //更新属性
        AI_UpdateProStatus = true;

        //设置自身离寻路最近的坐标点,要不离地面检测区太远会报错
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(this.gameObject.transform.position, out hit, 10.0f, 1);
        try
        {
            ai_NavMesh.Warp(hit.position);
            walkPosition.y = hit.position.y;
            ai_NavMesh.SetDestination(walkPosition);
            //设置AI的初始坐标，要不返回时会报错
            ai_StarPosition = this.transform.position;
            
        }
        catch
        {
            Debug.Log("移动报错！怪物ID：" + AI_ID + "name = " + AI_Name);
        }
	}
	
	// Update is called once per frame
	void Update () {

        //防止刷怪BUG做的掉落检测
        if (dropStatus) {
            dropNumSum = dropNumSum + Time.deltaTime;
            if(dropNumSum>=dropNum){
                dropStatus = false;
                dropNumSum = 0;
            }
        }

        //设置AI选中状态
        if (game_positionVar.UpdataSelectedStatus) {
        //if (Game_PublicClassVar.Get_game_PositionVar.UpdataSelectedStatus)
            if (rose_status.Obj_ActTarget != null)
            {
                if (this.gameObject!= rose_status.Obj_ActTarget)
                {
                    AI_Selected_Status = false;
                }
            }
            else
            {
                AI_Selected_Status = false;
            }
        }

        //怪物停止
        if (AIStopMoveStatus) {
            //ai_NavMesh.ResetPath();     //清理寻路,停止移动
            AI_MoveSpeed = 0;
            StopMoveTimeSum = StopMoveTimeSum + Time.deltaTime;
            if (StopMoveTimeSum >= StopMoveTime) {
                AIStopMoveStatus = false;
                AI_MoveSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MoveSpeed", "ID", AI_ID.ToString(), "Monster_Template"));     
            }
        }

        //怪物停止注释
        if (AIStopLookTargetStatus) {
            AIStopLookTargetTimeSum = AIStopLookTargetTimeSum + Time.deltaTime;
            if (AIStopLookTargetTimeSum >= AIStopLookTargetTime) {
                //清空数据
                AIStopLookTargetStatus = false;
                AIStopLookTargetTimeSum = 0;
                AIStopLookTargetTime = 0;
            }
        }
        


        //控制AI的攻击间隔,当AI刚攻击结束一次后必须满足下次攻击状态开启时才能追击目标
        actSpped_Sum = actSpped_Sum + Time.deltaTime;

        if (actSpped_Sum >= actSpeed/2)
        {
            NextActStatus = true;
        }

        //-------------------------------------------------设置AI状态-----------------------------------------------

        //判定角色离AI的距离
        targetPosition = AI_Target.gameObject.transform.position;
        AI_Distance = Vector3.Distance(targetPosition, this.gameObject.transform.position);

        //获取角色当前自动攻击
        if (ai_property.AI_Hp >0)
        {
            float roseDis = rose_status.NextAutomaticMonsterDis;
            if (rose_status.NextAutomaticMonsterObj == null)
            {
                rose_status.NextAutomaticMonsterObj = this.gameObject;
                rose_status.NextAutomaticMonsterDis = AI_Distance;
            }
            if (rose_status.NextAutomaticMonsterObj == this.gameObject) {
                rose_status.NextAutomaticMonsterDis = AI_Distance;
            }
            if (AI_Distance < roseDis)
            {
                //获取当前职业,魔法师职业不自动切换最近目标
                string roseOcc = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                switch (roseOcc) {
                    //战士攻击最近目标
                    case "1":
                        rose_status.NextAutomaticMonsterObj = this.gameObject;
                        rose_status.NextAutomaticMonsterDis = AI_Distance;
                    break;
                    //魔法师不执行任何操作
                    case "2":
                        rose_status.NextAutomaticMonsterObj = this.gameObject;
                        rose_status.NextAutomaticMonsterDis = AI_Distance;
                    break;
                }

            }
        }

        //与目标点的距离大于ai_chaseRange时,判定为处于巡逻状态
        if (AI_Distance >= ai_chaseRange + 2.0f)   //加2.0是表示怪物在追击范围+0.2的范围内是要注释玩家的
        {
            AI_Status = "0";        //设置为巡逻
            //判定怪物是否为玩家的宠物
            if (IfRosePet)
            {
                AI_Status = "7";            //设定怪物为宠物
            }

        }
        else {
            AI_Status = "1";        //设置为注视目标

            if (AI_Distance < ai_chaseRange)
            {
                AI_Status = "2";    //设置为追击模式
                ai_IfChase = true;

                if (AI_Distance <= ai_ActDistance)
                {
                    AI_Status = "3";    //设置为攻击模式
                }

                //获取目标是否已经死亡
                if (rose_status.RoseDeathStatus)
                {
                    AI_Status = "0";        //设置为巡逻
                }
            }
        }

        //如果受到攻击则直接开启追击模式
        if (HitStatus) {
            ai_IfChase = true;
        }
        //打开追击模式
        if (ai_IfChase) {
            if (AI_Status == "0") {
                AI_Status = "2";
                //获取目标是否已经死亡
                if (rose_status.RoseDeathStatus)
                {
                    AI_Status = "0";        //设置为巡逻
                }
            }
            if (AI_Status == "1") {
                AI_Status = "2";
            }
        }
        //每隔1秒监测一次被动技能
        passiveSkillTriggerTimeSum = passiveSkillTriggerTimeSum + Time.deltaTime;
        if (passiveSkillTriggerTimeSum >= 1.0f) {
            //怪物未死亡
            if (!ai_IfDeath)
            {
                actTriggerSkill("1", "2");       //尝试触发被动技能
            }

            //更新被动技能CD
            if (skillID[0] != "0") {
                for (int i = 0; i <= skillID.Length - 1; i++)
                {
                    if (passiveSkillTriggerOnce[i] == "0")
                    {
                        //更新CD
                        if (passiveSkillTriggerTimeCD[i] != "0")
                        {
                            float value = float.Parse(passiveSkillTriggerTimeCDSum[i]) + passiveSkillTriggerTimeSum;
                            passiveSkillTriggerTimeCDSum[i] = value.ToString();
                            //清空CD
                            //Debug.Log("更新被动技能时间:" + value.ToString());
                            if (value > float.Parse(passiveSkillTriggerTimeCD[i]))
                            {
                                //Debug.Log("清空技能CD,i = " + i);
                                ifSkillTrigger[i] = "0";
                                passiveSkillTriggerTimeCDSum[i] = "0";
                            }
                        }
                    }
                }
            }
            //清空累计数
            passiveSkillTriggerTimeSum = 0;
        }
        
        //怪物超过出生地一定距离后返回出生地
        float safetyDistance = Vector3.Distance(this.gameObject.transform.position, ai_StarPosition);
        if (safetyDistance >= ai_actRunRange)
        {
            //判定是否为宠物
            if (!IfRosePet) {
                AI_Status = "4";
                ai_IfReturn = true; //打开返回开关强制返回
            }
        }

        if (ai_IfReturn) {
            AI_Status = "4";
            //当自身的坐标点和出生点一致时,不再强制等于返回状态
            if (safetyDistance < 1.0f) {
                ai_IfReturn = false;
                ai_IfChase = false;
            }
            //血量恢复至最大生命
            ai_property.AI_Hp = AI_Hp_Max;
        }

        //释放状态
        if (SkillStatus) {

            AI_Status = "6";
            SkillStatus = false;

        }

        //眩晕状态,不执行任和操作
        if (XuanYunStatus)
        {
            //判定当前是否是返回状态
            if (AI_Status != "4")
            {
                //眩晕飘字
                if (!XuanYunFlyTextStatus)
                {
                    XuanYunFlyTextStatus = true;
                    Game_PublicClassVar.Get_function_UI.SpecialFlyText(HitObject, UI_Hp, "1");
                }

                xuanYunTimeSum = xuanYunTimeSum + Time.deltaTime;
                if (xuanYunTimeSum >= XuanYunTime)
                {
                    XuanYunStatus = false;
                    xuanYunTimeSum = 0;
                    //Debug.Log(AI_Name + "正在眩晕中");
                    XuanYunFlyTextStatus = false;       //眩晕完毕 修正飘字状态
                    //return;
                }
                AI_Status = "8";
            }
            else {
                XuanYunStatus = false;
            }

            //return;
        }

        //获取持久化数据
        if (AI_ID_Only != "0")
        {
            if (PlayerPrefs.GetInt(AI_ID_Only) != 0)
            {
                ai_property.AI_Hp = 0;
                //更新死亡复活时间
            }
        }

        //如果生命低于0,判定为死亡
        if (ai_property.AI_Hp <= 0)
        {
            AI_Status = "5";
        }



        //ai_NavMesh.speed = ai_WalkSpeed;
        //根据不同的AI状态触发不同的AI机制
        switch (AI_Status) { 

            //巡逻
            case "0":

                ai_IfDeath = false; //重置死亡状态

                //获取一个巡逻目标点
                if (ai_FindNextPatrol)
                {
                    //随机一个范围
                    float random_x = (Random.value - 0.5f) * ai_PatrolRange * 2;
                    float random_z = (Random.value - 0.5f) * ai_PatrolRange * 2;
                    walkPosition = new Vector3(ai_StarPosition.x + random_x, ai_StarPosition.y, ai_StarPosition.z + random_z);
                    //注视目标
                    transform.LookAt(new Vector3(walkPosition.x, transform.position.y, walkPosition.z));
                    //移动目标区域
                    ai_NavMesh.speed = ai_WalkSpeed;

                    //设置自身离寻路最近的坐标点,要不离地面检测区太远会报错
                    
                    UnityEngine.AI.NavMeshHit hit;
                    UnityEngine.AI.NavMesh.SamplePosition(this.gameObject.transform.position, out hit, 10.0f, 1);
                    try
                    {
                        ai_NavMesh.Warp(hit.position);
                        walkPosition.y = hit.position.y;
                        ai_NavMesh.SetDestination(walkPosition);
                    }
                    catch
                    {
                        Debug.Log("移动报错！怪物ID：" + AI_ID + "name = " + AI_Name);
                    }
                    
                    ai_FindNextPatrol = false;
                    //设置AI状态&播放对应动作
                    ai_status.IfUpdateStatus = true;
                    ai_status.AI_StatusValue = 1;
                    
                }

                //获取自己与目标点的距离(抛弃Y轴的高低差)
                Vector3 dis_1 = new Vector3(walkPosition.x, 0, walkPosition.z);
                Vector3 dis_2 = new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z);
                float distance = Vector3.Distance(dis_1, dis_2);

                //到达目标点,休息3秒
                if (distance <= 0.25f)
                {
                    //休息时间累积
                    ai_PatrolRestTime = ai_PatrolRestTime + Time.deltaTime;
                    //设置AI状态&播放对应动作
                    ai_status.IfUpdateStatus = true;
                    ai_status.AI_StatusValue = 0;
                    //ai_NavMesh.speed = 0;
                    ai_PatrolGuardTime = 0.0f;

                    //Debug.Log("ai_PatrolRestTime = " + ai_PatrolRestTime);

                    if (ai_PatrolRestTime >= 3.0f)
                    {
                        //清空数据
                        ai_FindNextPatrol = true;
                        ai_PatrolRestTime = 0.0f;
                    }
                }
                else
                {
                    //移动保护时间
                    ai_PatrolGuardTime = ai_PatrolGuardTime + Time.deltaTime;
                }

                //每次移动超过10秒再次寻找下一个巡逻目标点
                if (ai_PatrolGuardTime >= 10.0f)
                {
                    ai_FindNextPatrol = true;
                    ai_PatrolGuardTime = 0.0f;
                }

            break;

            //注视
            case "1":
                    
                //旋转到主角
                if (ai_property != null)
                {
                    //检测自身是否已经死亡
                    if (ai_property.AI_Hp > 0)
                    {
                        //注视目标
                        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                        ai_NavMesh.SetDestination(this.transform.position);
                        //ai_NavMesh.speed = 0;
                        ai_status.IfUpdateStatus = true;
                        ai_status.AI_StatusValue = 0;
                    }
                }
            break;

            //追击
            case "2":

                //注视目标
                if (!AIStopLookTargetStatus)
                {
                    transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                
                //if (this.gameObject.GetComponent<AI_NormalAct>().NextActStatus)
                    if (NextActStatus)
                    {
                        //停止巡逻
                        ai_PatrolStatus = false;
                        ai_NavMesh.speed = AI_MoveSpeed;



                        ai_NavMesh.SetDestination(AI_Target.gameObject.transform.position);
                        //更新动作
                        ai_status.IfUpdateStatus = true;
                        ai_status.AI_StatusValue = 2;
                    }
                }

            break;
            //普通攻击
            case "3":

                ai_NavMesh.ResetPath(); //停止移动目标
                //注视目标
                if (!AIStopLookTargetStatus) {
                    transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                }
                
                //当与主角距离少于3时，开始触发普通攻击
                if (AI_Distance <= ai_ActDistance)
                {
                    AI_Property aiproperty = GetComponent<AI_Property>();
                    if (aiproperty != null)
                    {
                        //Rose_Proprety roseproperty = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
                        //AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                        //判定是否满足攻击间隔时间
                        if (actSpped_Sum >= actSpeed)
                        {
                            ActStatus = true;
                            actSpped_Sum = 0.0f;
                            NextActStatus = false;
                            //actStatus = true;
                        }

                        //开启攻击状态
                        if (ActStatus)
                        {
                            //设置AI为攻击状态
                            this.gameObject.GetComponent<AI_Status>().AI_StatusValue = 3;
                            this.gameObject.GetComponent<AI_Status>().IfUpdateStatus = true;

                            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseIfHit = true;      //播放受击特效
                            //Game_PublicClassVar.Get_fight_Formult.MonsterActRose("60020001", this.gameObject);           //战斗
                            ActStatus = false;

                        }
                    }
                }

            break;
            //返回
            case "4":

                //当距离出生地30码时,怪物自动返回营地
                if (safetyDistance >= 15.0f)
                {
                    //注视目标
                    transform.LookAt(new Vector3(ai_StarPosition.x, transform.position.y, ai_StarPosition.z));
                    ai_NavMesh.speed = AI_MoveSpeed;

                    ai_NavMesh.SetDestination(ai_StarPosition);
                    ai_IfReturn = true;     //打开返回状态

                    //设置AI状态&播放对应动作
                    ai_status.IfUpdateStatus = true;
                    ai_status.AI_StatusValue = 2;

                    ai_FindNextPatrol = true;   //将巡逻状态打开


                    //重置技能
                    if (skillID[0] != "0")
                    {
                        string PassiveSkillTypeStr = "";        //技能类型参数
                        string PassiveSkillProStr = "";         //技能参数
                        string PassiveSkillTriggerOnceStr = "";  //技能只执行一次参数
                        string PassiveSkillTriggerTimeStr = "";     //技能触发时机
                        string IfPassiveSkillTrigger = "";          //技能触发状态
                        string passiveSkillTriggerTimeCDStr = "";          //技能触发状态
                        string passiveSkillTriggerTimeCDSumStr = "";          //技能触发状态
                        for (int i = 0; i <= skillID.Length - 1; i++)
                        {
                            //Debug.Log("skillID" + i + " = " +skillID[i]);
                            //循环获取技能数据
                            string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillType", "ID", skillID[i], "Skill_Template");
                            //Debug.Log("skillType = " + skillType + "skillID[i] = " + skillID[i]);
                            string skillPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillPro", "ID", skillID[i], "Skill_Template");
                            string skillTriggerOnce = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerOnce", "ID", skillID[i], "Skill_Template");
                            string skillTriggerTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerTime", "ID", skillID[i], "Skill_Template");
                            string skillTriggerTimeCD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", skillID[i], "Skill_Template");
                            PassiveSkillTypeStr = PassiveSkillTypeStr + skillType + ";";
                            PassiveSkillProStr = PassiveSkillProStr + skillPro + ";";
                            PassiveSkillTriggerOnceStr = PassiveSkillTriggerOnceStr + skillTriggerOnce + ";";
                            PassiveSkillTriggerTimeStr = PassiveSkillTriggerTimeStr + skillTriggerTime + ";";
                            IfPassiveSkillTrigger = IfPassiveSkillTrigger + "0;";
                            passiveSkillTriggerTimeCDStr = passiveSkillTriggerTimeCDStr + skillTriggerTimeCD + ";";
                            passiveSkillTriggerTimeCDSumStr = passiveSkillTriggerTimeCDSumStr + "0;";
                        }

                        //Debug.Log("技能类型"+PassiveSkillTypeStr);

                        passiveSkillType = PassiveSkillTypeStr.Split(';');
                        passiveSkillPro = PassiveSkillProStr.Split(';');
                        passiveSkillTriggerOnce = PassiveSkillTriggerOnceStr.Split(';');
                        passiveSkillTriggerTime = PassiveSkillTriggerTimeStr.Split(';');
                        ifSkillTrigger = IfPassiveSkillTrigger.Split(';');
                        passiveSkillTriggerTimeCD = passiveSkillTriggerTimeCDStr.Split(';');
                        passiveSkillTriggerTimeCDSum = passiveSkillTriggerTimeCDSumStr.Split(';');

                    }

                }

            break;
            //死亡
            case "5":

                if (!ai_IfDeath) {
                    
                    //monsterDestoryTime = 5.0f;        //设置怪物初始销毁模型时间
                    //清空角色自动攻击的目标
                    if (rose_status.NextAutomaticMonsterObj == this.gameObject) {
                    rose_status.NextAutomaticMonsterObj = null;
                    rose_status.NextAutomaticMonsterDis = 0;
                    //Debug.Log("清空攻击目标");
                    }

                    //隐藏碰撞体
                    this.GetComponent<CharacterController>().enabled = false;
                    ai_IfDeath = true;
                    ai_status.IfUpdateStatus = true;
                    ai_status.AI_StatusValue = 6;

                    ai_NavMesh.height = 0;  //隐藏碰撞高

                    //显示复活时间UI
                    //判定怪物是否死亡
                    if (ai_IfDeath)
                    {
                        //判定怪物的唯一ID
                        if (AI_ID_Only != "" && AI_ID_Only != "0")
                        {
                            //检测存储的复活数据里有没有相同的怪物ID
                            bool showDeathTimeStatus = false;
                            string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                            string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
                            if (deathMonsterIDListStr != "")
                            {
                                for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
                                {
                                    string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                                    if (deathMonsterID[0] == AI_ID_Only)
                                    {
                                        //Debug.Log("deathMonsterID[0] == " + deathMonsterID[0] + "   AI_ID_Only = " + AI_ID_Only);
                                        showDeathTimeStatus = true;
                                    }
                                }
                            }
                            //如果没有相同的怪物ID则现实复活时间
                            if (!showDeathTimeStatus)
                            {
                                showMonsterDeathTimeUI();       //怪物复活时间显示
                            }

                            monsterDestoryTime = 99999;
                            //Debug.Log("火速展示111！");
                        }
                    }

                    //存储唯一怪物ID
                    if (AI_ID_Only != "" && AI_ID_Only != "0")
                    {
                        //PlayerPrefs.SetInt(AI_ID_Only, 60);     //存储Int型 秒为单位
                        //存储复活时间
                        Game_PublicClassVar.Get_function_AI.SaveMonsterDeathTime(AI_ID_Only, monsterRebirthTime.ToString(), monsterOffLineTime.ToString());
                    }
                    
                    //取消选中状态;
                    AI_Selected_Status = false;
                    
                    //触发怪物掉落及任务相关规则
                    if (!AI_MonsterDeathStatus) {
                        AI_MonsterDeathStatus = true;   //保证执行一次

                        //怪物死后创建的一个掉落
                        if (!dropStatus)
                        {
                            dropStatus = true;
                            Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position);     //此代码消耗大
                            
                            //获取当前角色等级和怪物等级,绝对值小于10的才能享受难度奖励的加成
                            int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
                            int aiLv = this.GetComponent<AI_Property>().AI_Lv;
                            int chaLvValue = aiLv - roseLv;
                            if (Mathf.Abs(chaLvValue)<=100)         //玩家真凶我,老抱怨我就改啦
                            { 
                                switch (Game_PublicClassVar.Get_game_PositionVar.GameNanduValue) {
                                    //难度2执行有概率执行掉落
                                    case "2":
                                        if (Random.value >= 0.5f) {
                                            Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position);     //此代码消耗大
                                            //Debug.Log("执行了2次掉率");
                                        }
                                        break;

                                    //难度3执行有概率执行2次掉落
                                    case "3":
                                        Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position);     //此代码消耗大
                                        //Debug.Log("执行了地狱掉率");
                                        break;

                                }
                            }
                        }

                        //获取怪物是否有任务需求掉落
                        string[] dropTaskItem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropTaskItem", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template").Split(';');
                        if (dropTaskItem[0] != "0")
                        {
                            for (int i = 0; i <= dropTaskItem.Length - 1; i++)
                            {
                                string[] taskDropID = dropTaskItem[i].Split(',');
                                string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(taskDropID[0]);
                                if (taskStatus == "3")
                                {
                                    //当返回的任务状态为3,也就是任务已接取,未完成则触发一次掉落
                                    Game_PublicClassVar.Get_function_AI.DropIDToDropItem(taskDropID[1], this.transform.position, GetComponent<AI_Property>().AI_ID);
                                    //Debug.Log("我执行了一次任务掉落");
                                }
                            }
                        }
                        
                        //怪物死亡给人物增加对应的经验值
                        string monsterExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Exp", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                        Game_PublicClassVar.Get_function_Rose.AddExp(int.Parse(monsterExp));
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_GetExp = true;

                        //触发一次死亡技能
                        //Debug.Log("触发死亡技能");
                        //actTriggerSkill("1", "2");       //尝试触发被动技能

                        /*
                        if (AI_ID == 70002006) { 
                            //死亡创建一个怪物
                            Game_PublicClassVar.Get_function_AI.AI_CreatMonster("70002007", this.transform.position);
                        }
                        */

                        //是否为任务要求的怪物
                        
                        Game_PublicClassVar.Get_function_Task.TaskMonsterNum(GetComponent<AI_Property>().AI_ID, 1);
                        
                        //更新任务
                        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("2", "1", "1");
                        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("3", "1", "1");
                        //获取当前是否为BOSS
                        string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                        //Debug.Log("GetComponent<AI_Property>().AI_ID = " + GetComponent<AI_Property>().AI_ID + "    zzzzzzzzmonsterType = " + monsterType);
                        if (monsterType == "3") {
                            //Debug.Log("monsterType == 3");
                            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("2", "2", "1");
                            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("3", "2", "1");
                            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("3", "4", AI_ID.ToString(), AI_ID.ToString());       //传入隐藏任务指定BOSS的ID
                        }
                        
                    }

                    


                    //停止移动
                    try{
                        ai_NavMesh.SetDestination(this.transform.position);
                    }
                    catch
                    {
                        Debug.Log("移动报错！怪物ID：" + AI_ID + "name = " + AI_Name);
                    }
                    //隐藏阴影底
                    GameObject backObj = Obj_AIModel.transform.Find("BackDi").gameObject;
                    if (backObj != null) {
                        backObj.SetActive(false);
                    }

                    //播放怪物死亡音效
                    Game_PublicClassVar.Get_function_UI.PlaySource("20014", "2");

                    //更新玩家剧情状态
                    string TriggerStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerStoryID", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                    //Debug.Log("TriggerStoryID = " + TriggerStoryID);
                    if (TriggerStoryID != "0"){
                        //获取玩家当前剧情状态值
                        string roseStoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        //Debug.Log("TriggerStoryID = " + TriggerStoryID);
                        if (roseStoryStatus == TriggerStoryID)
                        {
                            Game_PublicClassVar.Get_function_Rose.UpdataRoseStoryStatus();
                            //Debug.Log("玩家完成指定任务,更新了剧情值");
                        }
                    }

                    //获取怪物是否需要重生
                    string ifRevive = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfRevive", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                    if (ifRevive == "0")
                    {
                        monsterDestoryTime = 0.3f;     //5秒尸体消失
                    }
                    else {
                        monsterDestoryTime = 0f;     //5秒尸体消失
                    }

                    //怪物死亡注销模型
                    if(IfDeathDestoryModel){
                        monsterDestoryTime = 0f;
                    }

                    //怪物死亡注销角色当前目标
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget == this.gameObject) {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget = null;
                    }
                }

                //超过一定时间秒尸体消失
                if (monsterReirthTimeSum >= monsterDestoryTime)
                {
                    Obj_AIModel.SetActive(false);  //模型隐藏
                    //Destroy(this.gameObject);
                    //Destroy(monsterDeathTimeObj);
                    //Debug.Log("隐藏模型");
                }
                //Debug.Log("AI重生");
                //AI重生
                monsterReirthTimeSum = monsterReirthTimeSum + Time.deltaTime;
                //Debug.Log("monsterReirthTimeSum = " + monsterReirthTimeSum + "      monsterRebirthTime = " + monsterRebirthTime);
                if (monsterReirthTimeSum >= monsterRebirthTime) {
                    Obj_AIModel.SetActive(true); //模型显示
                    ai_property.AI_Hp = ai_property.AI_HpMax;
                    //Debug.Log("怪物重生：恢复血量 = "+AI_Hp_Max);
                    ai_IfDeath = false;
                    monsterReirthTimeSum = 0.0f;
                    AI_Hp_Status = false;
                    //将出生点重置
                    this.gameObject.transform.position = ai_StarPosition;
                    ai_IfChase = false;         //追击模式取消
                    ai_FindNextPatrol = true;   //AI巡逻重置
                    AI_Status = "0";
					this.GetComponent<AI_Status>().AI_Animator.Play("Idle");
                    //显示碰撞体
                    this.GetComponent<CharacterController>().enabled = true;
                    //隐藏阴影底
                    GameObject backObj = Obj_AIModel.transform.Find("BackDi").gameObject;
                    if (backObj != null)
                    {
                        backObj.SetActive(true);
                    }

                    //设置碰撞体高
                    ai_NavMesh.height = 2;  

                    Destroy(monsterDeathTimeObj);
                    AI_MonsterDeathUIStatus = false;
                    
                    AI_MonsterDeathStatus = false;
                    ai_IfDeathTimeStatus = false;    //设置怪物死亡复活状态
                    //设置怪物复活时间
                    monsterRebirthTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ReviveTime", "ID", AI_ID.ToString(), "Monster_Template"));
                    monsterOffLineTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffLineReviveTime", "ID", AI_ID.ToString(), "Monster_Template"));
                }
            break;

            //释放技能
            case "6":
                string skillAnimation = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillAnimation", "ID", triggerSkillID, "Skill_Template");
                if (skillAnimation != "" && skillAnimation != "0")
                {
                    this.GetComponent<AI_Status>().AI_Animator.Play(skillAnimation);
                }
                break;

            //宠物模式
            case "7":

                //获取玩家当前位置与自己的距离
                float roseDistance = Vector3.Distance(this.gameObject.transform.position, obj_Rose.transform.position);


                //判定与玩家的距离,如果大于多少距离则进行移动
                if (roseDistance >= 2.0f) {
                    /*
                    //获取玩家当前朝向
                    //Debug.Log("主角的角度：" + obj_Rose.transform.localEulerAngles.y);
                    float roseTargetAngle = obj_Rose.transform.localEulerAngles.y;
                    //获取玩家当前坐标
                    Vector3 rosePositionVec3 = obj_Rose.transform.position;
                    float petMove_X;    //宠物要移动的X坐标点
                    float petMove_Z;    //宠物要移动的Z坐标点

                    //朝向右,X轴为前
                    if (roseTargetAngle <= 180)
                    {
                        //2.5范围内随机一个值
                        petMove_X = obj_Rose.transform.position.x + Random.value * 2.5f;

                    }
                    //朝向左,X轴为后
                    else { 
                       
                        //2.5范围内随机一个值
                        petMove_X = obj_Rose.transform.position.x - Random.value * 2.5f;
                    
                    }

                    //朝向上,Y轴为上
                    if (roseTargetAngle <= 90 || roseTargetAngle >= 270)
                    {
                        //2.5范围内随机一个值
                        petMove_Z = obj_Rose.transform.position.z - Random.value * 2.5f;
                    }
                    //朝向下,Y轴为下
                    else
                    {
                        //2.5范围内随机一个值
                        petMove_Z = obj_Rose.transform.position.z + Random.value * 2.5f;
                    }
                    */

            
                        //if (!petMoveStatus) {
                            //petMoveStatus = true;
                            //开始跟随移动
                            ai_NavMesh.speed = AI_MoveSpeed;
                            //Vector3 petMovePosition = new Vector3(petMove_X, obj_Rose.transform.position.y, petMove_Z);
                            //petMovePosition = new Vector3(obj_Rose.gameObject.transform.position.x + Random.value * 2.5f, obj_Rose.gameObject.transform.position.y, obj_Rose.transform.position.z + Random.value * 2.5f);
                            //ai_NavMesh.SetDestination(petMovePosition);

                            ai_NavMesh.SetDestination(obj_Rose.transform.position);
                            //更新动作
                            ai_status.IfUpdateStatus = true;
                            ai_status.AI_StatusValue = 2;
                        //}

                    

                }

                if (roseDistance <= 1.0f)
                {
                    ai_NavMesh.speed = 0;
                    //petMoveStatus = false;

                }
                

                

                //随机获取玩家身后的一个坐标点

                //向移动点移动


                break;

            case "8":

                ai_NavMesh.speed = 0;

            break;
        }

        
        if (!ai_IfReturn) {    //判定当前AI是否处于返回状态

            //判定角色离AI的距离
            AI_Distance = Vector3.Distance(AI_Target.transform.position, transform.position);
        }
        

        //取当前AI最大生命值,用来UI上的显示（此脚本只会执行一次,用来取当前怪物的最大血量，方便UI进度条的显示，放在star里面担心和ai属性脚本冲突,ai脚本是在Updata读取属性的）
        if (ai_property != null)
        {

            if (!MaxHp_Status)
            {
                AI_Hp_Max = ai_property.AI_HpMax;
                MaxHp_Status = true;
            }
        }

		//当角色离AI 20米内,AI显示血条
        if (AI_Distance < 20)
        {

            if (!AI_Hp_Status)
            {
                //显示UI后，表示为true;
                AI_Hp_Status = true;

                AI_Name = ai_property.AI_Name;          //显示AI姓名

                Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
                Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

                //实例化UI
                UI_Hp = (GameObject)Instantiate(UI_GameObject);

                //显示UI,并对其相应的属性修正
                UI_Hp.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet.transform);
                UI_Hp.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
                UI_Hp.transform.localScale = new Vector3(1f, 1f, 1f);
                UI_Hp.transform.GetComponent<UI_AIHp>().Obj_Monster = this.gameObject;

                //取得界面控件
                GameObject UI_Name = UI_Hp.transform.Find("Lal_Name").gameObject;

                //显示当前UI名称
                Text UIname = UI_Name.GetComponent<Text>();
                UIname.text = "等级：" + ai_property.AI_Lv + "  " + AI_Name;

                //Debug.Log("UI_Hp = " + AI_Name);

                //显示复活时间
                //判定怪物是否死亡
                if (ai_IfDeath)
                {
                    //判定怪物的唯一ID
                    if (AI_ID_Only != "" && AI_ID_Only != "0")
                    {
                        showMonsterDeathTimeUI();
                        //Debug.Log("火速展示222！");
                    }
                }
            }

            if (UI_Hp != null)
            {
                //UI显示当前血量
                //Transform HpTexture = UI_Hp.transform.Find("Img_Value");
                Image HpValue = UI_Hp.GetComponent<UI_AIHp>().Obj_aiImgValue.GetComponent<Image>();
                HpValue.fillAmount = (float)ai_property.AI_Hp / AI_Hp_Max;

            }

        }
        else { 
            //删除怪物名称
            Destroy(UI_Hp);
            AI_Hp_Status = false;
        }
		
		//出现血条后,不断修正血条位置
		if (AI_Hp_Status) {
            
			if(UI_Hp!=null){
			
				Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
				Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);
				
				//血条位置修正（根据分辨率的变化而变化）
				UI_Hp.transform.localPosition = new Vector3(Hp_show_position.x,Hp_show_position.y,0);
			}
        }
        //修正怪物复活显示位置
        //Debug.Log("a1");
        if (AI_MonsterDeathUIStatus) {
            //Debug.Log("a2");
            if ( monsterDeathTimeObj != null)
            {

                Vector3 DeathTime_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
                //Vector3 DeathTime_position = Camera.main.WorldToViewportPoint(ai_DeathTimePosition.position);
                DeathTime_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(DeathTime_position);

                //血条位置修正（根据分辨率的变化而变化）
                monsterDeathTimeObj.transform.localPosition = new Vector3(DeathTime_position.x, DeathTime_position.y, 0);
                //Debug.Log("a3");
            }
        }
		
		//当和AI距离超过20后，不显示其UI
		if(AI_Distance >= 20) {
			
			AI_Hp_Status = false;
			Destroy(UI_Hp);
            Destroy(monsterDeathTimeObj);
            AI_MonsterDeathUIStatus = false;
            //回归原来位置,以后添加
			
		}

		//选中状态触发时播放特效
		if (AI_Selected_Status) {

			//此次判定为了只实例化一次
			if (!Selected_Effect_Status) {

				if(Selected_Effect==null){

			    //Debug.Log("怪物选中特效为空");
				}else{
                
                }

				effect = (GameObject)Instantiate (Selected_Effect);
                effect.transform.SetParent(Selected_Effect_Position.transform);
                Selected_Effect_Status = true;

                //获取选中大小
                float selectedEffectSize = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SelectSize", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template"));
                //float selectedEffectSize = 2.0f;
                effect.GetComponent<Rose_SelectTarget>().SelectEffectSize = selectedEffectSize;

				//重置特效位置
				effect.transform.localPosition = new Vector3 (0, 0, 0);

                //选中描红边
                //ModelMesh.material.SetColor("_OutlineColor", Color.red);     //设置受击变色

			}

		} else {

			if(effect){
			    Destroy(effect);
			    Selected_Effect_Status = false;
                //选中描红边
                ModelMesh.material.SetColor("_OutlineColor", Color.black);     //设置受击变色
			}

		}

		//死亡时注销生命条
		if (ai_property != null) {

			if (ai_property.AI_Hp < 1) {
				Destroy (UI_Hp);
			}
		}

		//战斗飘字
		if (HitStatus) {
            hitMeshStatus = true;
            //防止死亡到底也变成红色
            if (ai_property.AI_Hp > 0) {
                ModelMesh.material.SetColor("_Color", Color.red);     //设置受击变色

            }
            //播放特效
            if (IfHitEffect) {
                if (HitEffect != null)
                {
                    GameObject effect = (GameObject)Instantiate(HitEffect);
                    effect.transform.SetParent(HitEffectt_Position.transform);

                    //重置特效位置
                    effect.transform.localPosition = new Vector3(0, 0, 0);
                    HitStatus = true;
                }

                //清空数据
                IfHitEffect = false;
            }

            //战斗飘字
			int damgeValue = LastHp - ai_property.AI_Hp;
            LastHp = LastHp - damgeValue;

			
			GameObject HitObject_p = (GameObject)Instantiate(HitObject);
			Text label = HitObject_p.GetComponent<Text>();
            if (HitCriStatus)
            {
                HitCriStatus = false;
                label.text = "暴击  -" + damgeValue.ToString();
            }
            else {
                label.text = "-" + damgeValue.ToString();
            }
			
            //伤害为0表示闪避
            if (damgeValue == 0) {
                label.text = "闪避";
            }

            if (UI_Hp != null) {
                //HitObject_p.transform.parent = UI_Hp.transform;
                HitObject_p.transform.SetParent(UI_Hp.transform);
                HitObject_p.transform.localPosition = new Vector3(0, 40, 0);
                HitObject_p.transform.localScale = new Vector3(1, 1, 1);
            }
			HitStatus=false;
		}

        if (hitMeshStatus)
        {
            hitMeshTime = hitMeshTime + Time.deltaTime;
            if (hitMeshTime > 0.15f) {
                if (ModelMesh != null) {
                    ModelMesh.material.SetColor("_Color", Color.white);     //设置受击变色
                    hitMeshStatus = false;
                    hitMeshTime = 0;
                }
            }
        }

        //判定自己是否为召唤物,如果父级为返回或死亡状态,则删除自己
        if (MonsterCreateObj != null) {
            string ai_statusValue = MonsterCreateObj.GetComponent<AI_1>().AI_Status;
            if (ai_statusValue == "1" || ai_statusValue == "4" || ai_statusValue == "5") {
                this.GetComponent<AI_Property>().AI_Hp = 0;
            }
        }
	}

    //AI近战攻击
	public void AIAct(){
		Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseIfHit = true;      //播放受击特效
		Game_PublicClassVar.Get_fight_Formult.MonsterActRose("60020001", this.gameObject);                      //战斗
        actTriggerSkill("2", "1");
	}

    //AI远程攻击
    public void AIAct_Remote() {
        actTriggerSkill("2","1");
    }

    //触发被动技能,triggerTime表示触发时间,1 表示起手动作前 ,2表示起手动作后    triggerType表示触发类型 1：表示攻击时触发  2：表示每帧都监测一次
    void actTriggerSkill( string triggerTime,string triggerType ) {
        //判定触发被动技能
        if (skillID[0] != "0")
        {
            for (int i = 0; i <= skillID.Length - 1; i++)
            {
                //Debug.Log("循环技能" + skillID[i]);
                //获取技能触发类型
                string skillType = passiveSkillType[i];
                switch (skillType)
                {
                    //每次攻击概率触发
                    case "1":
                        //如果当前技能不是1直接跳出
                        if (triggerType != "1")
                        {
                            break;
                        }

                        switch (passiveSkillTriggerTime[i]) {
                            //每次有攻击行为时,无限定条件
                            case "0":
                                if (Random.value <= float.Parse(passiveSkillPro[i]))
                                {
                                    /*
                                    if (skillID[i] == "62000201")
                                    {
                                        Debug.Log("620000201 = " + passiveSkillTriggerTime[i]);
                                    }
                                    */
                                    //if (actStatus)
                                    //{
                                        triggerSkill(i);        //触发怪物技能
                                    //}
                                }
                            break;
                            //每次有攻击行为时触发，必须与传入的值一样,才会触发
                            case "1":
                                if (passiveSkillTriggerTime[i] == triggerTime) {

                                    //if (actStatus)
                                    //{
                                        //actStatus = false;
                                        if (Random.value <= float.Parse(passiveSkillPro[i]))
                                        {
                                            triggerSkill(i);        //触发怪物技能
                                        }
                                    //}
                                }

                            break;
                            //每次有攻击动作时触发，必须与传入的值一样,才会触发
                            case "2":
        
                                if (passiveSkillTriggerTime[i] == triggerTime) {

                                    if (Random.value <= float.Parse(passiveSkillPro[i]))
                                    {
                                        triggerSkill(i);        //触发怪物技能
                                    }
                                }

                            break;
                        }

                        break;
                    //血量低于多少百分比触发
                    case "2":
                        //如果当前技能不是2直接跳出
                        if (triggerType != "2")
                        {
                            break;
                        }
                        //获取自身生命值
                        if (ai_property.AI_Hp <= ai_property.AI_HpMax * float.Parse(passiveSkillPro[i]))
                        {
                            if (ifSkillTrigger[i] == "0")
                            {
                                triggerSkill(i);            //触发怪物技能
                            }
                        }
                        break;
                }
            }
        }

        //清除状态
        //actStatus = false;
    }
    //参数 i 当前触发第几个技能
    void triggerSkill(int i) {

        bool triggerSkillStatus = false;

        if (ifSkillTrigger[i] == "0")
        {
            //防止技能执行多次
            if (passiveSkillTriggerOnce[i] == "1")
            {
                ifSkillTrigger[i] = "1";
            }

            //判定技能是否有CD,有CD设定不能触发
            if (passiveSkillTriggerTimeCD[i] != "0")
            {
                ifSkillTrigger[i] = "1";
                passiveSkillTriggerTimeCDSum[i] = "0";  //清空技能CD累计时间
            }

            triggerSkillStatus = true;
        }

        if (triggerSkillStatus) {

            //触发BUFF
            //string skillObjName = "Monster_FireWall_1";
            string skillObjName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", skillID[i], "Skill_Template");
            //skillObjName = "Monster_FireWall_1";
            //Debug.Log("skillID[i] = " + skillID[i]);
            GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill_Obj/" + skillObjName, typeof(GameObject));
            GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);
            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = AI_Target;
            string skillParent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParent", "ID", skillID[i], "Skill_Template");
            //Debug.Log("触发技能+" + skillID[i]);
            /*
            if (skillID[i] == "62000103") {
                Debug.Log("触发召唤技能");
            }
            */
            //skillParent = "2";      //测试无绑定点用的,后面需删掉
            switch (skillParent)
            {
                //绑定在身上
                case "0":
                    //目前只支持对自己附加
                    //Debug.Log("技能挂在自己身上+" + skillID[i]);
                    string skillParentPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID[i], "Skill_Template");
                    SkillObject_p.transform.SetParent(this.GetComponent<AI_1>().BoneSet.transform.Find(skillParentPosition));
                    SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);
                    SkillObject_p.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID[i];
                    SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                    SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;
                    triggerSkillID = skillID[i];
                    SkillStatus = true;         //开启技能状态
                    break;
                //无绑定点
                case "1":
                    //目前只支持对攻击目标区域释放
                    //获取攻击目标位置
                    Vector3 skillPosition = AI_Target.transform.position;
                    SkillObject_p.transform.position = skillPosition;
                    //skillID[i] = "60090002";
                    SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID[i];
                    SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                    SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;

                    SkillStatus = true;         //开启技能状态
                    triggerSkillID = skillID[i];
                    break;

                //无绑定点,释放起始位置位于AI中心
                case "2":
                    //目前只支持对攻击目标区域释放
                    //获取攻击目标位置
                    skillPosition = AI_Target.transform.position;
                    string playStartPoisition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID[i], "Skill_Template");
                    SkillObject_p.transform.position = BoneSet.transform.Find(playStartPoisition).transform.position;
                    //skillID[i] = "60090002";
                    SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID[i];
                    SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                    SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = AI_Target;

                    SkillStatus = true;         //开启技能状态
                    triggerSkillID = skillID[i];
                    break;
            }
        }
    }

    //出现怪物复活时间条
    void showMonsterDeathTimeUI() {
        Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
        //Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(ai_DeathTimePosition.position);
        monsterDeathTimeObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_MonsterDeathTime);
        monsterDeathTimeObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet.transform);
        monsterDeathTimeObj.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
        monsterDeathTimeObj.transform.localScale = new Vector3(1f, 1f, 1f);
        float deathTime = 3600;
        if (AI_ID_Only != "" && AI_ID_Only != "0")
        {
            //更新在线刷新时间
            float rebirthTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(AI_ID_Only);
            if (rebirthTime != 0)
            {
                deathTime = rebirthTime;
                //Debug.Log("*************************************************************************");
                //monsterRebirthTime = rebirthTime;
                monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().DeathRebirthTimeSum = monsterRebirthTime - rebirthTime;
            }
            //更新离线刷新时间
            float offLineTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathOffLineTime(AI_ID_Only);
            if (offLineTime != 0)
            {
                monsterOffLineTime = offLineTime;
            }
        }
        monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().DeathTime = monsterRebirthTime;
        monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().deathMonsterName = AI_Name;
        monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().Obj_MonsterObj = this.gameObject;
        monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().OffLineTime = monsterOffLineTime;
        AI_MonsterDeathUIStatus = true;     //更新显示
        //AI_MonsterDeathStatus = true;     
        ai_IfDeathTimeStatus = true;        //设置怪物死亡复活状态
    
    }
}
