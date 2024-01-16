using UnityEngine;
using System.Collections;

//施法状态脚本，用于玩家选择施法区域时调用此脚本
public class RoseSkill_Sing_1 : MonoBehaviour {

    //public float SkillRange;                  //技能范围
    private GameObject SkillEffect;             //技能特效
    private GameObject SkillObj;                //触发技能的OBJ
    public bool SkillCDStatus_Sing;             //技能CD
    public bool IfSkillSelect;                  //技能是否需要技能释放区域  true：表示需要  false：不需要
    //public bool SkillCD_Status;               //技能CD开关,技能开关开启后,主界面技能图标刷新CD显示

    private string skillID;                     //技能ID
    private int SkillDamge;                     //技能伤害
    private Vector3 SkillTargetPoint;           //技能释放点
    private Transform AI_MousterSet;            //附近怪物的集合
    private bool SkillSelectStatus;             //技能选中施法区域是否开启
    private bool ifSkillOpen;                   //技能是否开始释放
    private float skillDelay;                   //技能延迟时间,配合动作
    private float skillDelaySum;                //技能延迟计数器
    private GameObject skilleffect;             //内部实例化用到的技能特效
    private GameObject SkillSelectRangeEffect;   //技能选中范围特效
    private bool SelectRangeEffectStatus;       //技能选中特效状态
    private GameObject effect;                  //实例化的技能特效
    private GameObject SkillObject_p;           //实例化的技能GameObject控制体
    public bool UpdateUseOnce;                  //此开关控制在Update中使用一次脚本控制的方法
    private bool ifPublicSkillStatus;           //是否触发公共CD
    private bool playAnimationOnce;             //只播放一次动作
    public string SkillUseType;                 //释放技能的人的类型, 1：玩家释放技能  2：怪物释放技能
    public int ChuMoFingerId;                   //多点触摸的唯一ID

    //施法前吟唱
    public bool IfSkillFrontSingStatus;              //技能施法中吟唱状态
    private float skillFrontSingTime;                //技能施法中的吟唱时间
    public bool IfWaitSkillFrontSing;                //等待技能施法前吟唱状态完成
    public bool IfSkillFrontFail;                        //技能是否吟唱失败

    //绑点专用
    private GameObject gameStartVar;
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {

        //表示只更新一次
        UpdateUseOnce = false;
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;

        SkillUseType = "1";     //默认为玩家技能 以后需要修改，考虑怪物放技能因素
	}
	
