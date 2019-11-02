using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VehiclesProjectApi.Helpers;
using VehiclesProjectApi.Models;
using VehiclesProjectApi.Data;
using AuthenticationPlugin;

namespace VehiclesProjectApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private VehiclesProjectDbContext _dbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public AccountsController(VehiclesProjectDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User user)
        {
            var userWithSameEmail = _dbContext.Users.Where(u => u.Email == user.Email).SingleOrDefault();
            if (userWithSameEmail != null) return BadRequest("User with this email already exists");
            var userObj = new User
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Password = SecurePasswordHasherHelper.Hash(user.Password)
            };
            _dbContext.Users.Add(userObj);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            var userEmail =  _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userEmail == null) return StatusCode(StatusCodes.Status404NotFound);
            var hashedPassword = userEmail.Password;
            if (!SecurePasswordHasherHelper.Verify(user.Password, hashedPassword)) return Unauthorized();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_Id = userEmail.Id
            });
        }


        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound();
            }

            var hashedPassword = user.Password;
            if (!SecurePasswordHasherHelper.Verify(model.OldPassword, hashedPassword))
                return BadRequest("You can't change the password");
            user.Password = SecurePasswordHasherHelper.Hash(model.NewPassword);
            _dbContext.SaveChanges();
            return Ok("Your password has been changed");
        }
        
        [HttpPost]
        public IActionResult EditPhoneNumber([FromForm]ChangePhoneModel changePhone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return NotFound();
            user.Phone = changePhone.Number;
            _dbContext.SaveChanges();
            return Ok("Phone number has been updated");
        }


        [HttpPost]
        public IActionResult EditUserProfile([FromBody]byte[] ImageArray)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return BadRequest();
            var stream = new MemoryStream(ImageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot/ProfileImages";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FilesHelper.UploadPhoto(stream, folder, file);
            if (!response) return BadRequest();
            user.ImageUrl = imageFullPath;
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult UserProfileImage()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) return NotFound();
            var responseResult = _dbContext.Users
                .Where(x => x.Email == userEmail)
                .Select(x => new
                {
                    x.ImageUrl,
                })
                .SingleOrDefault();
            return Ok(responseResult);
        }



    }
}
