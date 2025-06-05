using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorLearnApi.DTOs.Requests;
using SeniorLearnApi.DTOs.Responses;
using SeniorLearnApi.Services;

namespace SeniorLearnApi.Controllers;

[ApiController]
[Route("api/profile")]
public class UserProfileController : ControllerBase
{
    private readonly UserContextService _userContextService;
    private readonly UserProfileService _userProfileService;
    private readonly UserSettingService _userSettingService;

    public UserProfileController(UserContextService userContextService, UserProfileService userProfileService, UserSettingService userSettingService)
    {
        _userContextService = userContextService;
        _userProfileService = userProfileService;
        _userSettingService = userSettingService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ApiResponse<ProfileResponse>>> GetProfile()
    {
        var currentUserId = _userContextService.GetUserId();
        var response = await _userProfileService.GetUserProfileAsync(currentUserId);

        if (response == null)
        {
            return NotFound(ApiResponse<ProfileResponse>.ErrorResponse("User not found"));
        }

        return Ok(ApiResponse<ProfileResponse>.SuccessResponse(response));
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<ApiResponse<ProfileResponse>>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var currentUserId = _userContextService.GetUserId();
        var response = await _userProfileService.UpdateUserProfileAsync(currentUserId, request);

        if (response == null)
        {
            return NotFound(ApiResponse<ProfileResponse>.ErrorResponse("User not found"));
        }

        return Ok(ApiResponse<ProfileResponse>.SuccessResponse(response, "Profile updated successfully"));
    }

    [HttpGet("settings")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<SettingsResponse>>> GetSettings()
    {
        var currentUserId = _userContextService.GetUserId();
        var response = await _userSettingService.GetUserSettingsAsync(currentUserId);

        if (response == null)
        {
            return NotFound(ApiResponse<SettingsResponse>.ErrorResponse("User settings not found"));
        }

        return Ok(ApiResponse<SettingsResponse>.SuccessResponse(response));
    }

    [HttpPut("settings")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<SettingsResponse>>> UpdateSettings([FromBody] UpdateSettingsRequest request)
    {
        var currentUserId = _userContextService.GetUserId();
        var response = await _userSettingService.UpdateUserSettingsAsync(currentUserId, request);

        if (response == null)
        {
            return NotFound(ApiResponse<SettingsResponse>.ErrorResponse("User not found"));
        }

        return Ok(ApiResponse<SettingsResponse>.SuccessResponse(response, "Settings updated successfully"));
    }
}
