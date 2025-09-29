using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class XieYiText : MonoBehaviour
{

    public Button TextButton_1;
    public Button TextButton_2;

    public GameObject YongHuXieYi;
    public GameObject YinSiXieYi;

    public Button YongHuXieYiClose;
    public Button YinSiXieYiClose;

    public GameObject TextYongHu;
    public GameObject TextYinSi;

    public GameObject UI_YinSiXieYi;

    private bool LoadYongHu = false;
    private bool LoadYinSi = false;

  
    public AndroidJavaClass jc;
    public AndroidJavaObject jo;

    //是java里的类，一些静态方法可以直接通过这个调用。
    //androidjavaobject 调用的话，会生成一个对象，就和java里new一个对象一样，可以通过对象去调用里面的方法以及属性。
    public AndroidJavaClass javaClass;
    public AndroidJavaObject javaActive;
    //"com.mafeng.alinewsdk.AliSDKActivity"是2018.11.01日更新的版本 对应安卓工程中的alinewsdk Module
    //而"com.mafeng.aliopensdk.AliSDKActivity"是之前的版本 对应安卓工程中的aliopensdk Module
    public string javaClassStr = "com.example.alinewsdk.AliSDKActivity";  //"com.mafeng.aliopensdk.AliSDKActivity";
    public string javaActiveStr = "currentActivity";


    // Start is called before the first frame update
    void Start()
    {
        this.YongHuXieYi.SetActive(false);
        this.YinSiXieYi.SetActive(false);


#if UNITY_ANDROID && !UNITY_EDITOR
		jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif

        if (PlayerPrefs.GetString(UIYinSiKey).Equals("1"))
        {
            this.UI_YinSiXieYi.SetActive(false);
            GameObject.FindWithTag("Tag_WWWSet").GetComponent<TapTapLogin>().InitSDKAndLogin();
            AdManager.Instance.InitSDK();
        }
        else
        {
            this.UI_YinSiXieYi.SetActive(true);
        }
    }

    /// <summary>
    /// 调用位置开发者可以自己指定，只需在使用SDK功能之前调用即可，
    /// 强烈建议开发者在终端用户点击应用隐私协议弹窗同意按钮后调用。
    /// </summary>
    public void SetIsPermissionGranted()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call("QuDaoRequestPermissions");
#else
        onRequestPermissionsResult("1_1");
#endif
    }


    private const string UIYinSiKey = "UIYinSi_092301";
    public void onRequestPermissionsResult(string permissons)
    {
        Debug.Log($"onRequestPermissionsResult: {permissons}");

        string[] values = permissons.Split('_');
        if (values[1] == "0")
        {
            Application.Quit();
            return;
        }

        PlayerPrefs.SetString(UIYinSiKey, "1");
        GameObject.FindWithTag("Tag_WWWSet").GetComponent<TapTapLogin>().InitSDKAndLogin();
        AdManager.Instance.InitSDK();
    }


    IEnumerator DelayedAction()
    {
        // 等待1秒
        yield return new WaitForSeconds(0.2f);

        // 1秒后执行的代码
        //this.YongHuXieYi.transform.Find("Scroll View").GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1); //.verticalNormalizedPosition = 1f;
        //this.YongHuXieYi.transform.Find("Scroll View").GetComponent<ScrollRect>().content.localPosition = new Vector3(0f, -3691.983f, 0f);
        this.YongHuXieYi.transform.Find("Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>().value = 1f;
    }

    public string GetYongHuText()
    {
        try
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            string dataurl =  "http://verification.weijinggame.com/weijing/MoLong/MoLongXieYi.txt" ;
            Byte[] pageData = MyWebClient.DownloadData(dataurl); //从指定网站下载数据
            string pageHtml = Encoding.UTF8.GetString(pageData);
            return pageHtml;
        }

        catch (WebException webEx)
        {
            Debug.Log(webEx.ToString());
        }
        return "服务器维护中！";
    }

    public string GetYingSiText()
    {
        try
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            string dataurl =  "http://verification.weijinggame.com/weijing/MoLong/MoLongYinSi.txt" ;
            Byte[] pageData = MyWebClient.DownloadData(dataurl); //从指定网站下载数据
            string pageHtml = Encoding.UTF8.GetString(pageData);
            return pageHtml;
        }

        catch (WebException webEx)
        {
            Debug.Log(webEx.ToString());
        }
        return "服务器维护中！";
    }


    public void ShowTextList(string info, GameObject textItem)
    {
        string pageHtml = info;

        string tempstr = string.Empty;
        string leftValue = pageHtml;
        int indexlist = pageHtml.IndexOf('\n');
        int whileNumber = 0;

        List<string> allString = new List<string>();

        while (indexlist != -1)
        {
            whileNumber++;
            if (whileNumber >= 1000)
            {
                break;
            }

            tempstr = leftValue.Substring(0, indexlist);
            allString.Add(tempstr);

            indexlist += 1;
            leftValue = leftValue.Substring(indexlist, leftValue.Length - indexlist);

            indexlist = leftValue.IndexOf('\n');

            if (indexlist == -1)
            {
                allString.Add(leftValue);
            }
        }

        string lineStr = string.Empty;

        Transform parentobject = textItem.transform.parent;
        int totalLength = allString.Count;
        for (int i = 0; i < totalLength; i++)
        {
            lineStr += allString[i] + '\n';

            if (lineStr.Length > 1000 || i == totalLength - 1)
            {
                lineStr = lineStr.Substring(0, lineStr.Length - 1);

                GameObject textGo = GameObject.Instantiate(textItem);
                textGo.transform.SetParent(parentobject);
                textGo.transform.localScale = Vector3.one;
                textGo.transform.localPosition = Vector3.zero;
                Text text = textGo.GetComponent<Text>();

                text.text = lineStr;

                text.GetComponent<RectTransform>().sizeDelta = new Vector2(1080, text.preferredHeight);

                text.gameObject.SetActive(false);
                text.gameObject.SetActive(true);

                lineStr = string.Empty;
            }


        }
    }

    public void OnButton_Btn_SelectTreue()
    {
        this.UI_YinSiXieYi.SetActive(false);

        SetIsPermissionGranted();
    }

    public void OnButton_Btn_SelectFalse()
    {
        this.UI_YinSiXieYi.SetActive(false);
        Application.Quit(); 
    }

    public void OnButton_Btn_Close()
    { 
        this.UI_YinSiXieYi.SetActive(false);
        Application.Quit();
    }

    public void OnButton_TextButton_1()
    {
        this.YongHuXieYi.SetActive(true);
        this.YinSiXieYi.SetActive(false);
        
        if (!this.LoadYongHu)
        {
            this.LoadYongHu = true;
            ShowTextList(GetYongHuText(), this.TextYongHu);
        }
    }


    public void OnButton_TextButton_2()
    {
        this.YongHuXieYi.SetActive(false);
        this.YinSiXieYi.SetActive(true);

        if (!this.LoadYinSi)
        {
            this.LoadYinSi = true;
            ShowTextList(GetYingSiText(), this.TextYinSi);
        }
    }

    public void OnButton_YongHuXieYiClose() 
    {
        this.YongHuXieYi.SetActive(false);
        this.YinSiXieYi.SetActive(false);
    }

    public void OnButton_YinSiXieYiClose()
    {
        this.YongHuXieYi.SetActive(false);
        this.YinSiXieYi.SetActive(false);
    }
}
