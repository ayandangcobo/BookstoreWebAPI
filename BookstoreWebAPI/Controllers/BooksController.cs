using BookStoreDomain.Contracts;
using BookStoreDomain.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookstoreWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix(ApiRoutes.Books)]
    public class BooksController : ApiController
    {
        private readonly IBooks _books;

        public BooksController(IBooks books) =>
            _books = books;

        [HttpPost]
        [Route(ApiRoutes.AddBooks)]
        public IHttpActionResult AddBook()
        {
            var response = _books.AddBook(User);
            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(ApiRoutes.ListBooks)]
        public IHttpActionResult GetBooks() =>
             Ok(_books.GetBooks());

        [AllowAnonymous]
        [HttpGet]
        [Route(ApiRoutes.ListBookDetail)]
        public IHttpActionResult GetBookDetail(int Id)
        {
            var response = _books.GetBookDetail(Id);
            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(ApiRoutes.SearchBooks)]
        public IHttpActionResult GetBooks([FromBody] BookSearchViewModel search)
        {
            var response = _books.SearchBook(search);

            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }

        [HttpPut]
        [Authorize(Roles = Roles.AllowPublish)]
        [Route(ApiRoutes.Publish)]
        public IHttpActionResult PublishBook(int Id)
        {
            var response = _books.PublishBook(Id, User);
            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }


        [HttpDelete]
        [Route(ApiRoutes.UnPublish)]
        public IHttpActionResult UnPublishBook(int Id)
        {
            var response = _books.UnPublishBook(Id, User);

            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }
    }
}