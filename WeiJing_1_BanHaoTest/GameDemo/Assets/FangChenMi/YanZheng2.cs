using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using LC.Newtonsoft.Json;
using UnityEngine.UI;


public class YanZheng2 : MonoBehaviour {

    private const String host = "https://naidcard.market.alicloudapi.com";
    private const String path = "/nidCard";
    private const String method = "GET";
    private const String appcode = "d59fefe68cf644f6a8f54dd039c3806f";

    // Use this for initialization
    void Start () {

        //YanZhengShenFen("贺松年", "37068219921211861X"); 
    }

    // Update is called once per frame
    void Update () {
		
	}


    public bool YanZhengShenFen(string Name, string CardID)
    {

        Debug.Log("1111111111111111");

        string querys = "idCard=" + CardID + "&name=" + Name;
        string bodys = "";
        string url = host + path;
        HttpWebRequest httpRequest = null;
        HttpWebResponse httpResponse = null;

        if (0 < querys.Length)
        {
            url = url + "?" + querys;
        }

        if (host.Contains("https://"))
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
        }
        else
        {
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
        }
        httpRequest.Method = method;
        httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
        if (0 < bodys.Length)
        {
            byte[] data = Encoding.UTF8.GetBytes(bodys);
            using (Stream stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }
        try
        {
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        }
        catch (WebException ex)
        {
            httpResponse = (HttpWebResponse)ex.Response;
        }

        Debug.Log(httpResponse.StatusCode);
        Debug.Log(httpResponse.Method);
        Debug.Log(httpResponse.Headers);
        Stream st = httpResponse.GetResponseStream();
        StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));

        //测试解析

        string xinxi = reader.ReadToEnd().ToString();
        LC.Newtonsoft.Json.Linq.JArray jsonArr = GetToJsonList("["+ xinxi + "]");
        string status = jsonArr[0]["status"].ToString();
        //Debug.Log("status = " + status);

        switch (status)
        {
            //实名认证通过
            case "01":
                Debug.Log("实名认证通过");
                return true;
                break;
            //实名认证不通过
            case "02":
                Debug.Log("实名认证未通过！");
                return false;
                break;
            //其他
            default:
                Debug.Log("实名认证未通过！");
                return false;
                break;
        }
    }

    public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }

    public static LC.Newtonsoft.Json.Linq.JArray GetToJsonList(string json)
    {
        LC.Newtonsoft.Json.Linq.JArray jsonArr = (LC.Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(json);
        return jsonArr;
    }


}


