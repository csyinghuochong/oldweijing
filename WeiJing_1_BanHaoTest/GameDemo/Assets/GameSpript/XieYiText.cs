using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class XieYiText : MonoBehaviour
{

    public Button TextButton_1;
    public Button TextButton_2;

    public GameObject YongHuXieYi;
    public GameObject YinSiXieYi;

    public Button YongHuXieYiClose;
    public Button YinSiXieYiClose;

    public GameObject TextYinSi;
    private bool LoadYinSi = false;

    // Start is called before the first frame update
    void Start()
    {
        this.YongHuXieYi.SetActive(false);
        this.YinSiXieYi.SetActive(false);
    }

    public string GetYingSiText()
    {
        try
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            string dataurl =  "http://verification.weijinggame.com/weijing/yinsi3.txt" ;
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


    public void ShowTextList(GameObject textItem)
    {
        string pageHtml = GetYingSiText();

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

                text.GetComponent<RectTransform>().sizeDelta = new Vector2(1400, text.preferredHeight);

                text.gameObject.SetActive(false);
                text.gameObject.SetActive(true);

                lineStr = string.Empty;
            }


        }
    }

    public void OnButton_TextButton_1()
    {
        this.YongHuXieYi.SetActive(true);
        this.YinSiXieYi.SetActive(false);
    }


    public void OnButton_TextButton_2()
    {
        this.YongHuXieYi.SetActive(false);
        this.YinSiXieYi.SetActive(true);

        if (!this.LoadYinSi)
        {
            this.LoadYinSi = true;
            ShowTextList(this.TextYinSi);
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
