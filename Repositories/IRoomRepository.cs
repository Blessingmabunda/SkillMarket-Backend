using System.Threading.Tasks;
using MongoDB.Bson;

namespace RoomApi.Models
{
public interface IRoomRepository
{
    Task<Room> RegisterAsync(Room room);
    Task<Room> GetByLocationAsync(string location);
    Task<Room> UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(string id);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<Room> GetByUserAsync(string userId);
}

}
