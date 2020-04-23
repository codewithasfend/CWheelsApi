using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehiclesProjectApi.Data;
using VehiclesProjectApi.Models;

namespace VehiclesProjectApi.Controllers
{
    /// <summary>
    /// Car categories.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly VehiclesProjectDbContext _dbContext;

        /// <summary>
        /// Categories CTOR.
        /// </summary>
        /// <param name="dbContext"></param>
        public CategoriesController(VehiclesProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Categories);
        }

        /// <summary>
        /// Post categories
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]Category categoryModel)
        {
            var category = new Category()
            {
                Type = categoryModel.Type,
            };
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}