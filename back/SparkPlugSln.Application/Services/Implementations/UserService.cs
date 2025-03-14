using Microsoft.EntityFrameworkCore;
using SparkPlugSln.Application.Security;
using SparkPlugSln.Application.Services.Interfaces;
using SparkPlugSln.Domain.Entities.User;
using SparkPlugSln.Domain.IRepositories;
using SparkPlugSln.Domain.Models.User;

namespace SparkPlugSln.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User?> GetUserByTell(string tell)
    {
        var user = await _userRepository.GetAllUsers().FirstOrDefaultAsync(u => u.Tell == tell);
        return user;
    }

    public async Task<bool> CreateUser(string tell)
    {
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "",
            Tell = tell,
            VerificationCode = Generator.GenerateVerificationCode().ToString()
        };

        await _userRepository.AddUser(newUser);
        return true;
    }
}