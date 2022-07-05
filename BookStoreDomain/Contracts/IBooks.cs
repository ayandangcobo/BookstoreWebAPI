using BookStoreDomain.Models;
using System.Security.Principal;

namespace BookStoreDomain.Contracts
{
    public interface IBooks
    {
        RetVal<string> AddBook(IPrincipal user);
        RetVal<BookViewModel> GetBooks();
        RetVal<BookViewModel> SearchBook(BookSearchViewModel bookSearch);
        RetVal<BookViewModel> GetBookDetail(int id);
        RetVal<string> PublishBook(int id, IPrincipal user);
        RetVal<string> UnPublishBook(int id, IPrincipal user);
    }

}
