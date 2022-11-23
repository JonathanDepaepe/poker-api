using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController:ControllerBase
{
    
    
        public IConfiguration _configuration;
        private readonly HpcmContext _context;

        public AuthController(IConfiguration configuration, HpcmContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [EnableCors("DefaultPolicy")]
        [HttpPost(Name ="Register")]
        public async Task<IActionResult> Post(Member checkMember)
        {
            if (checkMember != null && checkMember.Email != null && checkMember.HashedPassword != null)
            {
                var user = await GetUser(checkMember.Email, checkMember.HashedPassword);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("MemberId", user.MemberId.ToString()),
                        new Claim("Name", user.Name),
                        new Claim("UserName", user.Nickname),
                        new Claim("Email", user.Email),
                        new Claim("ProfilePictureUrl", user.ProfilePictureUrl)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<Member> GetUser(string email, string password)
        {
            return await _context.Members.FirstOrDefaultAsync(u => u.Email == email && u.HashedPassword == password);
        }
    
}