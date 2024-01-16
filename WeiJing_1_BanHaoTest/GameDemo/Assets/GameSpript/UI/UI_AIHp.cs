using UnityEngine;
using System.Collections;

public class UI_AIHp : MonoBehaviour {

    public GameObject Obj_aiName;
    public GameObject Obj_aiImgValue;
    public GameObject Obj_aiRoseName;
    public GameObject Obj_Monster;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Obj_Monster == null) {
            Destroy(this.gameObject);
        }
	}
}
