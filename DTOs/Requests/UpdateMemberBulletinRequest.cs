using SeniorLearnApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SeniorLearnApi.DTOs.Requests;

public class UpdateMemberBulletinRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public MemberBulletinCategory Category { get; set; }
}
