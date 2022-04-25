using System;
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
        protected readonly static string _baseURL = "https://api.hil.su/v2/";
        protected readonly static string _baseURLv0 = "https://hil.su/api/v0/";
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
            _token = token;
        }

        // Конструктур без наличия токена
        /// <summary>
        /// Авторизация без пользовательского токена, доступ ограничен
        /// </summary>
        public HilariousHub() 
        {

        }

        /// <summary>
        /// Получить топ игроков по валюте
        /// </summary>
        /// <param name="limit">Лимит выданных пользователей</param>
        /// <param name="currency">Валюта из топа игроков</param>
        public static string TopPlayers(int limit, string currency = "coins")
        {
            if (currency != "coins" || currency != "gems")
            {
                throw new EconomyCurrencyException();
            }

            HttpWebResponse request = Request("economy/top", $"limit={limit}&currency={currency}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            return answer;
        }

        /*public static string ServersOnline(string server = "all")
        {
            switch(server)
            {
                case "HiTech":
                    break;
                case "MiniGames":
                    break;
                case "Magick":
                    break;
                case "HiPower":
                    break;
                case "TFC":
                    break;
                case "Sandbox":
                    break;
                case "MageTech":
                    break;
                default:
                    HttpWebResponse request = Request("emonitoring/players", $"limit={limit}&currency={currency}&accessToken");
                    break;

            }
        }*/
        // https://hil.su/api/v0/monitoring/players
        protected static HttpWebResponse Request(string suburl, string parametrs, string version = "v2")
        {
            if (version == "v2")
                return (HttpWebResponse)WebRequest.Create($"{_baseURL}{suburl}?{parametrs}").GetResponse();
            else if (version == "v0")
                return (HttpWebResponse)WebRequest.Create($"{_baseURLv0}{suburl}?{parametrs}").GetResponse();
            else
                throw new RequestArgumentException();
        }
    }
}