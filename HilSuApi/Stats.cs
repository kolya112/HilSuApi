using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using static HilSuApi.HilariousHub;

namespace HilSuApi
{
    public partial class Stats : HilariousHub
    {
        public Stats(string token) : base(token) { }

        public Stats() : base() { }

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

        /// <summary>
        /// Получить топ игроков по валюте
        /// </summary>
        /// <param name="limit">Лимит выданных пользователей</param>
        /// <param name="currency">Валюта из топа игроков</param>
        public static string TopPlayers(int limit, Currency currency)
        {
            if (limit > 100)
                throw new TopPlayersLimitException();

            HttpWebResponse request = Request("economy/top", $"limit={limit}&currency={currency.ToString().ToLower()}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            return JObject.Parse(answer).SelectToken("response").ToString();
        }

        /// <summary>
        /// Получить время до ближайшего вайпа доп. миров на игровых серверах
        /// </summary>
        /// <returns></returns>
        public static string GetWipe()
        {
            HttpWebResponse request = Request("stats/wipe", version: "v0");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            string status = JObject.Parse(answer).SelectToken("success").ToString();
            if (status == "false")
                throw new GetWipeCheckException();
            return JObject.Parse(answer).SelectToken("response").ToString();
        }
    }
}
