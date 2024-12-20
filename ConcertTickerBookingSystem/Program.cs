using System;
using System.Collections.Generic;
using System.Linq;

interface IConcert
{
    string Name { get; set; }
    string Date { get; set; }
    string Location { get; set; }
    int AvailableSeats { get; set; }
    decimal TicketPrice { get; set; }
}

class Concert : IConcert
{
    public string Name { get; set; }
    public string Date { get; set; }
    public string Location { get; set; }
    public int AvailableSeats { get; set; }
    public decimal TicketPrice { get; set; }

    public virtual string GetDetails()
    {
        return $"{Name} - {Date} - {Location} - Miejsca: {AvailableSeats} - Cena: {TicketPrice} zł";
    }
}

class VIPConcert : Concert
{
    public string MeetingDetails { get; set; }

    public override string GetDetails()
    {
        return base.GetDetails() + $" - VIP Spotkanie: {MeetingDetails}";
    }
}

class OnlineConcert : Concert
{
    public string Platform { get; set; }

    public override string GetDetails()
    {
        return base.GetDetails() + $" - Platforma: {Platform}";
    }
}

class PrivateConcert : Concert
{
    public string InvitationOnly { get; set; }

    public override string GetDetails()
    {
        return base.GetDetails() + $" - Tylko na zaproszenie: {InvitationOnly}";
    }
}

class BookingSystem
{
    private List<IConcert> concerts = new();

    public void AddConcert()
    {
        Console.WriteLine("Typ koncertu: Regular, VIP, Online, Private");
        string type = Console.ReadLine()?.ToLower();

        Concert concert = type switch
        {
            "vip" => new VIPConcert(),
            "online" => new OnlineConcert(),
            "private" => new PrivateConcert(),
            _ => new Concert(),
        };

        Console.Write("Nazwa koncertu: ");
        concert.Name = Console.ReadLine();
        Console.Write("Data koncertu (DD-MM-YYYY): ");
        concert.Date = Console.ReadLine();
        Console.Write("Lokalizacja: ");
        concert.Location = Console.ReadLine();
        Console.Write("Liczba miejsc: ");
        concert.AvailableSeats = int.Parse(Console.ReadLine());
        Console.Write("Cena biletu: ");
        concert.TicketPrice = decimal.Parse(Console.ReadLine());

        if (concert is VIPConcert vip)
        {
            Console.Write("Szczegóły spotkania VIP: ");
            vip.MeetingDetails = Console.ReadLine();
        }
        else if (concert is OnlineConcert online)
        {
            Console.Write("Platforma koncertu online: ");
            online.Platform = Console.ReadLine();
        }
        else if (concert is PrivateConcert privateConcert)
        {
            Console.Write("Szczegóły zaproszenia: ");
            privateConcert.InvitationOnly = Console.ReadLine();
        }

        concerts.Add(concert);
        Console.WriteLine("Koncert został dodany!");
    }

    public void DisplayConcerts()
    {
        if (concerts.Count == 0)
        {
            Console.WriteLine("Brak dostępnych koncertów.");
        }
        else
        {
            foreach (var concert in concerts)
            {
                Console.WriteLine(concert is Concert c ? c.GetDetails() : null);
            }
        }
    }

    public void SearchConcerts()
    {
        Console.WriteLine("Wyszukiwanie koncertów:");
        Console.WriteLine("1 - Data");
        Console.WriteLine("2 - Lokalizacja");
        Console.WriteLine("3 - Cena maksymalna");

        string choice = Console.ReadLine();
        IEnumerable<IConcert> results = new List<IConcert>();

        switch (choice)
        {
            case "1":
                string date = ReadInput("Podaj datę (DD-MM-YYYY): ");
                results = concerts.Where(c => c.Date == date);
                break;

            case "2":
                string location = ReadInput("Podaj lokalizację: ");
                results = concerts.Where(c => c.Location.Contains(location, StringComparison.OrdinalIgnoreCase));
                break;

            case "3":
                if (decimal.TryParse(ReadInput("Podaj maksymalną cenę: "), out decimal maxPrice))
                {
                    results = concerts.Where(c => c.TicketPrice <= maxPrice);
                }
                else
                {
                    Console.WriteLine("Niepoprawna cena. Spróbuj ponownie.");
                    return;
                }
                break;

            default:
                Console.WriteLine("Nieprawidłowy wybór.");
                return;
        }

        if (results.Any())
        {
            Console.WriteLine("Znalezione koncerty:");
            foreach (var concert in results)
            {
                Console.WriteLine(concert is Concert c ? c.GetDetails() : null);
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono pasujących koncertów.");
        }
    }

    public void BookTicket()
    {
        var concertName = ReadInput("Podaj nazwę koncertu: ");
        var concert = concerts.FirstOrDefault(c => c.Name == concertName);

        if (concert == null)
        {
            Console.WriteLine("Koncert nie został znaleziony.");
            return;
        }

        int seatsToBook = int.Parse(ReadInput("Podaj liczbę miejsc do rezerwacji: "));
        if (seatsToBook <= concert.AvailableSeats)
        {
            concert.AvailableSeats -= seatsToBook;
            Console.WriteLine($"Zarezerwowano {seatsToBook} miejsc. Pozostało {concert.AvailableSeats} wolnych.");
            if (concert.AvailableSeats <= 10)
            {
                Console.WriteLine("Uwaga! Pozostało mniej niż 10 miejsc!");
            }
        }
        else
        {
            Console.WriteLine("Nie ma wystarczającej liczby miejsc.");
        }
    }

    public void CancelBooking()
    {
        var concertName = ReadInput("Podaj nazwę koncertu: ");
        var concert = concerts.FirstOrDefault(c => c.Name == concertName);

        if (concert == null)
        {
            Console.WriteLine("Koncert nie został znaleziony.");
            return;
        }

        int seatsToCancel = int.Parse(ReadInput("Podaj liczbę miejsc do anulowania: "));
        concert.AvailableSeats += seatsToCancel;
        Console.WriteLine($"Anulowano {seatsToCancel} rezerwacji. Dostępne miejsca: {concert.AvailableSeats}.");
    }

    private string ReadInput(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
}

class Program
{
    static void Main(string[] args)
    {
        BookingSystem system = new();
        while (true)
        {
            Console.WriteLine("\nWybierz opcję:");
            Console.WriteLine("1 - Dodaj koncert");
            Console.WriteLine("2 - Wyświetl koncerty");
            Console.WriteLine("3 - Wyszukaj koncerty");
            Console.WriteLine("4 - Zarezerwuj bilet");
            Console.WriteLine("5 - Anuluj rezerwację");
            Console.WriteLine("6 - Wyjdź");

            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    system.AddConcert();
                    break;
                case "2":
                    system.DisplayConcerts();
                    break;
                case "3":
                    system.SearchConcerts();
                    break;
                case "4":
                    system.BookTicket();
                    break;
                case "5":
                    system.CancelBooking();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Niepoprawny wybór. Spróbuj jeszcze raz.");
                    break;
            }
        }
    }
}
