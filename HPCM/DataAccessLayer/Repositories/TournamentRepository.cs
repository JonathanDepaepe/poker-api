using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly HpcmContext _db;

        public TournamentRepository(HpcmContext db)
        {
            _db = db;
        }

        public IQueryable<Tournament> AlterTournament(Tournament newTournamentDetails)
        {
            try
            {
                var tournament = _db.Tournaments.FirstOrDefault(item => item.TournamentId == newTournamentDetails.TournamentId); ;
                tournament.Public = newTournamentDetails.Public;
                tournament.Name = newTournamentDetails.Name;
                tournament.Description = newTournamentDetails.Description;
                tournament.Status = newTournamentDetails.Status;
                tournament.StartDateTime = newTournamentDetails.StartDateTime;
                tournament.Location = newTournamentDetails.Location;
                tournament.MaxPlayerCount = newTournamentDetails.MaxPlayerCount;
                _db.Tournaments.Update(tournament);
                _db.SaveChanges();
                return _db.Tournaments.Where(s => s.TournamentId == newTournamentDetails.TournamentId);
            }
            catch (Exception e)
            {
                throw new Exception("unable to alter Tournament details| ",e);
            }
        }

        public Tournament CreateTournament(Tournament tournamentDetails,int clubId,int leagueId)
        {
            try
            {
                //creates new tournament
                Tournament newTournament = new()
                {
                    Public = tournamentDetails.Public,
                    Name = tournamentDetails.Name,
                    Description = tournamentDetails.Description,
                    Status = "Upcoming",
                    StartDateTime = tournamentDetails.StartDateTime,
                    Location = tournamentDetails.Location,
                    MaxPlayerCount = tournamentDetails.MaxPlayerCount
                };

                _db.Tournaments.Add(newTournament);
                _db.SaveChanges();
                //creates new tournamentlink
                Tournament addedTournament = _db.Tournaments.Last();
                TournamentLink newTournamentLink = new()
                {
                    TournamentId = addedTournament.TournamentId,
                    ClubId = clubId,
                    LeagueId = leagueId
                };

                _db.TournamentLinks.Add(newTournamentLink);
                _db.SaveChanges();

                return newTournament;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create new Tournament| ",e);
            }
        }

        public bool DeleteTournament(int tournamentId)
        {
            try
            {
                _db.Tournaments.Remove(_db.Tournaments.Find(tournamentId));
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to delete Tournament| ", e);
            }
        }

        public IQueryable<Tournament> GetTournamentById(int tournamentId)
        {
            try
            {
                return _db.Tournaments.Where(s => s.TournamentId == tournamentId);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve Tournament by id| ", e);
            }
        }

        public IQueryable<Member> GetTournamentPlayers(int tournamentId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Tournament> GetTournaments()
        {
            try
            {
                return _db.Tournaments;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve all Tournaments| ", e);
            }
        }

        public List<Tournament> GetTournamentsByClub(int clubId)
        {
            try
            {
                IQueryable<TournamentLink> tournamentIds = _db.TournamentLinks.Where(s => s.ClubId == clubId);

                List<Tournament> foundTournaments = new();
                foreach (TournamentLink item in tournamentIds)
                {
                    IQueryable<Tournament> currentTournament = _db.Tournaments.Where(s => s.TournamentId == item.TournamentId);
                    foundTournaments.Add(currentTournament.Last());
                }

                return foundTournaments;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve Tournament by club| ", e);
            }
        }

        public TournamentEntry JoinTournamentAfterStart(int tournamentId, string memberId)
        {
            try
            {
                TournamentEntry entry = new()
                {
                    TournamentId = tournamentId,
                    MemberId = memberId,
                    Position = 0,
                    RegistrationDateTime = DateTime.UtcNow
                };

                _db.TournamentEntries.Add(entry);
                _db.SaveChanges();
                return _db.TournamentEntries.Last();
            }
            catch (Exception e)
            {
                throw new Exception("Unable to join tournament late| ", e);
            }
        }

        public TournamentReservation JoinTournamentUsingReservation(int tournamentId, string memberId)
        {
            try
            {
                TournamentReservation res = new()
                {
                    TournamentId = tournamentId,
                    MemberId = memberId
                };
                TournamentEntry entry = new()
                {
                    TournamentId = tournamentId,
                    MemberId = memberId,
                    Position = 0,
                    RegistrationDateTime = DateTime.UtcNow
                };

                _db.TournamentReservations.Add(res);
                _db.TournamentEntries.Add(entry);
                _db.SaveChanges();
                return _db.TournamentReservations.Last();
            }
            catch (Exception e)
            {
                throw new Exception("Unable to reserve a seat for tournament| ", e);
            }
        }

        //changes tournament state to running and all tournament reservations will be added to the tournament players
        public void StartTournament(int tournamentId)
        {
            try
            {
                //changes the status of the tournament
                var tournament = _db.Tournaments.FirstOrDefault(item => item.TournamentId == tournamentId); ;
                tournament.Status = "Running";
                _db.Tournaments.Update(tournament);
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("unable to change status of tournament| " , e);
            }
        }
    }
}
