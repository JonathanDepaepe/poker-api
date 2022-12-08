using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Shared.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<Member> GetMemberById(string memberId);

        Task<Member> CreateMember(IdentityUser newMemberDetails);

    }
}
