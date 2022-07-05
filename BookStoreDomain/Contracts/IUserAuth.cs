using BookStoreDomain.Implementation;
using BookStoreDomain.Models;
using Data;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace BookStoreDomain.Contracts
{
    public interface IUserAuth
    {
        RetVal<string> AuthUser(UserLoginViewModel user);
        RetVal<string> CheckAuthorPseudonym(string Pseudonym);
        RetVal<string> RegisterUser(UserRegistrationViewModel user);
    }

    public interface IUserAuthToken
    {
        string GenerateAuthToken(UserModel user, List<string> roles);
        string GetClaimValue(IPrincipal user, string claimName);
    }

    public interface IUserRoleManager
    {
        List<string> GetUserRole(int userId);
        RetVal<string> AddUserRole(UserRoleCreateViewModel user);
        RetVal<string> AddRole(RoleCreateViewModel user);
    }
}
