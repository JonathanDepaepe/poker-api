using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Shared.DTO;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly HpcmContext _db;

        public UserRepository(HpcmContext db)
        {
            _db = db;
        }

        public async Task<Member?> AlterMember(MemberDTO newMemberDetails)
        {
            try
            {
                var member = _db.Members.Where(s=>s.MemberId== newMemberDetails.MemberId).FirstOrDefault();
                if (member != null && newMemberDetails.Email!=string.Empty && newMemberDetails.Nickname != string.Empty && newMemberDetails.Name != string.Empty)
                {
                    member.Email = newMemberDetails.Email;
                    member.Nickname = newMemberDetails.Nickname;
                    member.ProfilePictureUrl = newMemberDetails.ProfilePictureUrl;
                    member.Name= newMemberDetails.Name;

                    _db.Members.Update(member);
                    await SaveAsync();
                    return member;
                }
                else
                {
                    throw new Exception("Member not found when trying to update details!");
                }


            }
            catch (Exception e)
            {

                throw new Exception("Failed to update member info! " + e);
            }
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
