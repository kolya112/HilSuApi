using System;
using System.IO;
using System.Net;

namespace HilSuApi
{
    public class HilariousHub
    {
        protected string _baseURL = "https://api.hil.su/v2/";
        protected string _baseURLv0 = "https://api.hil.su/v0/";
        protected string _token;
        protected string _user;
        protected string _password;

        // Конструктур с наличием токена
        public HilariousHub(string token)
        {
            _token = token;
        }

        // Конструктур без наличия токена
        public HilariousHub() { }

        // Конструктур для авторизации через аккаунт
        public HilariousHub(string nickname, string password)
        {
            _user = nickname;
            _password = password;
        }

        public class Economy : HilariousHub
        {
            /// <summary>
            /// Получить топ игроков по валюте
            /// </summary>
            /// <param name="limit"></param>
            /// <param name="currency"></param>
            public string TopPlayers(int limit, string currency = "coins")
            {
                HttpWebResponse request = Request("economy/top", $"limit={limit}&currency={currency}");
                string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
                return answer;
            }
        }

        protected HttpWebResponse Request(string suburl, string parametrs)
        {
            return (HttpWebResponse)WebRequest.Create($"{_baseURL}{suburl}?{parametrs}").GetResponse();
        }
    }
}