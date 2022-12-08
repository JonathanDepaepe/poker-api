using DataAccessLayer.AuthModels;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HpcmContext _db;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;


        public AuthRepository(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            HpcmContext db,
            IUserRepository userRepository) {
            _userManager = userManager;
            _userRepository = userRepository;
            _configuration = configuration;

        }

        public async Task<Response> Login(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var userId = await _userManager.GetUserIdAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);
                    var refreshToken = GenerateRefreshToken();
                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                    await _userManager.UpdateAsync(user);

                    return new Response()
                    {
                        Status = "Completed",
                        Message = "Login succesfull",
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo,
                        RefreshToken= refreshToken,
                        RefreshExpiration= DateTime.Now.AddDays(refreshTokenValidityInDays),
                        MemberId = userId
                    };
                }
                return new Response()
                {
                    Status = "Failed",
                    Message= "Failed to login!"
                };
            }
            catch (Exception e)
            {

                throw new Exception("Unable to handle login in repo| " + e);
            }
            
        }

        public async Task<Response> Register(RegisterModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);
                var userWithEmailExists = await _userManager.FindByEmailAsync(model.Email);
                //check if user exists
                if (userExists != null && userWithEmailExists != null)
                    return new Response()
                    {
                        Status = "Unauthorized",
                        Message = "User with this username or email already exists"
                    };

                ApplicationUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                //create the new user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    try //create a new member using the user info
                    {
                        var createdUser = await _userManager.FindByNameAsync(model.Username);
                        await _userRepository.CreateMember(createdUser);
                    }
                    catch (Exception)
                    {
                        return new Response()
                        {
                            Status = "Failed",
                            Message = "Unable to create member from new user"
                        };
                    }

                    LoginModel temp = new() {
                        Username= model.Username,
                        Password= model.Password
                    };

                    var loginResult = Login(temp);

                    return new Response()
                    {
                        Status = "Completed",
                        Message = "New User Created Succesfully",
                        Token = loginResult.Result.Token,
                        Expiration = loginResult.Result.Expiration,
                        RefreshToken = loginResult.Result.RefreshToken,
                        RefreshExpiration = loginResult.Result.RefreshExpiration,
                        MemberId = loginResult.Result.MemberId
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = "Failed",
                        Message = "Failed to create new user |" + result.Errors.ToString()
                    };
                }

            }
            catch (Exception)
            {
                throw new Exception("Unable to handle register in repo");
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                authClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<Response> UseRefreshToken(TokenModel tokenModel)
        {
            try
            {
                string? accessToken = tokenModel.AccessToken;
                string? refreshToken = tokenModel.RefreshToken;

                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    return new Response()
                    {
                        Status = "Failed",
                        Message = "Incorrect token combination"
                    };
                }
                else
                {
                    string username = principal.Identity.Name;

                    var user = await _userManager.FindByNameAsync(username);

                    if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                    {
                        return new Response()
                        {
                            Status = "Completed",
                            Message = "Invallid token combination"
                        };
                    }

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    var newAccessToken = GetToken(principal.Claims.ToList());
                    var newRefreshToken = GenerateRefreshToken();

                    user.RefreshToken = newRefreshToken;
                    await _userManager.UpdateAsync(user);

                    return new Response()
                    {
                        Status = "Success",
                        Message = "Creating new token succeded",
                        Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                        Expiration = newAccessToken.ValidTo,
                        RefreshToken = refreshToken,
                        RefreshExpiration = DateTime.Now.AddDays(refreshTokenValidityInDays),
                        MemberId = user.Id

                    };
                }

            }
            catch (Exception)
            {

                throw new Exception("Failed during creation of new token");
            }

        }
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}
