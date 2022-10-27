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
            throw new NotImplementedException();
        }

        public League CreateClubLeague(League newLeagueDetails)
        {
            throw new NotImplementedException();
        }

        public bool DeleteClub(int clubId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Club> GetClub()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Club> GetClubById(int clubId)
        {
            throw new NotImplementedException();
        }

        public ClubMember JoinClubAsMember(int clubId, int memberId, int memberTypeId)
        {
            throw new NotImplementedException();
        }
    }
}
