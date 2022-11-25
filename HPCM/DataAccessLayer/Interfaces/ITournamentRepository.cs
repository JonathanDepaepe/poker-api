using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace DAL.Interfaces
{
    public interface ITournamentRepository
    {
        IQueryable<Tournament> GetTournaments();
        IQueryable<Tournament> GetTournamentById(int tournamentId);
        List<Tournament> GetTournamentsByClub(int clubId);
        Tournament CreateTournament(Tournament tournamentDetails, int clubId, int leagueId);
        bool DeleteTournament(int tournamentId);
        IQueryable<Tournament> AlterTournament(Tournament newTournamentDetails);
        TournamentReservation JoinTournamentUsingReservation(int tournamentId, string memberId);
        TournamentEntry JoinTournamentAfterStart(int tournamentId,string memberId);
        void StartTournament(int tournamentId);
        IQueryable<Member> GetTournamentPlayers(int tournamentId);

    }
}
