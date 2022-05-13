﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HilSuApi
{
    public class HilariousHub
    {
        protected const string _baseURL = "https://api.hil.su/v2/";
        protected const string _baseURLv0 = "https://hil.su/api/v0/";
        protected static string _token;
        protected static string _userToken;
        protected static string _user;
        protected static string _password;

        // Конструктур с наличием токена
        /// <summary>
        /// Авторизация с пользовательским токеном
        /// </summary>
        /// <param name="token"></param>
        public HilariousHub(string token)
        {
            _userToken = token;
        }

        // Конструктур без наличия токена
        /// <summary>
        /// Авторизация без пользовательского токена, доступ ограничен
        /// </summary>
        public HilariousHub() 
        {

        }

        public enum Currency
        {
            Coins,
            Gems
        }

        /// <summary>
        /// Получить топ игроков по валюте
        /// </summary>
        /// <param name="limit">Лимит выданных пользователей</param>
        /// <param name="currency">Валюта из топа игроков</param>
        public string TopPlayers(int limit, Currency currency)
        {
            if (limit > 100)
                throw new TopPlayersLimitException();
            HttpWebResponse request = Request("economy/top", $"limit={limit}&currency={currency.ToString().ToLower()}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            return answer;
        }

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

        protected static HttpWebResponse Request(string suburl, string parametrs, string version = "v2", string type = "url")
        {
            if (version == "v2")
                return (HttpWebResponse)WebRequest.Create($"{_baseURL}{suburl}?{parametrs}").GetResponse();
            else if (version == "v0")
                return (HttpWebResponse)WebRequest.Create($"{_baseURLv0}{suburl}?{parametrs}").GetResponse();
            else
                throw new RequestArgumentException();
        }

        /// <summary>
        /// Получить баланс авторизованного аккаунта
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        public string GetMyBalance()
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            HttpWebResponse request = Request("economy/balance", $"accessToken={_userToken}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            return JObject.Parse(answer).SelectToken("response.balances").ToString();
        }
    }
}