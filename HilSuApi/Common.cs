using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HilSuApi
{
    public partial class Common : HilariousHub
    {
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
        /// Получить список команды проекта
        /// </summary>
        /// <returns></returns>
        /// <exception cref="StaffCheckException"></exception>
        public static string GetStaff()
        {
            HttpWebResponse request = Request("staff/list");
            string answer = new StreamReader(request.GetResponseStream()).ReadToEnd();
            string success = JObject.Parse(answer).SelectToken("success").ToString();

            if (success == "false")
                throw new StaffCheckException();

            return JObject.Parse(answer).SelectToken("response.staff").ToString();
        }
    }
}
