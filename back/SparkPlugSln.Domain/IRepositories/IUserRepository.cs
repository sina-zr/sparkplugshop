using SparkPlugSln.Domain.Entities.User;

namespace SparkPlugSln.Domain.IRepositories;

public interface IUserRepository
{


    #region User

    IQueryable<User> GetAllUsers();
    Task<User?> GetUserById(Guid id);
    Task<User> AddUser(User user);
    Task<IEnumerable<User>> AddRangeUsers(IEnumerable<User> users);
    User UpdateUser(User user);
    IEnumerable<User> UpdateRangeUsers(IEnumerable<User> users);
    bool DeleteUser(User user);
    bool DeleteRangeUsers(IEnumerable<User> users);

    #endregion

    #region UserAddress

    IQueryable<UserAddress> GetAllUserAddresses();

    Task<UserAddress?> GetUserAddressById(ulong id);

    Task<UserAddress> AddUserAddress(UserAddress userAddress);

    Task<IEnumerable<UserAddress>> AddRangeUserAddresses(IEnumerable<UserAddress> userAddresses);

    UserAddress UpdateUserAddress(UserAddress userAddress);

    IEnumerable<UserAddress> UpdateRangeUserAddresses(IEnumerable<UserAddress> userAddresses);

    bool DeleteUserAddress(UserAddress userAddress);

    bool DeleteRangeUserAddresses(IEnumerable<UserAddress> userAddresses);

    #endregion
}