using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HilSuApi
{
    public partial class Stats : HilariousHub
    {
        /// <summary>
        /// Получить информацию о серверах
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException">Для этого метода требуется токен доступа</exception>
        /// <exception cref="OnlineCheckException">Произошла ошибка в запросе для получения информации о серверах</exception>
        public string ServersMonitoring()
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            WebRequest request = WebRequest.Create($"{_baseURLv0}monitoring/players");
            request.Headers.Add("Authorization", $"Bearer {_userToken}");
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string answer = new StreamReader(response.GetResponseStream()).ReadToEnd();
            string status = JObject.Parse(answer).SelectToken("success").ToString();
            if (status == "false")
                throw new OnlineCheckException();
            string result = JObject.Parse(answer).SelectToken("response").ToString();
            return result;
        }
    }
}
