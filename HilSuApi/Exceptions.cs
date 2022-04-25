using System;

namespace HilSuApi
{
    // Ошибка авторизации
    internal class AuthException : Exception
    {
        public AuthException() : base() { }
    }

    // Ошибка подключения к api.hil.su
    internal class APIConnectException : Exception
    {
        public APIConnectException() : base() { }
    }

    // Ошибка запроса к api.hil.su
    internal class RequestArgumentException : Exception
    {
        public RequestArgumentException() : base() { }
    }

    // Введена несуществующая валюта
    internal class EconomyCurrencyException : Exception
    {
        public EconomyCurrencyException() : base() { }
    }
}
