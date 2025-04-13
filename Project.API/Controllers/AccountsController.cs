using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.API.DTOs;
using Project.API.Helpers;
using Project.API.Services;
using Project.Core.Entities;
using Project.Repositor.Data;

namespace Project.API.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly EmergencyContext _context;
        private readonly ITokenServices _tokenServices;

        public AccountsController(EmergencyContext context, ITokenServices tokenServices)
        {
            _context = context;
            _tokenServices = tokenServices;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO model)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }

            var user = new User()
            {
                USerName = model.DisplayName,
                Email = model.Email,
                PasswordHash = PasswordHelper.HashPassword(model.Password),
                FullName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            var userDTO = new UserDTO
            {
                DisplayName = user.USerName,
                Email = user.Email,
                Token = await _tokenServices.CreateToken(user)
            };

            return Ok(userDTO);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find the user by email
            var user = await _context.users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            // Return the user as UserDTO
            var userDto = new UserDTO()
            {
                DisplayName = user.USerName,
                Email = user.Email,
                Token = await _tokenServices.CreateToken(user)
            };

            return Ok(userDto);
        }
    }
}
