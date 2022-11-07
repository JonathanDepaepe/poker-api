using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace DAL.Interfaces
{
    public interface IClubRepository
    {
        IQueryable<Club> GetClubs();
        IQueryable<Club> GetClubById(int clubId);
        Task<Club?> CreateClub(Club newClubDetails);
        Task<Club?> AlterClub(Club newClubDetails);
        Task<Club?> DeleteClub(Club clubToBeDeleted);
        Task<League?> CreateClubLeague(League newLeagueDetails);
        Task<Invitation?> CreateInvitation(int memberId,int clubId,string role);
        Task<ClubMember?> JoinClubWithInvitation(int memberId, string hash);
    }
}
