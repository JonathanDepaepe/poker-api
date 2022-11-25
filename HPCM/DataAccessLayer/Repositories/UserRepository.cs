using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


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
                    
                    Nickname = createdUser.UserName,
                    Email = createdUser.Email,
                    MemberAssignedType = MemberTypes.BaseMember
                    
                };
                await _db.Members.AddAsync(newMember);
                await SaveAsync();
                return await _db.Members.LastAsync();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Creating member failed, " + e.ToString());
            }        
        }

        public IQueryable<Member> GetMemberById()
        {
            throw new NotImplementedException();
        }

        

        private async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
