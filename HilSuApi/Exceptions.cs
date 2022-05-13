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

    // Ошибка отображения онлайна серверов
    internal class OnlineCheckException : Exception
    {
        public OnlineCheckException() : base() { }
    }

    // Ошибка получения токена
    internal class TokenReferenceException: Exception
    {
        public TokenReferenceException() : base() { }
    }

    // Привышен лимит игроков в запросе богатых игроков
    internal class TopPlayersLimitException : Exception
    {
        public TopPlayersLimitException() : base() { }
    }
}
