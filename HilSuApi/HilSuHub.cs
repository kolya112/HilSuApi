using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

// TODO
// /v0/support/ticket/new - post
// /v0/support/servers
// /v2/support/categories - auth
// https://api.hil.su/v0/updates
// https://api.hil.su/v0/announces
// https://api.hil.su/v0/streams
// https://api.hil.su/v0/user/reward/return - auth
// https://api.hil.su/v0/stats/chat
// https://api.hil.su/v0/stats/wipe
// /v0/user/page - auth

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

        /// <summary>
        /// Авторизация без пользовательского токена
        /// </summary>
        public HilariousHub()
        {

        }

        public enum Currency
        {
            Coins,
            Gems
        }

        public partial class Common : HilSuApi.Common { }
        public partial class User : HilSuApi.User
        {
            public User(string token) : base(token)
            {
            }
        }
        public partial class Stats : HilSuApi.Stats { }

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