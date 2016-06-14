using Db.DataAccess;
using Ninject;

namespace Db.Api.Common
{
    public abstract class AbstractService
    {
        [Inject]
        public IBaseDb Db { get; set; }
    }
}
