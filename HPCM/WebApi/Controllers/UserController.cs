using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using System.ComponentModel;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    [EnableCors("DefaultPolicy")]
    [HttpGet("{MemberId}", Name = "GetMemberById"), Authorize]
    public async Task<ActionResult<IEnumerable<Member>>> GetMemberById(string MemberId)
    {
        return (_userRepository.GetMemberById(MemberId) is IQueryable<Member> memberById)
            ? Ok(await memberById
                .ToListAsync())
            : NotFound($"No Member found with id {MemberId}");
    }

    [EnableCors("DefaultPolicy")]
    [HttpPut("update", Name = "UpdateMember"), Authorize]
    public async Task<ActionResult<Member>> UpdateMember(MemberDTO memberInfo)
    {
        return (await _userRepository.AlterMember(memberInfo) is Member alteredMember)
        ? Accepted(alteredMember)
        : BadRequest();
    }
}