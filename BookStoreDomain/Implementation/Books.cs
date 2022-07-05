using BookStoreDomain.Contracts;
using BookStoreDomain.Models;
using Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BookStoreDomain.Implementation
{
    public class Books : IBooks
    {
        private readonly IBookStoreUnitOfWork _uow;
        private readonly IUserAuthToken _authToken;
        private readonly IPermission _permission;

        public Books(IBookStoreUnitOfWork uow, IUserAuthToken authToken, IPermission permission)
        {
            _uow = uow;
            _authToken = authToken;
            _permission = permission;
        }

        public RetVal<string> AddBook(IPrincipal user)
        {
            var response = new RetVal<string>();

            var book = new BookCreateViewModel();
            var request = HttpContext.Current.Request;

            book.Title = request.Params["title"];
            book.Description = request.Params["description"];

            int authorId = 0;
            int.TryParse(request.Params["author"], out authorId);
            book.Author = authorId;

            decimal price = 0;
            decimal.TryParse(request.Params["price"], out price);
            book.Price = price;


            var validateResults = DomainValidator.ValidateModel(book);

            if (validateResults.Any())
            {
                validateResults.ForEach(e =>
                {
                    response.errors.Add(e.ErrorMessage);
                });
                response.Message = "Book Add Error";
                return response;
            }

            //Fetch the cover image.
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                var postedFile = HttpContext.Current.Request.Files[0];
                var ImageExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".png" };

                if (!ImageExtensions.Contains(Path.GetExtension(postedFile.FileName)))
                {
                    response.Message = "Book Add include book cover image (formats: .jpg, .jpeg, .bmp, .png)";
                    response.IsSuccess = false;
                    return response;
                }

                string path = HttpContext.Current.Server.MapPath("~/Files/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //Save the image.
                postedFile.SaveAs(path + postedFile.FileName);

                var repo = _uow.GetRepo<Book>();

                int userId = 0;
                int.TryParse(_authToken.GetClaimValue(user, "userid"), out userId);

                repo.Add(new Book
                {
                    Author = userId,
                    BookDescription = book.Description,
                    CreatedDate = DateTime.Now,
                    CoverImage = Path.GetFileName(postedFile.FileName),
                    Title = book.Title,
                    Price = book.Price 

                });
                repo.SaveChanges();
            }
            else
            {
                response.Message = "Book Add Error";
                response.errors.Add("No book cover image attachment found. ");
                response.errors.Add("Please attach book cover image (formats: .jpg, .jpeg, .bmp, .png)");
                response.IsSuccess = false;
                return response;
            }



            response.Message = "Book added succesfully";
            response.IsSuccess = true;
            return response;
        }

        public RetVal<BookViewModel> GetBookDetail(int id)
        {
            var response = new RetVal<BookViewModel>();
            var repo = _uow.GetRepo<Book>();
            var book = repo.GetByIQuerable(b => b.BookId == id).FirstOrDefault();

            if (book != null)
            {
                response.IsSuccess = true;
                response.results.Add(book.ToBookModel());
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Book detail retrieval error";
                response.errors.Add("Book information not found");
                return response;
            }

            return response;
        }

        public RetVal<BookViewModel> GetBooks()
        {
            var response = new RetVal<BookViewModel>();
            var repo = _uow.GetRepo<Book>();

            response.IsSuccess = true;
            response.results = repo.GetByIQuerable().ToBookModelList();
            return response;
        }

        public RetVal<BookViewModel> SearchBook(BookSearchViewModel bookSearch)
        {
            var response = new RetVal<BookViewModel>();
            var repo = _uow.GetRepo<Book>();

            response.IsSuccess = true;
            response.results = repo.GetByIQuerable(b => b.Title.Contains(bookSearch.title)
            || b.User.AuthorPseudonym.Contains(bookSearch.author)
            || b.BookDescription.Contains(bookSearch.description)).ToBookModelList();

            return response;
        }

        public RetVal<string> PublishBook(int id, IPrincipal user)
        {
            var response = new RetVal<string>();
            var repo = _uow.GetRepo<Book>();

            int userId = 0;
            int.TryParse(_authToken.GetClaimValue(user, "userid"), out userId);

            var book = repo.GetByIQuerable(b => b.BookId == id).FirstOrDefault();
            if (book != null)
            {
                if (book.Author == userId)
                {
                    book.IsPublished = true;
                    book.DatePublished = DateTime.Now;
                    repo.Update(book);
                    repo.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Book published successfully";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Book publish error";
                    response.errors.Add("Unauthorized: Users are only allowed to publish their own book(s)");
                    return response;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Book publish error";
                response.errors.Add("Book not found");
                return response;
            }

            return response;
        }

        public RetVal<string> UnPublishBook(int id, IPrincipal user)
        {
            var response = new RetVal<string>();

            bool isAllowedToDelete = _permission.CheckDeletePermission(user, id);
            if (isAllowedToDelete)
            {
                int userId = 0;
                int.TryParse(_authToken.GetClaimValue(user, "userid"), out userId);

                var repo = _uow.GetRepo<Book>();
                var book = repo.GetByIQuerable(b => b.BookId == id && b.Author == userId).FirstOrDefault();
                if (book != null)
                {
                    //book.IsPublished = false;
                    //book.DatePublished = DateTime.MinValue;

                    repo.Delete(book);
                    repo.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Book unpublished successfully";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Book unpublish error";
                    response.errors.Add("Book not found");
                }


            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Book unpublish error";
                response.errors.Add("User not authorized to access resource");
                return response;
            }

            return response;
        }
    }
}
