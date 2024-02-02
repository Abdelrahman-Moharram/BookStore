using AutoMapper;
using BookStore.DTOs.Book;
using BookStore.Filters;
using BookStore.Helpers;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<BookReader> _readBookRepository;
        private readonly IBaseRepository<UploadedFile> _bookExtends;

        public BooksController(IBaseRepository<Book> bookRepository, IMapper mapper, 
            IBaseRepository<BookReader> readBookRepository, IBaseRepository<UploadedFile> bookExtends)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _readBookRepository = readBookRepository;
            _bookExtends = bookExtends;
        }


        [HttpGet]
        [Authorize(Policy = "permissions.Read.Book")]
        public async Task<IActionResult> Paginate([FromQuery] int start = 0, [FromQuery] int end = 10)
        {
            return  Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<BookDTO>>(await _bookRepository.PaginateAsync(start, end)));
        }


        [HttpGet("{bookId}")]
        [Authorize(Policy = "permissions.Read.Book")]
        public async Task<IActionResult> ReadBook([FromRoute] string bookId)
        {
            var userId = User.Claims.FirstOrDefault(i => i.Type == "userId")?.Value;
            if (!string.IsNullOrWhiteSpace(bookId) && userId != null)
            {
                var book = _bookRepository.GetByIdAsync(bookId).Result;
                if (book == null) return NotFound();

                if(_readBookRepository.FindAsync(i=>i.userId == userId && i.BookId==bookId).Result == null)
                {
                    await _readBookRepository.AddAsync(new BookReader
                    {
                        BookId = bookId,
                        userId = userId,
                    });
                    await _readBookRepository.SaveAsync();
                }

                return Ok(_mapper.Map<BookDTO>(book));
            }
            return BadRequest();
        }


        [HttpGet("{bookId}/download")]
        public async Task<IActionResult> DownloadFile([FromRoute] string bookId)
        {
            var book = await _bookRepository.FindAsync(f => f.Id == bookId);
            var uploadedFile = await _bookExtends.FindAsync(f => f.bookId == bookId);
            if (book is null || book.File is null)
                return NotFound();

            var path = Path.Combine("wwwroot", "books", uploadedFile.FileName);

            MemoryStream memoryStream = new();
            using FileStream fileStream = new(path, FileMode.Open);
            fileStream.CopyTo(memoryStream);

            memoryStream.Position = 0;

            return File(memoryStream, uploadedFile.ContentType, uploadedFile.FileName);
        }
        [HttpPost("{bookId}/rate")]
        [Authorize(Policy = "permissions.Read.Book")]
        public async Task<IActionResult> RateBook([FromRoute] string bookId, [FromBody] RateBookDTO rateDTO)
        {
            var userId = User.Claims.FirstOrDefault(i => i.Type == "userId")?.Value;
            if (!string.IsNullOrWhiteSpace(bookId) && bookId == rateDTO.bookId && userId != null)
            {
                var book = _bookRepository.GetByIdAsync(bookId).Result;
                if (book == null) return NotFound();

                var readBook = _readBookRepository.FindAsync(i => i.userId == userId && i.BookId == bookId).Result;
                if (readBook == null)
                {
                    readBook = await _readBookRepository.AddAsync(new BookReader
                    {
                        BookId = bookId,
                        userId = userId,
                    });
                }
                readBook.RateBook(rateDTO.rate);
                readBook.UpdateBookRate(_readBookRepository.GetAllAsync().Result.Select(i => i.Rate).ToArray());
                await _readBookRepository.SaveAsync();

                return Ok();
            }
            return BadRequest();
        }


        [HttpPost("add")]
        [Authorize(Policy = "permissions.Create.Book")]
        public async Task<IActionResult> Add([FromForm] AddBookDTO bookDTO, List<IFormFile> File)
        {
            if (ModelState.IsValid)
            {
                FileUpload fileUpload = new();


                var book = _mapper.Map<Book>(bookDTO);
                book.PublisherId = User.Claims.FirstOrDefault(i => i.Type == "userId")?.Value;
                book = await _bookRepository.AddAsync(book);
                var uploadedFiles =  fileUpload.UploadBook(File, book.Id);
                var result =  await _bookExtends.AddRangeAsync(uploadedFiles);
                book.FileId = result.ToList().First().Id;  
                await _bookRepository.SaveAsync();
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
