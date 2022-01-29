using T034.Core.Entity.Administration;

namespace T034.Tools.Auth
{
    public interface IUserProvider
    {
        User User { get; set; }
    }
}