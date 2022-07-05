using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Data
{
    internal class BookStoreRepository<T> : IBookStoreRepository<T> where T : class
    {
        public DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public BookStoreRepository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("DB context must be defined");
            }
            DbContext = dbContext;
            DbContext.Database.CommandTimeout = 120;
            DbSet = DbContext.Set<T>();
        }
        public void Add(T entity)
        {
            var dbEntityEntry = DbContext.Entry<T>(entity);

            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public void Update(T entity)
        {
            var dbEntityEntry = DbContext.Entry<T>(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public IQueryable<T> GetByIQuerable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            var cmd = DbContext.Database.Connection.CreateCommand();
            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                cmd.Connection.Open();
            cmd.CommandText = "SET ARITHABORT ON";
            cmd.ExecuteNonQuery();
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                IQueryable<T> query = DbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }

                var result = orderBy != null ? orderBy(query) : query;

                transaction.Complete();
                cmd.Connection.Close();
                cmd.Dispose();
                return result;
            }
        }


    }
}