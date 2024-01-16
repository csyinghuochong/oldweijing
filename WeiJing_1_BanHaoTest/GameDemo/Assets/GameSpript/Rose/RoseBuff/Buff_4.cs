using UnityEngine;
using System.Collections;

//增加和减少属性的BUFF
public class Buff_4 : MonoBehaviour {

    public string BuffID;                   //BuffID

    private GameObject SkillEffect;         //技能特效
    private float buffTime;                 //技能特效播放时间
    private float buffTimeSum;              //技能持续时间累计值
    private float damgePro;                 //攻击转换百分百
    private int damgeValue;                 //固定值
    private string[] continuedTime;            //2次持续扣血时间的间隔
    private float continuedTimeSum;         //2次间隔时间的累计时间
    private string ifImmediatelyUse;        //是否立即释放
    private string targetType;              //目标类型
    private GameObject effect;

    public string propretyType;            //属性类型
    public string propretyAddType;         //属性加成类型  1：固定值 2：百分比
    public string propretyAddValue;        //改变属性的值,支持负号
    private int propretyValue_1;
    private int propretyValue_2;

    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {
        //BuffID = "90010004";
        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();

        //Debug.Log("Buff4触发的ID：" + BuffID);
        //获取Buff参数
        continuedTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffParameter", "ID", BuffID, "SkillBuff_Template").Split(',');
        propretyType = continuedTime[0];
        propretyAddType = continuedTime[1];
        propretyAddValue = continuedTime[2];

        //获取治疗值
        damgePro = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgePro", "ID", BuffID, "SkillBuff_Template"));
        damgeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", BuffID, "SkillBuff_Template"));

        //获取Buff持续时间
        buffTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffTime", "ID", BuffID, "SkillBuff_Template"));
        //buffTime = 100;
        //获取Buff是否立即释放
        ifImmediatelyUse = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfImmediatelyUse", "ID", BuffID, "SkillBuff_Template");

        //获取技能目标
        targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", BuffID, "SkillBuff_Template");

        //播放Buff特效
        string ifPlayEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfPlayEffect", "ID", BuffID, "SkillBuff_Template");
        if (ifPlayEffect == "1")
        {
            //获取特效名称
            string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", BuffID, "SkillBuff_Template");
            //获取播放点
            string effectPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectPosition", "ID", BuffID, "SkillBuff_Template");
            //实例化技能特效
            if (effectName != "0") {
                //Debug.Log("开始实例化技能");
                GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
                effect = (GameObject)Instantiate(SkillEffect);
                effect.SetActive(false);
                //根据Buff目标绑定不同的位置
                if (targetType == "1")
                {
                    Rose_Bone bone = this.GetComponent<Rose_Bone>();
                    Transform effectTra = bone.BoneSet.transform.Find(effectPosition).transform;
                    effect.transform.parent = effectTra;
                }
                if (targetType == "2")
                {
                    AI_1 bone = this.GetComponent<AI_1>();
                    Transform effectTra = bone.BoneSet.transform.Find(effectPosition).transform;
                    effect.transform.parent = effectTra;
                }

                effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                effect.SetActive(true);
            }
        }

        //触发一次Buff
        buffUse();
	}
	
	// Update is called once per frame
	void Update () {
        
        buffTimeSum = buffTimeSum + Time.deltaTime;
        /*
        continuedTimeSum = continuedTimeSum + Time.deltaTime;
        if (continuedTimeSum >= continuedTime) {
            continuedTimeSum = 0;
            //触发一次Buff
            buffUse();
        }
         */
        //注销自己的脚本
        if (buffTimeSum > buffTime) {
            //注销特效
            if (effect != null)
            {
                Destroy(effect);
            }
            //注销自己  
            Destroy(this);   
        }

        //获取绑定对象的生命是否为0，为0删除自身脚本及特效
        bool ifdeath = Game_PublicClassVar.Get_function_Skill.ifDeath(targetType,this.gameObject);
        if (ifdeath)
        {
            Destroy(effect);
            Destroy(this);
        }
	}

    //销毁脚本时调用
    void OnDestroy() {
        //恢复属性
        propretyBuff(propretyType, propretyAddType, propretyAddValue, targetType, true);
        //注销特效
        if (effect != null) {
            Destroy(effect);
            //更新AI属性
            AI_Property aiProprety = this.gameObject.GetComponent<AI_Property>();
            if (aiProprety != null) {
                aiProprety.UpdataAIProperty = true;
            }
            
        }
        //Debug.Log(this.gameObject.name + "脚本销毁了！");
    
    }

    //触发一次Buff效果
    void buffUse() {

        propretyBuff(propretyType, propretyAddType, propretyAddValue, targetType, false);

        //玩家自己触发伤害及眩晕
        if (targetType == "1")
        {
            Game_PublicClassVar.Get_function_Rose.costRoseHp(damgeValue);
            //Debug.Log("触发扣血");
            
        }
        //怪物加血
        if (targetType == "2") {
            Game_PublicClassVar.Get_function_AI.AI_costHp(this.gameObject, damgeValue);
            //Game_PublicClassVar.Get_function_Skill.XuanYunBuff(buffTime, this.gameObject, "2");
        }
    }

