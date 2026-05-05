using System;
using System.Collections.Generic;

namespace ComputerNetworkApp
{
    // Тип комплектующего
    class AccType
    {
        private int id;
        private string name; // "RAM", "CPU", "HDD", ...
        private string unit; // "ГБ", "ГГц", ...

        public AccType(int id, string name, string unit)
        {
            this.id = id;
            this.name = name;
            this.unit = unit;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public string Unit { get { return unit; } }
    }

    // Комплектующее
    class Accessory
    {
        private int id;
        private int typeId;
        private string model;
        private double value; // объём в указанной единице

        public Accessory(int id, int typeId, string model, double value)
        {
            this.id = id;
            this.typeId = typeId;
            this.model = model;
            this.value = value;
        }

        public int Id { get { return id; } }
        public int TypeId { get { return typeId; } }
        public string Model { get { return model; } }
        public double Value { get { return value; } }
    }

    class Room
    {
        private int number;
        private string description;

        public Room(int number, string description)
        {
            this.number = number;
            this.description = description;
        }

        public int Number { get { return number; } }
        public string Description { get { return description; } set { description = value; } }
    }

    class Computer
    {
        private string hostname;
        private int roomNumber;
        private List<int> accessoryIds; // ID комплектующих

        public Computer(string hostname, int roomNumber)
        {
            this.hostname = hostname;
            this.roomNumber = roomNumber;
            this.accessoryIds = new List<int>();
        }

        public string Hostname { get { return hostname; } }
        public int RoomNumber { get { return roomNumber; } set { roomNumber = value; } }
        public List<int> AccessoryIds { get { return accessoryIds; } }
    }

    class Network
    {
        static Dictionary<string, Computer> computers;
        static Dictionary<int, Room> rooms;
        static Dictionary<int, Accessory> accessories;
        static Dictionary<int, AccType> types;
        static int nextAccId = 1;

        static Network()
        {
            computers = new Dictionary<string, Computer>();
            rooms = new Dictionary<int, Room>();
            accessories = new Dictionary<int, Accessory>();
            types = new Dictionary<int, AccType>();
        }

        static void AddType()
        {
            Console.Write("ID типа: ");
            int id = int.Parse(Console.ReadLine());
            if (types.ContainsKey(id)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Название (RAM, CPU, HDD...): ");
            string n = Console.ReadLine();
            Console.Write("Единица измерения: ");
            string u = Console.ReadLine();
            types.Add(id, new AccType(id, n, u));
            Console.WriteLine("Тип добавлен");
        }

        static void AddRoom()
        {
            Console.Write("Номер комнаты: ");
            int n = int.Parse(Console.ReadLine());
            if (rooms.ContainsKey(n)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Описание: ");
            string d = Console.ReadLine();
            rooms.Add(n, new Room(n, d));
            Console.WriteLine("Комната добавлена");
        }

        static void AddComputer()
        {
            Console.Write("Hostname компьютера: ");
            string h = Console.ReadLine();
            if (computers.ContainsKey(h)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Номер комнаты: ");
            int r = int.Parse(Console.ReadLine());
            if (!rooms.ContainsKey(r)) { Console.WriteLine("Нет такой комнаты"); return; }
            computers.Add(h, new Computer(h, r));
            Console.WriteLine("Компьютер добавлен");
        }

        static void AddAccessory()
        {
            Console.Write("Hostname компьютера: ");
            string h = Console.ReadLine();
            if (!computers.ContainsKey(h)) { Console.WriteLine("Нет такого компьютера"); return; }
            Console.Write("ID типа комплектующего: ");
            int tid = int.Parse(Console.ReadLine());
            if (!types.ContainsKey(tid)) { Console.WriteLine("Нет такого типа"); return; }
            Console.Write("Модель: ");
            string m = Console.ReadLine();
            Console.Write("Значение (число): ");
            double v = double.Parse(Console.ReadLine());
            int aid = nextAccId++;
            accessories.Add(aid, new Accessory(aid, tid, m, v));
            computers[h].AccessoryIds.Add(aid);
            Console.WriteLine("Комплектующее добавлено");
        }

        static void ShowComputer()
        {
            Console.Write("Hostname: ");
            string h = Console.ReadLine();
            if (!computers.ContainsKey(h)) { Console.WriteLine("Нет такого компьютера"); return; }
            Computer c = computers[h];
            Console.WriteLine("Хост: {0}, комната: {1}", c.Hostname, c.RoomNumber);
            Console.WriteLine("Комплектующие:");
            foreach (int aid in c.AccessoryIds)
            {
                Accessory a = accessories[aid];
                AccType t = types[a.TypeId];
                Console.WriteLine("  {0} ({1}): {2} {3}", t.Name, a.Model, a.Value, t.Unit);
            }
        }

        static void TotalByType()
        {
            Console.Write("ID типа комплектующего: ");
            int tid = int.Parse(Console.ReadLine());
            if (!types.ContainsKey(tid)) { Console.WriteLine("Нет такого типа"); return; }
            double sum = 0;
            foreach (var a in accessories.Values)
                if (a.TypeId == tid) sum += a.Value;
            Console.WriteLine("Суммарный объём ({0}): {1} {2}", types[tid].Name, sum, types[tid].Unit);
        }

        static void ListAllComputers()
        {
            Console.WriteLine("Все компьютеры в сети:");
            foreach (var c in computers.Values)
                Console.WriteLine("  {0} (комн. {1}), компонентов: {2}",
                    c.Hostname, c.RoomNumber, c.AccessoryIds.Count);
        }

        static void Main()
        {
            types.Add(1, new AccType(1, "RAM", "ГБ"));
            types.Add(2, new AccType(2, "HDD", "ГБ"));
            types.Add(3, new AccType(3, "CPU", "ГГц"));
            rooms.Add(101, new Room(101, "Аудитория"));
            rooms.Add(102, new Room(102, "Серверная"));
            computers.Add("PC-01", new Computer("PC-01", 101));
            computers.Add("PC-02", new Computer("PC-02", 101));
            // комплектующие для PC-01
            accessories.Add(nextAccId, new Accessory(nextAccId, 1, "Kingston", 16));
            computers["PC-01"].AccessoryIds.Add(nextAccId++);
            accessories.Add(nextAccId, new Accessory(nextAccId, 2, "Seagate", 1000));
            computers["PC-01"].AccessoryIds.Add(nextAccId++);
            // для PC-02
            accessories.Add(nextAccId, new Accessory(nextAccId, 1, "Corsair", 32));
            computers["PC-02"].AccessoryIds.Add(nextAccId++);

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить тип комплектующего");
                Console.WriteLine("2 - добавить комнату");
                Console.WriteLine("3 - добавить компьютер");
                Console.WriteLine("4 - добавить комплектующее в компьютер");
                Console.WriteLine("5 - показать характеристики компьютера");
                Console.WriteLine("6 - суммарный объём по типу комплектующего");
                Console.WriteLine("7 - список всех компьютеров");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddType(); break;
                    case 2: AddRoom(); break;
                    case 3: AddComputer(); break;
                    case 4: AddAccessory(); break;
                    case 5: ShowComputer(); break;
                    case 6: TotalByType(); break;
                    case 7: ListAllComputers(); break;
                }
            } while (choice != 0);
        }
    }
}
