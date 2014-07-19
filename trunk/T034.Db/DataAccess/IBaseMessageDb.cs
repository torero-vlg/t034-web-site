namespace Db.DataAccess
{
    /// <summary>
    /// Базовый интерфейс по работе с сообщениями
    /// </summary>
    public interface IBaseMessageDb
    {
        /// <summary>
        /// Получить сообщение
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="msgId">Код сообщения</param>
        /// <returns>Сообщение</returns>
        T GetMessage<T>(int msgId);
        
        /// <summary>
        /// Получить код следующего сообщения
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="messageId">Код текущего сообщения</param>
        /// <returns></returns>
        int GetNextMessageId<T>(int messageId) where T : Entity.Entity;

        /// <summary>
        /// Получить код предыдущего сообщения
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="messageId">Код текущего сообщения</param>
        /// <returns></returns>
        int GetPrevMessageId<T>(int messageId) where T : Entity.Entity;
    }
}
