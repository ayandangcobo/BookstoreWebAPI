using BookStoreDomain.Contracts;
using BookStoreDomain.Models;
using BookStoreDomain.Utils;
using System;
using System.Linq;
using Data;
using BookStoreDomain.Helpers;

namespace BookStoreDomain.Implementation
{
    public class UserAuth : IUserAuth
    {
        private readonly IBookStoreUnitOfWork _uow;
        private readonly IUserAuthToken _authToken;
        private readonly IUserRoleManager _roleManager;
        public UserAuth(IBookStoreUnitOfWork uow, IUserAuthToken authToken, IUserRoleManager roleManager)
        {
            _uow = uow;
            _authToken = authToken;
            _roleManager = roleManager;
        }

        public RetVal<string> AuthUser(UserLoginViewModel user)
        {
            var response = new RetVal<string>();
            var validateResults = DomainValidator.ValidateModel(user);

            if (validateResults.Any())
            {
                validateResults.ForEach(e =>
                {
                    response.errors.Add(e.ErrorMessage);
                });
                response.Message = "Login Error";
                return response;
            }

            var repo = _uow.GetRepo<User>();
            var checkUser = repo.GetByIQuerable(s => s.UserName == user.Username.Trim())
                .FirstOrDefault()
                .MapProperties<UserModel>();
            if (checkUser.UserId != 0)
            {
                bool isPasswordMatched = SaltManager.VerifyPassword(user.Password, checkUser.Password, checkUser.UserSalt);
                if (isPasswordMatched)
                {
                    //get token
                    var roles = _roleManager.GetUserRole(checkUser.UserId);
                    string token = _authToken.GenerateAuthToken(checkUser, roles);
                    response.results.Add(token);

                    response.Message = "User login successful.";
                    response.IsSuccess = true;
                    return response;
                }
            }

            response.IsSuccess = false;
            response.Message = "Login Error";
            response.errors.Add("username and password combination does not match! ");
            return response;
        }
        private bool CheckPseudonym(string Pseudonym)
        {
            bool exists = false;
            var repo = _uow.GetRepo<User>();
            var checkPseudonym = repo.GetByIQuerable(s => s.AuthorPseudonym == Pseudonym.Trim()).FirstOrDefault();

            if (checkPseudonym != null)
                exists = true;

            return exists;
        }
        public RetVal<string> RegisterUser(UserRegistrationViewModel user)
        {
            var response = new RetVal<string>();

            var validateResults = DomainValidator.ValidateModel(user);

            if (validateResults.Any())
            {
                validateResults.ForEach(e =>
                {
                    response.errors.Add(e.ErrorMessage);
                });
                response.Message = "Registration Error";
                return response;
            }


            var repo = _uow.GetRepo<User>();

            var checkUser = repo.GetByIQuerable(s => s.UserName == user.Username.Trim()).FirstOrDefault();

            if (checkUser == null)
            {
                if (!CheckPseudonym(user.AuthorPseudonym.Trim()))
                {
                    var hashSalt = SaltManager.GenerateSaltedHash(64, user.Password);
                    user.Password = hashSalt.Hash;
                    var dateStamp = DateTime.Now;

                    repo.Add(new User
                    {
                        UserName = user.Username,
                        AuthorPseudonym = user.AuthorPseudonym,
                        UserSalt = hashSalt.Salt,
                        CreatedDate = dateStamp,
                        IsActive = true,
                        Password = user.Password,
                    });
                    repo.SaveChanges();
                }
                else
                {
                    response.Message = "Registration Error";
                    response.errors.Add("Author Pseudonym already taken. Please try choosing a different one.");
                    return response;
                }
            }
            else
            {
                bool isPasswordMatched = SaltManager.VerifyPassword(user.Password, checkUser.Password, checkUser.UserSalt);
                if (isPasswordMatched)
                {
                    response.Message = "Registration Error";
                    response.errors.Add("User already exist.");
                    return response;
                }
            }
            response.Message = "User Registration Successful";
            response.IsSuccess = true;
            return response;
        }
        public RetVal<string> CheckAuthorPseudonym(string Pseudonym)
        {
            var response = new RetVal<string>();

            if (string.IsNullOrEmpty(Pseudonym.Trim()))
            {
                response.IsSuccess = false;
                response.Message = "Pseudonym Check Error";
                response.errors.Add("Please provide author pseudonym to check");
            }

            if (CheckPseudonym(Pseudonym.Trim()))
            {
                response.IsSuccess = false;
                response.Message = "Pseudonym Check";
                response.errors.Add($"Author Pseudonym '{Pseudonym}' already taken. Please try choosing a different one.");
            }
            else
            {
                response.IsSuccess = true;
                response.Message = "Author Pseudonym is available :)";
            }

            return response;
        }

    }

}
