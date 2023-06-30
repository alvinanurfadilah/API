namespace API.DTOs.Rooms;

public class RoomDTO
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }
}
