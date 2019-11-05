using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclesProjectApi.Data;
using VehiclesProjectApi.Models;

namespace VehiclesProjectApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private VehiclesProjectDbContext _dbContext;
        public VehiclesController(VehiclesProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET : api/Vehicles/HotAndNewAds
        [HttpGet("[action]")]
        public IQueryable<object> HotAndNewAds()
        {
            var vehicles = from v in _dbContext.Vehicles
                           where v.IsHotAndNew == true
                           select new
                           {
                               Id = v.Id,
                               Title = v.Title,
                               Price = v.Price,
                               Model = v.Model,
                               Company = v.Company,
                               IsFeatured = v.IsFeatured,
                               ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                           };
            return vehicles;
        }


        // GET : api/Vehicles/SearchVehicles?search=BMW
        [HttpGet("[action]")]
        public IQueryable<object> SearchVehicles(string search)
        {
            var vehicles = from v in _dbContext.Vehicles
                           where v.Title.StartsWith(search) || v.Company.StartsWith(search)
                           select new
                           {
                               Id = v.Id,
                               Title = v.Title,
                               Model = v.Model,
                               Company = v.Company,
                           };

            return vehicles.Take(15);
        }



        // GET : api/Vehicles?categoryId=1
        [HttpGet]
        public IQueryable<object> GetVehicles(int categoryId)
        {
            var vehicles = from v in _dbContext.Vehicles
                           where v.CategoryId == categoryId
                           select new
                           {

                               Id = v.Id,
                               Title = v.Title,
                               Price = v.Price,
                               Model = v.Model,
                               Location = v.Location,
                               Company = v.Company,
                               DatePosted = v.DatePosted,
                               IsFeatured = v.IsFeatured,
                               ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                           };
            return vehicles;
        }

        // GET : api/Vehicles/VehicleDetails?id=1
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
                               Id = v.Id,
                               Title = v.Title,
                               Description = v.Description,
                               Price = v.Price,
                               Model = v.Model,
                               Engine = v.Engine,
                               Color = v.Color,
                               Company = v.Company,
                               DatePosted = v.DatePosted,
                               Condition = v.Condition,
                               IsHotAndNew = v.IsHotAndNew,
                               IsFeatured = v.IsFeatured,
                               Location = v.Location,
                               Images = v.Images,
                               Email = u.Email,
                               Phone = u.Phone,
                               ImageUrl = u.ImageUrl,
                           }).FirstOrDefault();

            return Ok(vehicle);
        }


        // GET : api/Vehicles/FilterVehicles?categoryId=1&condition=New&sort=asc&price=20
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
                                   Id = v.Id,
                                   Title = v.Title,
                                   Price = v.Price,
                                   Model = v.Model,
                                   Location = v.Location,
                                   Company = v.Company,
                                   DatePosted = v.DatePosted,
                                   IsFeatured = v.IsFeatured,
                                   ImageUrl = v.Images.FirstOrDefault().ImageUrl,

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
                                   Id = v.Id,
                                   Title = v.Title,
                                   Price = v.Price,
                                   Model = v.Model,
                                   Location = v.Location,
                                   Company = v.Company,
                                   DatePosted = v.DatePosted,
                                   IsFeatured = v.IsFeatured,
                                   ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                               };
                    break;

                default:
                    vehicles = from v in _dbContext.Vehicles
                               where v.CategoryId == categoryId
                               select new
                               {
                                   Id = v.Id,
                                   Title = v.Title,
                                   Price = v.Price,
                                   Model = v.Model,
                                   Location = v.Location,
                                   Company = v.Company,
                                   DatePosted = v.DatePosted,
                                   IsFeatured = v.IsFeatured,
                                   ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                               };
                    break;
            }

            return vehicles;
        }

        // POST: api/Vehicles
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
                               Id = v.Id,
                               Title = v.Title,
                               Price = v.Price,
                               Model = v.Model,
                               Location = v.Location,
                               Company = v.Company,
                               DatePosted = v.DatePosted,
                               IsFeatured = v.IsFeatured,
                               ImageUrl = v.Images.FirstOrDefault().ImageUrl,
                           };
            return Ok(vehicles);
        }




        // PUT: api/Vehicles/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
