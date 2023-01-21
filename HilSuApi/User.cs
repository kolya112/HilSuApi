using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HilSuApi
{
    public partial class User : HilariousHub
    {
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
            var stringContent = new StringContent("{\"currency\":\"" + currency.ToString().ToLower() + "\",\"targetId\":\"" + UUID + "\",\"amount\":\"" + amount + "\",\"description\":\"" + description + "\"}", Encoding.UTF8, "application/json");
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
            string success = JObject.Parse(answer).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

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

            HttpWebResponse request = Request("economy/transfersCount", $"accessToken={_userToken}&currency={currency.ToString().ToLower()}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            string success = JObject.Parse(answer).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

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
            string success = JObject.Parse(answer).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

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
            string success = JObject.Parse(answer).SelectToken("success").ToString();

            if (success == "false")
                throw new TransferException();

            return answer;
        }

        /// <summary>
        /// Получить уровень стажа своего аккаунта
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        /// <exception cref="ExperienceCheckException"></exception>
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
                throw new ExperienceCheckException();

            return Convert.ToInt32(JObject.Parse(result).SelectToken("response.level").ToString());
        }

        /// <summary>
        /// Получить опыт своего аккаунта
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        /// <exception cref="ExperienceCheckException"></exception>
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
                throw new ExperienceCheckException();

            return Convert.ToInt32(JObject.Parse(result).SelectToken("response.exp").ToString());
        }

        /// <summary>
        /// Получить количество опыта до получения следующего уровня стажа
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        /// <exception cref="ExperienceCheckException"></exception>
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
                throw new ExperienceCheckException();

            return Convert.ToInt32(JObject.Parse(result).SelectToken("response.nextLevelExp").ToString());
        }

        /// <summary>
        /// Получить список собственной карьеры
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        /// <exception cref="StaffCheckException"></exception>
        public string GetJobs()
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            WebRequest request = WebRequest.Create($"{_baseURLv0}user/jobs");
            request.Headers.Add("Authorization", $"Bearer {_userToken}");
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            string success = JObject.Parse(result).SelectToken("success").ToString();

            if (success == "false")
                throw new JobsCheckException();

            return JObject.Parse(result).SelectToken("response").ToString();
        }

        /// <summary>
        /// Получить полную информацию об аккаунте
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        /// <exception cref="UserInfoCheckException"></exception>
        public string GetUserInfoIndex()
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            WebRequest request = WebRequest.Create($"{_baseURLv0}user/info/index");
            request.Headers.Add("Authorization", $"Bearer {_userToken}");
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            string success = JObject.Parse(result).SelectToken("success").ToString();

            if (success == "false")
                throw new UserInfoCheckException();

            return JObject.Parse(result).SelectToken("response").ToString();
        }

        /// <summary>
        /// Получить количества своих рефералов
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenReferenceException"></exception>
        /// <exception cref="UserInfoCheckException"></exception>
        public int GetReferalsCount()
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            WebRequest request = WebRequest.Create($"{_baseURLv0}user/info/index");
            request.Headers.Add("Authorization", $"Bearer {_userToken}");
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            string success = JObject.Parse(result).SelectToken("success").ToString();

            if (success == "false")
                throw new UserInfoCheckException();

            return Convert.ToInt32(JObject.Parse(result).SelectToken("response.stats.ref").ToString());
        }
    }
}