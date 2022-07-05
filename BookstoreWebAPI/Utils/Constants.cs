using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookstoreWebAPI
{
    public static class ApiRoutes
    {
        #region User auth

        public const string Auth = "api/auth";
        public const string Login = "login";
        public const string Register = "register";
        public const string CheckPseudonym = "author/{name}";

        #endregion

        #region Roles
        public const string AddRole = "role/add";
        public const string AddUserRole = "role/adduser";
        #endregion


        #region Books

        public const string Books = "api/books";

        public const string AddBooks = "add";
        public const string ListBooks = "list";
        public const string SearchBooks = "search";
        public const string ListBookDetail = "detail/{Id:int}";
        public const string Publish = "publish/{Id:int}";
        public const string UnPublish = "unpublish/{Id:int}";

        #endregion



    }


    public static class Roles
    {
        public const string AllowPublish = "publisher";

    }
}