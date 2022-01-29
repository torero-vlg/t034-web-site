using T034.Core.Entity.Administration;

namespace T034.Repository
{
    public interface IRepository
    {
        User Login(string email, string password);
        User GetUser(string email);
    }
}