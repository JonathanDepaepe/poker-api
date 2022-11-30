using DataAccessLayer.Models;
using Shared.DTO;


namespace DataAccessLayer.Interfaces
{
    public interface ILeagueRepository
    {
        IQueryable<League> GetLeagues();
        IQueryable<League> GetLeaguesById(int leagueId);
        IQueryable<League> GetLeaguesByClubId(int clubId);
        Task<League?> CreateClubLeague(LeagueCreationDTO newLeagueDetails);
        Task<Invitation?> CreateLeagueInvitation(string memberId, int leagueId);
        Task<ClubMember?> JoinClubWithInvitation(string memberId, string hash);
        Task<ClubMember?> JoinClub(string memberId, int clubId, ClubRoles role);
    }
}
