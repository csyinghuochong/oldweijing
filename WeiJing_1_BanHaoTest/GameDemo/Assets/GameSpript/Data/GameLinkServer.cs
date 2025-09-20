using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;


public class GameLinkServer : MonoBehaviour {


    //private TcpListener tcpListener;                //声明一个监听事件
    private string ipAddress;                       //服务器IP  
    private int port;                               //服务器端口
    private Thread mainThread;
    private TcpClient tc;
    private NetworkStream ns;

    public bool ServerLinkStatus;                   //服务器连接状态
    public string ServerLinkTimeStamp;              //服务器第一次连接后反馈的时间戳

    public bool SendDataStatus;

    //状态数据
    public bool SendPlayDataStatus;                 //发送玩家信息

	// Use this for initialization
	void Start () {

        Debug.Log("噜啦啦噜啦啦");
        //Control.CheckForIllegalCrossThreadCalls = false;
        //调用主线程
        mainThread = new Thread(MainThread);
        mainThread.Start();
	    
    }
	
	// Update is called once per frame
	void Update () {

        //测试一些发送的信息
        if (SendDataStatus) {
            SendDataStatus = false;
            sendDataTest();
        }

        //Debug.Log("SendPlayDataStatus = " + SendPlayDataStatus);
        if (!SendPlayDataStatus) {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
            {
                Debug.Log("发送信息成功！！！");
                SendPlayDataStatus = true;
                DoThing("110003");
            }
        }
	}

    void OnDestroy() {

        CloseServer();

    }


    //主线程无线循环
    private void MainThread()
    {

        //ipAddress = "192.168.1.14";
        ipAddress = "molongzhixi.weijinggame.com";
        port = 17906;
        IPAddress[] serveIp = Dns.GetHostAddresses(ipAddress);
        Debug.Log("serveIp = " + serveIp[0]);

        //构建一个端口(ip,端口)
        //IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        IPEndPoint ipend = new IPEndPoint(serveIp[0], port);
        //链接服务器
        tc = new TcpClient();
        //tc.SendTimeout = 5000;
        //tc.ReceiveTimeout = 1000;
        Debug.Log("开始链接！");
        try {
            tc.Connect(ipend);
        }
        catch (Exception ex)
        {
            Debug.Log("服务器连接失败！");

            Debug.Log("ex = " + ex.Message);
            Debug.Log("ex = " + ex.GetBaseException());
            if (ex.ToString() == "System.Net.Sockets.SocketException") {
                
            }
        }

        Debug.Log("链接完毕！");
        //创建数据流
        ns = tc.GetStream();
        Debug.Log("链接服务器和通讯流数据成功！");
        

        //发送第一个链接服务器的信息
        byte[] bt = Encoding.Default.GetBytes("100000");
        //byte[] bt = Encoding.Default.GetBytes("GET / HTTP/1.1");
        ns.Write(bt, 0, bt.Length);
        //ns.ReadTimeout = 10000;

        //声明一个空数组
        byte[] btyArr = new byte[1024];
        int btLen = 0;

        while (true)
        {
            Debug.Log("循环开始");
            //接受服务端传送过来的数据&& ns.CanRead 
            if (ns != null)
            {
                //Read为无敌循环的执行逻辑
                //Debug.Log("循环开始1");
                try
                {
                    //Debug.Log("tc.Connected111 = " + tc.Connected);
                    btLen = ns.Read(btyArr, 0, btyArr.Length);
                    //Debug.Log("我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！我收到了消息！");
                    //btLen = ns.BeginRead(btyArr, 0, btyArr.Length);
                }
                catch (Exception ex)
                {
                    //Debug.Log("tc.Connected222 = " + tc.Connected);
                    Debug.Log("EX = " + ex.ToString());
                }
                finally {
                    //Debug.Log("tc.Connected333 = " + tc.Connected);
                }
                //Debug.Log("btLen = " + btLen);
                //Debug.Log("循环开始2");
                if (btLen < 1)
                {
                    Debug.Log("服务端断开了！");
                    break;
                }
                else {
                    //.Replace("\0", "");一定要加 该字符串不能再后缀加任何字符串,坑了我1晚上时间（Replace为旧的字符串替换新的字符串,所以为空即可,此操作可以生成一个全新的字符串,参数为“\0”的说明：由于\0在字符串中代表字符串截止，因此，c#在处理字符串时，当遇到\0,就会截止对字符串的操作。）
                    string dataStr = Encoding.Default.GetString(btyArr).Replace("\0", "");
                    Debug.Log("dataStr = " + dataStr);
                    DoThing(dataStr);

                    //清空数据
                    btyArr = new byte[1024];
                }
            }
            
            //Debug.Log("tc.Connected = " + tc.Connected);
            /*
            if (!tc.Connected) {
                Debug.Log("服务器被强制断开了！");
                break;
            }
            */
            btLen = 0;
            
            //将进程暂时挂起1000毫秒,也就是1秒执行一次无线循环
            //Thread.Sleep(1000);

            //Debug.Log("循环结束");

        }
    }


    //检测服务端是否连接
    private static bool TcpIsOnline(TcpClient c)
    {
        return !((c.Client.Poll(1000, SelectMode.SelectRead) && (c.Client.Available == 0)) || !c.Client.Connected);
    }

