using System;
using System.Collections.Generic;

namespace AirTicketsApp
{
    class Airport
    {
        private string code;
        private string name;

        public Airport(string code, string name)
        {
            this.code = code;
            this.name = name;
        }

        public string Code { get { return code; } }
        public string Name { get { return name; } }
    }

    class Flight
    {
        private string number;
        private string fromCode;
        private string toCode;
        private decimal price;
        private DateTime departureTime;

        public Flight(string number, string fromCode, string toCode, decimal price, DateTime t)
        {
            this.number = number;
            this.fromCode = fromCode;
            this.toCode = toCode;
            this.price = price;
            this.departureTime = t;
        }

        public string Number { get { return number; } }
        public string FromCode { get { return fromCode; } }
        public string ToCode { get { return toCode; } }
        public decimal Price { get { return price; } set { price = value; } }
        public DateTime DepartureTime { get { return departureTime; } set { departureTime = value; } }
    }

    class Customer
    {
        private int id;
        private string name;

        public Customer(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
    }

    class Ticket
    {
        public int Id;
        public string FlightNumber;
        public int CustomerId;
        public decimal SoldPrice;
        public DateTime SaleDate;

        public Ticket(int id, string flightNumber, int customerId, decimal soldPrice, DateTime saleDate)
        {
            Id = id;
            FlightNumber = flightNumber;
            CustomerId = customerId;
            SoldPrice = soldPrice;
            SaleDate = saleDate;
        }
    }

    class Airlines
    {
        static Dictionary<string, Airport> airports;
        static Dictionary<string, Flight> flights;
        static Dictionary<int, Customer> customers;
        static Dictionary<int, Ticket> tickets;
        static int nextTicketId = 1;

        static Airlines()
        {
            airports = new Dictionary<string, Airport>();
            flights = new Dictionary<string, Flight>();
            customers = new Dictionary<int, Customer>();
            tickets = new Dictionary<int, Ticket>();
        }

        static void AddAirport()
        {
            Console.Write("Код аэропорта: ");
            string c = Console.ReadLine();
            if (airports.ContainsKey(c)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Название: ");
            string n = Console.ReadLine();
            airports.Add(c, new Airport(c, n));
            Console.WriteLine("Добавлен");
        }

        static void AddFlight()
        {
            Console.Write("Номер рейса: ");
            string n = Console.ReadLine();
            if (flights.ContainsKey(n)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Код вылета: ");
            string fc = Console.ReadLine();
            Console.Write("Код прибытия: ");
            string tc = Console.ReadLine();
            if (!airports.ContainsKey(fc) || !airports.ContainsKey(tc))
            { Console.WriteLine("Нет такого аэропорта"); return; }
            Console.Write("Цена билета: ");
            decimal p = decimal.Parse(Console.ReadLine());
            Console.Write("Год вылета: "); int y = int.Parse(Console.ReadLine());
            Console.Write("Месяц: "); int mo = int.Parse(Console.ReadLine());
            Console.Write("День: "); int d = int.Parse(Console.ReadLine());
            flights.Add(n, new Flight(n, fc, tc, p, new DateTime(y, mo, d)));
            Console.WriteLine("Рейс добавлен");
        }

        static void AddCustomer()
        {
            Console.Write("ID клиента: ");
            int id = int.Parse(Console.ReadLine());
            if (customers.ContainsKey(id)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("ФИО: ");
            string n = Console.ReadLine();
            customers.Add(id, new Customer(id, n));
            Console.WriteLine("Клиент добавлен");
        }

        static void SellTicket()
        {
            Console.Write("Номер рейса: ");
            string fn = Console.ReadLine();
            Console.Write("ID клиента: ");
            int cid = int.Parse(Console.ReadLine());
            if (!flights.ContainsKey(fn)) { Console.WriteLine("Нет такого рейса"); return; }
            if (!customers.ContainsKey(cid)) { Console.WriteLine("Нет такого клиента"); return; }
            Ticket t = new Ticket(nextTicketId++, fn, cid, flights[fn].Price, DateTime.Now);
            tickets.Add(t.Id, t);
            Console.WriteLine("Билет №{0} продан клиенту {1} на рейс {2} за {3}",
                t.Id, customers[cid].Name, fn, t.SoldPrice);
        }

        static void TotalRevenue()
        {
            decimal sum = 0;
            foreach (var t in tickets.Values) sum += t.SoldPrice;
            Console.WriteLine("Общая выручка: {0}", sum);
        }

        static void RevenueByFlight()
        {
            Console.Write("Номер рейса: ");
            string fn = Console.ReadLine();
            if (!flights.ContainsKey(fn)) { Console.WriteLine("Нет такого рейса"); return; }
            decimal sum = 0; int cnt = 0;
            foreach (var t in tickets.Values)
                if (t.FlightNumber == fn) { sum += t.SoldPrice; cnt++; }
            Console.WriteLine("По рейсу {0} продано {1} билетов на сумму {2}", fn, cnt, sum);
        }

        static void Main()
        {
            airports.Add("LED", new Airport("LED", "Пулково"));
            airports.Add("SVO", new Airport("SVO", "Шереметьево"));
            flights.Add("SU100", new Flight("SU100", "LED", "SVO", 5000m, new DateTime(2025, 6, 1)));
            customers.Add(1, new Customer(1, "Иванов"));
            customers.Add(2, new Customer(2, "Петров"));
            tickets.Add(nextTicketId, new Ticket(nextTicketId, "SU100", 1, 5000m, DateTime.Now));
            nextTicketId++;
            tickets.Add(nextTicketId, new Ticket(nextTicketId, "SU100", 2, 5000m, DateTime.Now));
            nextTicketId++;

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить аэропорт");
                Console.WriteLine("2 - добавить рейс");
                Console.WriteLine("3 - добавить клиента");
                Console.WriteLine("4 - продать билет");
                Console.WriteLine("5 - общая выручка");
                Console.WriteLine("6 - выручка по рейсу");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddAirport(); break;
                    case 2: AddFlight(); break;
                    case 3: AddCustomer(); break;
                    case 4: SellTicket(); break;
                    case 5: TotalRevenue(); break;
                    case 6: RevenueByFlight(); break;
                }
            } while (choice != 0);
        }
    }
}
