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
        public static string TopPlayers(int limit, Currency currency)
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

        /// <summary>
        /// Получить баланс авторизованного аккаунта
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        public string GetMyBalance(Currency currency)
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            HttpWebResponse request = Request("economy/balance", $"accessToken={_userToken}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            string status = JObject.Parse(answer).SelectToken("success").ToString();
            if (status == "false")
                throw new BalanceCheckException();
            return JObject.Parse(answer).SelectToken($"response.balances.{currency.ToString().ToLower()}").ToString();
        }

        
        /// <summary>
        /// Сделать перевод другому пользователю через UUID
        /// </summary>
        /// <param name="currency">Валюта</param>
        /// <param name="UID">уникальный индентификатор</param>
        /// <param name="amount">сумма</param>
        /// <param name="description">описание</param>
        /// <returns></returns>
        public async Task<string> TransferByUUIDAsync(Currency currency, ulong UUID, decimal amount, string description = "")
        {
            var stringContent = new StringContent("{\"currency\":\"" + currency.ToString().ToLower() + "\",\"targetId\":\"" + UUID + "\",\"amount\":\"" + amount +"\",\"description\":\"" + description + "\"}", Encoding.UTF8, "application/json");
            var response = await new HttpClient().PostAsync("https://api.hil.su/v2/economy/transfer", stringContent);
            string str = await response.Content.ReadAsStringAsync();

            string success = JObject.Parse(str).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

            return str;
        }

        /// <summary>
        /// Сделать перевод другому пользователю через никнейм
        /// </summary>
        /// <param name="currency">валюта</param>
        /// <param name="nickname">игровой ник</param>
        /// <param name="amount">сумма</param>
        /// <param name="description">описание</param>
        /// <returns></returns>
        public async Task<string> TransferByNameAsync(Currency currency, string nickname, decimal amount, string description = "")
        {
            var stringContent = new StringContent("{\"currency\":\"" + currency.ToString().ToLower() + "\",\"targetName\":\"" + nickname + "\",\"amount\":\"" + amount + "\",\"description\":\"" + description + "\"}", Encoding.UTF8, "application/json");
            var response = await new HttpClient().PostAsync("https://api.hil.su/v2/economy/transfer", stringContent);
            string str = await response.Content.ReadAsStringAsync();

            string success = JObject.Parse(str).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

            return str;
        }

        /// <summary>
        /// Получение списка переводов
        /// </summary>
        /// <param name="currency">валюта</param>
        /// <param name="limit">лимит записей</param>
        /// <param name="offset">смещение в списке</param>
        /// <returns></returns>
        public string GetTransfers(Currency currency, int offset, int limit = 100)
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            HttpWebResponse request = Request("economy/transfers", $"accessToken={_userToken}&currency={currency.ToString().ToLower()}&limit={limit}&offset={offset}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            return answer;
        }

        /// <summary>
        /// Получение количества переводов
        /// </summary>
        /// <param name="currency">валюта</param>
        /// <returns></returns>
        public string GetTransfersCount(Currency currency)
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            HttpWebResponse request = Request("economy/transferCount", $"accessToken={_userToken}&currency={currency.ToString().ToLower()}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            return answer;
        }

        /// <summary>
        /// Получение списка изменений за определённый день и из определённого источника
        /// </summary>
        /// <param name="currency">валюта</param>
        /// <param name="limit">лимит записей</param>
        /// <param name="offset">смещение в списке</param>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        public string GetChanges(Currency currency, int limit, int offset)
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            HttpWebResponse request = Request("economy/changes", $"accessToken={_userToken}&currency={currency.ToString().ToLower()}&limit={limit}&offset={offset}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            return answer;
        }

        /// <summary>
        /// Получение количества записей в списке изменений
        /// </summary>
        /// <param name="currency">валюта</param>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        public string GetChangesCount(Currency currency)
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            HttpWebResponse request = Request("economy/changesCount", $"accessToken={_userToken}&currency={currency.ToString().ToLower()}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            return answer;
        }

        public int GetLevel()
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            WebRequest request = WebRequest.Create($"{_baseURL}experience");
            request.Headers.Add("Authorization", $"Bearer {_userToken}");
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            string success = JObject.Parse(result).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

           return Convert.ToInt32(JObject.Parse(result).SelectToken("response.level").ToString());
        }

        public int GetExp()
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            WebRequest request = WebRequest.Create($"{_baseURL}experience");
            request.Headers.Add("Authorization", $"Bearer {_userToken}");
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            string success = JObject.Parse(result).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

            return Convert.ToInt32(JObject.Parse(result).SelectToken("response.exp").ToString());
        }

        public int GetNextLevelExp()
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            WebRequest request = WebRequest.Create($"{_baseURL}experience");
            request.Headers.Add("Authorization", $"Bearer {_userToken}");
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            string success = JObject.Parse(result).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

            return Convert.ToInt32(JObject.Parse(result).SelectToken("response.nextLevelExp").ToString());
        }

        public static string GetStaff()
        {
            HttpWebResponse request = Request("staff/list");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            string success = JObject.Parse(answer).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

            return JObject.Parse(answer).SelectToken("response.staff").ToString();
        }

        protected static HttpWebResponse Request(string suburl, string parametrs = "", string version = "v2", string type = "url")
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