namespace DataAccessLayer.AuthModels;

public class Response
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshExpiration { get; set; }
    public string? MemberId { get; set; }
}