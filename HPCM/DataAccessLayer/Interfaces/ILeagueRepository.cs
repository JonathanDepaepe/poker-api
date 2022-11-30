using DataAccessLayer.Models;


namespace DataAccessLayer.Interfaces
{
    public interface ILeagueRepository
    {
        IQueryable<League> GetLeagues();
        IQueryable<League> GetLeaguesById(int leagueId);
        IQueryable<League> GetLeaguesByClubId(int clubId);
    }
}
