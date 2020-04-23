using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VehiclesProjectApi.Data;
using VehiclesProjectApi.Models;

namespace VehiclesProjectApi.Controllers
{
    /// <summary>
    /// Vehicle Ads.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly VehiclesProjectDbContext _dbContext;

        /// <summary>
        /// Vehicles CTOR.
        /// </summary>
        /// <param name="dbContext"></param>
        public VehiclesController(VehiclesProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// GET : api/Vehicles/HotAndNewAds
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IQueryable<object> HotAndNewAds()
        {
            var vehicles = from v in _dbContext.Vehicles
                           where v.IsHotAndNew
                           select new
                           {
                               v.Id,
                               v.Title,
                               v.Price,
                               v.Model,
                               v.Company,
                               v.IsFeatured,
                               v.Images.FirstOrDefault().ImageUrl,
                           };
            return vehicles;
        }

        /// <summary>
        /// GET : api/Vehicles/SearchVehicles?search=BMW
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IQueryable<object> SearchVehicles(string search)
        {
            var vehicles = from v in _dbContext.Vehicles
                           where v.Title.StartsWith(search) || v.Company.StartsWith(search)
                           select new
                           {
                               v.Id,
                               v.Title,
                               v.Model,
                               v.Company,
                           };

            return vehicles.Take(15);
        }

        /// <summary>
        /// GET : api/Vehicles?categoryId=1
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<object> GetVehicles(int categoryId)
        {
            var vehicles = from v in _dbContext.Vehicles
                           where v.CategoryId == categoryId
                           select new
                           {
                               v.Id,
                               v.Title,
                               v.Price,
                               v.Model,
                               v.Location,
                               v.Company,
                               v.DatePosted,
                               v.IsFeatured,
                               v.Images.FirstOrDefault().ImageUrl,
                           };
            return vehicles;
        }

        /// <summary>
        /// GET : api/Vehicles/VehicleDetails?id=1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> VehicleDetails(int id)
        {
            var foundVehicle = await _dbContext.Vehicles.FindAsync(id);
            if (foundVehicle == null)
            {
                return NoContent();
            }

            var vehicle = (from v in _dbContext.Vehicles
                           join u in _dbContext.Users on v.UserId equals u.Id
                           where v.Id == id
                           select new
                           {
                               v.Id,
                               v.Title,
                               v.Description,
                               v.Price,
                               v.Model,
                               v.Engine,
                               v.Color,
                               v.Company,
                               v.DatePosted,
                               v.Condition,
                               v.IsHotAndNew,
                               v.IsFeatured,
                               v.Location,
                               v.Images,
                               u.Email,
                               u.Phone,
                               u.ImageUrl,
                           }).FirstOrDefault();

            return Ok(vehicle);
        }

        /// <summary>
        /// GET : api/Vehicles/FilterVehicles?categoryId=1&amp;condition=New&amp;sort=asc&amp;price=20
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="condition"></param>
        /// <param name="sort"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IQueryable<object> FilterVehicles(int categoryId, string condition, string sort, double price)
        {
            IQueryable<object> vehicles;

            switch (sort)
            {
                case "desc":
                    vehicles = from v in _dbContext.Vehicles
                               join u in _dbContext.Users on v.UserId equals u.Id
                               join c in _dbContext.Categories on v.CategoryId equals c.Id
                               where v.Price >= price && c.Id == categoryId && v.Condition == condition
                               orderby v.Price descending
                               select new
                               {
                                   v.Id,
                                   v.Title,
                                   v.Price,
                                   v.Model,
                                   v.Location,
                                   v.Company,
                                   v.DatePosted,
                                   v.IsFeatured,
                                   v.Images.FirstOrDefault().ImageUrl
                               };
                    break;

                case "asc":
                    vehicles = from v in _dbContext.Vehicles
                               join u in _dbContext.Users on v.UserId equals u.Id
                               join c in _dbContext.Categories on v.CategoryId equals c.Id
                               where v.Price >= price && c.Id == categoryId && v.Condition == condition
                               orderby v.Price ascending
                               select new
                               {
                                   v.Id,
                                   v.Title,
                                   v.Price,
                                   v.Model,
                                   v.Location,
                                   v.Company,
                                   v.DatePosted,
                                   v.IsFeatured,
                                   v.Images.FirstOrDefault().ImageUrl
                               };
                    break;

                default:
                    vehicles = from v in _dbContext.Vehicles
                               where v.CategoryId == categoryId
                               select new
                               {
                                   v.Id,
                                   v.Title,
                                   v.Price,
                                   v.Model,
                                   v.Location,
                                   v.Company,
                                   v.DatePosted,
                                   v.IsFeatured,
                                   v.Images.FirstOrDefault().ImageUrl
                               };
                    break;
            }

            return vehicles;
        }

        /// <summary>
        /// POST: api/Vehicles
        /// </summary>
        /// <param name="vehicleModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] Vehicle vehicleModel)
        {
            try
            {
                var vehicle = new Vehicle()
                {
                    Title = vehicleModel.Title,
                    Description = vehicleModel.Description,
                    CategoryId = vehicleModel.CategoryId,
                    Color = vehicleModel.Color,
                    Company = vehicleModel.Company,
                    Condition = vehicleModel.Condition,
                    DatePosted = vehicleModel.DatePosted,
                    Engine = vehicleModel.Engine,
                    Price = vehicleModel.Price,
                    Model = vehicleModel.Model,
                    Location = vehicleModel.Location,
                    IsHotAndNew = false,
                    IsFeatured = false,
                    UserId = vehicleModel.UserId
                };
                _dbContext.Vehicles.Add(vehicle);
                _dbContext.SaveChanges();

                return Ok(new { status = true, message = "Vehicle Added Successfully", vehicleId = vehicle.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// User's Ads.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult MyAds()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return NotFound();

            var vehicles = from v in _dbContext.Vehicles
                           where v.UserId == user.Id
                           orderby v.DatePosted descending
                           select new
                           {
                               v.Id,
                               v.Title,
                               v.Price,
                               v.Model,
                               v.Location,
                               v.Company,
                               v.DatePosted,
                               v.IsFeatured,
                               v.Images.FirstOrDefault().ImageUrl
                           };
            return Ok(vehicles);
        }

        /// <summary>
        /// PUT: api/Vehicles/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            // Intentionally left empty.
        }

        /// <summary>
        /// DELETE: api/ApiWithActions/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // Intentionally left empty.
        }
    }
}