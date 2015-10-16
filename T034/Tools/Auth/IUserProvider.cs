using Db.Entity.Administration;

namespace T034.Tools.Auth
{
    public interface IUserProvider
    {
        User User { get; set; }
    }
}