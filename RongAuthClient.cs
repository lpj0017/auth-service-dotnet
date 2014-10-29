using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Security.Cryptography;
namespace com.rongcloud.demo.auth
{


    public static class DateTimeExtensions
    {
        private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long currentTimeMillis()
        {
            return (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
        }
    }
        public class RongAuthClient
        {

            private static RongAuthClient rongAuthClient;

            private static object obj = new object();
            private static object objrq = new object();
            /// <summary>
            /// 请求地址
            /// </summary>
            private static readonly string URL = "https://api.cn.rong.io/user/getToken.json";

            /// <summary>
            /// 
            /// </summary>
            private static readonly string ContentType = "text/xml;charset=UTF-8";

            /// <summary>
            /// 
            /// </summary>
            private static readonly string PostContentType = "application/x-www-form-urlencoded";

            /// <summary>
            /// get提交方式
            /// </summary>
            private static readonly string MethodGET = "GET";

            /// <summary>
            /// post提交方式
            /// </summary>
            private static readonly string MethodPOST = "POST";

            /// <summary>
            /// 单例RongAuthClient 类
            /// </summary>
            public static RongAuthClient Instance
            {
                get
                {
                    if (rongAuthClient == null)
                    {
                        lock (obj)
                        {
                            if (rongAuthClient == null)
                            {
                                rongAuthClient = new RongAuthClient();
                            }
                        }
                    }
                    return rongAuthClient;
                }
            }


      


            public static String byteToHexString(byte[] bytes)
            {
                String stmp = "";
                StringBuilder sb = new StringBuilder("");
                for (int n = 0; n < bytes.Length; n++)
                {
                    stmp = (bytes[n] & 0xFF).ToString("X");
                    sb.Append((stmp.Length == 1) ? "0" + stmp : stmp);
                }
                return sb.ToString().ToUpper().Trim();
            }

            public static string hexSHA1(string value)
            {

               SHA1 sha1= System.Security.Cryptography.SHA1.Create();

               byte[] encodingData= System.Text.Encoding.GetEncoding("UTF-8").GetBytes(value);         
               sha1.ComputeHash(encodingData, 0, encodingData.Length);
               byte[] hashs = sha1.Hash;
               return byteToHexString(hashs);

            }

            public String Auth(String appKey, String appSecret, String userId,
             String name, String portraitUri)
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = MethodPOST;
                request.ContentType = PostContentType;

                Random random = new Random();
                String nonce = random.Next(100000).ToString();
                String timestamp = DateTimeExtensions.currentTimeMillis().ToString();
                StringBuilder toSign = new StringBuilder(appSecret).Append(nonce).Append(timestamp);


                //添加融云认证
                request.Headers.Add("App-Key", appKey);
                request.Headers.Add("Timestamp", timestamp);
                request.Headers.Add("Nonce", nonce);
                request.Headers.Add("Signature",hexSHA1(toSign.ToString()));
                nonce= nonce.PadLeft(6,'0');

                #region POST提交数据
                //添加参数
                StringBuilder sb = new StringBuilder();
                sb.Append("userId=")
                        .Append(userId)
                        .Append("&name=").Append(name)
                        .Append("&portraitUri=")
                        .Append(portraitUri);

                string postData = sb.ToString();
                byte[] bs = Encoding.UTF8.GetBytes(postData);
                #endregion

                string result = string.Empty;//返回结果
                System.IO.StreamReader sr = null;

                try
                {
                    using (Stream stream = request.GetRequestStream())//获取用于写入的流
                    {
                        stream.Write(bs, 0, bs.Length);

                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    try
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                            result = sr.ReadToEnd().Trim();
                        }
                    }
                    finally
                    {
                        sr.Close();
                        response.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error  is message " + ex.Message);
                }

                return result;
            }

        }
    }
