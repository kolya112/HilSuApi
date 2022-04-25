using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HilSuApi
{
    public class AuthHub
    {
        public static async Task<string> GetUserToken(string username, string password)
        {
            var stringContent = new StringContent("{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}", Encoding.UTF8, "application/json");
            var response = await new HttpClient().PostAsync("https://api.hil.su/v2/auth/account/login/password", stringContent);
            string str = await response.Content.ReadAsStringAsync();

            string success = JObject.Parse(str).SelectToken("success").ToString();

            if (success == "false")
                throw new AuthException();

            string token = JObject.Parse(str).SelectToken("response.accessToken").ToString();
            return token;
        }
    }
}
