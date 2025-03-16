using SparkPlugSln.Domain.Entities.User;
using SparkPlugSln.Domain.Models.User;

namespace SparkPlugSln.Application.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByTell(string tell);
    Task<User?> GetUserById(Guid userId);
    Task<bool> UpsertUser(string tell);
    Task<List<UserAddress>> GetUserAddresses(Guid userId);
    Task<bool> AddUserAddress(Guid userId, string address);
    Task<UsersListVm> GetUsersListAsync(int pageId, int pageSize, string? filterName, string? filterTell);
    Task<User?> GetUserWithDetails(Guid userId);
    Task<bool> EditUser(EditUserDto editUserDto);
    Task<bool> DeleteUser(Guid userId);
}