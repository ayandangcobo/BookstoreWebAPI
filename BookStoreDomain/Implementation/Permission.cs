using BookStoreDomain.Contracts;
using Data;
using System.Linq;
using System.Security.Principal;

namespace BookStoreDomain.Implementation
{
    public class Permission : IPermission
    {
        private readonly IBookStoreUnitOfWork _uow;
        private readonly IUserAuthToken _authToken;

        public Permission(IBookStoreUnitOfWork uow, IUserAuthToken authToken)
        {
            _uow = uow;
            _authToken = authToken;
        }

        public bool CheckDeletePermission(IPrincipal user, int bookId)
        {
            bool isAllowed = false;

            int userId = 0;
            int.TryParse(_authToken.GetClaimValue(user, "userid"), out userId);

            var repo = _uow.GetRepo<Book>();
            var book = repo.GetByIQuerable(b => b.BookId == bookId && b.Author == userId).FirstOrDefault();

            if (book == null)
                isAllowed = false;
            else
                isAllowed = true;

            return isAllowed;
        }
    }
}
