using Microsoft.AspNetCore.Mvc;
using RoomApi.Models;
using MongoDB.Bson;

using System.Threading.Tasks;

namespace RoomApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("register-room")]
        public async Task<ActionResult<Room>> RegisterRoom(Room room)
        {
            var registeredRoom = await _roomService.RegisterAsync(room);
            return CreatedAtAction(nameof(RegisterRoom), new { id = registeredRoom.Location }, registeredRoom);
        }

        [HttpGet("rooms/{location}")]
        public async Task<ActionResult> GetRoomByLocation(string location)
        {
            var room = await _roomService.GetByLocationAsync(location);
            if (room == null)
            {
                return NotFound(); // Room not found
            }
            return Ok(room); // Return room
        }

        [HttpPut("update-room")]
        public async Task<ActionResult<Room>> UpdateRoom(Room room)
        {
            var updatedRoom = await _roomService.UpdateRoomAsync(room);
            if (updatedRoom == null)
            {
                return NotFound(); // Room not found
            }
            return Ok(updatedRoom); // Return updated room
        }

        [HttpDelete("rooms/delete/{id}")]
        public async Task<ActionResult> DeleteRoom(string id)
        {
            await _roomService.DeleteRoomAsync(id);
            return NoContent(); // Successfully deleted
        }

        [HttpGet("get-all-rooms")]
        public async Task<ActionResult> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms); // Return all rooms
        }

        [HttpGet("get-rooms-by/{userId}")]
        public async Task<ActionResult> GetRoomByUser(string userId)
        {
            var room = await _roomService.GetByUserAsync(userId);
            if (room == null)
            {
                return NotFound(); // Room not found
            }
            return Ok(room); // Return room
        }
    }
}