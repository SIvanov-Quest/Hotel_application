using System.Collections.Generic;

namespace Hotel
{
    public class Room
    {
        public int Id { get; set; }
        public List<int> ReservedDays { get; set; } = new List<int>();

        public Room(int roomNumber)
        {
            Id = roomNumber;
        }
    }
}
