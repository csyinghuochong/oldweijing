using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class Rose_Proprety : MonoBehaviour {

    //定义角色属性，方便其他脚本调用
    public string Rose_Name;                    //名称
    public string Rose_Occupation;              //职业
    public ObscuredInt Rose_Lv;                         //角色等级
    public bool Rose_GetExp;                    //角色每获得一次经验打开此开关
    public ObscuredInt Rose_ExpNow;                     //角色当前等级经验
    public ObscuredInt Rose_Exp;                        //角色当前升级需要经验
    public ObscuredInt Rose_Hp;                         //角色拥有血量
    public ObscuredInt Rose_HpNow;                      //角色当前血量    
    public ObscuredInt Rose_ActMin;                     //角色最小攻击
    public ObscuredInt Rose_ActMax;                     //角色最大攻击
    public ObscuredInt Rose_DefMin;                     //角色最小物防
    public ObscuredInt Rose_DefMax;                     //角色最大物防
    public ObscuredInt Rose_AdfMin;                     //角色最小魔防
    public ObscuredInt Rose_AdfMax;                     //角色最大魔防
    public ObscuredFloat Rose_HealHpValuePro;           //角色恢复最大生命的比例
    public ObscuredInt Rose_HealHpValue;                //角色恢复的固定值
    public ObscuredFloat Rose_HealHpTime;               //角色2次恢复生命的时间
    private ObscuredFloat rose_HealHpTimeSum;
    private Rose_Status rose_Status;

    public ObscuredInt Rose_Act;                        //角色攻击
    public ObscuredInt Rose_Def;                        //角色防御
    public ObscuredInt Rose_Adf;                        //角色魔防
    public ObscuredFloat Rose_Cri;                      //角色暴击
    public ObscuredFloat Rose_Hit;                      //角色命中
    public ObscuredFloat Rose_Dodge;                    //角色闪避
    public ObscuredFloat Rose_DefAdd;                   //角色物理免伤
    public ObscuredFloat Rose_AdfAdd;                   //角色魔法免伤
    public ObscuredFloat Rose_DamgeSub;                 //角色免伤值
    public ObscuredFloat Rose_DamgeAdd;                 //角色伤害加成
    public ObscuredFloat Rose_Lucky;                    //角色幸运值

    public ObscuredFloat Rose_MoveSpeed;                //角色移动速度
    private GameObject Obj_UI_RoseHp;           //角色Hp的UI
    public bool updataOnly;                     //更新标题显示
    public bool updateLuckValue;                //更新幸运值
    public float updatePrppertyValue;

    public bool HuDunStatus;                    //护盾状态
    public int HuDunValue;                      //护盾值
    public float HuDunTime;                     //护盾时间
    public float HudunTimeSum;                  //护盾时间累计
    public GameObject HuDunEffect;              //护盾特效

    //血量上限计算系数
    public int Rose_HpAdd_1;            //加法，在公式的内测
    public int Rose_HpAdd_2;            //加法，在公式的最外侧
    public int Rose_HpMul_1;            //乘法，在公式的外侧

    //最小攻击计算系数
    public int Rose_ActMinAdd_1;       //加法，在公式的内测
    public int Rose_ActMinAdd_2;       //加法，在公式的最外侧
    public float Rose_ActMinMul_1;     //乘法，在公式的外侧

    //最大攻击计算系数
    public int Rose_ActMaxAdd_1;       //加法，在公式的内测
    public int Rose_ActMaxAdd_2;       //加法，在公式的最外侧
    public float Rose_ActMaxMul_1;     //乘法，在公式的外侧

    //最小物防计算系数
    public int Rose_DefMinAdd_1;       //加法，在公式的内测
    public int Rose_DefMinAdd_2;       //加法，在公式的最外侧
    public float Rose_DefMinMul_1;     //乘法，在公式的外侧

    //最大物防计算系数
    public int Rose_DefMaxAdd_1;       //加法，在公式的内测
    public int Rose_DefMaxAdd_2;       //加法，在公式的最外侧
    public float Rose_DefMaxMul_1;     //乘法，在公式的外侧

    //最小魔防计算系数
    public int Rose_AdfMinAdd_1;       //加法，在公式的内测
    public int Rose_AdfMinAdd_2;       //加法，在公式的最外侧
    public float Rose_AdfMinMul_1;     //乘法，在公式的外侧

    //最大魔防计算系数
    public int Rose_AdfMaxAdd_1;       //加法，在公式的内测
    public int Rose_AdfMaxAdd_2;       //加法，在公式的最外侧
    public float Rose_AdfMaxMul_1;     //乘法，在公式的外侧

    //暴击
    public float Rose_CriMul_1;        //乘法

    //命中
    public float Rose_HitMul_1;        //乘法

    //闪避
    public float Rose_DodgeMul_1;        //乘法

    //物理免疫
    public float Rose_DefMul_1;        //乘法

    //魔法免疫
    public float Rose_AdfMul_1;        //乘法

    //伤害减免
    public float Rose_DamgeSubtractMul_1;    //乘法

    //移动速度计算系数
    public int Rose_MoveSpeedAdd_1;     //加法，在公式的内测
    public int Rose_MoveSpeedAdd_2;     //加法，在公式的最外侧
    public float Rose_MoveSpeedMul_1;   //乘法，在公式的外侧
    public float Rose_MoveSpeedYaoGan;  //摇杆控制的系数
    public float rose_LastMoveSpeed;

    //public ObscuredInt zuobiInt;

	// Use this for initialization
	void Start () {
        Rose_HpNow = 10;    //防止从主城进入关卡直接触发死亡
        Obj_UI_RoseHp = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp;

		Rose_GetExp = true;
		//初始化角色当前属性
		Game_PublicClassVar.Get_game_PositionVar.UpdataRoseProperty = true;

        //初始化回血数据
        Rose_HealHpValuePro = 0.01f;
        Rose_HealHpValue = 1;
        Rose_HealHpTime = 5;

        rose_Status = this.GetComponent<Rose_Status>();
        Rose_MoveSpeedYaoGan = 1;
	}
	
	// Update is called once per frame
	void Update () {

	    //更新角色生命值UI
        float hpPro = float.Parse(Rose_HpNow.ToString()) / float.Parse(Rose_Hp.ToString());
        GameObject AA = Obj_UI_RoseHp.transform.Find("Img_Value").gameObject;
        Obj_UI_RoseHp.transform.Find("Img_Value").GetComponent<Image>().fillAmount = hpPro;


        //获取当前角色血量
        if (Rose_HpNow <= 0)
        {
            //设置死亡状态
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus == false) {
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus = true;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatusOnce = false;
            }
        }

        //更新当前经验
        if (Rose_GetExp) {
			Rose_GetExp = false;
			Rose_Lv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            Rose_Exp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", Rose_Lv.ToString(), "RoseExp_Template"));
            Rose_ExpNow = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        }

        //放在开始出会和开始的缓存有冲突
        if (!updataOnly)
        {
            //显示角色血量
            Rose_HpNow = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseHpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            this.GetComponent<Rose_Status>().rose_LastHp = Rose_HpNow;

            //显示角色名称
            string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Rose_Lv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            Obj_UI_RoseHp.transform.Find("Lab_RoseName").GetComponent<Text>().text = "等级：" + Rose_Lv + "  "+roseName;
            updataOnly = true;
        }


        updatePrppertyValue = updatePrppertyValue + Time.deltaTime;

        if (updatePrppertyValue >= 0.2f) {

            updatePrppertyValue = 0;

            //获取实际攻击
            Rose_Act = (int)((Rose_ActMax - Rose_ActMin) * Random.value) + Rose_ActMin;
            if (LuckActMax())
            {
                Rose_Act = Rose_ActMax;
            }

            //获取实际防御
            Rose_Def = (int)((Rose_DefMax - Rose_DefMin) * Random.value) + Rose_DefMin;
            //获取实际魔防
            Rose_Adf = (int)((Rose_AdfMax - Rose_AdfMin) * Random.value) + Rose_AdfMin;

            if (rose_Status.RoseStatus != "1" && rose_Status.RoseStatus != "2" && rose_Status.RoseStatus != "8")
            {
                //正常回血
                rose_HealHpTimeSum = rose_HealHpTimeSum + Time.deltaTime;
                if (rose_HealHpTimeSum >= Rose_HealHpTime)
                {
                    int healValue = (int)(Rose_Hp * Rose_HealHpValuePro) + Rose_HealHpValue;
                    Game_PublicClassVar.Get_function_Rose.addRoseHp(healValue);
                    rose_HealHpTimeSum = 0;
                    //Debug.Log("开始回血！！！,恢复值："+ healValue);
                }
            }
        }


        //护盾开启
        if (HuDunStatus) {
            HudunTimeSum = HudunTimeSum + Time.deltaTime;
            //时间到取消护盾
            if (HudunTimeSum > HuDunTime) {
                HuDunStatus = false;
            }
            //护盾值降低取消护盾
            if (HuDunValue <= 0) {
                Debug.Log("伤害抵消,护盾");
                HuDunStatus = false;
            }

            //注销护盾
            if (!HuDunStatus) {
                HudunTimeSum = 0;
                if (HuDunEffect != null)
                {
                    Destroy(HuDunEffect);  //删除护盾特效
                }
            }
        }

        //更新摇杆速度
        if (rose_Status.YaoGan_Status)
        {
            //摇杆靠近中心时速度降低
            Rose_MoveSpeed = rose_LastMoveSpeed * Rose_MoveSpeedYaoGan;

        }
        else {
            Rose_MoveSpeed = rose_LastMoveSpeed * 1;
        }
	}

    //幸运值换算最大攻击概率
    public bool LuckActMax(){
        int luckValue = (int)(Rose_Lucky);
        float luckProValue = 0;
        switch (luckValue)
        {

            case 0:
                luckProValue = 0f;
            break;
            case 1:
                luckProValue = 0.025f;
            break;
            case 2:
                luckProValue = 0.05f;
            break;
            case 3:
                luckProValue = 0.1f;
            break;
            case 4:
                luckProValue = 0.15f;
            break;
            case 5:
                luckProValue = 0.2f;
            break;
            case 6:
                luckProValue = 0.3f;
            break;
            case 7:
                luckProValue = 0.4f;
            break;
            case 8:
                luckProValue = 0.5f;
            break;
            case 9:
                luckProValue = 1.0f;
            break;
        }

        if (luckValue >= 9)
        {
            luckProValue = 1.0f;
            return true;
        }

        if (Random.value >= luckProValue)
        {
            return false;
        }
        else {
            Debug.Log("触发最大攻击");
            return true;
        }
    }
}
