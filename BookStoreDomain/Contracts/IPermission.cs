using System.Security.Principal;
namespace BookStoreDomain.Contracts
{
    public interface IPermission
    {
        bool CheckDeletePermission(IPrincipal user, int bookId);

    }
}
