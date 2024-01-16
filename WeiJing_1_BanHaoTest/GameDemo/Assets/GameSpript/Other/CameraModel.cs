using UnityEngine;
using System.Collections;

public class CameraModel : MonoBehaviour {

    public GameObject Obj_ModelZhanShi;
    public GameObject Obj_ModelFaShi;

	// Use this for initialization
	void Start () {
	
        //获取当前职业
        string occ = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        switch(occ){
            case "1":
                Obj_ModelZhanShi.SetActive(true);
                Obj_ModelFaShi.SetActive(false);
                break;

            case "2":
                Obj_ModelZhanShi.SetActive(false);
                Obj_ModelFaShi.SetActive(true);
                break;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
