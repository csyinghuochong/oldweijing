using UnityEngine;
using System.Collections;

public class HuDunEffect : MonoBehaviour {

    private Vector3 rosePosiVec3;
    private Rose_Bone roseBone;

	// Use this for initialization
	void Start () {
        roseBone = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>();
	}
	
	// Update is called once per frame
	void Update () {
	    
        //始终跟随这Rose进行移动
        rosePosiVec3 = roseBone.Bone_Center.transform.position;
        this.transform.position = rosePosiVec3;

	}
}
