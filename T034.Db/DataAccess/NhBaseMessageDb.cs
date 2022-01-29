using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace T034.Core.DataAccess
{
    /// <summary>
    /// Базовый класс по работе с сообщениями
    /// </summary>
    public class NhBaseMessageDb : NhBaseDb, IBaseMessageDb
    {
        public NhBaseMessageDb(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public T GetMessage<T>(int msgId)
        {
            T result;
            using (var session = SessionFactory.OpenSession())
            {
                try
                {
                    result = session.Get<T>(msgId);
                }
                catch (Exception ex)
                {
                    Logger.Fatal("Ошибка выполнения процедуры GetMessage : " + ex.Message, ex, new[] { msgId });
                    throw ;
                }
            }
            return result;
        }

        /// <summary>
        /// Получить ИД следующего сообщения
        /// </summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="messageId">ИД сообщения</param>
        /// <returns></returns>
        public int GetNextMessageId<T>(int messageId) where T : Entity.Entity
        {
            List<int> result;
            using (var session = SessionFactory.OpenSession())
            {
                try
                {
                    result = (List<int>) session.QueryOver<T>()
                        .Where(p => p.Id > messageId)
                        .Select(p => p.Id)
                        .OrderBy(p => p.Id).Asc
                        .Take(1)
                        .List<int>();
                }
                catch (Exception ex)
                {
                    Logger.Fatal("Ошибка выполнения процедуры GetNextMessageId : " + ex.Message, ex, new[] { messageId });
                    throw;
                }
            }
            return !result.Any() ? 0 : result.First();
        }

        /// <summary>
        /// Получить ИД предыдущего сообщения
        /// </summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="messageId">ИД сообщения</param>
        /// <returns></returns>
        public int GetPrevMessageId<T>(int messageId) where T : Entity.Entity
        {
            List<int> result;
            using (var session = SessionFactory.OpenSession())
            {
                try
                {
                    result = (List<int>) session.QueryOver<T>()
                        .Where(p => p.Id < messageId)
                        .Select(p => p.Id)
                        .OrderBy(p => p.Id).Desc
                        .Take(1)
                        .List<int>();

                }
                catch (Exception ex)
                {
                    Logger.Fatal("Ошибка выполнения процедуры GetPrevMessageId : " + ex.Message, ex, new[] { messageId });
                    throw;
                }
            }
            return !result.Any() ? 0 : result.First();
        }
    }
}
