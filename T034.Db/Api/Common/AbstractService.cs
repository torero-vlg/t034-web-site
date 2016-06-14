using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db.DataAccess;
using Ninject;
using NLog;

namespace Db.Api.Common
{
    public abstract class AbstractService
    {
        [Inject]
        public IBaseDb Db { get; set; }

        //TODO проверить как работает логгер в наследуемых классах
        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();
    }
}
