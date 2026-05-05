using System;
using System.Collections.Generic;

namespace ResidentsApp
{
    class Person
    {
        private int id;
        private string name;
        private DateTime birthday;

        public Person(int id, string name, DateTime birthday)
        {
            this.id = id;
            this.name = name;
            this.birthday = birthday;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public DateTime Birthday { get { return birthday; } }
    }

    class Flat
    {
        private int number;
        private double area;
        private int rooms;

        public Flat(int number, double area, int rooms)
        {
            this.number = number;
            this.area = area;
            this.rooms = rooms;
        }

        public int Number { get { return number; } }
        public double Area { get { return area; } set { area = value; } }
        public int Rooms { get { return rooms; } set { rooms = value; } }
    }

    // Заселение: связь человек-квартира с датой въезда/выезда
    class Setting
    {
        public int PersonId;
        public int FlatNumber;
        public DateTime MoveIn;
        public DateTime? MoveOut; // null - ещё проживает

        public Setting(int personId, int flatNumber, DateTime moveIn)
        {
            PersonId = personId;
            FlatNumber = flatNumber;
            MoveIn = moveIn;
            MoveOut = null;
        }
    }

    // Коммунальный платеж
    class Bill
    {
        public int FlatNumber;
        public DateTime Period; // месяц-год
        public decimal Amount;

        public Bill(int flatNumber, DateTime period, decimal amount)
        {
            FlatNumber = flatNumber;
            Period = period;
            Amount = amount;
        }
    }

    class House
    {
        static Dictionary<int, Person> people;
        static Dictionary<int, Flat> flats;
        static List<Setting> settings;
        static List<Bill> bills;

        static House()
        {
            people = new Dictionary<int, Person>();
            flats = new Dictionary<int, Flat>();
            settings = new List<Setting>();
            bills = new List<Bill>();
        }

