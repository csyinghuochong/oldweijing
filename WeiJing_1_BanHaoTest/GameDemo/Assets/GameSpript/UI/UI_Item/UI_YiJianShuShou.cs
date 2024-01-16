using UnityEngine;
using System.Collections;

public class UI_YiJianShuShou : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //出售绿装
    public void Btn_SellLvZhuang() 
    {
        Game_PublicClassVar.Get_function_Rose.SellBagYiJianItemToMoney("3");
    }

    //出售绿色材料
    public void Btn_SellLvCaiLiao()
    {
        Game_PublicClassVar.Get_function_Rose.SellBagYiJianItemToMoney("2");
    }

    //出售制作书
    public void Btn_SellLvZhiZuoShu()
    {
        Game_PublicClassVar.Get_function_Rose.SellBagYiJianItemToMoney("1");
    }

    public void closeUI() {
        Destroy(this.gameObject);
    }
}
