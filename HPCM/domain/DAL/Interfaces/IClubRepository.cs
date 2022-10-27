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
        IQueryable<Club> GetClubs();
        IQueryable<Club> GetClubById(int clubId);
        Club CreateClub(Club newClubDetails);
        bool AlterClub(Club newClubDetails);
        bool DeleteClub(int clubId);
        League CreateClubLeague(League newLeagueDetails);
        Invitation CreateInvitation(int memberId,int clubId);
        IQueryable<ClubMember> JoinClubWithInvitation(int memberId, string hash);
    }
}
