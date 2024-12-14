namespace Common.Models;

public class AccessToken
{
    public string Id { get; set; } = default!;
    public string CompanyId { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
    public string Value { get; set; } = default!;
}