using BookStore.DTOs.Book;
using BookStore.Filters;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public IBaseRepository<Book> _bookRepository { get; }

        public BooksController(IBaseRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }


        [HttpGet]
        [Authorize(Policy = "permissions.Read.Book")]
        public IActionResult All()
        {
            return Ok();
        }

        [HttpPost("add")]
        [Authorize(Policy = "permissions.Create.Book")]
        public IActionResult Add([FromBody] AddBookDTO bookDTO)
        {

            if (ModelState.IsValid)
            {
                var userId = User.Claims.FirstOrDefault(i => i.Type == "userId")?.Value;
                if (userId == null)
                    return BadRequest("Invalid Credintials");


                var book = new Book
                {
                    AuthorId = bookDTO.AuthorId,
                    CategoryId = bookDTO.CategoryId,
                    Name = bookDTO.Name,
                    PublishDate = bookDTO.PublishDate,
                    PublisherId = userId,
                };
                

                _bookRepository.AddAsync(book);
                _bookRepository.Save();
                return Ok(book);
            }
            return BadRequest(bookDTO);
        }

        [HttpPost("update/{bookId}")]
        [Authorize(Policy = "permissions.Update.Book")]
        [BookPublisher]
        public IActionResult Update([FromBody] AddBookDTO bookDTO, string bookId)
        {

            return Ok();
        }

    }
}
