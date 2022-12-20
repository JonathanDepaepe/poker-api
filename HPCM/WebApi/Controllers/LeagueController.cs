using AutoMapper.Execution;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {

        private readonly ILeagueRepository _leagueRepository;

        public  LeagueController(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        [EnableCors("DefaultPolicy")]
        [HttpGet("/api/League/{id:int}", Name = "GetLeagueById"),Authorize]
        public async Task<ActionResult<IEnumerable<League>>> GetLeagueById(int id)
        {

            return (_leagueRepository.GetLeaguesById(id) is IQueryable<League> leagueById)
                ? Ok(await leagueById
                    .ToListAsync())
                : NotFound($"No League found with id {id}");
        }

        [EnableCors("DefaultPolicy")]
        [HttpGet("/api/League", Name = "GetLeagues"),Authorize]
        public async Task<ActionResult<IEnumerable<Club>>> GetLeagues()
        {

            return (_leagueRepository.GetLeagues() is IQueryable<League> allLeageus)
                ? Ok(await allLeageus
                    .ToListAsync())
                : NotFound("No Leagues found");
        }

        [EnableCors("DefaultPolicy")]
        [HttpGet("/api/League/Club={clubId:int}", Name = "GetLeagueByClubId"), Authorize]
        public async Task<ActionResult<IEnumerable<League>>> GetLeagueByClubId(int clubId)
        {
            return (_leagueRepository.GetLeaguesByClubId(clubId) is IQueryable<League> leagueByClubId)
                ? Ok(await leagueByClubId
                    .ToListAsync())
                : NotFound($"No Leagues found with ClubId {clubId}");
        }

 
        [EnableCors("DefaultPolicy")]
        [HttpPost("/api/League", Name = "CreateLeague"), Authorize]

        public async Task<ActionResult<League>> CreateLeague([FromBody] LeagueCreationDTO newLeague)
        {
            try
            {
                return (await _leagueRepository.CreateClubLeague(newLeague) is League createdClubLeague)
                    ? Created(Url.Link("CreateLeague", new { id = createdClubLeague?.LeagueId }) ?? throw new InvalidOperationException(), createdClubLeague)
                    : BadRequest();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("unable to create league| ", e);
            }

        }

        [EnableCors("DefaultPolicy")]
        [HttpPost("/api/league/invite", Name = "CreateLeagueInvite"), Authorize]
        public async Task<ActionResult<LeagueInvitation>> CreateLeagueInvite([FromBody] LeagueInviteDTO invite)
        {
            return (await _leagueRepository.CreateLeagueInvitation(invite.memberId, invite.leagueId, invite.duration) is LeagueInvitation createdInvitation)
                ? Created(Url.Link("CreateLeagueInvitation", new { id = createdInvitation?.LeagueId }) ?? throw new InvalidOperationException(), createdInvitation)
                : BadRequest();
        }


    }
}
