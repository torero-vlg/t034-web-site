using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NLog;

namespace T034.Core.DataAccess
{
    public class NhBaseDb : IBaseDb
    {
        protected readonly ISessionFactory SessionFactory;
        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public NhBaseDb(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        [Obsolete("Использовать Get<T>(object id)")]
        public T Get<T>(int id) where T : Entity.Entity
        {
            Logger.Trace($"{System.Reflection.MethodBase.GetCurrentMethod().Name}<{typeof(T)}>.Параметры: {id}");

            using (var session = SessionFactory.OpenSession())
            {
                try
                {
                    return session.Get<T>(id);

                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex);
                    throw;
                }
            }
        }

        public T Get<T>(object id) where T : Entity.Entity
        {
            Logger.Trace($"{System.Reflection.MethodBase.GetCurrentMethod().Name}<{typeof(T)}>.Параметры: {id}");

            using (var session = SessionFactory.OpenSession())
            {
                try
                {
                    return session.Get<T>(id);

                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex);
                    throw;
                }
            }
        }

        public List<T> Select<T>() where T : Entity.Entity
        {
            Logger.Trace($"{System.Reflection.MethodBase.GetCurrentMethod().Name}<{typeof(T)}>");
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
                    Logger.Fatal(ex);
                    throw;
                }
            }
            return result;
        }

        public List<T> Where<T>(Expression<Func<T, bool>> expression) where T : Entity.Entity
        {
            Logger.Trace($"{System.Reflection.MethodBase.GetCurrentMethod().Name}<{typeof(T)}>.Параметры: {expression}");
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
                    Logger.Fatal(ex);
                    throw;
                }
            }
            return result;
        }

        public T SingleOrDefault<T>(Expression<Func<T, bool>> expression) where T : Entity.Entity
        {
            Logger.Trace($"{System.Reflection.MethodBase.GetCurrentMethod().Name}<{typeof(T)}>.Параметры: {expression}");
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
                    Logger.Fatal(ex);
                    throw;
                }
            }
            return result;
        }

        public int Save<T>(T entity) where T : Entity.Entity
        {
            Logger.Trace($"{System.Reflection.MethodBase.GetCurrentMethod().Name}<{typeof(T)}>.Параметры: {entity}");
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
                        Logger.Fatal(ex);
                        tran.Rollback();
                        result = 0;
                    }
                }
            }
            return result;
        }

        public void Save<T>(List<T> list) where T : Entity.Entity
        {
            Logger.Trace($"{System.Reflection.MethodBase.GetCurrentMethod().Name}<{typeof(T)}>.Параметры: {string.Join(",", list)}");
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
                        Logger.Fatal(ex);
                        tran.Rollback();
                    }
                }
            }
        }

        public int SaveOrUpdate<T>(T entity) where T : Entity.Entity
        {
            Logger.Trace($"{System.Reflection.MethodBase.GetCurrentMethod().Name}<{typeof(T)}>.Параметры: {entity}");
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
                        Logger.Fatal(ex);
                        tran.Rollback();
                        result = 0;
                    }
                }
            }
            return result;
        }

        public bool Delete<T>(T entity) where T : Entity.Entity
        {
            Logger.Trace($"{System.Reflection.MethodBase.GetCurrentMethod().Name}<{typeof(T)}>.Параметры: {entity}");
            bool result;
            using (var session = SessionFactory.OpenSession())
            {
                using (var tran = session.BeginTransaction())
                {
                    try
                    {
                        session.Delete(entity);

                        tran.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Fatal(ex);
                        tran.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }
    }
}
