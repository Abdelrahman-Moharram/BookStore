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
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IBaseRepository<Category> categoryRepository, ILogger<CategoriesController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
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
            _logger.LogWarning("The category data entered is invalid.");
            return BadRequest(category);
        }



    }
}
