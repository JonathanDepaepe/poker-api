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
        Task<LeagueInvitation?> CreateLeagueInvitation(string memberId, int leagueId, int durationInDays);
        Task<LeagueMember?> JoinLeagueWithInvitation(string memberId, string hash);
        Task<LeagueMember?> JoinLeague(string memberId, int leagueId);
    }
}