using SparkPlugSln.Domain.Entities.User;
using SparkPlugSln.Domain.Models.User;

namespace SparkPlugSln.Application.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByTell(string tell);
    Task<bool> CreateUser(string tell);
}