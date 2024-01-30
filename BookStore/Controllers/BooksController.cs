using AutoMapper;
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
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IMapper _mapper;

        public BooksController(IBaseRepository<Book> bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize(Policy = "permissions.Read.Book")]
        public async Task<IActionResult> Paginate([FromQuery] int start = 0, [FromQuery] int end = 10)
        {
            return  Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<BookDTO>>(await _bookRepository.PaginateAsync(start, end)));
        }

        [HttpPost("add")]
        [Authorize(Policy = "permissions.Create.Book")]
        public async Task<IActionResult> Add([FromBody] AddBookDTO bookDTO)
        {
            if (ModelState.IsValid)
            {
                var book = _mapper.Map<Book>(bookDTO);
                book.PublisherId = User.Claims.FirstOrDefault(i => i.Type == "userId")?.Value;
                book = await _bookRepository.AddAsync(book);
                _bookRepository.Save();
                return Ok(_mapper.Map<Book, BookDTO>(book));
            }
            return BadRequest(bookDTO);
        }

        [HttpGet("search")]
        [Authorize(Policy = "permissions.Read.Book")]
        public async Task<IActionResult> Search([FromQuery] SearchBookDTO searchDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookRepository.FindAllAsync(
                    i =>
                        i.Name.Contains(searchDTO.BookName) ||
                        i.PublishDate == searchDTO.PublishDate ||
                        i.Publisher.UserName.Contains(searchDTO.PublisherName) ||
                        i.Author.FullName.Contains(searchDTO.AuthorName) ||
                        i.Category.Name.Contains(searchDTO.CategoryName)
                    );
                return Ok(result);
            }
            return BadRequest(searchDTO);
        }


        [HttpPut("Edit/{bookId}")]
        [Authorize(Policy = "permissions.Update.Book")]
        [BookPublisherOrAdmin]
        public async Task<IActionResult> Update([FromBody] UpdateBookDTO bookDTO,[FromRoute] string bookId)
        {
            if(bookId == bookDTO.Id && ModelState.IsValid)
            {
                var book = _mapper.Map<UpdateBookDTO, Book>(bookDTO);
                book = await _bookRepository.UpdateAsync(book, bookId);
                if (book != null)
                {
                    await _bookRepository.SaveAsync();
                    return Ok(book);
                }
                return NotFound("Can't Found Book with this Id");
            }
            return BadRequest(bookDTO);
        }

        [HttpDelete("Delete/{bookId}")]
        [Authorize(Policy = "permissions.Delete.Book")]
        [BookPublisherOrAdmin]
        public async Task<IActionResult> Delete([FromRoute]  string bookId)
        {
            if (bookId != null)
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if(book  != null)
                {
                    book = await _bookRepository.DeleteAsync(book);
                    await _bookRepository.SaveAsync();
                    return Ok(book);
                }
                return NotFound();
            }
            return BadRequest(bookId);
        }


        

    }
}
