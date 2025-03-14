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
        return user.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
    }
    
    public static int GetId(this ClaimsPrincipal user)
    {
        var idString = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        int id = int.Parse(idString);
        return id;
    }
}