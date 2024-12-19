using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System;

namespace UserApi.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly MongoClient _mongoClient;

        public UserRepository(IOptions<UserMongoDBSettings> settings)
        {
            ValidateMongoDBSettings(settings.Value);

            _mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = _mongoClient.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>("Users");
        }

        private void ValidateMongoDBSettings(UserMongoDBSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ConnectionString) 
                || string.IsNullOrEmpty(settings.DatabaseName))
            {
                throw new ArgumentException("Invalid MongoDB settings");
            }
        }

        public async Task<User> RegisterAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                await _users.InsertOneAsync(user);
                return user;
            }
            catch (MongoCommandException ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                var result = await _users.ReplaceOneAsync(u => u.Username == user.Username, user);
                return result.IsAcknowledged ? user : null;
            }
            catch (MongoCommandException ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            try
            {
                var result = await _users.DeleteOneAsync(u => u.Username == username);
                if (!result.IsAcknowledged)
                {
                    Console.WriteLine("Delete operation not acknowledged.");
                }
            }
            catch (MongoCommandException ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }
    }
}