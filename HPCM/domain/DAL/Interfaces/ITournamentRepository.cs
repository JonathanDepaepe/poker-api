using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ITournamentRepository
    {
        IQueryable<Tournament> GetTournaments();
        IQueryable<Tournament> GetTournamentById();
        IQueryable<Tournament> GetTournamentsByClub();
        Tournament CreateTournament(Tournament newTournament);
        bool DeleteTournament(int tournamentId);
        bool AlterTournament(Tournament newTournamentDetails);
        TournamentReservation JoinTournamentAsMember(int tournamentId, int memberId);
        void StartTournament(int tournamentId);
        IQueryable<Member> GetTournamentPlayers(int tournamentId);

    }
}
