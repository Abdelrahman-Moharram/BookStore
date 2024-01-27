using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IBaseRepository<Author> _authorRepository;

        public AuthorsController(IBaseRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpPost("add")]
        [Authorize(Policy = "permissions.Create.Author")]
        public async Task<IActionResult> Add([FromBody] Author author)
        {
            if (ModelState.IsValid)
            {
                _authorRepository.AddAsync(author);
                _authorRepository.Save();
                return Ok(author);
            }
            return BadRequest(author);
        }
    }
}
