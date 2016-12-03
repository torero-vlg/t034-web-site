using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Db.DataAccess
{
    public interface IBaseDb
    {
        [Obsolete("Использовать Get<T>(object id)")]
        T Get<T>(int id) where T : Entity.Entity;
        T Get<T>(object id) where T : Entity.Entity;
        List<T> Select<T>() where T : Entity.Entity;
        List<T> Where<T>(Expression<Func<T, bool>> expression) where T : Entity.Entity;
        T SingleOrDefault<T>(Expression<Func<T, bool>> expression) where T : Entity.Entity;
        int SaveOrUpdate<T>(T entity) where T : Entity.Entity;
        int Save<T>(T entity) where T : Entity.Entity;
        void Save<T>(List<T> list) where T : Entity.Entity;
        bool Delete<T>(T entity) where T : Entity.Entity;
    }
}
