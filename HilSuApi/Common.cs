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
