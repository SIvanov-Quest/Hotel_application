using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotel
{
    public class Reservation
    {
        public Tuple<string, int, int, string> BookARoom(int? roomId, Tuple<int, int> startEndDays, Hotel hotel)
        {
			if (!ValidateInputDays(startEndDays.Item1, startEndDays.Item2))
			{
				return PrintBooking(hotel.BookingId++, startEndDays, "Declined");
			}

			if (roomId != null)
			{
				var room = CheckIfRoomExists(roomId, hotel);
				if (room == null)
				{
					return PrintBooking(hotel.BookingId++, startEndDays, "Declined (Room with this Id does not exist)");
				}

				return PrintBooking(hotel.BookingId++, startEndDays, ResolveReservation(room, startEndDays));
            }

			return BookFirstAvailableRoom(startEndDays, hotel);
		}

        public string ResolveReservation(Room room, Tuple<int, int> reservedDays)
        {
            if (room.ReservedDays.Count > 0)
            {
                foreach (var day in Enumerable.Range(reservedDays.Item1, reservedDays.Item2 - reservedDays.Item1))
                {
                    if (CheckAvailability(room.ReservedDays, day))
                    {
                        return "Declined";
                    }
                }
            }

            room.ReservedDays.AddRange(Enumerable.Range(reservedDays.Item1, reservedDays.Item2 - reservedDays.Item1));

            return "Accepted";
        }

        private Room CheckIfRoomExists(int? roomId, Hotel hotel)
        {
			return hotel.Rooms.Where(x => x.Id == roomId)
				.FirstOrDefault();
        }

        private bool CheckAvailability(List<int> reservedDays, int day)
        {
            return reservedDays.Contains(day);
        }

        private bool ValidateInputDays(int start, int end)
        {
            return start < 0 || end > 365 || start > end ? false : true;
        }

        private Tuple<string, int, int, string> PrintBooking(int id, Tuple<int, int> reservedDays, string status)
        {
            return Tuple.Create($"Booking {id.ToString()}", reservedDays.Item1, reservedDays.Item2, status);
        }

		public Tuple<string, int, int, string> BookFirstAvailableRoom(Tuple<int, int> days, Hotel hotel, bool checkDays = false)
		{
			string status = "Declined";

			if (checkDays = true && !ValidateInputDays(days.Item1, days.Item2))
			{
				return PrintBooking(hotel.BookingId++, days, "Declined");
			}

            foreach (var room in hotel.Rooms)
			{
				status = ResolveReservation(room, days);

                if (status == "Accepted")
				{
                    break;
				}
			}

            return PrintBooking(hotel.BookingId++, days, status);
        }
    }
}
