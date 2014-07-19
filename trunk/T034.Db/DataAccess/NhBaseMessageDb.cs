using System;
using System.Collections.Generic;
using System.Linq;
using Db.Tools;
using NHibernate;

namespace Db.DataAccess
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
                    MonitorLog.WriteLog("Ошибка выполнения процедуры GetMessage : " + ex.Message,
                        MonitorLog.typelog.Error, true);
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
                    MonitorLog.WriteLog("Ошибка выполнения процедуры GetNextMessageId : " + ex.Message,
                        MonitorLog.typelog.Error, true);
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
                    MonitorLog.WriteLog("Ошибка выполнения процедуры GetPrevMessageId : " + ex.Message,
                        MonitorLog.typelog.Error, true);
                    throw;
                }
            }
            return !result.Any() ? 0 : result.First();
        }
    }
}
