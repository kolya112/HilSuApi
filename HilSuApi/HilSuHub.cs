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
            HttpWebResponse request = Request("economy/top", $"limit={limit}&currency={currency.ToString().ToLower()}");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            return answer;
        }

        public string ServersOnline(string server = "all")
        {
            if (_userToken == null)
                throw new TokenReferenceException();

            switch (server)
            {
                case "HiTech":
                    WebRequest HTRequest = WebRequest.Create($"{_baseURLv0}monitoring/players");
                    HTRequest.Headers.Add("Authorization", $"Bearer {_userToken}");
                    HTRequest.ContentType = "application/json";
                    HTRequest.Method = "GET";
                    HttpWebResponse HTResponse = (HttpWebResponse)HTRequest.GetResponse();
                    string HTAnswer = new StreamReader(HTResponse.GetResponseStream()).ReadToEnd();
                    string HTStatus = JObject.Parse(HTAnswer).SelectToken("success").ToString();
                    if (HTStatus == "false")
                        throw new OnlineCheckException();
                    string HTResult = JObject.Parse(HTAnswer).SelectToken("response.[3]").ToString();
                    return HTResult;
                case "MiniGames":
                    WebRequest MGRequest = WebRequest.Create($"{_baseURLv0}monitoring/players");
                    MGRequest.Headers.Add("Authorization", $"Bearer {_userToken}");
                    MGRequest.ContentType = "application/json";
                    MGRequest.Method = "GET";
                    HttpWebResponse MGResponse = (HttpWebResponse)MGRequest.GetResponse();
                    string MGAnswer = new StreamReader(MGResponse.GetResponseStream()).ReadToEnd();
                    string MGStatus = JObject.Parse(MGAnswer).SelectToken("success").ToString();
                    if (MGStatus == "false")
                        throw new OnlineCheckException();
                    string MGResult = JObject.Parse(MGAnswer).SelectToken("response.[4]").ToString();
                    return MGResult;
                case "Magick":
                    WebRequest MRequest = WebRequest.Create($"{_baseURLv0}monitoring/players");
                    MRequest.Headers.Add("Authorization", $"Bearer {_userToken}");
                    MRequest.ContentType = "application/json";
                    MRequest.Method = "GET";
                    HttpWebResponse MResponse = (HttpWebResponse)MRequest.GetResponse();
                    string MAnswer = new StreamReader(MResponse.GetResponseStream()).ReadToEnd();
                    string MStatus = JObject.Parse(MAnswer).SelectToken("success").ToString();
                    if (MStatus == "false")
                        throw new OnlineCheckException();
                    string MResult = JObject.Parse(MAnswer).SelectToken("response.[6]").ToString();
                    return MResult;
                case "HiPower":
                    WebRequest HPRequest = WebRequest.Create($"{_baseURLv0}monitoring/players");
                    HPRequest.Headers.Add("Authorization", $"Bearer {_userToken}");
                    HPRequest.ContentType = "application/json";
                    HPRequest.Method = "GET";
                    HttpWebResponse HPResponse = (HttpWebResponse)HPRequest.GetResponse();
                    string HPAnswer = new StreamReader(HPResponse.GetResponseStream()).ReadToEnd();
                    string HPStatus = JObject.Parse(HPAnswer).SelectToken("success").ToString();
                    if (HPStatus == "false")
                        throw new OnlineCheckException();
                    string HPResult = JObject.Parse(HPAnswer).SelectToken("response.[2]").ToString();
                    return HPResult;
                case "TFC":
                    WebRequest TFCRequest = WebRequest.Create($"{_baseURLv0}monitoring/players");
                    TFCRequest.Headers.Add("Authorization", $"Bearer {_userToken}");
                    TFCRequest.ContentType = "application/json";
                    TFCRequest.Method = "GET";
                    HttpWebResponse TFCResponse = (HttpWebResponse)TFCRequest.GetResponse();
                    string TFCAnswer = new StreamReader(TFCResponse.GetResponseStream()).ReadToEnd();
                    string TFCStatus = JObject.Parse(TFCAnswer).SelectToken("success").ToString();
                    if (TFCStatus == "false")
                        throw new OnlineCheckException();
                    string TFCResult = JObject.Parse(TFCAnswer).SelectToken("response.[5]").ToString();
                    return TFCResult;
                case "Sandbox":
                    WebRequest SBRequest = WebRequest.Create($"{_baseURLv0}monitoring/players");
                    SBRequest.Headers.Add("Authorization", $"Bearer {_userToken}");
                    SBRequest.ContentType = "application/json";
                    SBRequest.Method = "GET";
                    HttpWebResponse SBResponse = (HttpWebResponse)SBRequest.GetResponse();
                    string SBAnswer = new StreamReader(SBResponse.GetResponseStream()).ReadToEnd();
                    string SBStatus = JObject.Parse(SBAnswer).SelectToken("success").ToString();
                    if (SBStatus == "false")
                        throw new OnlineCheckException();
                    string SBResult = JObject.Parse(SBAnswer).SelectToken("response.[0]").ToString();
                    return SBResult;
                case "MageTech":
                    WebRequest MTRequest = WebRequest.Create($"{_baseURLv0}monitoring/players");
                    MTRequest.Headers.Add("Authorization", $"Bearer {_userToken}");
                    MTRequest.ContentType = "application/json";
                    MTRequest.Method = "GET";
                    HttpWebResponse MTResponse = (HttpWebResponse)MTRequest.GetResponse();
                    string MTAnswer = new StreamReader(MTResponse.GetResponseStream()).ReadToEnd();
                    string MTStatus = JObject.Parse(MTAnswer).SelectToken("success").ToString();
                    if (MTStatus == "false")
                        throw new OnlineCheckException();
                    string MTResult = JObject.Parse(MTAnswer).SelectToken("response.[1]").ToString();
                    return MTResult;
                case "all":
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

            return null;
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
    }
}