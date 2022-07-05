using System.Data.Entity;

namespace Data
{
    public class BookStoreUnitOfWork : IBookStoreUnitOfWork
    {
        public DbContext DbContext { get; private set; }

        public BookStoreUnitOfWork(DbContext context) =>
            DbContext = context;

        public void Commit() =>
            DbContext.SaveChanges();
        
        public void Dispose() =>
            DbContext.Dispose();
        

        public IBookStoreRepository<T> GetRepo<T>() where T : class
        {
            return new BookStoreRepository<T>(DbContext);
        }
    }

}
