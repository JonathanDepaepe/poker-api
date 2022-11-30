using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace DataAccessLayer.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly HpcmContext _db;

        public ClubRepository(HpcmContext db)
        {
            _db = db;
        }

        public Task<Club?> AlterClub(ClubCreationDTO newClubDetails)
        {
            throw new NotImplementedException();
        }

        public async Task<Club?> CreateClub(ClubCreationDTO newClubDetails)
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
                Club createdClub = _db.Clubs.OrderBy(b => b.ClubId).Last();

                //Adds the owner as the admin of the club
                await JoinClub(newClubDetails.OwnerId, createdClub.ClubId, ClubRoles.AdminRole);
                return createdClub;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new club FAILED, check input Params", e.ToString());
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
                League test = _db.Leagues.OrderBy(b=>b.LeagueId).Last();
                return test;
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
                if (clubToDelete is null) throw new ArgumentException("Unable to find Specified club");
                _db.Clubs.Remove(clubToDelete);
                await SaveAsync();
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
                return _db.Clubs
                    .Include(c=>c.Owner)
                    .Include(c=>c.Announcements)
                    .Include(c=>c.Leagues)
                    .Select(b=>new Club
                    {
                        ClubId = b.ClubId,
                        OwnerId = b.OwnerId,
                        Name = b.Name,
                        PictureUrl = b.PictureUrl,
                        Public = b.Public,
                        CreationDateTime = b.CreationDateTime,
                        Owner = b.Owner
                    });
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving clubs from db| ",e);
            }
        }
        public IQueryable<Club> GetPublicClubs()
        {
            try
            {
                return _db.Clubs
                    .Include(c => c.Owner)
                    .Include(c => c.Announcements)
                    .Include(c => c.Leagues)
                    .Select(b => new Club
                    {
                        ClubId = b.ClubId,
                        OwnerId = b.OwnerId,
                        Name = b.Name,
                        PictureUrl = b.PictureUrl,
                        Public = b.Public,
                        CreationDateTime = b.CreationDateTime,
                        Owner = b.Owner
                    })
                    .Where(c=>c.Public==true);
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving clubs from db| ", e);
            }
        }

        public List<Club> GetClubsByMemberId(string memberId)
        {
            try
            {
                List<int> memberClubParticipations = _db.ClubMembers.Where(d => d.MemberId == memberId).Select(b=>b.ClubId).ToList();
                List<Club>? foundClubs = null;

                foreach (int item in memberClubParticipations)
                {
                    foundClubs.Add(GetClubById(item).OrderBy(e=>e.ClubId).First());
                }

                return foundClubs;
            }
            catch (Exception e)
            {

                throw new Exception("Unable to query clubs by MemberId| " + e.ToString());
            }
        }

        public IQueryable<Club> GetClubById(int clubId)
        {
            try
            {
                return GetClubs().Where(s => s.ClubId == clubId);
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

        public async Task<Invitation?> CreateInvitation(string memberId,int clubId,ClubRoles role)
        {
            try
            {
                //idea, create defined invite hash for club so multiple members can join. Deletable in club management
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

        public async Task<ClubMember?> JoinClubWithInvitation(string memberId, string hash)
        {
            try
            {
                Invitation invitationAttempt = _db.Invitations.Where(s=>s.InvitationHash == hash).First();
                var memberExists = _db.ClubMembers.Where(c => c.ClubId == invitationAttempt.ClubId && c.MemberId == memberId);
                if (memberExists != null)
                {
                    throw new Exception($"{memberId} already exists in the player base of the club.");
                }
                    

                if (invitationAttempt.MemberId == memberId)
                {
                    ClubMember createdMember = await JoinClub(invitationAttempt.MemberId, invitationAttempt.ClubId,invitationAttempt.Role);
                    _db.Invitations.Remove(invitationAttempt);
                    await _db.SaveChangesAsync();
                    return createdMember;
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

        public async Task<ClubMember> JoinClub(string memberId, int clubId,ClubRoles role)
        {
            try
            {

                ClubMember newClubMember = new()
                {
                    ClubId = clubId,
                    MemberId = memberId,
                    Role = role
                };
                IQueryable<ClubMember> currentClubMembers = RetrieveClubMembers(clubId);
                if (currentClubMembers.Any(o=>o.ClubId==newClubMember.ClubId&&o.MemberId==newClubMember.MemberId)) 
                {
                    throw new Exception($"{memberId} already exists in the player base of the club.");
                }
                
                await _db.ClubMembers.AddAsync(newClubMember);
                await SaveAsync();
                return _db.ClubMembers.OrderBy(b => b.MemberId).Last();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new club FAILED, check input Params", e.ToString());
            }
        }

        public IQueryable<ClubMember?> RetrieveClubMembers(int clubId)
        {
            try
            {
                return _db.ClubMembers.Where(c => c.ClubId == clubId);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve all club members| " + e);
            }
        }
    }
}
