using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

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

    [EnableCors("DefaultPolicy")]
    
    [HttpGet (Name="GetClubs")]
    public async Task<ActionResult<IEnumerable<Club>>> GetClubs(){

        return (_clubRepository.GetClubs() is IQueryable<Club> allClubs)
            ? Ok(await allClubs
                .ToListAsync())
            : NotFound("No clubs found");
    }
    
    [EnableCors("DefaultPolicy")]
    [HttpGet("{id:int}",Name="GetClubById")]
    public async Task<ActionResult<IEnumerable<Club>>> GetClub(int id){

        return (_clubRepository.GetClubById(id) is IQueryable<Club> clubById)
            ? Ok(await clubById
                .ToListAsync())
            : NotFound($"No club found with id {id}");
    }
    
    [EnableCors("DefaultPolicy")]
    [HttpPost (Name="CreateClub")]
    public async Task<ActionResult<Club>> CreateClub([FromBody]ClubCreationDTO newClub)
    {
        return (await _clubRepository.CreateClub(newClub) is Club createdClub)
            ? Created(Url.Link("CreateClub", new { id = createdClub?.ClubId }) ?? throw new InvalidOperationException(), createdClub)
            : BadRequest();
    }
    
    [EnableCors("DefaultPolicy")]
    [HttpPost("/api/League")]
    
    public async Task<ActionResult<League>> CreateLeague([FromBody]LeagueCreationDTO newLeague)
    {
        return (await _clubRepository.CreateClubLeague(newLeague) is League createdClubLeague)
            ? Created(Url.Link("CreateLeague", new { id = createdClubLeague?.LeagueId }) ?? throw new InvalidOperationException(), createdClubLeague)
            : BadRequest();
    }

    
    



}
