using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorLearnApi.DTOs.Requests;
using SeniorLearnApi.DTOs.Responses;
using SeniorLearnApi.Services;

namespace SeniorLearnApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserContextService _userContextService;
    private readonly AuthService _authService;

    public AuthController(UserContextService userContextService, AuthService authService)
    {
        _userContextService = userContextService;
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request)
    {
        var user = await _authService.RegisterAsync(request);
        if (user == null)
        {
            return Conflict(ApiResponse<AuthResponse>.ErrorResponse("Username or email already exists"));
        }

        var authResponse = await _authService.CreateAuthResponseAsync(user);
        return StatusCode(201, ApiResponse<AuthResponse>.SuccessResponse(authResponse, "Registration successful"));
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> SignIn([FromBody] SignInRequest request)
    {
        var user = await _authService.SignInAsync(request);
        if (user == null)
        {
            return Unauthorized(ApiResponse<AuthResponse>.ErrorResponse("Invalid username or password"));
        }

        var authResponse = await _authService.CreateAuthResponseAsync(user);
        return Ok(ApiResponse<AuthResponse>.SuccessResponse(authResponse, "Sign In successful"));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var authResponse = await _authService.RefreshTokenAsync(request);
        if (authResponse == null)
        {
            return Unauthorized(ApiResponse<AuthResponse>.ErrorResponse("Invalid or expired refresh token"));
        }
        return Ok(ApiResponse<AuthResponse>.SuccessResponse(authResponse, "Token refreshed successfully"));
    }

    [HttpPost("sign-out")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<bool>>> SignOut([FromBody] SignOutRequest request)
    {
        var currentUserId = _userContextService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(ApiResponse<bool>.ErrorResponse("Unable to identify current user"));
        }

        var result = await _authService.SignOutAsync(currentUserId, request.RefreshToken);
        if (!result)
        {
            return BadRequest(ApiResponse<bool>.ErrorResponse("Invalid refresh token or token does not belong to user"));
        }

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Sign out successful"));
    }
}
