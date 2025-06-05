using SeniorLearnApi.Enums;

namespace SeniorLearnApi.DTOs.Responses;

public class MemberBulletinListItemResponse : BulletinListItemResponseBase
{
    public MemberBulletinCategory Category { get; set; }
}