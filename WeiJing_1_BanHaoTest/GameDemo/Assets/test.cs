using UnityEngine;
using System.Collections;
using System.Data;
using System.Xml;
using System.IO;

public class test : MonoBehaviour {
    private float time;
    private float time_1;
    private float time_2;
    private bool aaaa;
    GameObject MonsterObj;
    public GameObject ditu;
    public GameObject shexiangji;
    public GameObject DengGuang;
    

    public string michiID;
    
	// Use this for initialization
	void Start () {

       //int a= int.Parse("asd2sdasd");
       //Debug.Log("aaaaaaaaaaaa = " + a);
	
	}
	
	// Update is called once per frame
	void Update () {
        time = time + Time.deltaTime;
        //Debug.Log("Time = " + time);

        if (aaaa) {
            
            //Debug.Log("Time = " + (time - time_1).ToString());
            //Debug.Log("Time = " + (Time.time - time_1).ToString());
            //Debug.Log("Time = " + Time.time);
            aaaa = false;
        }

	}

    public void SellItem(){
        //开启出售状态
        if (!Game_PublicClassVar.Get_game_PositionVar.SellItemStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.SellItemStatus = true;
        }
        else {
            Game_PublicClassVar.Get_game_PositionVar.SellItemStatus = false;
        }
    }

    public void TESTbTN() {

        //Game_PublicClassVar.Get_xmlScript.addFileXml();


        //获取充值额度
        Game_PublicClassVar.Get_xmlScript.addStr(michiID);

    }

    public void CloseMonsterObj() {
        if (MonsterObj == null) {
            MonsterObj = GameObject.FindWithTag("MonsterSet");
        }
        
        if (MonsterObj.active == false)
       {
           MonsterObj.SetActive(true);
       }
       else {
           MonsterObj.SetActive(false);
       }
       
    }

    public void closeDitu()
    {


        if (ditu.active == false)
        {
            ditu.SetActive(true);
        }
        else
        {
            ditu.SetActive(false);
        }
       
    }

    //关闭摄像机
    public void closeSheXiangJi()
    {


        if (shexiangji.active == false)
        {
            shexiangji.SetActive(true);
        }
        else
        {
            shexiangji.SetActive(false);
        }

    }

    public void closeDengGuang() {

        if (DengGuang.active == false)
        {
            DengGuang.SetActive(true);
        }
        else
        {
            DengGuang.SetActive(false);
        }
    }

    //开始协同怪物表
    private IEnumerator testBBBB()
    {

        //Debug.Log("测试开始" + Time.time);
        time_1 = Time.time;
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        //将掉落的道具ID添加到背包内
        for (int i = 1; i <= 100; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", "64", "RoseBag");
        }
        Debug.Log("测试完毕" + time_1);
        aaaa = true;

        yield return "";
        Debug.Log("测试读取表成功");
    }

    //开始协同怪物表
    private void testAAAA()
    {

        //Debug.Log("测试开始" + Time.time);
        time_1 = Time.time;
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        //将掉落的道具ID添加到背包内
        for (int i = 1; i <= 100; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", "64", "RoseBag");
        }
        Debug.Log("测试完毕" + time_1);
        aaaa = true;

        //yield return "";
        Debug.Log("测试读取表成功");

    }

}
