using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly HpcmContext _db;

        public bool AlterTournament(Tournament newTournamentDetails)
        {
            throw new NotImplementedException();
        }

        public Tournament CreateTournament(Tournament newTournament)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTournament(int tournamentId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Tournament> GetTournamentById()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Member> GetTournamentPlayers(int tournamentId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Tournament> GetTournaments()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Tournament> GetTournamentsByClub()
        {
            throw new NotImplementedException();
        }

        public TournamentReservation JoinTournamentAsMember(int tournamentId, int memberId)
        {
            throw new NotImplementedException();
        }

        public void StartTournament(int tournamentId)
        {
            throw new NotImplementedException();
        }
    }
}
