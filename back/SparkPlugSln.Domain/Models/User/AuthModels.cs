using System.ComponentModel.DataAnnotations;
using SparkPlugSln.Domain.Enums;

namespace SparkPlugSln.Domain.Models.User;

public class LoginDto
{
    [Required]
    public string Tell { get; set; }
    [Required]
    public string Code { get; set; }
}

public class LoginResponseDto
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public string Tell { get; set; }
    public string? FullName { get; set; }
    public UserRoles Role { get; set; }
}