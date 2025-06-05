using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorLearnApi.DTOs.Requests;
using SeniorLearnApi.DTOs.Responses;
using SeniorLearnApi.Enums;
using SeniorLearnApi.Services;

namespace SeniorLearnApi.Controllers;

[ApiController]
[Route("api/bulletins/member")]
public class MemberBulletinController : ControllerBase
{
    private readonly UserContextService _userContextService;
    private readonly MemberBulletinService _memberBulletinService;

    public MemberBulletinController(UserContextService userContextService, MemberBulletinService memberBulletinService)
    {
        _userContextService = userContextService;
        _memberBulletinService = memberBulletinService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ApiResponse<List<MemberBulletinListItemResponse>>>> GetAllMemberBulletins(
    [FromQuery] MemberBulletinCategory? category = null,
    [FromQuery] string? userId = null)
    {
        var response = await _memberBulletinService.GetMemberBulletinsAsync(category, userId);
        return Ok(ApiResponse<List<MemberBulletinListItemResponse>>.SuccessResponse(response));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<MemberBulletinDetailResponse>>> GetMemberBulletinById(string id)
    {
        var response = await _memberBulletinService.GetMemberBulletinByIdAsync(id);
        if (response == null)
        {
            return NotFound(ApiResponse<MemberBulletinDetailResponse>.ErrorResponse("Bulletin not found"));
        }
        return Ok(ApiResponse<MemberBulletinDetailResponse>.SuccessResponse(response));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<MemberBulletinDetailResponse>>> CreateMemberBulletin([FromBody] CreateMemberBulletinRequest request)
    {
        var memberId = _userContextService.GetUserId();
        var memberName = _userContextService.GetUsername();
        if (string.IsNullOrEmpty(memberId) || string.IsNullOrEmpty(memberName))
        {
            return Unauthorized(ApiResponse<MemberBulletinDetailResponse>.ErrorResponse("Unable to identify current user"));
        }

        var response = await _memberBulletinService.CreateMemberBulletinAsync(request, memberId, memberName);
        return StatusCode(201, ApiResponse<MemberBulletinDetailResponse>.SuccessResponse(response, "Member bulletin created successfully"));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<MemberBulletinDetailResponse>>> UpdateMemberBulletin(string id, [FromBody] UpdateMemberBulletinRequest request)
    {
        var currentUserId = _userContextService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(ApiResponse<MemberBulletinDetailResponse>.ErrorResponse("Unable to identify current user"));
        }

        var response = await _memberBulletinService.UpdateMemberBulletinAsync(id, request, currentUserId);
        if (response == null)
        {
            return NotFound(ApiResponse<MemberBulletinDetailResponse>.ErrorResponse("Bulletin not found or you don't have permission to update it"));
        }
        return Ok(ApiResponse<MemberBulletinDetailResponse>.SuccessResponse(response, "Bulletin updated successfully"));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteMemberBulletin(string id)
    {
        var currentUserId = _userContextService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(ApiResponse<bool>.ErrorResponse("Unable to identify current user"));
        }

        var result = await _memberBulletinService.DeleteMemberBulletinAsync(id, currentUserId);
        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse("Bulletin not found or you don't have permission to delete it"));
        }
        return NoContent();
    }
}
