using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour {

    public float LoadingValue;
    public GameObject Obj_LoadingValue;
    public GameObject Obj_LoadingTextValue;
    private bool loadingStatus;             //当加载成功时,此值为true
    public GameObject Obj_ImgBack;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        Obj_LoadingValue.GetComponent<Image>().fillAmount = LoadingValue;
        float value = LoadingValue * 100.0f;
        int valueToint = (int)value;
        //Debug.Log("臭腾腾明天给我5万元宝：" + valueToint);
        //防止出现101%这样的值
        if (valueToint >= 100) {
            //Debug.Log("加载成功！");
            valueToint = 100;
            loadingStatus = true;
        }
        //当加载成功时候,进度条强制一直显示100
        if (loadingStatus) {
            valueToint = 100;
            Obj_LoadingValue.GetComponent<Image>().fillAmount = 1;
        }
        Obj_LoadingTextValue.GetComponent<Text>().text = valueToint.ToString() + "%";
	}

    void OnDestroy() { 
            //获取当前场景名称
            string sceneName = Application.loadedLevelName;
            //Debug.Log("sceneName = " + sceneName);
            if (sceneName != "StartGame") {
                string sceneBGM = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneBGM", "ID", sceneName, "Scene_Template");
                Game_PublicClassVar.Get_function_UI.PlaySource(sceneBGM, "3");
            }
            //Game_PublicClassVar.Get_function_UI.PlaySource("30001", "3");
    }
}
