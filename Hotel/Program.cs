using System;
using System.Collections.Generic;
using System.IO;

namespace Hotel
{
    class Program
    {
        private static Reservation Reservation { get; } = new Reservation();
        static void Main(string[] args)
        {
            Console.WriteLine("Enter size of your hotel");
            var size = Console.ReadLine();

            if (CheckIfNumber(size))
            {
                if (Convert.ToInt32(size) > 1000)
                {
                    Console.WriteLine("Size cannot exceed 1000 rooms");
                    return;
                }

                var hotel = new Hotel(Convert.ToInt32(size));

                Booking(hotel, new List<Tuple<string, int, int, string>>());
            }

            //TestCases(); //Uncomment for test cases 

            Console.WriteLine("Press any key to close application");
            //PrintToFile(); //Uncomment if you want to print test cases into txt file 
            Console.ReadLine();
        }

        private static void Booking(Hotel hotel, List<Tuple<string, int, int, string>> bookingsList)
        {
            Console.WriteLine("Please choose a room you wish to book");
            var roomId = Console.ReadLine();

            Console.WriteLine("Please enter start day");
            var startDay = Console.ReadLine();

            Console.WriteLine("Please enter end day");
            var endDay = Console.ReadLine();

            if (CheckIfNumber(roomId) && CheckIfNumber(startDay) && CheckIfNumber(endDay))
            {
                bookingsList.Add(Reservation.BookARoom(Convert.ToInt32(roomId), Tuple.Create(Convert.ToInt32(startDay), Convert.ToInt32(endDay)), hotel));
                PrintBookingsTable(bookingsList);
            }

            Console.WriteLine("Do you wish to make another booking? (Y/N)");
            var answer = Console.ReadLine().ToLower();

            if (answer == "y")
            {
				Booking(hotel, bookingsList);
            }
            else
            {
				PrintBookingsTable(bookingsList, true);
                PrintRoomsTable(hotel.Rooms);
			}
        }

        private static bool CheckIfNumber(string n)
        {
            if (int.TryParse(n, out int number))
            {                
                return true;
            }

            Console.WriteLine($"{n} is not a number");
            return false;
        }

        private static void TestCases()
        {
            var casesList = new List<Tuple<string, int, int, string>>();

            var testCase2 = new Hotel(3);
            var testCase3 = new Hotel(3);
            var testCase4 = new Hotel(3);
            var testCase5 = new Hotel(2);

            //1a1b: Requests outside our planning period are declined (Size=1)
            Console.WriteLine("**TEST CASE 1a**");
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(-4, 2), new Hotel(1)));
			PrintBookingsTable(casesList, true);

            //1b: Requests outside our planning period are declined (Size=1)
            Console.WriteLine("**TEST CASE 1b**");
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(200, 400), new Hotel(1)));
			PrintBookingsTable(casesList, true);


            //2: Requests are accepted (Size=3)
            Console.WriteLine("**TEST CASE 2**");
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(0, 5), testCase2));
            casesList.Add(Reservation.BookARoom(1, Tuple.Create(7, 13), testCase2));
            casesList.Add(Reservation.BookARoom(2, Tuple.Create(3, 9), testCase2));
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(5, 7), testCase2));
            casesList.Add(Reservation.BookARoom(1, Tuple.Create(6, 6), testCase2));
            casesList.Add(Reservation.BookARoom(1, Tuple.Create(0, 4), testCase2));
			PrintBookingsTable(casesList, true);

            //3: Requests are declined (Size=3)
            Console.WriteLine("**TEST CASE 3**");
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(1, 3), testCase3));
            casesList.Add(Reservation.BookARoom(1, Tuple.Create(2, 5), testCase3));
            casesList.Add(Reservation.BookARoom(2, Tuple.Create(1, 9), testCase3));
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(0, 15), testCase3));
			PrintBookingsTable(casesList, true);

            //4: Requests can be accepted after a decline (Size=3)
            Console.WriteLine("**TEST CASE 4**");
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(1, 3), testCase4));
            casesList.Add(Reservation.BookARoom(1, Tuple.Create(0, 15), testCase4));
            casesList.Add(Reservation.BookARoom(2, Tuple.Create(1, 9), testCase4));
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(2, 5), testCase4));
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(4, 9), testCase4));
			PrintBookingsTable(casesList, true);

            //5: Complex Requests (Size=2)
            Console.WriteLine("**TEST CASE 5**");
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(1, 3), testCase5));
            casesList.Add(Reservation.BookARoom(1, Tuple.Create(0, 4), testCase5));
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(2, 3), testCase5));
            casesList.Add(Reservation.BookARoom(1, Tuple.Create(5, 5), testCase5));
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(4, 10), testCase5));
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(10, 10), testCase5));
            casesList.Add(Reservation.BookARoom(1, Tuple.Create(6, 7), testCase5));
            casesList.Add(Reservation.BookARoom(0, Tuple.Create(8, 10), testCase5));
            casesList.Add(Reservation.BookARoom(1, Tuple.Create(8, 9), testCase5));
			PrintBookingsTable(casesList, true);
        }

        private static void PrintBookingsTable(List<Tuple<string, int, int, string>> list, bool clear = false)
        {
            var t = new TablePrinter("Booking Id", "Start day", "End day", "Status");
            foreach (var items in list)
            {
                t.AddRow(items.Item1, items.Item2, items.Item3, items.Item4);
            }
            t.Print();

			if (clear)
			{
				list.Clear();
            }
		}

		private static void PrintRoomsTable(List<Room> rooms)
		{
			var t = new TablePrinter("Room Id", "Reserved days");
			foreach (var room in rooms)
			{
				t.AddRow($"Room {room.Id}", string.Join(", ", room.ReservedDays));
			}
			t.Print();
		}

        private static void PrintToFile()
        {
            TextWriter oldOut = Console.Out;

			if (File.Exists("./Reservations.txt"))
			{
                File.Delete("./Reservations.txt");
			}

            FileStream ostrm = new FileStream("./Reservations.txt", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(ostrm);

            Console.SetOut(writer);
            TestCases();
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
        }
    }
}
