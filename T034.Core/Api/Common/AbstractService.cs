using T034.Core.DataAccess;

namespace T034.Core.Api.Common
{
    public abstract class AbstractService
    {
        public AbstractService(IBaseDb db)
        {
            Db = db;
        }

        protected IBaseDb Db { get; set; }
    }
}
