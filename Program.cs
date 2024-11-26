using Microsoft.Extensions.Configuration;
using System.IO;

namespace movie_tickets
{
    class Booking
    {
        private string _name;
        private int _row;
        private int _col;

        public string Name {
            get { return _name; } 
            set { _name = value; }
        }

        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public int Col
        {
            get { return _col; }
            set { _col = value; }
        }

        public Booking(string name, int row, int col)
        {
            _name = name;
            _row = row;
            _col = col;
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] seating = new int[10, 10];
            int availableCount = 0;
            int bookedCount = 0;
            //Booking[] bookings = new Booking[100];
            List<Booking> bookings = new List<Booking> { };

            void displaySeating() 
            {
                Console.WriteLine("---Seating Arrangement---");
                Console.WriteLine();
                for (int x = 0; x < seating.GetLength(1); x++)
                {
                    Console.Write($"  {x}  ");
                }
                Console.WriteLine();

                bookedCount = 0;
                availableCount = 0;

                for (int i = 0; i < seating.GetLength(0); i++)
                {

                    Console.Write($"{i}");
                    for (int j = 0; j < seating.GetLength(1); j++)
                    {
                        if (seating[i, j] != 0)
                        {
                            Console.Write("[ X ]");
                            bookedCount++;
                        }
                        else
                        {
                            Console.Write("[ O ]");
                            availableCount++;
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"\nAavailable (O): {availableCount} Seats \nUnavailable (X): {bookedCount} Seats");
            }

            void bookTickets()
            {
                do
                {
                    int option = 0;

                    displaySeating();

                    try
                    {
                        Console.WriteLine("Menu");
                        Console.WriteLine("1. Book a ticket");
                        Console.WriteLine("2. Cancel a booking (Admin)");
                        Console.WriteLine("3. View bookings (Admin)");
                        Console.WriteLine("4. Exit");
                        Console.Write("Please select an option:");
                        option = int.Parse(Console.ReadLine());

                    } catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Press any key to continue!");
                        Console.ReadKey();
                        continue;
                    }

                    switch (option)
                    {
                        case 1:
                            book();
                            break;
                        case 2:
                            cancelBooking();
                            break;
                        case 3:
                            viewBookings();
                            break;
                        case 4:
                            Console.WriteLine("Exiting...");
                            return;
                        default:
                            Console.WriteLine("Invalid input press any key to continue!");
                            Console.ReadKey();
                            break;
                    }

                } while ( true );
            }

            void viewBookings()
            {

                try
                {
                    Console.WriteLine("Enter admin credentials to view bookings.");
                    Console.Write("User name:");
                    string username = Console.ReadLine();

                    Console.Write("Password:");
                    string password = Console.ReadLine();

                    var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
                    var configuration = builder.Build();

                    if (username != configuration["username"] || password != configuration["password"])
                    {
                        Console.WriteLine("Invalid user name or password!");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine("|     Name     |     Col     |     Row     |");
                    Console.WriteLine("|--------------|-------------|-------------|");

                    foreach (Booking item in bookings)
                    {
                        Console.WriteLine($"| {item.Name,-12} | {item.Col,-11} | {item.Row,-11} |");
                    }
                    Console.WriteLine("Press any key to continue!");
                    Console.ReadKey();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to continue!");
                    Console.ReadKey();
                }

            }

            void book()
            {
                try
                {
                    if (availableCount <= 0)
                    {
                        Console.WriteLine("HOUSE FULL!");
                        return;
                    }
                    Console.WriteLine("Enter your name: ");
                    string name = Console.ReadLine();

                    if (name == null)
                    {
                        Console.WriteLine("Please provide a valid name!");
                        Console.WriteLine("Press nay key to continue...");
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine("Select the row and Column number of the seat to book");

                    Console.Write("Row number: ");
                    int row = int.Parse(Console.ReadLine());
                    Console.Write("Column number: ");
                    int col = int.Parse(Console.ReadLine());

                    if (seating.GetLength(0) < col || seating.GetLength(1) < row)
                    {
                        Console.WriteLine("Invalid seating number!");
                        Console.WriteLine("Press any key to continue!");
                        Console.ReadKey();
                        return;
                    }

                    if (seating[row, col] != 0)
                    {
                        Console.WriteLine($"[{row}, {col}] is not available for book!");
                        Console.ReadKey();
                    }
                    else
                    {
                        seating[row, col] = 1;
                        Console.WriteLine($"[{row}, {col}] is booked");
                        Console.WriteLine("Press any key to continue!");

                        Booking newBooking = new Booking(name, row, col);
                        bookings.Add(newBooking);
                        Console.ReadKey();
                    }
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to continue!");
                    Console.ReadKey();
                }

            }

            void cancelBooking()
            {
                try
                {
                    Console.WriteLine("Enter admin credentials to cancel booking.");
                    Console.Write("User name:");
                    string username = Console.ReadLine();

                    Console.Write("Password:");
                    string password = Console.ReadLine();

                    var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
                    var configuration = builder.Build();

                    if (username != configuration["username"] || password != configuration["password"])
                    {
                        Console.WriteLine("Invalid user name or password!");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine("Select the row and Column number of the seat to cancel booking");

                    Console.Write("Row number: ");
                    int row = int.Parse(Console.ReadLine());
                    Console.Write("Column number: ");
                    int col = int.Parse(Console.ReadLine());

                    if (seating[row, col] == 0)
                    {
                        Console.WriteLine($"[{row}, {col}] is not booked to cancel!");
                        Console.ReadKey();
                    }
                    else
                    {
                        seating[row, col] = 0;
                        Console.WriteLine($"[{row}, {col}] booking is canceled");
                        Console.WriteLine("Press any key to continue!");
                        Console.ReadKey();
                    }

                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to continue!");
                    Console.ReadKey();
                }
            }

            void bookingSystem()
            {
                bookTickets();
            }

            bookingSystem();
        }
    }
}
