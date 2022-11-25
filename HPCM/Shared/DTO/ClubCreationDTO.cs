using System.ComponentModel.DataAnnotations;

namespace Shared.DTO;

public class ClubCreationDTO
{
    [Required]
    public string OwnerId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public string? PictureUrl { get; set; }
    [Required]
    public bool Public { get; set; }
}