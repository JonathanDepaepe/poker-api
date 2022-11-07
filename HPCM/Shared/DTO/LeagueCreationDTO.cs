namespace Shared.DTO;

public class LeagueCreationDTO
{
    public int ClubId { get; set; }
    public string Name { get; set; } = null!;
    public short Public { get; set; }
    public string? Description { get; set; }
}