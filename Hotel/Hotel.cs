using System.Collections.Generic;

namespace Hotel
{
    public class Hotel
    {
        public int NumberOfRooms { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
        public int BookingId { get; set; } = 1;

        public Hotel(int number)
        {
            NumberOfRooms = number;

            for (int i = 0; i < NumberOfRooms; i++)
            {
                Rooms.Add(new Room(i));
            }
        }
    }
}
