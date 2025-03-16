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

    public async Task<User?> GetUserById(Guid userId)
    {
        return await _userRepository.GetUserById(userId);
    }

    public async Task<bool> UpsertUser(string tell)
    {
        var user = await _userRepository.GetAllUsers().FirstOrDefaultAsync(u => u.Tell == tell);

        if (user == null)
        {
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                FullName = "",
                Tell = tell,
                VerificationCode = Generator.GenerateVerificationCode().ToString()
            };

            // sms code

            await _userRepository.AddUser(newUser);
        }
        else
        {
            // sms code

            user.VerificationCode = Generator.GenerateVerificationCode().ToString();
            _userRepository.UpdateUser(user);
        }

        return true;
    }

    public async Task<List<UserAddress>> GetUserAddresses(Guid userId)
    {
        var addressList = await _userRepository.GetAllUserAddresses()
            .Where(ua => ua.UserId == userId).ToListAsync();

        return addressList;
    }

    public async Task<bool> AddUserAddress(Guid userId, string address)
    {
        if (!string.IsNullOrEmpty(address))
        {
            return false;
        }

        await _userRepository.AddUserAddress(new UserAddress()
        {
            UserId = userId,
            Address = address
        });

        return true;
    }

    public async Task<UsersListVm> GetUsersListAsync(int pageId, int pageSize, string? filterName, string? filterTell)
    {
        // Fetch paginated users from the repository
        var query = _userRepository.GetAllUsers();

        if (!string.IsNullOrEmpty(filterName))
        {
            query = query.Where(u => u.FullName.Contains(filterName));
        }

        if (!string.IsNullOrEmpty(filterTell))
        {
            query = query.Where(u => u.Tell.Contains(filterTell));
        }

        // pagination
        int totalCount = await query.CountAsync();
        int pageCount = (int)(Math.Ceiling((decimal)totalCount / pageSize));
        if (pageId > pageCount)
        {
            pageId = pageCount;
        }
        int skip = (pageId - 1) * pageSize;
        if (skip < 0)
        {
            skip = 0;
        }

        var users = await query.OrderByDescending(u => u.CreateDate).Skip(skip).Take(pageSize)
            .Select(u => new UserVmForAdmin()
            {
                Id = u.Id,
                FullName = u.FullName,
                Tell = u.Tell,
                SignInDate = u.CreateDate,
                PurchasedSum = 0 // TODO: Calculate
            }).ToListAsync();


        return new UsersListVm()
        {
            Users = users,
            CurrentPage = pageId,
            TotalPages = pageCount
        };
    }

    public async Task<User?> GetUserWithDetails(Guid userId)
    {
        var user = await _userRepository.GetAllUsers()
        .Include(u => u.UserAddresses)
        .FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }

    public async Task<bool> EditUser(EditUserDto editUserDto)
    {
        var user = await _userRepository.GetAllUsers()
        .FirstOrDefaultAsync(u => u.Id == editUserDto.Id);

        if (user == null)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(editUserDto.FullName))
        {
            user.FullName = editUserDto.FullName;
        }
        user.Tell = editUserDto.Tell;

        if (!string.IsNullOrEmpty(editUserDto.Password))
        {
            user.Password = editUserDto.Password;
        }
        user.IsTellVerified = editUserDto.IsTellVerified;
        user.Role = editUserDto.Role;

        if (editUserDto.ProfileImageName != null)
        {
            user.ProfileImageName = IOHelper.StoreNewFile(editUserDto.ProfileImageName, "wwwroot/images/users", user.Id.ToString());
        }

        _userRepository.UpdateUser(user);

        if (editUserDto.Addresses != null)
        {
            // Get current addresses
            var currentAddresses = await _userRepository.GetAllUserAddresses()
                .Where(ua => ua.UserId == editUserDto.Id)
                .ToListAsync();

            // Delete addresses that are not in the new list
            var addressesToDelete = currentAddresses
                .Where(ca => !editUserDto.Addresses.Contains(ca.Address))
                .ToList();
            
            foreach (var address in addressesToDelete)
            {
                _userRepository.DeleteUserAddress(address);
            }

            // Add new addresses that don't exist
            var existingAddresses = currentAddresses.Select(ca => ca.Address).ToList();
            var newAddresses = editUserDto.Addresses
                .Where(a => !existingAddresses.Contains(a))
                .Select(a => new UserAddress
                {
                    UserId = editUserDto.Id,
                    Address = a
                });

            await _userRepository.AddRangeUserAddresses(newAddresses);
        }
        return true;
    }

    public async Task<bool> DeleteUser(Guid userId)
    {
        var user = await _userRepository.GetAllUsers()
        .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        _userRepository.DeleteUser(user);
        return true;
    }
}