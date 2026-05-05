using System;
using System.Collections.Generic;

namespace AutoServiceApp
{
    class Customer
    {
        private int id;
        private string name;
        private string phone;

        public Customer(int id, string name, string phone)
        {
            this.id = id;
            this.name = name;
            this.phone = phone;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public string Phone { get { return phone; } set { phone = value; } }
    }

    class Car
    {
        private string plate; // гос. номер
        private string model;
        private int customerId;

        public Car(string plate, string model, int customerId)
        {
            this.plate = plate;
            this.model = model;
            this.customerId = customerId;
        }

        public string Plate { get { return plate; } }
        public string Model { get { return model; } set { model = value; } }
        public int CustomerId { get { return customerId; } set { customerId = value; } }
    }

    class Mechanic
    {
        private int id;
        private string name;
        private string specialty;

        public Mechanic(int id, string name, string specialty)
        {
            this.id = id;
            this.name = name;
            this.specialty = specialty;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public string Specialty { get { return specialty; } set { specialty = value; } }
    }

    // Выполненный ремонт
    class Repair
    {
        public int Id;
        public string CarPlate;
        public int MechanicId;
        public DateTime Date;
        public string Description;
        public decimal Cost;

        public Repair(int id, string carPlate, int mechanicId, DateTime date, string description, decimal cost)
        {
            Id = id;
            CarPlate = carPlate;
            MechanicId = mechanicId;
            Date = date;
            Description = description;
            Cost = cost;
        }
    }

    class AutoService
    {
        static Dictionary<int, Customer> customers;
        static Dictionary<string, Car> cars;
        static Dictionary<int, Mechanic> mechanics;
        static Dictionary<int, Repair> repairs;
        static int nextRepairId = 1;

        static AutoService()
        {
            customers = new Dictionary<int, Customer>();
            cars = new Dictionary<string, Car>();
            mechanics = new Dictionary<int, Mechanic>();
            repairs = new Dictionary<int, Repair>();
        }

        static void AddCustomer()
        {
            Console.Write("ID клиента: ");
            int id = int.Parse(Console.ReadLine());
            if (customers.ContainsKey(id)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("ФИО: ");
            string n = Console.ReadLine();
            Console.Write("Телефон: ");
            string p = Console.ReadLine();
            customers.Add(id, new Customer(id, n, p));
            Console.WriteLine("Клиент добавлен");
        }

        static void AddCar()
        {
            Console.Write("Гос. номер: ");
            string pl = Console.ReadLine();
            if (cars.ContainsKey(pl)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Модель: ");
            string m = Console.ReadLine();
            Console.Write("ID клиента-владельца: ");
            int cid = int.Parse(Console.ReadLine());
            if (!customers.ContainsKey(cid)) { Console.WriteLine("Нет такого клиента"); return; }
            cars.Add(pl, new Car(pl, m, cid));
            Console.WriteLine("Автомобиль добавлен");
        }

        static void AddMechanic()
        {
            Console.Write("ID мастера: ");
            int id = int.Parse(Console.ReadLine());
            if (mechanics.ContainsKey(id)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("ФИО: ");
            string n = Console.ReadLine();
            Console.Write("Специализация: ");
            string s = Console.ReadLine();
            mechanics.Add(id, new Mechanic(id, n, s));
            Console.WriteLine("Мастер добавлен");
        }

        static void AddRepair()
        {
            Console.Write("Гос. номер автомобиля: ");
            string pl = Console.ReadLine();
            if (!cars.ContainsKey(pl)) { Console.WriteLine("Нет такого авто"); return; }
            Console.Write("ID мастера: ");
            int mid = int.Parse(Console.ReadLine());
            if (!mechanics.ContainsKey(mid)) { Console.WriteLine("Нет такого мастера"); return; }
            Console.Write("Описание работ: ");
            string d = Console.ReadLine();
            Console.Write("Стоимость: ");
            decimal c = decimal.Parse(Console.ReadLine());
            int rid = nextRepairId++;
            repairs.Add(rid, new Repair(rid, pl, mid, DateTime.Now, d, c));
            Console.WriteLine("Ремонт №{0} зарегистрирован", rid);
        }

        static void RepairsByMechanic()
        {
            Console.Write("ID мастера: ");
            int mid = int.Parse(Console.ReadLine());
            if (!mechanics.ContainsKey(mid)) { Console.WriteLine("Нет такого мастера"); return; }
            int cnt = 0;
            decimal total = 0;
            foreach (var r in repairs.Values)
                if (r.MechanicId == mid) { cnt++; total += r.Cost; }
            Console.WriteLine("Мастер {0}: {1} ремонтов, общая стоимость {2}",
                mechanics[mid].Name, cnt, total);
        }

        static void RepairsByCar()
        {
            Console.Write("Гос. номер: ");
            string pl = Console.ReadLine();
            if (!cars.ContainsKey(pl)) { Console.WriteLine("Нет такого авто"); return; }
            Console.WriteLine("История ремонтов авто {0}:", pl);
            foreach (var r in repairs.Values)
                if (r.CarPlate == pl)
                    Console.WriteLine("  №{0} {1}: {2} (мастер {3}, {4} руб.)",
                        r.Id, r.Date.ToShortDateString(), r.Description,
                        mechanics[r.MechanicId].Name, r.Cost);
        }

        static void Main()
        {
            customers.Add(1, new Customer(1, "Иванов И.И.", "+79001112233"));
            customers.Add(2, new Customer(2, "Петров П.П.", "+79004445566"));
            cars.Add("А123БВ77", new Car("А123БВ77", "Lada Vesta", 1));
            cars.Add("О456РК78", new Car("О456РК78", "Toyota Camry", 2));
            mechanics.Add(1, new Mechanic(1, "Сидоров", "двигатель"));
            mechanics.Add(2, new Mechanic(2, "Кузнецов", "электрика"));
            repairs.Add(nextRepairId, new Repair(nextRepairId, "А123БВ77", 1, DateTime.Now.AddDays(-10), "Замена масла", 3000m));
            nextRepairId++;
            repairs.Add(nextRepairId, new Repair(nextRepairId, "О456РК78", 2, DateTime.Now.AddDays(-5), "Замена аккумулятора", 8000m));
            nextRepairId++;
            repairs.Add(nextRepairId, new Repair(nextRepairId, "А123БВ77", 1, DateTime.Now.AddDays(-2), "Ремонт двигателя", 25000m));
            nextRepairId++;

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить клиента");
                Console.WriteLine("2 - добавить автомобиль");
                Console.WriteLine("3 - добавить мастера");
                Console.WriteLine("4 - зарегистрировать ремонт");
                Console.WriteLine("5 - количество и сумма ремонтов мастера");
                Console.WriteLine("6 - история ремонтов авто");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddCustomer(); break;
                    case 2: AddCar(); break;
                    case 3: AddMechanic(); break;
                    case 4: AddRepair(); break;
                    case 5: RepairsByMechanic(); break;
                    case 6: RepairsByCar(); break;
                }
            } while (choice != 0);
        }
    }
}
