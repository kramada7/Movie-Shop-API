using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Services;
using MovieShop.Entities;
using MovieShop.API.DTO;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _config = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserDTO createUserDTO)
        {
            if (createUserDTO == null || string.IsNullOrEmpty(createUserDTO.Email) || string.IsNullOrEmpty(createUserDTO.Password))
            {
                return BadRequest();
            }

            var user = await _userService.CreateUserAsync(createUserDTO.Email, createUserDTO.Password, createUserDTO.FirstName, createUserDTO.LastName);

            if (user == null)
            {
                return BadRequest("Email already existed");
            }
            //need to change to the newly created method
            return Ok("User created");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> ValidateUserAsync([FromBody]ValidateUserDTO validateUserDTO)
        {
            if (validateUserDTO == null || string.IsNullOrEmpty(validateUserDTO.Email) || string.IsNullOrEmpty(validateUserDTO.Password))
            {
                return BadRequest();
            }

            var user = await _userService.ValidateUserAsync(validateUserDTO.Email, validateUserDTO.Password);

            if (user == null)
            {
                return Unauthorized("Email or password is wrong");
            }
            //need to change to the newly created method
            return Ok(new
            {
                // creating the instance of the token
                token = GenerateToken(user)
            });
        }


        [HttpGet]
        [Route("{id}/purchases")]
        [Authorize]
        public async Task<IActionResult> GetUserPurchasedMovies(int id)
        {
            var userMovies = await _userService.GetPurchases(id);
            return Ok(userMovies);
        }

        private string GenerateToken(User user)
        {
            //claims info in payload
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim("alias", user.FirstName[0] + user.LastName[0].ToString()),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenSettings:PrivateKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var expires = DateTime.UtcNow.AddSeconds(Convert.ToDouble(_config["TokenSettings:ExpirationDays"]));

            // generate the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = credentials,
                Issuer = _config["TokenSettings:Issuer"], //movie website name
                Audience = _config["TokenSettings:Audience"]
            };


            var encodedJwt = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(encodedJwt);
        }
    }
}