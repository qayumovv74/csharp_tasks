using System;
using System.Collections.Generic;

namespace AirTransportApp
{
    class Airport
    {
        private string code;
        private string name;
        private string city;

        public Airport(string code, string name, string city)
        {
            this.code = code;
            this.name = name;
            this.city = city;
        }

        public string Code { get { return code; } }
        public string Name { get { return name; } set { name = value; } }
        public string City { get { return city; } set { city = value; } }
    }

    class Flight
    {
        private string number;
        private string fromCode;
        private string toCode;

        public Flight(string number, string fromCode, string toCode)
        {
            this.number = number;
            this.fromCode = fromCode;
            this.toCode = toCode;
        }

        public string Number { get { return number; } }
        public string FromCode { get { return fromCode; } set { fromCode = value; } }
        public string ToCode { get { return toCode; } set { toCode = value; } }
    }

    // Отправление: вылет конкретного рейса в конкретную дату/время
    class Departure
    {
        public string FlightNumber;
        public DateTime Time;

        public Departure(string flightNumber, DateTime time)
        {
            FlightNumber = flightNumber;
            Time = time;
        }
    }

    // Прибытие: приземление конкретного рейса
    class Arrival
    {
        public string FlightNumber;
        public DateTime DepartureTime; // ссылка на конкретный вылет
        public DateTime Time;

        public Arrival(string flightNumber, DateTime departureTime, DateTime time)
        {
            FlightNumber = flightNumber;
            DepartureTime = departureTime;
            Time = time;
        }
    }

    class AirSystem
    {
        static Dictionary<string, Flight> flights;
        static Dictionary<string, Airport> airports;
        static List<Departure> departures;
        static List<Arrival> arrivals;

        static AirSystem()
        {
            flights = new Dictionary<string, Flight>();
            airports = new Dictionary<string, Airport>();
            departures = new List<Departure>();
            arrivals = new List<Arrival>();
        }

        static void AddAirport()
        {
            Console.Write("Введите код аэропорта (например, LED): ");
            string c = Console.ReadLine();
            if (airports.ContainsKey(c)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Введите название: ");
            string n = Console.ReadLine();
            Console.Write("Введите город: ");
            string city = Console.ReadLine();
            airports.Add(c, new Airport(c, n, city));
            Console.WriteLine("Аэропорт добавлен");
        }

        static void AddFlight()
        {
            Console.Write("Введите номер рейса: ");
            string n = Console.ReadLine();
            if (flights.ContainsKey(n)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Код аэропорта вылета: ");
            string fc = Console.ReadLine();
            Console.Write("Код аэропорта прилёта: ");
            string tc = Console.ReadLine();
            if (!airports.ContainsKey(fc) || !airports.ContainsKey(tc))
            { Console.WriteLine("Нет такого аэропорта"); return; }
            flights.Add(n, new Flight(n, fc, tc));
            Console.WriteLine("Рейс добавлен");
        }

        static void EditFlight()
        {
            Console.Write("Введите номер рейса: ");
            string n = Console.ReadLine();
            if (!flights.ContainsKey(n)) { Console.WriteLine("Нет такого рейса"); return; }
            Flight f = flights[n];
            Console.WriteLine("Текущий: {0} -> {1}", f.FromCode, f.ToCode);
            Console.Write("Новый код аэропорта вылета (Enter - оставить): ");
            string fc = Console.ReadLine();
            if (fc != "" && airports.ContainsKey(fc)) f.FromCode = fc;
            Console.Write("Новый код аэропорта прилёта (Enter - оставить): ");
            string tc = Console.ReadLine();
            if (tc != "" && airports.ContainsKey(tc)) f.ToCode = tc;
            Console.WriteLine("Обновлено");
        }

        static DateTime ReadDateTime()
        {
            Console.Write("Год: "); int y = int.Parse(Console.ReadLine());
            Console.Write("Месяц: "); int mo = int.Parse(Console.ReadLine());
            Console.Write("День: "); int d = int.Parse(Console.ReadLine());
            Console.Write("Час: "); int h = int.Parse(Console.ReadLine());
            Console.Write("Минута: "); int mi = int.Parse(Console.ReadLine());
            return new DateTime(y, mo, d, h, mi, 0);
        }

        static void AddDeparture()
        {
            Console.Write("Введите номер рейса: ");
            string n = Console.ReadLine();
            if (!flights.ContainsKey(n)) { Console.WriteLine("Нет такого рейса"); return; }
            Console.WriteLine("Дата и время вылета:");
            DateTime t = ReadDateTime();
            departures.Add(new Departure(n, t));
            Console.WriteLine("Вылет зарегистрирован");
        }

        static void AddArrival()
        {
            Console.Write("Введите номер рейса: ");
            string n = Console.ReadLine();
            Console.WriteLine("Дата и время вылета (для идентификации):");
            DateTime dt = ReadDateTime();
            // проверим что такой вылет был
            bool found = false;
            foreach (var d in departures)
                if (d.FlightNumber == n && d.Time == dt) { found = true; break; }
            if (!found) { Console.WriteLine("Не найден соответствующий вылет"); return; }
            Console.WriteLine("Дата и время прибытия:");
            DateTime at = ReadDateTime();
            arrivals.Add(new Arrival(n, dt, at));
            Console.WriteLine("Прибытие зарегистрировано");
        }

        // Вылетевшие, но не прибывшие рейсы
        static void ShowInFlight()
        {
            Console.WriteLine("Вылетевшие, но не прибывшие рейсы:");
            int cnt = 0;
            foreach (var d in departures)
            {
                bool arrived = false;
                foreach (var a in arrivals)
                    if (a.FlightNumber == d.FlightNumber && a.DepartureTime == d.Time)
                    { arrived = true; break; }
                if (!arrived)
                {
                    Flight f = flights[d.FlightNumber];
                    Console.WriteLine("  {0}: {1} -> {2}, вылет {3}",
                        d.FlightNumber, f.FromCode, f.ToCode, d.Time);
                    cnt++;
                }
            }
            if (cnt == 0) Console.WriteLine("  нет таких рейсов");
        }

        static void Main()
        {
            airports.Add("LED", new Airport("LED", "Пулково", "Санкт-Петербург"));
            airports.Add("SVO", new Airport("SVO", "Шереметьево", "Москва"));
            airports.Add("AER", new Airport("AER", "Сочи", "Сочи"));
            flights.Add("SU100", new Flight("SU100", "LED", "SVO"));
            flights.Add("SU200", new Flight("SU200", "SVO", "AER"));
            departures.Add(new Departure("SU100", new DateTime(2025, 1, 15, 10, 0, 0)));
            arrivals.Add(new Arrival("SU100", new DateTime(2025, 1, 15, 10, 0, 0), new DateTime(2025, 1, 15, 11, 30, 0)));
            departures.Add(new Departure("SU200", new DateTime(2025, 1, 15, 14, 0, 0)));
            // SU200 вылетел но ещё не прибыл

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить аэропорт");
                Console.WriteLine("2 - добавить рейс");
                Console.WriteLine("3 - редактировать рейс");
                Console.WriteLine("4 - зарегистрировать вылет");
                Console.WriteLine("5 - зарегистрировать прибытие");
                Console.WriteLine("6 - вылетевшие, но не прибывшие рейсы");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddAirport(); break;
                    case 2: AddFlight(); break;
                    case 3: EditFlight(); break;
                    case 4: AddDeparture(); break;
                    case 5: AddArrival(); break;
                    case 6: ShowInFlight(); break;
                }
            } while (choice != 0);
        }
    }
}
