using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStoreDomain.Contracts;
using BookStoreDomain.Implementation;
using Data;
using BookStoreDomain.Models;

namespace BookstoreWebAPI.Tests.Controllers
{
    /// <summary>
    /// Summary description for AuthControllerTest
    /// </summary>
    [TestClass]
    public class AuthControllerTest
    {
        private readonly IUserAuth _auth;
        private readonly IBookStoreUnitOfWork _uow;
        private readonly IUserRoleManager _roleManager;
        private readonly IUserAuthToken _authToken;

        public AuthControllerTest()
        {
            _uow = new BookStoreUnitOfWork(new BookStoreEntities());
            _authToken = new UserAuthToken();
            _auth = new UserAuth(_uow, _authToken, _roleManager);
        }


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void can_validate_registration_model()
        {
            var result = _auth.RegisterUser(new UserRegistrationViewModel());
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void can_register_wookie_to_store()
        {
            var result = _auth.RegisterUser(new UserRegistrationViewModel
            {
                Username = $"testuser@wookie.com",
                AuthorPseudonym = $"Bruce Wayne",
                Password = "1234",
                ConfirmPassword = "1234"
            });
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void ensure_only_registered_wookies_can_get_token()
        {
            var login = _auth.AuthUser(new UserLoginViewModel
            {
                Username = $"testuser@wookie.com",
                Password = "1234",
            });

            Assert.IsTrue(login.IsSuccess);
        }


        
    }
}
