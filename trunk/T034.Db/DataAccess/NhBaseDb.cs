using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Db.Tools;
using NHibernate;

namespace Db.DataAccess
{
    public class NhBaseDb : IBaseDb
    {
        protected readonly ISessionFactory SessionFactory;

        public NhBaseDb(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        public T Get<T>(int id) where T : Entity.Entity
        {
            using (var session = SessionFactory.OpenSession())
            {
                try
                {
                    return session.Get<T>(id);
                }
                catch (Exception ex)
                {
                    MonitorLog.WriteLog("Ошибка выполнения процедуры Get<" + typeof(T) + "> : " + ex.Message,
                        MonitorLog.typelog.Error, true);
                    throw;
                }
            }
        }

        public List<T> Select<T>() where T : Entity.Entity
        {
            List<T> result;
            using (var session = SessionFactory.OpenSession())
            {
                try
                {
                    var query = session.QueryOver<T>();

                    result = (List<T>) query
                        .OrderBy(p => p.Id).Asc
                        .List<T>();
                }
                catch (Exception ex)
                {
                    MonitorLog.WriteLog("Ошибка выполнения процедуры Select<" + typeof(T) + "> : " + ex.Message,
                        MonitorLog.typelog.Error, true);
                    throw;
                }
            }
            return result;
        }

        public List<T> Where<T>(Expression<Func<T, bool>> expression) where T : Entity.Entity
        {
            List<T> result;
            using (var session = SessionFactory.OpenSession())
            {
                try
                {
                    var query = session.QueryOver<T>().Where(expression);
                    result = (List<T>)query
                        .OrderBy(p => p.Id).Asc
                        .List<T>();
                }
                catch (Exception ex)
                {
                    MonitorLog.WriteLog("Ошибка выполнения процедуры Where<" + typeof(T) + "> : " + ex.Message,
                        MonitorLog.typelog.Error, true);
                    throw;
                }
            }
            return result;
        }

        public T SingleOrDefault<T>(Expression<Func<T, bool>> expression) where T : Entity.Entity
        {
            T result;
            using (var session = SessionFactory.OpenSession())
            {
                try
                {
                    var query = session.QueryOver<T>().Where(expression).SingleOrDefault();
                    result = query;
                }
                catch (Exception ex)
                {
                    MonitorLog.WriteLog("Ошибка выполнения процедуры SingleOrDefault<" + typeof(T) + "> : " + ex.Message,
                        MonitorLog.typelog.Error, true);
                    throw;
                }
            }
            return result;
        }

        public int Save<T>(T entity) where T : Entity.Entity
        {
            int result;
            using (var session = SessionFactory.OpenSession())
            {
                using (var tran = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(entity);

                        tran.Commit();
                        result = entity.Id;
                    }
                    catch (Exception ex)
                    {
                        MonitorLog.WriteLog("Ошибка выполнения процедуры Save<" + typeof(T) + "> : " + ex.Message, MonitorLog.typelog.Error, true);
                        tran.Rollback();
                        result = 0;
                    }
                }
            }
            return result;
        }

        public void Save<T>(List<T> list) where T : Entity.Entity
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var tran = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var entity in list)
                        {
                            session.SaveOrUpdate(entity);                           
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        MonitorLog.WriteLog("Ошибка выполнения процедуры Save<" + typeof(T) + "> : " + ex.Message, MonitorLog.typelog.Error, true);
                        tran.Rollback();
                    }
                }
            }
        }
    }
}
