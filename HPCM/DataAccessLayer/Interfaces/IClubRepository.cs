using DataAccessLayer.Models;
using Shared.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IClubRepository
    {
        IQueryable<Club> GetClubs();
        IQueryable<Club> GetClubById(int clubId);
        Task<Club?> CreateClub(ClubCreationDTO newClubDetails);
        Task<Club?> AlterClub(Club newClubDetails);
        Task<Club?> DeleteClub(Club clubToBeDeleted);
        Task<League?> CreateClubLeague(LeagueCreationDTO newLeagueDetails);
        IQueryable<League> GetLeagues();
        IQueryable<League> GetLeaguesById(int leagueId);
        IQueryable<League> GetLeaguesByClubId(int clubId);
        Task<Invitation?> CreateInvitation(int memberId,int clubId,string role);
        Task<ClubMember?> JoinClubWithInvitation(int memberId, string hash);
    }
}
