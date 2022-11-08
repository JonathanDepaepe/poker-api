using System.Security.Cryptography;
using System.Text;
using DAL.Interfaces;
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

        public Task<Club?> AlterClub(Club newClubDetails)
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
                return _db.Clubs.OrderBy(b=>b.ClubId).Last();
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
                Console.WriteLine("returned league: id:" + test.LeagueId + " Name: " + test.Name + " ClubId" + test.ClubId);
                return test;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating new League FAILED, check input Params| ",e.ToString());
            }
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
                Console.WriteLine("Unable to retrieve all leagues",e);
                throw;
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
                Console.WriteLine("Unable to retrieve leagues by id",e);
                throw;
            }
        }

        public IQueryable<League> GetLeaguesByClubId(int clubId)
        {
            throw new NotImplementedException();
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
