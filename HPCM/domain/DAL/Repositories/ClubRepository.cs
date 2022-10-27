using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly HpcmContext _db;

        public bool AlterClub(Club newClubDetails)
        {
            throw new NotImplementedException();
        }

        public Club CreateClub(Club newClubDetails)
        {
            try
            {
                Club newClub = new()
                {
                    OwnerId = newClubDetails.OwnerId,
                    Name = newClubDetails.Name,
                    PictureUrl = newClubDetails.PictureUrl,
                    Public = newClubDetails.Public,
                    CreationDateTime = DateTime.UtcNow
                };
                _db.Clubs.Add(newClub);
                _db.SaveChanges();
                return _db.Clubs.OrderBy(s => s.ClubId).Last();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new club FAILED, check input Params", e.ToString());
            }
        }

        public League CreateClubLeague(League newLeagueDetails)
        {
            try
            {
                League league = new()
                {
                    ClubId = newLeagueDetails.ClubId,
                    Name = newLeagueDetails.Name,
                    Public = newLeagueDetails.Public,
                    Description = newLeagueDetails.Description
                };
                _db.Leagues.Add(league);
                _db.SaveChanges();
                return _db.Leagues.OrderBy(s => s.LeagueId).Last();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new League FAILED, check input Params| ",e.ToString());
            }
        }

        public bool DeleteClub(int clubId)
        {
            try
            {
                _db.Clubs.Remove(_db.Clubs.Find(clubId));
                _db.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                throw new Exception("Unable to delete specified club| ", e);
            }
        }

        public IQueryable<Club> GetClubs()
        {
            try
            {
                return _db.Clubs;
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving clubs from db| ",e);
            }
        }

        public IQueryable<Club> GetClubById(int clubId)
        {
            try
            {
                return _db.Clubs.Where(s => s.ClubId == clubId);
            }
            catch (Exception e)
            {

                throw new Exception("Unable to retrieve club by id from db| ",e);
            }
        }

        public Invitation CreateInvitation(int memberId,int clubId)
        {
            try
            {
                return new Invitation()
                {
                    MemberId = memberId,
                    ClubId = clubId,
                    ExpirationDate = DateTime.UtcNow.AddDays(7),
                };
            }
            catch (Exception e)
            {

                throw new Exception("Unable to get Invitation| ",e);
            }
        }

        public IQueryable<ClubMember> JoinClubWithInvitation(int memberId, string hash)
        {
            try
            {
                Invitation invitationAttempt = _db.Invitations.Where(s=>s.InvitationHash == hash).First();
                if (invitationAttempt.MemberId == memberId)
                {
                    _db.ClubMembers.Add(new ClubMember()
                    {
                        ClubId = invitationAttempt.ClubId,
                        MemberId = memberId,
                        Role =  invitationAttempt.Role
                    });
                    _db.Invitations.Remove(invitationAttempt);
                    _db.SaveChanges();
                    return _db.ClubMembers.Where(s => s.MemberId == memberId).Where(s => s.ClubId == invitationAttempt.ClubId);
                }
                else
                {
                    throw new Exception("Invalid attempt at joining club");
                }

            }
            catch (Exception e)
            {
                throw new Exception("Unable to join club| ",e);
            }
        }
    }
}
