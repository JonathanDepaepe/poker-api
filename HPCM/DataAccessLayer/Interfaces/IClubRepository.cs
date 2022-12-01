using DataAccessLayer.Models;
using Shared.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IClubRepository
    {
        IQueryable<Club> GetClubs();
        IQueryable<Club> GetPublicClubs();
        IQueryable<Club> GetClubById(int clubId);
        List<Club> GetClubsByMemberId(string memberId);
        Task<Club?> CreateClub(ClubCreationDTO newClubDetails);
        IQueryable<Club?> AlterClub(ClubCreationDTO newClubDetails, int clubId);
        Task<bool?> DeleteClub(int clubId, string memberId);
        Task<Invitation?> CreateInvitation(string creatorId,string memberId,int clubId,string role,int durationInDays);
        Task<ClubMember?> JoinClubWithInvitation(string memberId, string hash);
        Task<ClubMember?> JoinClub(string memberId, int clubId,ClubRoles role);
        IQueryable<ClubMember?> RetrieveClubMembersConnection(int clubId);
        List<IQueryable<Member?>> RetrieveClubMembers(int clubId);
    }
}
