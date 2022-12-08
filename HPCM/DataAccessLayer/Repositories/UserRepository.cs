using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly HpcmContext _db;

        public UserRepository(HpcmContext db)
        {
            _db = db;
        }

        public async Task<Member> CreateMember(IdentityUser createdUser)
        {
            try
            {
                Member newMember = new()
                {
                    MemberId = createdUser.Id,
                    Name = "",
                    Nickname = createdUser.UserName,
                    Email = createdUser.Email,
                    MemberAssignedType = MemberTypes.BaseMember,
                    ProfilePictureUrl = ""
                };
                await _db.Members.AddAsync(newMember);
                await SaveAsync();
                return newMember;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating member failed, " + e.ToString());
            }        
        }

        public IQueryable<Member> GetMemberById(string memberId)
        {
            return _db.Members.Where(c => c.MemberId == memberId);
        }


        private async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
