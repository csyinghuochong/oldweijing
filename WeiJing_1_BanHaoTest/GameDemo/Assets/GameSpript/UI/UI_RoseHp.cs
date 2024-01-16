using UnityEngine;
using System.Collections;

public class UI_RoseHp : MonoBehaviour {

    private Transform RoseBoneSet;
    public GameObject Obj_TaskRun;
    private Rose_Status rose_Status;

	// Use this for initialization
	void Start () {
        RoseBoneSet = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Head.transform;
        rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
	}
	
	// Update is called once per frame
	void Update () {

        //显示UI,并对其相应的属性修正
        Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(RoseBoneSet.position);
        Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

        //血条位置修正（根据分辨率的变化而变化）
        this.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Hp_show_position.x, Hp_show_position.y + 20.0f, 0);

        if (rose_Status.TaskRunStatus == "2") {
            Obj_TaskRun.SetActive(true);
        }else{
            Obj_TaskRun.SetActive(false);
        }

	}
}
