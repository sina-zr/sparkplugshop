using System.Security.Claims;
using SparkPlugSln.Domain.Enums;

namespace SparkPlugSln.Application.Helpers;

public static class HttpContextHelper
{
    public static bool CheckUserRole(this ClaimsPrincipal user,  UserRoles role)
    {
        var userRoleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        if (userRoleClaim != null && int.TryParse(userRoleClaim, out int userRole) && userRole <= (int)role)
        {
            return true;
        }

        return false;
    }

    public static string GetName(this ClaimsPrincipal user)
    {
        return user.FindFirst("FullName")?.Value;
    }
    
    public static Guid GetId(this ClaimsPrincipal user)
    {
        var idString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var id = Guid.Parse(idString);
        return id;
    }
}