using API.Contracts;
using API.DTOs.Rooms;
using API.DTOs.Universities;
using API.Models;
using API.Repositories;

namespace API.Services;

public class RoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public IEnumerable<RoomDTO>? GetRoom()
    {
        var rooms = _roomRepository.GetAll();
        if (!rooms.Any())
        {
            return null; // No rooms found
        }

        var toDTO = rooms.Select(room => new RoomDTO
        {
            Guid = room.Guid,
            Name = room.Name,
            Floor = room.Floor,
            Capacity = room.Capacity
        }).ToList();

        return toDTO; // Rooms found
    }

    public RoomDTO? GetRoom(Guid guid)
    {
        var room = _roomRepository.GetByGuid(guid);
        if (room is null)
        {
            return null; // No Rooms found
        }

        var toDTO = new RoomDTO
        {
            Guid = room.Guid,
            Name = room.Name,
            Floor = room.Floor,
            Capacity = room.Capacity
        };

        return toDTO; // Rooms found
    }

    public RoomDTO? CreateRoom(NewRoomDTO newRoomDTO)
    {
        var room = new Room
        {
            Name = newRoomDTO.Name,
            Floor = newRoomDTO.Floor,
            Capacity = newRoomDTO.Capacity,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdRoom = _roomRepository.Create(room);
        if (createdRoom is null)
        {
            return null; // Room not created
        }

        var toDTO = new RoomDTO
        {
            Guid = createdRoom.Guid,
            Name = createdRoom.Name,
            Floor = createdRoom.Floor,
            Capacity = createdRoom.Capacity
        };

        return toDTO; // Room created
    }

    public int UpdateRoom(RoomDTO roomDTO)
    {
        var isExist = _roomRepository.IsExist(roomDTO.Guid);
        if (!isExist)
        {
            return -1; // Room not found
        }

        var getRoom = _roomRepository.GetByGuid(roomDTO.Guid);

        var room = new Room
        {
            Guid = roomDTO.Guid,
            Name = roomDTO.Name,
            Floor = roomDTO.Floor,
            Capacity = roomDTO.Capacity,
            ModifiedDate = DateTime.Now,
            CreatedDate = getRoom!.CreatedDate
        };

        var isUpdate = _roomRepository.Update(room);
        if (!isUpdate)
        {
            return 0; // Room not found
        }
        return 1;
    }

    public int DeleteRoom(Guid guid)
    {
        var isExist = _roomRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Room not found
        }

        var room = _roomRepository.GetByGuid(guid);
        var isDelete = _roomRepository.Delete(room!);
        if (!isDelete)
        {
            return 0; // Room not deleted
        }

        return 1;
    }
}