    //触发一次Buff
    void propretyBuff(string propretyBuffType, string propretyBuffAddType, string propretyBuffValue, string targetType,bool ifExitBuff) {
        
        int buffValue_add = 0;
        float buffValue_mul = 0;
        switch (propretyBuffAddType)
        {
                //固定值
                case "1":
                    buffValue_add = int.Parse(propretyBuffValue);
                break;

                //百分比
                case "2":
                    buffValue_mul = float.Parse(propretyBuffValue);
                break;
            }

        
        //退出Buff时,把参数都设置为配置的负数
        if (ifExitBuff) {
            buffValue_add = buffValue_add * -1;
            buffValue_mul = buffValue_mul * -1;
        }

        //玩家
        if (targetType == "1")
        {
            
            Rose_Proprety roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
            switch (propretyBuffType) { 

                //物理最小攻击
                case "11":
                    switch (propretyBuffAddType)
                    {
                        //固定值
                        case "1":
                            roseProprety.Rose_ActMinAdd_2 = roseProprety.Rose_ActMinAdd_2 + buffValue_add;
                            roseProprety.Rose_ActMaxAdd_2 = roseProprety.Rose_ActMaxAdd_2 + buffValue_add;
                        break;
                        //百分比
                        case "2":
                            roseProprety.Rose_ActMinMul_1 = roseProprety.Rose_ActMinMul_1 + buffValue_mul;
                            roseProprety.Rose_ActMaxMul_1 = roseProprety.Rose_ActMaxMul_1 + buffValue_mul;
                        break;
                    }
                break;

                //物理防御
                case "17" :
                    switch (propretyBuffAddType)
                    {
                        //固定值
                        case "1":
                            roseProprety.Rose_DefMinAdd_2 = roseProprety.Rose_DefMinAdd_2 + buffValue_add;
                            roseProprety.Rose_DefMaxAdd_2 = roseProprety.Rose_DefMaxAdd_2 + buffValue_add;
                        break;
                        //百分比
                        case "2":
                            roseProprety.Rose_DefMinMul_1 = roseProprety.Rose_DefMinMul_1 + buffValue_mul;
                            roseProprety.Rose_DefMaxMul_1 = roseProprety.Rose_DefMaxMul_1 + buffValue_mul;
                        break;
                    }
                break;

                //魔法防御
                case "20":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_AdfMinAdd_2 = roseProprety.Rose_AdfMinAdd_2 + buffValue_add;
                        roseProprety.Rose_AdfMaxAdd_2 = roseProprety.Rose_AdfMaxAdd_2 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_AdfMinMul_1 = roseProprety.Rose_AdfMinMul_1 + buffValue_mul;
                        roseProprety.Rose_AdfMaxMul_1 = roseProprety.Rose_AdfMaxMul_1 + buffValue_mul;
                        break;
                }
                break;


                //暴击
                case "30":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_CriMul_1 = roseProprety.Rose_CriMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_CriMul_1 = roseProprety.Rose_CriMul_1 + buffValue_add;
                        break;
                }
                break;

                //命中
                case "31":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_HitMul_1 = roseProprety.Rose_HitMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_HitMul_1 = roseProprety.Rose_HitMul_1 + buffValue_add;
                        break;
                }
                break;

                //闪避
                case "32":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_DodgeMul_1 = roseProprety.Rose_DodgeMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_DodgeMul_1 = roseProprety.Rose_DodgeMul_1 + buffValue_add;
                        break;
                }
                break;

                //物理免伤
                case "33":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_DefMul_1 = roseProprety.Rose_DefMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_DefMul_1 = roseProprety.Rose_DefMul_1 + buffValue_add;
                        break;
                }
                break;

                //魔法免伤
                case "34":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_AdfMul_1 = roseProprety.Rose_AdfMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_AdfMul_1 = roseProprety.Rose_AdfMul_1 + buffValue_add;
                        break;
                }
                break;

                //速度
                case "35":
                //Debug.Log("开始出发加速效果");
                switch (propretyBuffAddType) { 
                    //固定值
                    case "1":
                        roseProprety.Rose_MoveSpeedAdd_2 = roseProprety.Rose_MoveSpeedAdd_2 + buffValue_add;
                    break;
                    //百分比
                    case "2":
                        roseProprety.Rose_MoveSpeedMul_1 = roseProprety.Rose_MoveSpeedMul_1 + buffValue_mul;
                    break;
                }
                break;

                //伤害免伤
                case "36":
                //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_DamgeSubtractMul_1 = roseProprety.Rose_DamgeSubtractMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_DamgeSubtractMul_1 = roseProprety.Rose_DamgeSubtractMul_1 + buffValue_mul;
                        break;
                }
                break;

            }

            //调整完属性时,更新当前角色属性
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseProperty = true;

        }
        //怪物
        if (targetType == "2")
        {
            AI_Property aiProprety = this.gameObject.GetComponent<AI_Property>();
            aiProprety.UpdataAIProperty = true;
            switch (propretyBuffType)
            {
                //攻击
                case "11":

                    aiProprety.ActMul = aiProprety.ActMul + buffValue_mul;

                    break;

                //物理防御
                case "17":

                    aiProprety.DefMul = aiProprety.DefMul + buffValue_mul;

                    break;

                //魔法防御
                case "20":
                    aiProprety.AdfMul = aiProprety.AdfMul + buffValue_mul;
                    break;


                //暴击
                case "30":
                    //暂时不做暴击
                    break;

                //命中
                case "31":
                    
                    //暂时不做命中
                    break;
                //闪避
                case "32":
                    //暂时不做闪避
                    break;
                //物理免伤
                case "33":
                    aiProprety.DefMul = aiProprety.DefMul + buffValue_mul;
                    break;
                //魔法免伤
                case "34":
                    aiProprety.AdfMul = aiProprety.AdfMul + buffValue_mul;
                    break;
                //速度
                case "35":
                    aiProprety.MoveSpeedMul = aiProprety.MoveSpeedMul + buffValue_mul;

                    break;
                //伤害免伤
                case "36":
                    aiProprety.AI_DamgeAdd = aiProprety.AI_DamgeAdd + buffValue_mul;
                    break;
            }
        }
        
    }
}

