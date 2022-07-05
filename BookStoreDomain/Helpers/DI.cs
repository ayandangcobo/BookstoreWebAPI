using BookStoreDomain.Contracts;
using BookStoreDomain.Implementation;
using Data;
using Ninject;

namespace BookStoreDomain.Helpers
{
    public static class DI
    {
        public static void RegisterDIServices(IKernel kernel)
        {
            kernel.Bind<IBookStoreUnitOfWork>().ToConstructor(i => new BookStoreUnitOfWork(new BookStoreEntities()));
            kernel.Bind<IUserRoleManager>().To<RoleManager>();
            kernel.Bind<IUserAuthToken>().To<UserAuthToken>();
            kernel.Bind<IUserAuth>().To<UserAuth>();
            kernel.Bind<IBooks>().To<Books>();
            kernel.Bind<IPermission>().To<Permission>();
        }
    }
}

