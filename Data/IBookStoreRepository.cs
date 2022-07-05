using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Data
{
    public interface IBookStoreRepository<T> 
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
        IQueryable<T> GetByIQuerable(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
    }

}
