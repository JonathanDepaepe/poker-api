using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
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
    }
}
