using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServerMessage : MonoBehaviour {

    //服务器消息字符串
    public bool ServerMessageStatus;
    public string ServerMessageStr;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //接受服务器消息
        if (ServerMessageStatus)
        {
            ServerMessageStatus = false;
            string[] ServerMessageStrList = ServerMessageStr.Split(';');
            for (int i = 0; i < ServerMessageStrList.Length; i++) {
                if (ServerMessageStrList[i] != "") {
                    string[] serverList = ServerMessageStrList[i].Split(',');
                    string messageType = serverList[0];
                    switch (messageType)
                    {
                        //发送消息
                        case "10001":
                            Game_PublicClassVar.Get_function_UI.GameGirdHint(serverList[1]);
                            break;

                        //发送道具
                        case "10002":
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(serverList[1], int.Parse(serverList[2]));
                            break;
                    }
                }

            }

            ServerMessageStr = "";
        }

        
        //循环检测


	}
}
