using SparkPlugSln.Domain.Entities.User;
using SparkPlugSln.Domain.IRepositories;

namespace SparkPlugSln.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PersistenceDbContext _context;

    public UserRepository(PersistenceDbContext context)
    {
        _context = context;
    }

    #region User

    public IQueryable<User> GetAllUsers()
    {
        return _context.Users.AsQueryable();
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> AddUser(User user)
    {
        var result = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<IEnumerable<User>> AddRangeUsers(IEnumerable<User> users)
    {
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
        return users;
    }

    public User UpdateUser(User user)
    {
        var result = _context.Users.Update(user);
        _context.SaveChanges();
        return result.Entity;
    }

    public IEnumerable<User> UpdateRangeUsers(IEnumerable<User> users)
    {
        _context.Users.UpdateRange(users);
        _context.SaveChanges();
        return users;
    }

    public bool DeleteUser(User user)
    {
        try
        {
            _context.Users.Remove(user);
            var changes = _context.SaveChanges();
            if (changes > 0)
            {
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            // Log
            return false;
        }
    }

    public bool DeleteRangeUsers(IEnumerable<User> users)
    {
        try
        {
            _context.Users.RemoveRange(users);
            var changes = _context.SaveChanges();
            if (changes > 0)
            {
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            // Log
            return false;
        }
    }

    #endregion

    #region UserAddress

    public IQueryable<UserAddress> GetAllUserAddresses()
    {
        return _context.UsersAddresses.AsQueryable();
    }

    public async Task<UserAddress?> GetUserAddressById(ulong id)
    {
        return await _context.UsersAddresses.FindAsync(id);
    }

    public async Task<UserAddress> AddUserAddress(UserAddress userAddress)
    {
        await _context.UsersAddresses.AddAsync(userAddress);
        await _context.SaveChangesAsync();
        return userAddress;
    }

    public async Task<IEnumerable<UserAddress>> AddRangeUserAddresses(IEnumerable<UserAddress> userAddresses)
    {
        await _context.UsersAddresses.AddRangeAsync(userAddresses);
        await _context.SaveChangesAsync();
        return userAddresses;
    }

    public UserAddress UpdateUserAddress(UserAddress userAddress)
    {
        _context.UsersAddresses.Update(userAddress);
        _context.SaveChanges();
        return userAddress;
    }

    public IEnumerable<UserAddress> UpdateRangeUserAddresses(IEnumerable<UserAddress> userAddresses)
    {
        _context.UsersAddresses.UpdateRange(userAddresses);
        _context.SaveChanges();
        return userAddresses;
    }

    public bool DeleteUserAddress(UserAddress userAddress)
    {
        try
        {
            _context.UsersAddresses.Remove(userAddress);
            var changes = _context.SaveChanges();
            return changes > 0;
        }
        catch (Exception e)
        {
            // Log exception
            return false;
        }
    }

    public bool DeleteRangeUserAddresses(IEnumerable<UserAddress> userAddresses)
    {
        try
        {
            _context.UsersAddresses.RemoveRange(userAddresses);
            var changes = _context.SaveChanges();
            return changes > 0;
        }
        catch (Exception e)
        {
            // Log exception
            return false;
        }
    }

    #endregion
}