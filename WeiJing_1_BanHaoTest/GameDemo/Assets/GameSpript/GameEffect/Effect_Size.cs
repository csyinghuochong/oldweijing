using UnityEngine;
using System.Collections;

public class Effect_Size : MonoBehaviour {

    public bool EffectChangeStatus;
    public float EffectChangeTime;          //模型改变时间
    public float EffectChangeSize;          //模型改变大小
    private float effectChangeSize_Minute;
    private float effectTimeSum;
    private float effectChangeSize_Now;     //模型改变大小

	// Use this for initialization
	void Start () {
        effectChangeSize_Now = this.transform.localScale.x;
        effectChangeSize_Minute = (EffectChangeSize - effectChangeSize_Now) / EffectChangeTime;
        EffectChangeStatus = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (EffectChangeStatus) {
            effectTimeSum = effectTimeSum + Time.deltaTime;
            if (effectTimeSum < EffectChangeTime)
            {
                float nowSize = effectChangeSize_Now + (EffectChangeSize - effectChangeSize_Now) * (effectTimeSum / EffectChangeTime);
                this.transform.localScale = new Vector3(nowSize, nowSize, nowSize);
            }
        }

	}
}
