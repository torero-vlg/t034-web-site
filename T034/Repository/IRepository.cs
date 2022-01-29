using Db.Entity.Administration;

namespace T034.Repository
{
    public interface IRepository
    {
        User Login(string email, string password);
        User GetUser(string email);
    }
}