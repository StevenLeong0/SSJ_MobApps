using SeniorLearnApi.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SeniorLearnApi.Services;

public class UserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? GetCurrentUser()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.User?.Identity?.IsAuthenticated == true ? context.User : null;
    }

    public string? GetUserId()
    {
        var user = GetCurrentUser();
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string? GetUsername()
    {
        var user = GetCurrentUser();
        return user?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public UserRole? GetUserRole()
    {
        var user = GetCurrentUser();
        if (user == null) return null;

        var roleString = user.FindFirst(ClaimTypes.Role)?.Value;
        return Enum.TryParse(roleString, out UserRole role) ? role : null;
    }

    public bool IsAdmin()
    {
        return GetUserRole() == UserRole.Administrator;
    }

    public bool IsOwner(string resourceOwnerId)
    {
        var currentUserId = GetUserId();
        return !string.IsNullOrEmpty(currentUserId) &&
               currentUserId.Equals(resourceOwnerId, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsOwnerOrAdmin(string resourceOwnerId)
    {
        return IsAdmin() || IsOwner(resourceOwnerId);
    }
}
