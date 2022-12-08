using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccessLayer.AuthModels;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using NuGet.Protocol;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController:ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserRepository userRepository, IAuthRepository authRepository)
    {
        _userManager = userManager;
        _configuration = configuration;
        _userRepository = userRepository;
        _authRepository = authRepository;
        
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            Response res = await _authRepository.Login(model);

            if (res.Status == "Completed")
            {
                return Ok(res);
            }
            else
            {
                return Unauthorized();
            }
        }
        catch (Exception e)
        {

            throw new Exception("Failed to login| " + e);
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            Response res = await _authRepository.Register(model);

            if (res.Status == "Completed")
            {
                return Ok(res);
            }
            else if (res.Status == "Unauthorized")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User with this username already exists!");
            }
            else if(res.Status=="Failed")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, res.Message);
            }
            else
            {
                return BadRequest(res);
            }
        }
        catch (Exception e)
        {

            throw new Exception("Failed to register| " + e);
        }
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        try
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            Response res = await _authRepository.UseRefreshToken(tokenModel);

            if (res.Status=="Completed")
            {
                return Ok(res);
            }
            else
            {
                return Unauthorized(res);
            }
        
        }


        catch (Exception e)
        {

            throw new Exception("Unable to create new access tokens| " + e);
        }
    }



}


