using UnityEngine;
using System.Collections;

public class Rose_Bone : MonoBehaviour {
    public GameObject BoneSet;
    public GameObject Bone_Low;
    public GameObject Bone_Center;
    public GameObject Bone_Head;
    public GameObject Bone_Hand;
    public GameObject Bone_HandObj;
    public GameObject PetPositionSet;

    //战士
    public RuntimeAnimatorController ZhanShi_Animator;
    public Avatar ZhanShi_Avatar;
    public GameObject ZhanShi_Obj;

    //法师
    public RuntimeAnimatorController FaShi_Animator;
    public Avatar FaShi_Avatar;
    public GameObject FaShi_Obj;

    //public bool testCreatMonster;

    //public bool testCreatMonster;

	// Use this for initialization
	void Start () {

        string nowOcc = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        switch (nowOcc) {
            //战士
            case "1":
                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = ZhanShi_Animator;
                this.gameObject.GetComponent<Animator>().avatar = ZhanShi_Avatar;
                ZhanShi_Obj.SetActive(true);
                break;
            //法师
            case "2":
                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = FaShi_Animator;
                this.gameObject.GetComponent<Animator>().avatar = FaShi_Avatar;
                FaShi_Obj.SetActive(true);
                break;
        }

	}
	
	// Update is called once per frame
	void Update () {

        Bone_Hand.transform.position = Bone_HandObj.transform.position; 
        /*
        if (testCreatMonster) {
            testCreatMonster = false;
            Game_PublicClassVar.Get_function_AI.AI_CreatMonster("70001004",Vector3.zero);
        }
	    */
	}
}
