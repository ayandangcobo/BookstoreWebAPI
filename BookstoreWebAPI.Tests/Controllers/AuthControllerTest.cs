using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStoreDomain.Contracts;
using BookStoreDomain.Implementation;
using Data;
using BookStoreDomain.Models;
using BookstoreWebAPI.Controllers;
using System.Web.Http.Results;
using BookStoreDomain;
using System.Linq;
using System.Net;

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
            _roleManager = new RoleManager(_uow);
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
            Assert.IsTrue(result.errors.Any());

            //var controller = new AuthController(_auth, _roleManager);
            //var ActionResult = controller.Register(new UserRegistrationViewModel());
            //var ContentResult = ActionResult as OkNegotiatedContentResult<RetVal<string>>;

            //Assert.IsFalse(ContentResult.Content.IsSuccess);
            //Assert.IsInstanceOfType(ActionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void can_register_wookie_to_store()
        {
            int randomVal = 0;
            randomVal = new Random().Next();
            var controller = new AuthController(_auth, _roleManager);
            var ActionResult = controller.Register(new UserRegistrationViewModel
            {
                Username = $"testuser{randomVal}@wookie.com",
                AuthorPseudonym = $"Test{randomVal}",
                Password = "1234",
                ConfirmPassword = "1234"
            });
            var ContentResult = ActionResult as OkNegotiatedContentResult<RetVal<string>>;

            Assert.IsTrue(ContentResult.Content.IsSuccess);
            Assert.IsFalse(ContentResult.Content.errors.Any());



        }

        [TestMethod]
        public void ensure_only_registered_wookies_can_get_token()
        {

            var controller = new AuthController(_auth, _roleManager);
            var ActionResult = controller.Login(new UserLoginViewModel
            {
                Username = $"testuser@wookie.com",
                Password = "1234",
            });
            var ContentResult = ActionResult as OkNegotiatedContentResult<string>;
            Assert.IsNotNull(ContentResult.Content);

        }


        
    }
}
