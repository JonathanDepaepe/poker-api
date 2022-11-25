using System.ComponentModel.DataAnnotations;

namespace Shared.DTO;

public class ClubOwnerDTO
{
    public string MemberId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    
}