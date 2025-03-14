using Microsoft.AspNetCore.Http;
using SparkPlugSln.Domain.Enums;

namespace SparkPlugSln.Domain.Models.User;

public class UserVmForAdmin
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string Tell { get; set; }
    public ulong PurchasedSum { get; set; }
    public DateTime SignInDate { get; set; }
}

public class UsersListVm
{
    public List<UserVmForAdmin> Users { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class UserDetailsVm
{
    public string FullName { get; set; } = "";
    public string Tell { get; set; }
    public string? Password { get; set; }
    public string VerificationCode { get; set; }
    public bool IsTellVerified { get; set; }
    public UserRoles Role { get; set; } = UserRoles.User;
    public string? ProfileImageName { get; set; }
    public List<string> Addresses { get; set; }
}

public class EditUserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
    public string Tell { get; set; }
    public string? Password { get; set; }
    public string VerificationCode { get; set; }
    public bool IsTellVerified { get; set; }
    public UserRoles Role { get; set; } = UserRoles.User;
    public IFormFile? ProfileImageName { get; set; }
    public List<string> Addresses { get; set; }
}