    //根据接受到的服务端的请求处理不同的数据
    private void DoThing(string dataStr)
    {
        //如果接受数据链协议类型都不满足协议头的长度则表示本条接受数据无效
        if (dataStr.Length < 6)
        {
            Debug.Log("dataStr.Length长度不足");
            return;
        }

        string[] dataStrList = dataStr.Split(';');
        for (int i = 0; i < dataStrList.Length; i++)
        {
            string nowDataStr = dataStrList[i].Replace("\0", "");
            Debug.Log("dataStrList_" + i + " = " + dataStrList[i]);
            //获取协议类型
            string DataTypeStr = "";
            if (nowDataStr.Length >= 6)
            {
                DataTypeStr = nowDataStr.Substring(0, 6);
            }
            Debug.Log("DataTypeStr = " + DataTypeStr);
            //根据不同协议类型执行不同操作
            switch (DataTypeStr)
            {
                //获取服务器第一次返回的信息
                case "100000":
                    ServerLinkStatus = true;

                    //根据不同平台获取不同的版本号
#if UNITY_ANDROID
                    sendToServerData("100003;");
#endif
#if UNITY_IPHONE
                    sendToServerData("100004");
#endif

                    //获取时间戳
                    sendToServerData("100001;");

                    break;

                //获取时间戳
                case "100001":
                    string timeStamp = dataStr.Substring(6, dataStr.Length - 6);
                    ServerLinkTimeStamp = timeStamp;

                    //服务器发送关闭请求(因为服务器只是用时间戳,所以获取时间戳后直接关闭服务器连接,减轻服务器压力) 此功能暂时屏蔽,现在由服务器完成
                    //sendToServerData("109999");
                    break;

                //获取测试数据
                case "100002":
                    string getServerStr = dataStr.Substring(6, dataStr.Length - 6);
                    Debug.Log("接受到服务器的测试数据 = " + getServerStr);
                    sendToServerData("100002123;");
                    break;

                //获取当前最新安卓版本号
                case "100003":
                    //Debug.Log("进来啦进来啦进来啦进来啦进来啦进来啦进来啦");
                    //Debug.Log("dataStr = " + dataStr);
                    getServerStr = dataStr.Substring(6, dataStr.Length - 6);
                    Game_PublicClassVar.Get_wwwSet.GameServerVersionStr = getServerStr;
                    break;

                //获取当前最新IOS版本号
                case "100004":
                    getServerStr = dataStr.Substring(6, dataStr.Length - 6);
                    Game_PublicClassVar.Get_wwwSet.GameServerVersionStr = getServerStr;
                    break;

                //接受发送的广播
                case "110001":
                    //if (this.gameObject.scene.name != "StarGame") {
                        getServerStr = dataStr.Substring(6, dataStr.Length - 6);
                        Game_PublicClassVar.Get_GameServerMessage.ServerMessageStr = Game_PublicClassVar.Get_GameServerMessage.ServerMessageStr + "10001," + getServerStr + ";";
                        Game_PublicClassVar.Get_GameServerMessage.ServerMessageStatus = true;
                    //}
                    break;

                //接受发送的道具
                case "110002":
                    //if (this.gameObject.scene.name != "StarGame") {
                    getServerStr = dataStr.Substring(6, dataStr.Length - 6);
                    Game_PublicClassVar.Get_GameServerMessage.ServerMessageStr = Game_PublicClassVar.Get_GameServerMessage.ServerMessageStr + "10002," + getServerStr + ";";
                    Game_PublicClassVar.Get_GameServerMessage.ServerMessageStatus = true;
                    //}
                    break;

                //获取玩家名称
                case "110003":        
                    string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string GoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string RMB = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string RMBPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string shopDes = zhanghaoID + "," + roseName + "," + roseLv + "," + GoldNum + "," + RMB + "," + RMBPayValue;
                    sendToServerData("110003" + shopDes + ";");
                    Debug.Log("shopDes = " + shopDes);
                    break;

                //关闭服务器的最后一条消息
                case "109999":
                    Debug.Log("关闭服务器连接");
                    break;
            }
        }

        //执行关闭
        Debug.Log("执行关闭Server");
    }

    //发送信息
    public void sendToServerData(string dataStr) {

        //判定是否连接服务器
        Debug.Log("dataStr = " + dataStr + ";ns = " + ns);
        //发送信息
        byte[] bt = Encoding.Default.GetBytes(dataStr);
        ns.Write(bt, 0, bt.Length);
    }


    private void sendDataTest() {
        Debug.Log("测试发送数据");
        //发送信息
        byte[] bt = Encoding.Default.GetBytes("100001我想要个朋友222！");
        ns.Write(bt, 0, bt.Length);
        Debug.Log("测试发送数据成功");
    }

    //关闭服务器
    public void CloseServer() {

        Debug.Log("关闭链接服务器及相关数据流");

        //关闭数据流
        if (ns != null)
        {
            ns.Close();
        }

        //关闭tcp
        if (tc != null)
        {
            tc.Close();
        }

        //关闭线程
        if (mainThread != null && mainThread.IsAlive)
        {
            mainThread.Abort();
        }
    
    }

}
