using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System;
using MongoDB.Bson;

namespace RoomApi.Models
{
   public class RoomRepository : IRoomRepository
{
    private readonly IMongoCollection<Room> _rooms;
    private readonly MongoClient _mongoClient;

    public RoomRepository(IOptions<RoomMongoDBSettings> settings)
    {
        if (settings == null || settings.Value == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        ValidateMongoDBSettings(settings.Value);

        _mongoClient = new MongoClient(settings.Value.ConnectionString);
        var database = _mongoClient.GetDatabase(settings.Value.DatabaseName);
        _rooms = database.GetCollection<Room>("Rooms");
    }

    private void ValidateMongoDBSettings(RoomMongoDBSettings settings)
    {
        if (string.IsNullOrEmpty(settings.ConnectionString) 
            || string.IsNullOrEmpty(settings.DatabaseName))
        {
            throw new ArgumentException("Invalid MongoDB settings");
        }
    }

    public async Task<Room> RegisterAsync(Room room)
    {
        if (room == null)
        {
            throw new ArgumentNullException(nameof(room));
        }

        try
        {
            await _rooms.InsertOneAsync(room);
            return room;
        }
        catch (MongoCommandException ex)
        {
            // Handle MongoDB exceptions
            Console.WriteLine($"Error registering room: {ex.Message}");
            throw;
        }
    }

    public async Task<Room> GetByLocationAsync(string location)
    {
        if (string.IsNullOrEmpty(location))
        {
            throw new ArgumentNullException(nameof(location));
        }

        return await _rooms.Find(r => r.Location == location).FirstOrDefaultAsync();
    }

    public async Task<Room> UpdateRoomAsync(Room room)
    {
        if (room == null)
        {
            throw new ArgumentNullException(nameof(room));
        }

        try
        {
            var result = await _rooms.ReplaceOneAsync(r => r.Id == room.Id, room);
            return result.IsAcknowledged ? room : null;
        }
        catch (MongoCommandException ex)
        {
            Console.WriteLine($"Error updating room: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteRoomAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        try
        {
            var result = await _rooms.DeleteOneAsync(r => r.Id == ObjectId.Parse(id));
            if (!result.IsAcknowledged)
            {
                Console.WriteLine("Delete operation not acknowledged.");
            }
        }
        catch (MongoCommandException ex)
        {
            Console.WriteLine($"Error deleting room: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await _rooms.Find(_ => true).ToListAsync();
    }

    public async Task<Room> GetByUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException(nameof(userId));
        }

        return await _rooms.Find(r => r.UserId == userId).FirstOrDefaultAsync();
    }
}
}