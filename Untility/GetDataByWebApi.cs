using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Untility
{
    public class GetDataByWebApi
    {
        /// <summary>
        /// 通过web api获取数据的方法
        /// </summary>
        /// <param name="url">api的url</param>
        /// <param name="method">请求类型,默认是get</param>
        /// <param name="postData">post请求所携带的数据</param>
        /// <returns></returns>
        public static string RequestData(string url, string method = "Get", string postData = null)
        {
            try
            {
                method = method.ToUpper();
                ////设置安全通信协议       通信协议是https时如果沒有下面的设置会报错:HttpWebRequest底层连接已关闭:传送时发生意外错误  
                //ServicePointManager.SecurityProtocol =
                //    SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                //    SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                //创建请求实例
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                //设置请求类型
                request.Method = method;
                //设置请求消息主体的编码方法
                request.ContentType = "application/json";

                //POST方式處理
                if (method == "POST")
                {
                    //用UTF8字符集对post请求携带的数据进行编码,可防止中文乱码
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    //指定客户端post请求携带的数据的长度
                    request.ContentLength = byteArray.Length;

                    //创建一个tream,用于写入post请求所携带的数据(该数据写入了请求体)
                    Stream stream = request.GetRequestStream();
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Close();
                }

                //获取请求的响应实例
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //获取读取流实体,用来以UTF8字符集读取响应流中的数据
                StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                //进行数据读取
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                return retString;
            }
            catch (Exception ex)
            {
                //拋出異常
                throw ex;
            }
        }
    }
}
