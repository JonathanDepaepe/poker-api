using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using DataAccessLayer.Exceptions;


namespace DAL.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly HpcmContext _db;

        public Task<Club?> AlterClub(Club newClubDetails)
        {
            throw new NotImplementedException();
        }

        public async Task<Club?> CreateClub(Club newClubDetails)
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
                await _db.Clubs.AddAsync(newClub);
                await SaveAsync();
                return _db.Clubs.Last();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new club FAILED, check input Params", e.ToString());
            }
        }

        public async Task<League?> CreateClubLeague(League newLeagueDetails)
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
                return _db.Leagues.Last();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new League FAILED, check input Params| ",e.ToString());
            }
        }

        public async Task<Club?> DeleteClub(Club clubToBeDeleted)
        {
            try
            {
                Club? clubToDelete = await GetClubByIdAsync(clubToBeDeleted.ClubId);
                if (clubToDelete is Club)
                {
                    _db.Clubs.Remove(clubToDelete);
                    await SaveAsync();
                }
                return clubToDelete;
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
        public async Task<Club?> GetClubByIdAsync(int clubId)
        {
            try
            {
                Club? foundClub = await _db.Clubs
                    .SingleOrDefaultAsync(b => b.ClubId == clubId);
                return foundClub;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve club by id from db| ",e);
            }
        }

        public async Task<Invitation?> CreateInvitation(int memberId,int clubId,string role)
        {
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    
                    Invitation inv = new Invitation()
                    {
                        MemberId = memberId,
                        ClubId = clubId,
                        ExpirationDate = DateTime.UtcNow.AddDays(7),
                        InvitationHash = GetHash(sha256Hash,(memberId.ToString() +DateTime.UtcNow.AddDays(7))),
                        Role = role
                    };
                    
                    await _db.Invitations.AddAsync(inv);
                    await SaveAsync();
                    return inv;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create Invitation| ",e);
            }
        }

        public async Task<ClubMember?> JoinClubWithInvitation(int memberId, string hash)
        {
            try
            {
                Invitation invitationAttempt = _db.Invitations.Where(s=>s.InvitationHash == hash).First();
                if (invitationAttempt.MemberId == memberId)
                {
                    ClubMember newMember = new ClubMember()
                    {
                        ClubId = invitationAttempt.ClubId,
                        MemberId = memberId,
                        Role =  invitationAttempt.Role
                    };
                    await _db.ClubMembers.AddAsync(newMember);
                    _db.Invitations.Remove(invitationAttempt);
                    await _db.SaveChangesAsync();
                    return newMember;
                }
                throw new InvalidInvitationException("Error using invite, wrong member Id");
            }
            catch (Exception e)
            {
                throw new Exception("Unable to join club| ",e);
            }
        }
        private async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
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
