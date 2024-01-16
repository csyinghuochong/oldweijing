using UnityEngine;
using System.Collections;

public class AI_Property : MonoBehaviour {

	//定义角色属性，方便其他脚本调用
	public string AI_Name;              //怪物名称
    public int AI_Lv;                   //怪物等级
    public int AI_HpMax;                //怪物血量上限
	public int AI_Hp;                   //怪物血量
	public int AI_Act;                  //怪物攻击
	public int AI_Def;                  //怪物物防
    public int AI_Adf;                  //怪物魔防
    public float AI_Cri;                //怪物暴击
    public float AI_Hit;                //怪物命中
    public float AI_Dodge;              //怪物闪避
    public float AI_DefAdd;             //怪物物理免伤
    public float AI_AdfAdd;             //怪物魔法免伤
    public float AI_DamgeAdd;           //怪物伤害免伤
	public float AI_MoveSpeed;          //怪物移动速度
    public string AI_ID;
    public float ActMul;                //攻击加成
    public float DefMul;                //物防加成
    public float AdfMul;                //魔防加成
    public float DamgeAddMul;           //怪物伤害免伤加成
    public float MoveSpeedMul;          //移动速度加成
	private Animator AI_Animator;
	private float DestroyTime;
	public GameObject DropGameObject;           //掉落物体
    public bool UpdataAIProperty;           //打开

	private int LastHp; //用于生命UI显示

	private int base_Act;
    public int base_Def;
    public int base_Adf;
    public float base_DamgeAdd;
    public float base_MoveSpeed;
    

    //绑点专用
    private GameObject gameStartVar;
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {

        //初始化怪物属性
        if (this.gameObject.GetComponent<AI_1>() != null)
        {
            AI_ID = this.gameObject.GetComponent<AI_1>().AI_ID.ToString();
        }
        else {
            AI_ID = this.gameObject.GetComponent<AIPet>().AI_ID.ToString();
        }
        
        AI_Name = "喜洋洋";
        
        AI_Name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", AI_ID, "Monster_Template");
		AI_Lv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", AI_ID, "Monster_Template"));
		AI_Hp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Hp", "ID", AI_ID, "Monster_Template"));
		AI_Act = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Act", "ID", AI_ID, "Monster_Template"));
		AI_Def = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Def", "ID", AI_ID, "Monster_Template"));
		AI_Adf = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Adf", "ID", AI_ID, "Monster_Template"));
		AI_MoveSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MoveSpeed", "ID", AI_ID, "Monster_Template"));
		AI_Cri = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Cri", "ID", AI_ID, "Monster_Template"));
		AI_Hit = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Hit", "ID", AI_ID, "Monster_Template"));
		AI_Dodge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Dodge", "ID", AI_ID, "Monster_Template"));
		AI_DefAdd = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DefAdd", "ID", AI_ID, "Monster_Template"));
		AI_DamgeAdd = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeAdd", "ID", AI_ID, "Monster_Template"));

        //备份初级数据
        base_Act = AI_Act;
        base_Def = AI_Def;
        base_Adf = AI_Adf;
        base_DamgeAdd = AI_DamgeAdd;
        base_MoveSpeed = AI_MoveSpeed;

        //获取绑点
        gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();

        //怪物难度设定
        //string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", AI_ID, "Monster_Template");
        //if (monsterType == "3") {

            float nanduValue_HP = 1;
            float nanduValue_Other = 1;
            switch (Game_PublicClassVar.Get_game_PositionVar.GameNanduValue)
            {
                case "1":
                    nanduValue_HP = 1;
                    nanduValue_Other = 1;
                    break;


                case "2":
                    nanduValue_HP = 1.75f;
                    nanduValue_Other = 1.5f;
                    AI_Name = AI_Name + "(挑战)";
                    break;


                case "3":
                    nanduValue_HP = 2.5f;
                    nanduValue_Other = 2.0f;
                    AI_Name = AI_Name + "(地狱)";
                    break;
            }

            AI_Hp = (int)(nanduValue_HP * AI_Hp);
            AI_Act = (int)(nanduValue_Other * AI_Act);

        //}

        //赋值其他属性
        AI_HpMax = AI_Hp;
        LastHp = AI_Hp;

	}
	
	// Update is called once per frame
	void Update () {


        //刷新属性
        if (UpdataAIProperty) {
            AI_Act = (int)(base_Act * (1 + ActMul));
            AI_Adf = (int)(base_Adf * (1 + AdfMul));
            AI_Def = (int)(base_Def * (1 + DefMul));
            AI_DamgeAdd = base_DamgeAdd + DamgeAddMul;
            AI_MoveSpeed = base_MoveSpeed * (1 + MoveSpeedMul);
            GetComponent<AI_1>().AI_MoveSpeed = AI_MoveSpeed;
            //确保只执行一次
            UpdataAIProperty = false;
        }

        /*
		AI_Animator = GetComponent<Animator>();

		//当AI死亡时
		if (AI_Hp <= 0) {
		
			AI_Animator.Play("Death");

			//创建掉落GameObject
			//GameObject drop = (GameObject)Instantiate(DropGameObject);
			//drop.transform.parent=game_PositionVar.GameObject_AI_Drop.transform;
			//drop.transform.position = this.transform.position;

			//30秒后销毁尸体
			Destroy(this.gameObject, 30);
			Destroy(this);

			//怪物死后创建一个可以拾取物品的碰撞体



            //怪物死后创建的一个掉落（此掉落为掉落2）
            Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(AI_ID, this.transform.position);


            //怪物死亡给人物增加对应的经验值
            Game_PublicClassVar.Get_function_Rose.AddRoseExp("100");


            //是否为任务要求的怪物
            Function_Task function_Task = new Function_Task();
            function_Task.TaskMonsterNum("100001", 1);

            //卸载碰撞脚本
            //Destroy(GetComponent<CapsuleCollider>());
            //Destroy(GetComponent<CharacterController>());
            //Destroy(GetComponent<AI_Status>());
            //Destroy(GetComponent<AI_Property>());
            //Destroy(GetComponent<AI_1>(),0.05f);
            //GetComponent<CapsuleCollider>().enabled = false;
            //GetComponent<CharacterController>().enabled = false;
            GetComponent<AI_1>().enabled = false;
		}
        */
	}
}
