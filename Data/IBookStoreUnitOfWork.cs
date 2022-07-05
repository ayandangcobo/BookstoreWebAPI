namespace Data
{
    public interface IBookStoreUnitOfWork
    {
        void Commit();
        IBookStoreRepository<T> GetRepo<T>() where T : class;
    }

}
