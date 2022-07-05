using BookStoreDomain.Models;
using Data;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreDomain
{
    public static class Extensions
    {
        public static List<BookViewModel> ToBookModelList(this IQueryable<Book> items)
        {
            return items.Select(b => new BookViewModel
            {
                BookId = b.BookId,
                CoverImage = b.CoverImage,
                Title = b.Title,
                BookDescription = b.BookDescription,
                DatePublished = b.DatePublished,
                CreatedDate = b.CreatedDate,
                IsPublished = b.IsPublished,
                Price = b.Price,
                Author = new UserViewModel { 
                    UserId = b.User.UserId,
                    AuthorPseudonym = b.User.AuthorPseudonym,
                    UserName = b.User.UserName,
                    CreatedDate = b.User.CreatedDate
                }

            }).ToList();
        }

        public static BookViewModel ToBookModel(this Book b)
        {
            return new BookViewModel
            {
                BookId = b.BookId,
                CoverImage = b.CoverImage,
                Title = b.Title,
                BookDescription = b.BookDescription,
                DatePublished = b.DatePublished,
                CreatedDate = b.CreatedDate,
                IsPublished = b.IsPublished,
                Price = b.Price,
                Author = new UserViewModel
                {
                    UserId = b.User.UserId,
                    AuthorPseudonym = b.User.AuthorPseudonym,
                    UserName = b.User.UserName
                }

            };
        }

    }
}
