using T034.Core.DataAccess;

namespace T034.Core
{
    /// <summary>
    /// Абстрактная фабрика
    /// </summary>
    public abstract class AbstractDbFactory
    {
        /// <summary>
        /// Создать базовый менеджер
        /// </summary>
        /// <returns></returns>
        public abstract IBaseDb CreateBaseDb();
    }
}
