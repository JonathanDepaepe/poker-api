using DataAccessLayer.Exceptions;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using StackExchange.Redis;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DataAccessLayer.Repositories
{
    public class LeagueRepository: ILeagueRepository
    {

        private readonly HpcmContext _db;

        public LeagueRepository(HpcmContext db)
        {
            _db = db;
        }

        public IQueryable<League> GetLeagues()
        {
            try
            {
                IQueryable<League> foundLeagues =
                    _db.Leagues
                        .Include(b => b.Club)
                        .Select(b => new League()
                        {
                            LeagueId = b.LeagueId,
                            ClubId = b.ClubId,
                            Name = b.Name,
                            Description = b.Description,
                            Club = b.Club
                        });
                return foundLeagues;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve all leagues", e);
            }
        }

        public IQueryable<League> GetLeaguesById(int leagueId)
        {
            try
            {
                return GetLeagues().Where(b => b.LeagueId == leagueId);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve leagues by id", e);
            }
        }

        public IQueryable<League> GetLeaguesByClubId(int clubId)
        {
            try
            {
                return GetLeagues().Where(b => b.ClubId == clubId);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve Leagues by ClubId | " + e);
            }
        }

        public async Task<League?> CreateClubLeague(LeagueCreationDTO newLeagueDetails)
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
                await _db.Leagues.AddAsync(league);
                await SaveAsync();
                League test = _db.Leagues.OrderBy(b => b.LeagueId).Last();
                return test;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new League FAILED, check input Params| ", e.ToString());
            }
        }

        private async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<LeagueInvitation?> CreateLeagueInvitation(string memberId, int leagueId, int durationInDays)
        {
            try
            {
                //idea, create defined invite hash for club so multiple members can join. Deletable in club management
                using (SHA256 sha256Hash = SHA256.Create())
                {

                    LeagueInvitation inv = new()
                    {
                        MemberId = memberId,
                        LeagueId = leagueId,
                        ExpirationDate = DateTime.UtcNow.AddDays(durationInDays),
                        InvitationHash = GetHash(sha256Hash, (memberId.ToString() + DateTime.UtcNow.AddDays(durationInDays))),
                    };

                    await _db.LeagueInvitations.AddAsync(inv);
                    await SaveAsync();
                    return inv;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create Invitation| ", e);
            }
        }



        public async Task<LeagueMember?> JoinLeagueWithInvitation(string memberId, string hash)
        {
            try
            {


                //check if member already in League
                LeagueInvitation invitationAttempt = _db.LeagueInvitations.Where(s => s.InvitationHash == hash).First();
                var memberExists = _db.LeagueMembers.Where(c => c.LeagueId == invitationAttempt.LeagueId && c.MemberId == memberId);
                if (memberExists != null)
                {
                    throw new Exception($"{memberId} already exists in the player base of the club.");
                }

                //add new member to League
                if (invitationAttempt.MemberId == memberId)
                {
                    LeagueMember? createdMember = await JoinLeague(invitationAttempt.MemberId, invitationAttempt.LeagueId);
                    _db.LeagueInvitations.Remove(invitationAttempt);
                    await _db.SaveChangesAsync();
                    return createdMember;
                }
                throw new InvalidInvitationException("Error using invite, wrong member Id");
            }
            catch (Exception e)
            {
                throw new Exception("Unable to join club| ", e);
            }
        }

        public async Task<LeagueMember?> JoinLeague(string memberId, int leagueId)
        {
            try
            {

                LeagueMember newLeagueMember = new()
                {
                    LeagueId = leagueId,
                    MemberId = memberId
                };
                IQueryable<LeagueMember> currentLeagueMembers = RetrieveLeagueMembersConnection(leagueId);
                if (currentLeagueMembers.Any(o => o.LeagueId == newLeagueMember.LeagueId && o.MemberId == newLeagueMember.MemberId))
                {
                    throw new Exception($"{memberId} already exists in the player base of the club.");
                }

                await _db.LeagueMembers.AddAsync(newLeagueMember);
                await SaveAsync();
                return _db.LeagueMembers.Where(s=>s.MemberId==memberId).OrderBy(b => b.MemberId).Last();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Joining new club FAILED, check input Params", e.ToString());
            }
        }

        public IQueryable<LeagueMember?> RetrieveLeagueMembersConnection(int leagueId)
        {
            try
            {
                return _db.LeagueMembers.Where(c => c.LeagueId == leagueId);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve all league members| " + e);
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}