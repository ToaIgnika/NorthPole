using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SantaAPI.Data;
using SantaAPI.Models;
using SantaAPI.ViewModels;

namespace SantaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string DEFAULT_ROLE = "Child";

        public AuthController(ApplicationDbContext Context, UserManager<ApplicationUser> UserManager, IConfiguration configuration)
        {
            _context = Context;
            _userManager = UserManager;
            _configuration = configuration;
        }

        [EnableCors("AllAccessCors")]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterChildAsync([FromBody] RegisterViewModel model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = DateTime.Parse(model.BirthDate),
                    Street = model.Street,
                    City = model.City,
                    Province = model.Province,
                    PostalCode = model.PostalCode,
                    Country = model.Country,
                    Latitude = model.Latitude,
                    Longitude = model.Longitute,
                    isNaughty = false
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddPasswordAsync(user, model.Password);
                    await _userManager.AddToRoleAsync(user, DEFAULT_ROLE);
                    return Ok(new { result = "User successfully Created" });
                }
                else
                {
                    return Ok(new { result = result.Errors.ToString() });
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = e.Message });
            }
        }


        [EnableCors("AllAccessCors")]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claim = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claim, "Token");
                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in userRoles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                  issuer: _configuration["Jwt:Site"],
                  audience: _configuration["Jwt:Site"],
                  claims: claimsIdentity.Claims,
                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(
                  new
                  {
                      token = new JwtSecurityTokenHandler().WriteToken(token),
                      expiration = token.ValidTo,
                      id = user.Id
                  });
            }
            return Unauthorized();
        }
    }
}