using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStoreDomain.Contracts;
using Data;
using BookStoreDomain.Implementation;
using BookstoreWebAPI.Controllers;
using BookStoreDomain;
using BookStoreDomain.Models;
using System.Web.Http.Results;
using Moq;
using System.Linq;
using System.Web.Http;

namespace BookstoreWebAPI.Tests.Controllers
{
    /// <summary>
    /// Summary description for BookControllerTest
    /// </summary>
    [TestClass]
    public class BookControllerTest
    {
        private readonly IUserAuth _auth;
        private readonly IBookStoreUnitOfWork _uow;
        private readonly IUserRoleManager _roleManager;
        private readonly IUserAuthToken _authToken;
        private readonly IBooks _books;
        private readonly IPermission _permissions;

        public BookControllerTest()
        {
            _uow = new BookStoreUnitOfWork(new BookStoreEntities());
            _authToken = new UserAuthToken();
            _auth = new UserAuth(_uow, _authToken, _roleManager);
            _permissions = new Permission(_uow, _authToken);
            _books = new Books(_uow, _authToken, _permissions);
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
        public void can_retrieve_book_list()
        {
            BooksController controller = new BooksController(_books);
            var actionResult = controller.GetBookDetail(1);

            var contentResult = actionResult as OkNegotiatedContentResult<RetVal<BookViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.IsSuccess);
            Assert.AreEqual(true, contentResult.Content.results.Any());
        }

        public void can_retrieve_book_detail()
        {
            BooksController controller = new BooksController(_books);
            var actionResult = controller.GetBookDetail(1);

            var contentResult = actionResult as OkNegotiatedContentResult<RetVal<BookViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.results?.FirstOrDefault().BookId);

        }
    }
}
