using BCrypt.Net;
using System.Threading.Tasks;

namespace UserApi.Models
{public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> RegisterAsync(User user)
    {
        // Hash the password before saving
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        return await _userRepository.RegisterAsync(user);
    }

    public async Task<User> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null; // Invalid credentials
        }
        return user; // Successful login
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        // Optionally, hash the password if it's being changed
        if (!string.IsNullOrEmpty(user.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        }
        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(string username)
    {
        await _userRepository.DeleteUserAsync(username);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }
}

}
