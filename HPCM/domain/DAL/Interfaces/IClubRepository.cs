using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IClubRepository
    {
        IQueryable<Club> GetClub();
        IQueryable<Club> GetClubById(int clubId);
        Club CreateClub(Club newClubDetails);
        bool AlterClub(Club newClubDetails);
        bool DeleteClub(int clubId);
        ClubMember JoinClubAsMember(int clubId, int memberId, int memberTypeId);
        League CreateClubLeague(League newLeagueDetails);
        

    }
}
