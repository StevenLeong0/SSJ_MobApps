using SeniorLearnApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SeniorLearnApi.DTOs.Requests;

public class CreateMemberBulletinRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public MemberBulletinCategory Category { get; set; }
}
