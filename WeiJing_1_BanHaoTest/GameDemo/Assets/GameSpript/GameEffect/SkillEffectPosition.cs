﻿using UnityEngine;
using System.Collections;

public class SkillEffectPosition : MonoBehaviour {

    public GameObject TargetObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position = TargetObj.transform.position;
	}
}
