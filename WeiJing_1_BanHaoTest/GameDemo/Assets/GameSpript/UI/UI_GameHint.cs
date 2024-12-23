﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GameHint : MonoBehaviour {

	public string HintText;
	public GameObject Obj_HintText;
	private float SizeTime;         //字放大时间
	private float DamgeFlyTimeSum;       //伤害时间
 

	void Start () {
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y-15.0f, transform.localPosition.z);
		SizeTime = 0.0f;
		Obj_HintText.GetComponent<Text>().text = HintText;
	}
	
	// Update is called once per frame
	void Update () {
		
		DamgeFlyTimeSum = DamgeFlyTimeSum + Time.deltaTime;
		if (DamgeFlyTimeSum < 0.1f)
		{
			transform.localScale = new Vector3(1f, 1f, 1);
			if (DamgeFlyTimeSum < 0.03f) {
				transform.localScale = new Vector3(1, 1, 1);
			}
		}
		else {
			transform.localScale = new Vector3(1,1,1);
		}
		transform.localPosition =new Vector3(transform.localPosition.x, transform.localPosition.y + 5.0f,transform.localPosition.z);
		Destroy (this.gameObject,0.5f);
		
	}
}
