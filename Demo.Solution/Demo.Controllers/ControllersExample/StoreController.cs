using Demo.Controllers.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers.ControllersExample
{
    public class StoreController : Controller
    {
        [Route("/store/books")]

        public IActionResult Index(int? bookid)
        {
            if (bookid.HasValue == false)
            {
                return BadRequest("Book id is not supplied or empty");
            }

            if (bookid <= 0 || bookid > 2000)
            {
                return BadRequest("Book id must be between 1 and 1999");
            }

            return Ok($"Book id: {bookid}");
        }


        //validation using route params

        [Route("/store/books/{bookid}")]

        public IActionResult Index1([FromRoute] int? bookid)
        {
            if (bookid.HasValue == false)
            {
                return BadRequest("Book id is not supplied or empty");
            }

            if (bookid <= 0 || bookid > 2000)
            {
                return BadRequest("Book id must be between 1 and 1999");
            }

            return Ok($"Book id: {bookid}");
        }

        //form-urlencoded
        //bookId and author

        [Route("/store")]

        public IActionResult Index1(Book book)
        {
            if (book.BookId.HasValue == false)
            {
                return BadRequest("Book id is not supplied or empty");
            }

            if (book.BookId <= 0 || book.BookId > 2000)
            {
                return BadRequest("Book id must be between 1 and 1999");
            }

            return Ok($"Book info: id: {book.BookId}, author: {book.Author}");
        }
    }
}


