namespace Shared.DTO;

public class MemberDTO
{
    public string MemberId { get; set; }
    public string Name { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ProfilePictureUrl { get; set; } = null!;
}