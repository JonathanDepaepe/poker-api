using DAL.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly HpcmContext _db;

        public IQueryable<Member> GetMemberById()
        {
            throw new NotImplementedException();
        }
    }
}
