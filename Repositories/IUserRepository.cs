using System.Threading.Tasks;

namespace UserApi.Models
{
   public interface IUserRepository
{
    Task<User> RegisterAsync(User user);
    Task<User> GetByUsernameAsync(string username);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(string username);
    Task<IEnumerable<User>> GetAllUsersAsync();
}

}
