using System;

namespace HilSuApi
{
    // Ошибка авторизации
    public class AuthException : Exception
    {
        public AuthException() : base() { }
    }

    // Ошибка подключения к api.hil.su
    public class APIConnectException : Exception
    {
        public APIConnectException() : base() { }
    }

    // Ошибка запроса к api.hil.su
    public class RequestArgumentException : Exception
    {
        public RequestArgumentException() : base() { }
    }

    // Ошибка отображения онлайна серверов
    public class OnlineCheckException : Exception
    {
        public OnlineCheckException() : base() { }
    }

    // Ошибка получения токена
    public class TokenReferenceException: Exception
    {
        public TokenReferenceException() : base() { }
    }

    // Привышен лимит игроков в запросе богатых игроков
    public class TopPlayersLimitException : Exception
    {
        public TopPlayersLimitException() : base() { }
    }

    // Ошибка при получении баланса на аккаунте
    public class BalanceCheckException : Exception
    {
        public BalanceCheckException() : base() { }
    }

    public class TransferException : Exception
    {
        public TransferException() : base() { }
    }

    public class ExperienceCheckException : Exception
    {
        public ExperienceCheckException() : base() { }
    }

    public class StaffCheckException : Exception
    {
        public StaffCheckException() : base() { }
    }

    public class JobsCheckException : Exception
    {
        public JobsCheckException() : base() { }
    }

    public class UserInfoCheckException : Exception
    {
        public UserInfoCheckException() : base() { }
    }
}