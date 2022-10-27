using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly HpcmContext _db;

        public bool AlterClub(Club newClubDetails)
        {
            throw new NotImplementedException();
        }

        public Club CreateClub(Club newClubDetails)
        {
            try
            {
                Club newClub = new()
                {
                    OwnerId = newClubDetails.OwnerId,
                    Name = newClubDetails.Name,
                    PictureUrl = newClubDetails.PictureUrl,
                    Public = newClubDetails.Public,
                    CreationDateTime = DateTime.UtcNow
                };
                _db.Clubs.Add(newClub);
                _db.SaveChanges();
                return _db.Clubs.OrderBy(s => s.ClubId).Last();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new club FAILED, check input Params", e.ToString());
            }
        }

        public League CreateClubLeague(League newLeagueDetails)
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
                _db.Leagues.Add(league);
                _db.SaveChanges();
                return _db.Leagues.OrderBy(s => s.LeagueId).Last();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new League FAILED, check input Params",e.ToString());
            }
        }

        public bool DeleteClub(int clubId)
        {
            try
            {
                _db.Clubs.Remove(_db.Clubs.Find(clubId));
                _db.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                throw new Exception("Unable to delete specified club", e);
            }
        }

        public IQueryable<Club> GetClubs()
        {
            try
            {
                return _db.Clubs;
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving clubs from db",e);
            }
        }

        public IQueryable<Club> GetClubById(int clubId)
        {
            try
            {
                return _db.Clubs.Where(s => s.ClubId == clubId);
            }
            catch (Exception e)
            {

                throw new Exception("Unable to retrieve club by id from db",e);
            }
        }

        public ClubMember JoinClubAsMember(int clubId, int memberId, int memberTypeId)
        {
            throw new NotImplementedException();
        }
    }
}
