using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorLearnApi.DTOs.Requests;
using SeniorLearnApi.DTOs.Responses;
using SeniorLearnApi.Services;

namespace SeniorLearnApi.Controllers;

[ApiController]
[Route("api/bulletins/official")]
public class OfficialBulletinController : ControllerBase
{
    private readonly UserContextService _userContextService;
    private readonly OfficialBulletinService _officialBulletinService;

    public OfficialBulletinController(UserContextService userContextService, OfficialBulletinService officialBulletinService)
    {
        _userContextService = userContextService;
        _officialBulletinService = officialBulletinService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<OfficialBulletinListItemResponse>>>> GetAllOfficialBulletins()
    {
        var response = await _officialBulletinService.GetAllOfficialBulletinsAsync();
        return Ok(ApiResponse<List<OfficialBulletinListItemResponse>>.SuccessResponse(response));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<OfficialBulletinDetailResponse>>> GetOfficialBulletinById(string id)
    {
        var response = await _officialBulletinService.GetOfficialBulletinByIdAsync(id);
        if (response == null)
        {
            return NotFound(ApiResponse<OfficialBulletinDetailResponse>.ErrorResponse("Bulletin not found"));
        }
        return Ok(ApiResponse<OfficialBulletinDetailResponse>.SuccessResponse(response));
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ApiResponse<OfficialBulletinDetailResponse>>> CreateOfficialBulletin([FromBody] CreateOfficialBulletinRequest request)
    {
        var adminId = _userContextService.GetUserId();
        var adminName = _userContextService.GetUsername();
        if (string.IsNullOrEmpty(adminId) || string.IsNullOrEmpty(adminName))
        {
            return Unauthorized(ApiResponse<OfficialBulletinDetailResponse>.ErrorResponse("Unable to identify current user"));
        }

        var response = await _officialBulletinService.CreateOfficialBulletinAsync(request, adminId, adminName);
        return StatusCode(201, ApiResponse<OfficialBulletinDetailResponse>.SuccessResponse(response, "Official bulletin created successfully"));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ApiResponse<OfficialBulletinDetailResponse>>> UpdateOfficialBulletin(string id, [FromBody] UpdateOfficialBulletinRequest request)
    {
        var currentUserId = _userContextService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(ApiResponse<OfficialBulletinDetailResponse>.ErrorResponse("Unable to identify current user"));
        }

        var response = await _officialBulletinService.UpdateOfficialBulletinAsync(id, request);
        if (response == null)
        {
            return NotFound(ApiResponse<OfficialBulletinDetailResponse>.ErrorResponse("Bulletin not found"));
        }
        return Ok(ApiResponse<OfficialBulletinDetailResponse>.SuccessResponse(response, "Bulletin updated successfully"));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> DeleteOfficialBulletin(string id)
    {
        var currentUserId = _userContextService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(ApiResponse<bool>.ErrorResponse("Unable to identify current user"));
        }

        var result = await _officialBulletinService.DeleteOfficialBulletinAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse("Bulletin not found"));
        }

        return NoContent();
    }
}
