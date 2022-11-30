using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DataAccessLayer.Repositories
{
    public class LeagueRepository: ILeagueRepository
    {

        private readonly HpcmContext _db;

        public LeagueRepository(HpcmContext db)
        {
            _db = db;
        }

        public IQueryable<League> GetLeagues()
        {
            try
            {
                IQueryable<League> foundLeagues =
                    _db.Leagues
                        .Include(b => b.Club)
                        .Select(b => new League()
                        {
                            LeagueId = b.LeagueId,
                            ClubId = b.ClubId,
                            Name = b.Name,
                            Description = b.Description,
                            Club = b.Club
                        });
                return foundLeagues;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve all leagues", e);
            }
        }

        public IQueryable<League> GetLeaguesById(int leagueId)
        {
            try
            {
                return GetLeagues().Where(b => b.LeagueId == leagueId);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve leagues by id", e);
            }
        }

        public IQueryable<League> GetLeaguesByClubId(int clubId)
        {
            try
            {
                return GetLeagues().Where(b => b.ClubId == clubId);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve Leagues by ClubId | " + e);
            }
        }

        public async Task<League?> CreateClubLeague(LeagueCreationDTO newLeagueDetails)
        {
            try
            {
                League league = new()
                {
                    ClubId = newLeagueDetails.ClubId,
                    Name = newLeagueDetails.Name,
                    Public = newLeagueDetails.Public,
                    Description = newLeagueDetails.Description
                };
                await _db.Leagues.AddAsync(league);
                await SaveAsync();
                League test = _db.Leagues.OrderBy(b => b.LeagueId).Last();
                return test;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new League FAILED, check input Params| ", e.ToString());
            }
        }

        private async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public Task<Invitation?> CreateLeagueInvitation(string memberId, int leagueId)
        {
            throw new NotImplementedException();
        }

        public Task<ClubMember?> JoinClubWithInvitation(string memberId, string hash)
        {
            throw new NotImplementedException();
        }

        public Task<ClubMember?> JoinClub(string memberId, int clubId, ClubRoles role)
        {
            throw new NotImplementedException();
        }
    }
}