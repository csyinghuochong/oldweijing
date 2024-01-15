using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ddd
{
    public class TimeLimited : MonoBehaviour
    {
        public void Start()
        {
            string str = GetWebClient("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=2");
            //string str = GetTimeStamp();   // 验证时间戳该有的长度
            //Console.WriteLine(str);   
            string timeStamp = str.Split('=')[1].Substring(0, 10);  //网页获取的数据长度超了，所以要裁剪
            //Console.WriteLine(timeStamp);

            DateTime datetime = GetTime(timeStamp);

            Debug.Log((int)datetime.Year + "年" + (int)datetime.Month + "月" + (int)datetime.Day + "日");//输出时间
            if ((int)datetime.Year > 2016) //判断是否大于2016年
            {
                Application.Quit();
            }
            if ((int)datetime.Month != 8) //是否是当前月份
            {
                Application.Quit();
            }
            if ((int)datetime.Day > 20) //是否大于20号
            {
                Application.Quit();
            }
        }

        private static string GetWebClient(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }

        /// <summary>
        /// 获取时间戳
        /// 本方法只是为了测试时间戳样式
        /// </summary>
        /// <returns></returns>
        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}