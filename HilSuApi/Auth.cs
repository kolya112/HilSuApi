using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace HilSuApi
{
    internal class Auth
    {
        // Конструктур с наличием токена
        public Auth(string token)
        {

        }

        // Конструктур без наличия токена
        public Auth() { }

        // Конструктур для авторизации через аккаунт
        public Auth(string nickname, string password)
        {

        }
    }
}
