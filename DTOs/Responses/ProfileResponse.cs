using SeniorLearnApi.Enums;

namespace SeniorLearnApi.DTOs.Responses;

public class ProfileResponse
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public DateTime MembershipDate { get; set; }
    public List<MemberBulletinListItemResponse> MyBulletins { get; set; }
}
