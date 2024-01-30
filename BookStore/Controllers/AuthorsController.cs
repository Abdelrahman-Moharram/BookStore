using AutoMapper;
using BookStore.DTOs.Author;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IBaseRepository<Author> _authorRepository;

        public IMapper _mapper { get; }

        public AuthorsController(IBaseRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        [HttpPost("add")]
        [Authorize(Policy = "permissions.Create.Author")]
        public async Task<IActionResult> Add([FromBody] AuthorDTO authorDTO)
        {
            if (ModelState.IsValid)
            {
                var author = _mapper.Map<AuthorDTO, Author>(authorDTO);
                authorDTO = _mapper.Map<AuthorDTO>(await _authorRepository.AddAsync(author));
                await _authorRepository.SaveAsync();
                return Ok(authorDTO);
            }
            return BadRequest(authorDTO);
        }


        [HttpGet("search")]
        [Authorize(Policy = "permissions.Read.Author")]
        public async Task<IActionResult> Search([FromQuery] AuthorDTO AuthorDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorRepository.FindAllAsync(
                    i =>
                        i.FullName.Contains(AuthorDTO.FullName) 
                    );
                return Ok(result);
            }
            return BadRequest(AuthorDTO);
        }


        [HttpPut("Edit/{AuthorId}")]
        [Authorize(Policy = "permissions.Update.Author")]
        public async Task<IActionResult> Update([FromBody] AuthorDTO authorDTO, [FromRoute] string authorId)
        {
            if (authorId == authorDTO.Id && ModelState.IsValid)
            {
                authorDTO = _mapper.Map<AuthorDTO>(await _authorRepository.UpdateAsync(_mapper.Map<AuthorDTO, Author>(authorDTO), authorId));
                if (authorDTO != null)
                {
                    await _authorRepository.SaveAsync();
                    return Ok(authorDTO);
                }
                return NotFound("Can't Found Author with this Id");
            }
            return BadRequest(authorDTO);
        }

        [HttpDelete("Delete/{AuthorId}")]
        [Authorize(Policy = "permissions.Delete.Author")]
        public async Task<IActionResult> Delete([FromRoute] string authorId)
        {
            if (authorId != null)
            {
                var Author = await _authorRepository.GetByIdAsync(authorId);
                if (Author != null)
                {
                    var authorDto = _mapper.Map<Author, AuthorDTO > (await _authorRepository.DeleteAsync(Author));
                    await _authorRepository.SaveAsync();
                    return Ok(authorDto);
                }
                return NotFound();
            }
            return BadRequest(authorId);
        }


    }
}
