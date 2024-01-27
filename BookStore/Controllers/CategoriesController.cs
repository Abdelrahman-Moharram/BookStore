using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IBaseRepository<Category> _categoryRepository;

        public CategoriesController(IBaseRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        [HttpPost("add")]
        [Authorize(Policy = "permissions.Create.Category")]
        public async Task<IActionResult> Add([FromBody] Category category) 
        { 
            if(ModelState.IsValid)
            {
                _categoryRepository.AddAsync(category);
                _categoryRepository.Save();
                return Ok(category);
            }
            return BadRequest(category);
        }



    }
}
