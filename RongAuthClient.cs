using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace ConsoleApplication4
{
    public class RongAuthClient
    {
        private static HttpWebRequest _request;

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
        public static RongAuthClient Instance {
            get
            {
                if (rongAuthClient == null) {
                    lock (obj)
                    {
                        if (rongAuthClient == null) { 
                        rongAuthClient=new RongAuthClient();
                        }
                    }
                }
                return rongAuthClient;
            }
        }
        /// <summary>
        /// 单例创建HttpWebRequest
        /// </summary>
        public static HttpWebRequest request
        {
            get
            {
                if (_request == null)
                {
                    lock (objrq)
                    {
                        if (_request == null)
                        {
                            _request = (HttpWebRequest)WebRequest.Create(URL);
                            _request.Method = MethodPOST;
                            _request.ContentType = PostContentType;

                        }
                    }
                }
                return _request;
            }
        }

        /// <summary>
        /// 获取验证
        /// </summary>
        /// <param name="appKey">key</param>
        /// <param name="appSecret">secret</param>
        /// <param name="userId">用户Id</param>
        /// <param name="name">用户名</param>
        /// <param name="portraitUri">用户头像地址</param>
        /// <returns>返回String 是Json格式</returns>
        public String Auth(String appKey, String appSecret, String userId,
              String name, String portraitUri)
        {
            //添加融云认证
            request.Headers.Add("appKey", appKey);
            request.Headers.Add("appSecret", appSecret);


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
            }

            return result;
        }

    }
}