        static void AddPerson()
        {
            Console.Write("ID человека: ");
            int id = int.Parse(Console.ReadLine());
            if (people.ContainsKey(id)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("ФИО: ");
            string n = Console.ReadLine();
            Console.Write("Год рождения: "); int y = int.Parse(Console.ReadLine());
            Console.Write("Месяц: "); int mo = int.Parse(Console.ReadLine());
            Console.Write("День: "); int d = int.Parse(Console.ReadLine());
            people.Add(id, new Person(id, n, new DateTime(y, mo, d)));
            Console.WriteLine("Человек добавлен");
        }

        static void AddFlat()
        {
            Console.Write("Номер квартиры: ");
            int num = int.Parse(Console.ReadLine());
            if (flats.ContainsKey(num)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Площадь: ");
            double area = double.Parse(Console.ReadLine());
            Console.Write("Количество комнат: ");
            int r = int.Parse(Console.ReadLine());
            flats.Add(num, new Flat(num, area, r));
            Console.WriteLine("Квартира добавлена");
        }

        static void EditFlat()
        {
            Console.Write("Номер квартиры: ");
            int num = int.Parse(Console.ReadLine());
            if (!flats.ContainsKey(num)) { Console.WriteLine("Нет такой квартиры"); return; }
            Flat f = flats[num];
            Console.WriteLine("Текущая: площадь {0}, комнат {1}", f.Area, f.Rooms);
            Console.Write("Новая площадь (Enter - оставить): ");
            string a = Console.ReadLine();
            if (a != "") f.Area = double.Parse(a);
            Console.Write("Новое количество комнат (Enter - оставить): ");
            string r = Console.ReadLine();
            if (r != "") f.Rooms = int.Parse(r);
            Console.WriteLine("Обновлено");
        }

        static void MoveIn()
        {
            Console.Write("ID человека: ");
            int pid = int.Parse(Console.ReadLine());
            Console.Write("Номер квартиры: ");
            int fn = int.Parse(Console.ReadLine());
            if (!people.ContainsKey(pid)) { Console.WriteLine("Нет такого человека"); return; }
            if (!flats.ContainsKey(fn)) { Console.WriteLine("Нет такой квартиры"); return; }
            // проверим что человек ещё нигде не проживает
            foreach (var s in settings)
                if (s.PersonId == pid && s.MoveOut == null)
                { Console.WriteLine("Человек уже проживает где-то"); return; }
            settings.Add(new Setting(pid, fn, DateTime.Now));
            Console.WriteLine("Заселён");
        }

        static void MoveOut()
        {
            Console.Write("ID человека: ");
            int pid = int.Parse(Console.ReadLine());
            if (!people.ContainsKey(pid)) { Console.WriteLine("Нет такого человека"); return; }
            for (int i = 0; i < settings.Count; i++)
                if (settings[i].PersonId == pid && settings[i].MoveOut == null)
                {
                    settings[i].MoveOut = DateTime.Now;
                    Console.WriteLine("Выселен из квартиры {0}", settings[i].FlatNumber);
                    return;
                }
            Console.WriteLine("Человек нигде не проживает");
        }

        static void AddBill()
        {
            Console.Write("Номер квартиры: ");
            int fn = int.Parse(Console.ReadLine());
            if (!flats.ContainsKey(fn)) { Console.WriteLine("Нет такой квартиры"); return; }
            Console.Write("Год: "); int y = int.Parse(Console.ReadLine());
            Console.Write("Месяц: "); int mo = int.Parse(Console.ReadLine());
            Console.Write("Сумма: ");
            decimal s = decimal.Parse(Console.ReadLine());
            bills.Add(new Bill(fn, new DateTime(y, mo, 1), s));
            Console.WriteLine("Платёж добавлен");
        }

        static void ShowFlatResidents()
        {
            Console.Write("Номер квартиры: ");
            int fn = int.Parse(Console.ReadLine());
            if (!flats.ContainsKey(fn)) { Console.WriteLine("Нет такой квартиры"); return; }
            Console.WriteLine("Текущие жильцы квартиры {0}:", fn);
            int cnt = 0;
            foreach (var s in settings)
                if (s.FlatNumber == fn && s.MoveOut == null)
                {
                    Console.WriteLine("  {0} (с {1})", people[s.PersonId].Name, s.MoveIn.ToShortDateString());
                    cnt++;
                }
            if (cnt == 0) Console.WriteLine("  никто не проживает");
        }

        static void TotalBills()
        {
            decimal sum = 0;
            foreach (var b in bills) sum += b.Amount;
            Console.WriteLine("Суммарные коммунальные платежи по дому: {0}", sum);
        }

        static void Main()
        {
            people.Add(1, new Person(1, "Иванов И.И.", new DateTime(1980, 5, 15)));
            people.Add(2, new Person(2, "Иванова А.А.", new DateTime(1982, 7, 20)));
            people.Add(3, new Person(3, "Петров П.П.", new DateTime(1975, 1, 10)));
            flats.Add(1, new Flat(1, 45.5, 2));
            flats.Add(2, new Flat(2, 60.0, 3));
            settings.Add(new Setting(1, 1, new DateTime(2020, 1, 1)));
            settings.Add(new Setting(2, 1, new DateTime(2020, 1, 1)));
            settings.Add(new Setting(3, 2, new DateTime(2018, 6, 1)));
            bills.Add(new Bill(1, new DateTime(2025, 1, 1), 5500m));
            bills.Add(new Bill(2, new DateTime(2025, 1, 1), 7200m));

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить человека");
                Console.WriteLine("2 - добавить квартиру");
                Console.WriteLine("3 - редактировать квартиру");
                Console.WriteLine("4 - вселить");
                Console.WriteLine("5 - выселить");
                Console.WriteLine("6 - добавить коммунальный платёж");
                Console.WriteLine("7 - жильцы квартиры");
                Console.WriteLine("8 - суммарные платежи по дому");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddPerson(); break;
                    case 2: AddFlat(); break;
                    case 3: EditFlat(); break;
                    case 4: MoveIn(); break;
                    case 5: MoveOut(); break;
                    case 6: AddBill(); break;
                    case 7: ShowFlatResidents(); break;
                    case 8: TotalBills(); break;
                }
            } while (choice != 0);
        }
    }
}
