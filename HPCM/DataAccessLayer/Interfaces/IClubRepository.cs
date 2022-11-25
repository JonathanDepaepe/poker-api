using DataAccessLayer.Models;
using Shared.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IClubRepository
    {
        IQueryable<Club> GetClubs();
        IQueryable<Club> GetPublicClubs();
        IQueryable<Club> GetClubById(int clubId);
        List<Club> GetClubsByMemberId(string memberId);
        Task<Club?> CreateClub(ClubCreationDTO newClubDetails);
        Task<Club?> AlterClub(ClubCreationDTO newClubDetails);
        Task<Club?> DeleteClub(Club clubToBeDeleted);
        Task<League?> CreateClubLeague(LeagueCreationDTO newLeagueDetails);
        IQueryable<League> GetLeagues();
        IQueryable<League> GetLeaguesById(int leagueId);
        IQueryable<League> GetLeaguesByClubId(int clubId);
        Task<Invitation?> CreateInvitation(string memberId,int clubId,ClubRoles role);
        Task<ClubMember?> JoinClubWithInvitation(string memberId, string hash);
        Task<ClubMember?> JoinClub(string memberId, int clubId,ClubRoles role);
        IQueryable<ClubMember?> RetrieveClubMembers(int clubId);

    }
}
