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

        public IQueryable<Club?> AlterClub(ClubCreationDTO newClubDetails, int clubId)
        {
            try
            {
                var club = _db.Clubs.FirstOrDefault(item => item.ClubId == clubId); ;
                club.Public = newClubDetails.Public;
                club.Name = newClubDetails.Name;
                club.PictureUrl = newClubDetails.PictureUrl;
                club.Public = newClubDetails.Public;

                _db.Clubs.Update(club);
                _db.SaveChanges();
                return GetClubs().Where(s => s.ClubId == clubId);
            }
            catch (Exception e)
            {
                throw new Exception("unable to alter Tournament details| ", e);
            }
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

        public async Task<bool?> DeleteClub(int clubId, string memberId)
        {
            try
            {
                Club? clubToDelete = await GetClubByIdAsync(clubId);
                if (clubToDelete is not null)
                {
                    if (clubToDelete.OwnerId == memberId)
                    {
                        _db.Clubs.Remove(clubToDelete);
                        await SaveAsync();
                        return true;
                    }
                    else
                    {
                        throw new InvalidPermissionsException("Invalid permissions to delete club!");
                    }
                }
                else
                {
                    throw new ArgumentException("Unable to find Specified club");
                }
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
                        Owner = b.Owner,
                        Leagues = (ICollection<League>)b.Leagues.Select(b=>new League
                        {
                            LeagueId = b.LeagueId,
                            Name= b.Name,
                            Public= b.Public,
                            Description= b.Description
                        }),
                        Announcements = (ICollection<Announcement>)b.Announcements.Select(b=>new Announcement
                        {
                            PostId= b.PostId,
                            CreatorId= b.CreatorId,
                            Title= b.Title,
                            Description= b.Description,
                            CreationDateTime= b.CreationDateTime
                        })
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
                return GetClubs().Where(c => c.Public == true); 
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

        public async Task<Invitation?> CreateInvitation(string creatorId,string memberId,int clubId,string role,int durationInDays)
        {
            try
            {   //Method first checks if the creator has the correct permissions in the club, then it will create a new invite
                //idea, create defined invite hash for club so multiple members can join. Deletable in club management
                if (CheckPermissions(creatorId, clubId, ClubRoles.ModeratorRole.ToString()) || CheckPermissions(creatorId, clubId, ClubRoles.AdminRole.ToString()))
                {
                    ClubRoles definedRole = (ClubRoles) Enum.Parse(typeof(ClubRoles), role);
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        Invitation inv = new()
                        {
                            MemberId = memberId,
                            ClubId = clubId,
                            CreatorId= creatorId,
                            ExpirationDate = DateTime.UtcNow.AddDays(durationInDays),
                            InvitationHash = GetHash(sha256Hash, (memberId.ToString() + DateTime.UtcNow.AddDays(7))),
                            Role = definedRole
                        };

                        await _db.Invitations.AddAsync(inv);
                        await SaveAsync();
                        return inv;
                    }
                }
                else
                {
                    throw new InvalidInvitationException("Invalid permissions to create invitation!");
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
                //check if member already in club
                Invitation invitationAttempt = _db.Invitations.Where(s=>s.InvitationHash == hash).First();
                var memberExists = _db.ClubMembers.Where(c => c.ClubId == invitationAttempt.ClubId && c.MemberId == memberId);
                if (memberExists != null)
                {
                    throw new Exception($"{memberId} already exists in the player base of the club.");
                }
                if (invitationAttempt.ExpirationDate < DateTime.UtcNow)
                {
                    throw new Exception("The invitation has expired!");
                }   
                //add new member to club
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

        public async Task<ClubMember?> JoinClub(string memberId, int clubId,ClubRoles role)
        {
            try
            {

                ClubMember newClubMember = new()
                {
                    ClubId = clubId,
                    MemberId = memberId,
                    Role = role
                };
                IQueryable<ClubMember> currentClubMembers = RetrieveClubMembersConnection(clubId);
                if (currentClubMembers.Any(o=>o.ClubId==newClubMember.ClubId&&o.MemberId==newClubMember.MemberId)) 
                {
                    throw new Exception($"{memberId} already exists in the player base of the club.");
                }
                
                await _db.ClubMembers.AddAsync(newClubMember);
                await SaveAsync();
                return _db.ClubMembers.Where(s=>s.MemberId==memberId).OrderBy(b => b.MemberId).Last();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Joining new club FAILED, check input Params", e.ToString());
            }
        }

        public IQueryable<ClubMember?> RetrieveClubMembersConnection(int clubId)
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

        public List<IQueryable<Member?>> RetrieveClubMembers(int clubId)
        {
            try
            {
                var retrievedMembersIds = RetrieveClubMembersConnection(clubId);

                List<IQueryable<Member?>> joinedMembers = new List<IQueryable<Member?>>();

                foreach (var item in retrievedMembersIds)
                {
                    joinedMembers.Add(_db.Members.Where(s => s.MemberId == item.MemberId));
                }

                return joinedMembers;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve all club members| " + e);
            }
        }

        private bool CheckPermissions(string memberId,int clubId,string role)
        {
            //will check if the given member has the correct permissions to execute a specific method/task
            try
            {
                IQueryable<ClubMember?> foundMember = RetrieveClubMembersConnection(clubId).Where(c => c.MemberId == memberId);
                if (foundMember.Any())
                {
                    ClubRoles foundRole = foundMember.SingleOrDefault().Role;
                    if (foundRole.ToString()==role)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception e)
            {

                throw new Exception("Unable to check permission for member| " +e);
            }
        }
    }
}