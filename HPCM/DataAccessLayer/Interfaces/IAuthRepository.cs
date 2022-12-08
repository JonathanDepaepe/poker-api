using DataAccessLayer.AuthModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IAuthRepository
    {

        Task<Response> Login(LoginModel model);
        Task<Response> Register(RegisterModel model);
        Task<Response> UseRefreshToken(TokenModel model);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }
}
