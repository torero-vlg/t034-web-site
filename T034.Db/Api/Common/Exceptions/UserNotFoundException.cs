namespace T034.Core.Api.Common.Exceptions
{
    public class UserNotFoundException : BusinessException
    {
        public UserNotFoundException(string email) 
            : base($"Не найден пользователь {email}")
        {
        }
    }
}
