using AutoMapper.Execution;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.DTO;
using StackExchange.Redis;
using Member = DataAccessLayer.Models.Member;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClubController : ControllerBase
{
    private readonly IClubRepository _clubRepository;

    public ClubController(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }
    [EnableCors("DefaultPolicy"), Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Club>>> GetAllClubs(){

        return (_clubRepository.GetClubs() is IQueryable<Club> allClubs)
            ? Ok(await allClubs
                .ToListAsync())
            : NotFound("No clubs found");
    }
    
    [EnableCors("DefaultPolicy")]
    [HttpGet("ClubId/{ClubId:int}",Name="GetClubById"), Authorize]
    public async Task<ActionResult<IEnumerable<Club>>> GetClubById(int ClubId){

        return (_clubRepository.GetClubById(ClubId) is IQueryable<Club> clubById)
            ? Ok(await clubById
                .ToListAsync())
            : NotFound($"No club found with id {ClubId}");
    }

    [EnableCors("DefaultPolicy")]
    [HttpGet("ClubId/{ClubId:int}/members", Name = "GetClubMembers"), Authorize]
    public async Task<ActionResult<IEnumerable<Member>>> GetClubMembers(int ClubId)
    {
        //todo fix this
        return (_clubRepository.RetrieveClubMembers(ClubId) is IQueryable<Member> MembersByClub)
            ? Ok(await MembersByClub
                .ToListAsync())
            : NotFound($"No members found in club: {ClubId}");
    }


    [EnableCors("DefaultPolicy")]
    [HttpGet("MemberId/{MemberId}"), Authorize]
    public async Task<ActionResult<IEnumerable<Club>>> GetClubByMemberId(string MemberId)
    {

        return (_clubRepository.GetClubsByMemberId(MemberId) is IQueryable<Club> clubsByMemberId)
            ? Ok(await clubsByMemberId
                .ToListAsync())
            : NotFound($"No clubs found with {MemberId} as a participant");
    }
    [EnableCors("DefaultPolicy")]
    [HttpGet("public/")]
    public async Task<ActionResult<IEnumerable<Club>>> GetPublicClubs()
    {
        return (_clubRepository.GetPublicClubs() is IQueryable<Club> publicClubs)
            ? Ok(await publicClubs
                .ToListAsync())
            : NotFound($"No public clubs found");
    }




    [EnableCors("DefaultPolicy")]
    [HttpPost (Name="CreateClub"), Authorize]
    public async Task<ActionResult<Club>> CreateClub([FromBody]ClubCreationDTO newClub)
    {
        return (await _clubRepository.CreateClub(newClub) is Club createdClub)
            ? Created(Url.Link("CreateClub", new { id = createdClub?.ClubId }) ?? throw new InvalidOperationException(), createdClub)
            : BadRequest();
    }

    [EnableCors("DefaultPolicy")]
    [HttpDelete("DeleteClub/{ClubId:int}"), Authorize]
    public async Task<bool> DeleteClub([FromBody] int ClubId, string memberID)
    {
        return (bool)await _clubRepository.DeleteClub(ClubId, memberID);
    }

    [EnableCors("DefaultPolicy")]
    [HttpPost("CreateClubInvite",Name = "CreateClubInvite"), Authorize]
    public async Task<ActionResult<Invitation>> CreateClubInvite([FromBody]ClubInviteDTO invite)
    {
        return (await _clubRepository.CreateInvitation(invite.creatorId, invite.memberId, invite.clubId, invite.role,invite.duration) is Invitation createdInvitation)
            ? Created(Url.Link("CreateClubInvitation", new { id = createdInvitation?.ClubId }) ?? throw new InvalidOperationException(), createdInvitation)
            : BadRequest();
    }










}
