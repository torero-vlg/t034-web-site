using T034.Core.DataAccess;
using Ninject;

namespace T034.Core.Api.Common
{
    public abstract class AbstractService
    {
        [Inject]
        public IBaseDb Db { get; set; }
    }
}
