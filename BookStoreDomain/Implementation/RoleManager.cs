using BookStoreDomain.Contracts;
using BookStoreDomain.Models;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreDomain.Implementation
{
    public class RoleManager : IUserRoleManager
    {
        private readonly IBookStoreUnitOfWork _uow;

        public RoleManager(IBookStoreUnitOfWork uow) =>
            _uow = uow;

        public RetVal<string> AddRole(RoleCreateViewModel newRole)
        {
            var response = new RetVal<string>();
            var validateResults = DomainValidator.ValidateModel(newRole);

            if (validateResults.Any())
            {
                validateResults.ForEach(e =>
                {
                    response.errors.Add(e.ErrorMessage);
                });
                return response;
            }
            var repo = _uow.GetRepo<Role>();
            var role = repo.GetByIQuerable(r => r.RoleName == newRole.Name.Trim());

            if(role != null)
            {
                repo.Add(new Role
                {
                    RoleName = newRole.Name.Trim(),
                    CreatedDate = DateTime.Now
                });

                response.Message = "Role added successfully";
                response.IsSuccess = true;
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Role add error";
                response.errors.Add("Role already exists");
                return response;
            }
        }

        public RetVal<string> AddUserRole(UserRoleCreateViewModel userRole)
        {
            var response = new RetVal<string>();
            var validateResults = DomainValidator.ValidateModel(userRole);

            if (validateResults.Any())
            {
                validateResults.ForEach(e =>
                {
                    response.errors.Add(e.ErrorMessage);
                });
                return response;
            }

            var repo = _uow.GetRepo<UserRole>();

            repo.Add(new UserRole
            {
                RoleId = userRole.RoleId,
                UserId = userRole.UserId,
                CreatedDate = DateTime.Now
            });
            repo.SaveChanges();

            response.Message = "User role added successfully";
            response.IsSuccess = true;
            return response;
        }

        public List<string> GetUserRole(int userId)
        {
            var response = new List<string>();

            var repo = _uow.GetRepo<UserRole>();
            var roles = repo.GetByIQuerable(r => r.UserId == userId, null, "Role");

            if (roles.Any())
            {
                response.AddRange(roles.Select(r => r.Role.RoleName));
            }

            return response;
        }
    }
}
