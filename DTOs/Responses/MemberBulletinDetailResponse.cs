using SeniorLearnApi.Enums;

namespace SeniorLearnApi.DTOs.Responses;

public class MemberBulletinDetailResponse : BulletinDetailResponseBase
{
    public MemberBulletinCategory Category { get; set; }
}