	// Update is called once per frame
	void Update () {

        //获取技能ID
        skillID = this.gameObject.GetComponent<MainUI_SkillGrid>().UseSkillID;

        //开启技能选中施法范围
        if (IfSkillSelect)
        {
            //实例化,只执行一次
            if (!UpdateUseOnce)
            {
                //Debug.Log("实例化选中特效");
                //将角色设定为施法状态
                Rose_Status rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
                rose_Status.SkillStatus = true;

                //实例化选中特效
                SkillSelectRangeEffect = (GameObject)Resources.Load("Effect/Rose/Rose_SelectRange", typeof(GameObject));        //实例化范围特效
                effect = (GameObject)Instantiate(SkillSelectRangeEffect);
                effect.transform.parent = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.Find("SclectEffectSet");
                float rangeSize = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillRangeSize", "ID", skillID, "Skill_Template"));
                effect.GetComponent<Rose_SelectRange>().RangeSize = rangeSize;

                //技能范围选择状态开启,后面执行循环
                SkillSelectStatus = true;

                //技能范围选择特效开启
                SelectRangeEffectStatus = false;

                //技能选中特效范围
                string damgeRangeType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeRangeType", "ID", GetComponent<MainUI_SkillGrid>().UseSkillID, "Skill_Template");
                ParticleSystem aa = effect.GetComponent<ParticleSystem>();

                UpdateUseOnce = true;
            }

            if (SkillSelectStatus)
            {
                //Debug.Log("技能选择范围中111");
                //技能选择范围，当触发鼠标点击,下面的特效变量会注销掉
                if (!SelectRangeEffectStatus)
                {
                    //Debug.Log("技能选择范围中222");
                    //不断修正选中范围的位置
                    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Input.touchCount <= 1) {
                        ChuMoFingerId = 0;
                    }

                    Ray ray;
                    //Debug.Log("Application.platform = " + Application.platform);
                    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    }
                    else {
                        ray = Camera.main.ScreenPointToRay(Input.GetTouch(ChuMoFingerId).position);
                    }
                    
                    //Debug.Log("ChuMoFingerId = " + ChuMoFingerId);
                    RaycastHit ray_hit;

                    if (Physics.Raycast(ray, out ray_hit))
                    {
                        Vector3 SelectRangeEffect = ray_hit.point;
                        if (effect != null) {
                            effect.transform.position = new Vector3(SelectRangeEffect.x, SelectRangeEffect.y + 0.1f, SelectRangeEffect.z);
                        }
                        
                    }

                    //Debug.Log("开始移动！");
                }



                //鼠标点击，确认释放技能
                //if (Input.GetMouseButtonUp(0))
                bool ifTriggerSkill = false;
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        ifTriggerSkill = true;
                    }
                }
                else {
                    if (Input.GetTouch(ChuMoFingerId).phase == TouchPhase.Ended) {
                        ifTriggerSkill = true;
                    }
                }
                if (ifTriggerSkill)
                {
                    //Debug.Log("停止移动");
                    //鼠标点击取消角色施法状态
                    Rose_Status rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
                    rose_Status.SkillStatus = false;

                    //鼠标点击后注销特效光圈
                    Destroy(effect);
                    SelectRangeEffectStatus = true;
                    
                    //开启刷新技能冷却CD
                    if (SkillCDStatus_Sing)
                    {
                        this.gameObject.GetComponent<MainUI_SkillGrid>().skillCDSelfStatus = true;
                    }

                    //从摄像机处向点击的目标处发送一条射线
                    Ray Move_Target_Ray;
                    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        Move_Target_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    }
                    else { 
                        Move_Target_Ray = Camera.main.ScreenPointToRay(Input.GetTouch(ChuMoFingerId).position);
                    }

                    //获取当前鼠标状态,判定是否要对目标直接释放
                    if (this.GetComponent<MainUI_SkillGrid>().MouseEnterStatus)
                    {
                        //获取当前攻击目标
                        GameObject actObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
                        if (actObj != null)
                        {
                            //判定与目标的距离
                            float dis = Vector3.Distance(actObj.transform.position, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                            if (dis <= 20)
                            {
                                Vector3 viewPosition = Camera.main.WorldToScreenPoint(actObj.transform.position);
                                Move_Target_Ray = Camera.main.ScreenPointToRay(viewPosition);
                            }
                        }
                    }

                    //声明一个光线触碰器
                    RaycastHit Move_Target_Hit;

                    //检测射线是否碰触到对象
                    //标记OUT变量在传入参数进去后可能发生改变，和ref类似，但是ref需要给他一个初始值
                    //第一个参数射线变量  第二个参数光线触碰器的反馈变量

                    //设定射线只碰撞Terrain层级
                    LayerMask mask = 1 << 10;
                    if (Physics.Raycast(Move_Target_Ray, out Move_Target_Hit, 200, mask))
                    {

                        //取得鼠标点击的位置为释放技能的范围中心点
                        SkillTargetPoint = Move_Target_Hit.point;


                        //技能释放距离角色要小于50距离
                        if (Vector3.Distance(SkillTargetPoint, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position) < 50.0f)
                        {
                            ifSkillOpen = true;
                            UpdateUseOnce = false;
                            //将释放技能是,使其朝向面对选中的区域
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.LookAt(Move_Target_Hit.point);
                        }
                        else
                        {
                            //Debug.Log("释放法术的距离过长,应在角色50米内");
                        }
                    }
                }
            }
        }
        //表示技能不需要选中区域的操作，可以直接施放
        else
        {
            ifSkillOpen = true;
            //设置释放技能是自身朝向目标释放,并做动作
            string ifLookAtTarget = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfLookAtTarget", "ID", skillID, "Skill_Template");
            if (ifLookAtTarget == "1")
            {
                if (game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget != null) {
                    Transform objActTarget = game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget.GetComponent<AI_1>().BoneSet.transform.Find("Center").transform;
                    Vector3 lookVec3 = new Vector3(objActTarget.position.x, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.y, objActTarget.position.z);
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.LookAt(lookVec3);
                }
            }
        }


        //技能打开释放技能，处理技能延迟播放
        if (ifSkillOpen) {
            //Debug.Log("ifSkillOpen! ifSkillOpen! ifSkillOpen!");
            //关闭施法前吟唱状态
            //IfSkillFrontSingStatus = false;

            //关闭选择施法区域的状态
            IfSkillSelect = false;
 
            //更新技能延迟时间
            skillDelay = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillDelayTime", "ID", skillID, "Skill_Template"));


            //延迟施法处理(执行一次)
            if (!IfSkillFrontSingStatus)
            {
                //技能施法前是否需要吟唱
                if (ifSkillOpen)
                {
                    skillFrontSingTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillFrontSingTime", "ID", skillID, "Skill_Template"));
                    if (skillFrontSingTime > 0)
                    {
                        IfSkillFrontSingStatus = true;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingStatus = true;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingTime = skillFrontSingTime;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingSkillObj = this.gameObject;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingType = "1";
                    }
                    else {
                        IfWaitSkillFrontSing = true;
                    }
                }
            }

            //等待施法失败
            if (IfSkillFrontFail)
            {
                Debug.Log("技能施法失败");
                IfSkillFrontFail = false;
                IfSkillFrontSingStatus = false;
                cleanSkillData();
            }



            //等待判定施法状态是否完成
            if (IfWaitSkillFrontSing)
            {
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingStatus = false;       //设置角色吟唱结束
                IfSkillFrontSingStatus = false;

                //累计延迟时间
                skillDelaySum = skillDelaySum + Time.deltaTime;

                //是否播放技能动作
                if (!playAnimationOnce) {

                    //设置动作僵直时间,播放技能动画
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().SkillAnimationStatus = true;
                    float skillRigidity = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillRigidity", "ID", skillID, "Skill_Template"));
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().SkillAnimationTime = skillRigidity;
                    //设置技能ID
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseNowUseSkillID = skillID;
                    //获取技能对应的自身动作名称
                    string animationName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillAnimation", "ID", skillID, "Skill_Template");
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseNowUseSkillAnimationName = animationName;
                    playAnimationOnce = true;

                    //设置释放技能是自身朝向目标释放,并做动作
                    string ifLookAtTarget = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfLookAtTarget", "ID", skillID, "Skill_Template");
                    if (ifLookAtTarget == "1") {

                        switch (SkillUseType)
                        {
                            //玩家施放的技能
                            case "1":
                                //设置玩家朝向目标
                                if (game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget != null)
                                {
                                    Transform objActTarget = game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget.GetComponent<AI_1>().BoneSet.transform.Find("Center").transform;
                                    Vector3 lookVec3 = new Vector3(objActTarget.position.x, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.y, objActTarget.position.z);
                                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.LookAt(lookVec3);
                                }
                                break;
                            //怪物释放的技能(以后怪物技能补充)
                            case "2":

                                break;
                        }
                    }
                }

                //技能延迟,消耗道具
                if (skillDelaySum >= skillDelay)
                {
                    skillOpen();        //执行技能
                    /*
                    string skillSingTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillSingTime", "ID", skillID, "Skill_Template");
                    if (skillSingTime == "0") {
                        cleanSkillData();       //清理技能数据
                    }
                     */
                    cleanSkillData();       //清理技能数据
                    this.GetComponent<MainUI_SkillGrid>().skillCDSelfStatus = true;
                    //开启消耗道具
                    this.GetComponent<MainUI_SkillGrid>().ItemCostStatus = true;

                    //触发技能后触发技能公共冷却CD
                    bool ifPublicSkillStatus = false;
                    string ifPublicSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfPublicSkillCD", "ID", skillID, "Skill_Template");
                    if (ifPublicSkill == "0")
                    {
                        ifPublicSkillStatus = true;
                    }
                    else {
                        ifPublicSkillStatus = false;
                    }
                    if (ifPublicSkillStatus)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Rose_PublicSkillCDStatus = true;
                    }
                }
            }
        }
	}

    //将技能信息实例化出来
    void skillOpen() {

        //设置实例化技能
        string skillObjName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", skillID, "Skill_Template");
        SkillObj = (GameObject)Resources.Load("Effect/Skill_Obj/" + skillObjName, typeof(GameObject));
        SkillObject_p = (GameObject)Instantiate(SkillObj);
        //设定父节点
        string skillParent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParent", "ID", this.GetComponent<MainUI_SkillGrid>().UseSkillID, "Skill_Template");
        switch (skillParent)
        {
            case "0":
                //获取存放的点
                string positionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", this.GetComponent<MainUI_SkillGrid>().UseSkillID, "Skill_Template");
                SkillObject_p.transform.parent = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().BoneSet.transform.Find(positionName).transform;
                SkillObject_p.transform.localPosition = Vector3.zero;
                //Debug.Log("实例化特效位置");
                break;

            case "1":
                //SkillObject_p.transform.parent = this.transform;
                SkillObject_p.transform.localPosition = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                //Debug.Log("实例化特效位置skillObjName = " + skillObjName + "SkillObject_p = " + SkillObject_p.transform.position);
                break;
        }
        
        //设置技能位置
        SkillObject_p.transform.localRotation = this.transform.rotation;
        //SkillObject_p.transform.rotation = Quaternion.Euler(Vector3.zero);
        SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
        //传递技能对应的值
        SkillObjBase skillStatus = SkillObject_p.transform.GetComponent<SkillObjBase>();
        skillStatus.SkillTargetPoint = SkillTargetPoint;        //技能目标点
        skillStatus.SkillID = skillID;
        //skillStatus.DamgeValue_Fixed = SkillDamge;              //技能伤害

        skillStatus.SkillOpen = true;   //开启技能，需要在设置完值后开启技能

        switch (SkillUseType) { 
            //玩家施放的技能
            case "1":
                //设置释放技能的目标
                skillStatus.SkillTargetObj = game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
                break;
            //怪物释放的技能(以后怪物技能补充)
            case "2":

                break;

        }

    }

    //清理技能数据
    void cleanSkillData() {
        //Debug.Log("清理技能施法数据");
        ifSkillOpen = false;
        skillDelaySum = 0.0f;
        this.enabled = false;
        playAnimationOnce = false;
        IfWaitSkillFrontSing = false;
        //获取当前技能是否需要选中释放区域
        /*
        string ifSkillRange = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfSelectSkillRange", "ID", skillID, "Skill_Template");
        if (ifSkillRange == "0")
        {
            this.GetComponent<RoseSkill_Sing_1>().IfSkillSelect = false;
        }
        if (ifSkillRange == "1")
        {
            Debug.Log("需要施法范围zxzzzzzzzzzzzzzzzzzzzzzzzzzzz");
            this.GetComponent<RoseSkill_Sing_1>().IfSkillSelect = true;
            //this.GetComponent<RoseSkill_Sing_1>().ChuMoFingerId = 0;
        }
        */
    }

}
