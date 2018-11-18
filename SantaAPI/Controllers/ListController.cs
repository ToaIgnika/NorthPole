using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SantaAPI.Data;
using SantaAPI.Models;
using SantaAPI.ViewModels;
using System.Globalization;


namespace SantaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        private Child makeCopy(ApplicationUser childU)
        {
            Child child = new Child();
            child.id = childU.Id;
            child.UserName = childU.UserName;
            child.FirstName = childU.FirstName;
            child.LastName = childU.LastName;
            child.BirthDate = childU.BirthDate;
            child.Street = childU.Street;
            child.City = childU.Street;
            child.Province = childU.Province;
            child.PostalCode = childU.PostalCode;
            child.Country = childU.Country;
            child.Latitude = childU.Latitude;
            child.Longitude = childU.Longitude;
            child.isNaughty = childU.isNaughty;
            return child;
        }

        public ListController(ApplicationDbContext Context, UserManager<ApplicationUser> UserManager, IConfiguration configuration)
        {
            _context = Context;
            _userManager = UserManager;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        [EnableCors("AllAccessCors")]
        [HttpGet("claims")]
        public object CheckClaims()
        {
            return User.Claims.Select(c =>
              new
              {
                  Type = c.Type,
                  Value = c.Value
              });
        }

        // GET api/list
        [Authorize(Roles = "Admin")]
        [EnableCors("AllAccessCors")]
        [HttpGet]
        public async Task<ActionResult<String>> GetAsync()
        {
            var listU = await _userManager.GetUsersInRoleAsync("Child");
            IList list = new List<Child>();
            foreach (ApplicationUser c in listU)
            {
                list.Add(makeCopy(c));
            }
            return JsonConvert.SerializeObject(list);
        }

        // GET api/list/id
        [Authorize(Roles = "Admin")]
        [EnableCors("AllAccessCors")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Child>> GetAsync(string id)
        {
            var childU = await _userManager.FindByIdAsync(id);
            if (childU == null)
            {
                return Ok(new { result = "Child not found" });
            }
            var childRole = await _userManager.IsInRoleAsync(childU, "Admin");
            if (childRole)
            {
                return Ok(new { result = "Permission denied" });
            }
            Child child = makeCopy(childU);
            return child;
        }

        // GET api/list/id
        [Authorize(Roles = "Child")]
        [EnableCors("AllAccessCors")]
        [HttpGet("c/{id}")]
        public async Task<ActionResult<Child>> GetCAsync(string id)
        {
            var childU = await _userManager.FindByIdAsync(id);
            if (childU == null)
            {
                return Ok(new { result = "Child not found" });
            }
            if (id == childU.Id)
            {
                return makeCopy(childU);
            }
            return Ok(new { result = "Child not found" });

        }

        // POST api/values
        [Authorize(Roles = "Admin")]
        [EnableCors("AllAccessCors")]
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/values/5
        [Authorize(Roles = "Admin")]
        [EnableCors("AllAccessCors")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, [FromBody] EditViewModel model)
        {
            var child = await _userManager.FindByIdAsync(id);
            if (child == null)
            {
                return Ok(new { result = "Child not found" });
            }
            var childRole = await _userManager.IsInRoleAsync(child, "Admin");
            if (childRole)
            {
                return Ok(new { result = "Permission denied" });
            }

            child.Street = model.Street;
            child.City = model.City;
            child.Province = model.Province;
            child.PostalCode = model.PostalCode;
            child.Country = model.Country;
            child.Latitude = model.Latitude;
            child.Longitude = model.Longitude;
            child.isNaughty = model.isNaughty;
            _context.ApplicationUser.Update(child);
            _context.SaveChanges();

            return Ok(new { result = "Child information updated" });
        }

        // PUT api/values/c/5
        [Authorize(Roles = "Child")]
        [EnableCors("AllAccessCors")]
        [HttpPut("c/{id}")]
        public async Task<ActionResult> PutCAsync(string id, [FromBody] EditCViewModel model)
        {
            var child = await _userManager.FindByIdAsync(id);
            if (child == null)
            {
                return Ok(new { result = "Child not found" });
            }
            var childRole = await _userManager.IsInRoleAsync(child, "Admin");
            if (childRole)
            {
                return Ok(new { result = "Permission denied" });
            }

            child.FirstName = model.FirstName;
            child.LastName = model.LastName;
            Console.WriteLine("THIS: " + model.BirthDate);
            child.BirthDate = DateTime.ParseExact(model.BirthDate, "yyyy-MM-dd",
                                        null);
            Console.WriteLine("THIS: " + child.BirthDate);


            child.Street = model.Street;
            child.City = model.City;
            child.Province = model.Province;
            child.PostalCode = model.PostalCode;
            child.Country = model.Country;
            child.Latitude = model.Latitude;
            child.Longitude = model.Longitude;
            _context.ApplicationUser.Update(child);
            _context.SaveChanges();

            return Ok(new { result = "Child information updated" });
        }

        // DELETE api/values/5
        [Authorize(Roles = "Admin")]
        [EnableCors("AllAccessCors")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var child = await _userManager.FindByIdAsync(id);
            if (child == null)
            {
                return Ok(new { result = "Child not found" });
            }
            var childRole = await _userManager.IsInRoleAsync(child, "Admin");
            if (childRole)
            {
                return Ok(new { result = "Permission denied" });
            }
            child.Street = "";
            child.City = "";
            child.Province = "";
            child.PostalCode = "";
            child.Country = "";
            child.Latitude = 666;
            child.Longitude = 666;
            _context.ApplicationUser.Update(child);
            _context.SaveChanges();
            
            return Ok(new { result = "Child deleted" });
        }
    }
}