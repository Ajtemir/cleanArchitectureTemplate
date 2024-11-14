namespace Application.Common.Dto;

public class UserDto
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public string? PatronymicName { get; set; }
    public int? BranchId { get; set; }
    public string? Pin { get; set; }
    public string? PhotoBase64 { get; set; }
    public required string RefreshToken { get; set; }
    public required string AccessToken { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}
