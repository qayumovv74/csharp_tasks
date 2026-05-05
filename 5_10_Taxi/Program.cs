using System;
using System.Collections.Generic;

namespace TaxiApp
{
    class Driver
    {
        private int id;
        private string name;
        private string licenseNumber;

        public Driver(int id, string name, string licenseNumber)
        {
            this.id = id;
            this.name = name;
            this.licenseNumber = licenseNumber;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } set { name = value; } }
        public string LicenseNumber { get { return licenseNumber; } set { licenseNumber = value; } }
    }

    class Car
    {
        private string plate;
        private string model;
        private int? driverId; // nullable - водитель может быть не назначен

        public Car(string plate, string model)
        {
            this.plate = plate;
            this.model = model;
            this.driverId = null;
        }

        public string Plate { get { return plate; } }
        public string Model { get { return model; } set { model = value; } }
        public int? DriverId { get { return driverId; } set { driverId = value; } }
    }

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
        public string Name { get { return name; } set { name = value; } }
        public string Phone { get { return phone; } set { phone = value; } }
    }

    // Заказ
    class Custom
    {
        public int Id;
        public int CustomerId;
        public string CarPlate;
        public DateTime OrderTime;
        public string From;
        public string To;
        public decimal Cost;

        public Custom(int id, int customerId, string carPlate, DateTime orderTime,
                      string from, string to, decimal cost)
        {
            Id = id;
            CustomerId = customerId;
            CarPlate = carPlate;
            OrderTime = orderTime;
            From = from;
            To = to;
            Cost = cost;
        }
    }

    class Taxi
    {
        static Dictionary<int, Driver> drivers;
        static Dictionary<string, Car> cars;
        static Dictionary<int, Customer> customers;
        static Dictionary<int, Custom> customs;
        static int nextOrderId = 1;

        static Taxi()
        {
            drivers = new Dictionary<int, Driver>();
            cars = new Dictionary<string, Car>();
            customers = new Dictionary<int, Customer>();
            customs = new Dictionary<int, Custom>();
        }

        static void AddDriver()
        {
            Console.Write("ID водителя: ");
            int id = int.Parse(Console.ReadLine());
            if (drivers.ContainsKey(id)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("ФИО: ");
            string n = Console.ReadLine();
            Console.Write("Номер прав: ");
            string l = Console.ReadLine();
            drivers.Add(id, new Driver(id, n, l));
            Console.WriteLine("Водитель добавлен");
        }

        static void AddCar()
        {
            Console.Write("Гос. номер: ");
            string pl = Console.ReadLine();
            if (cars.ContainsKey(pl)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Модель: ");
            string m = Console.ReadLine();
            cars.Add(pl, new Car(pl, m));
            Console.WriteLine("Автомобиль добавлен");
        }

        static void AssignDriver()
        {
            Console.Write("Гос. номер авто: ");
            string pl = Console.ReadLine();
            if (!cars.ContainsKey(pl)) { Console.WriteLine("Нет такого авто"); return; }
            Console.Write("ID водителя: ");
            int did = int.Parse(Console.ReadLine());
            if (!drivers.ContainsKey(did)) { Console.WriteLine("Нет такого водителя"); return; }
            cars[pl].DriverId = did;
            Console.WriteLine("Водитель назначен");
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

        static void EditCustomer()
        {
            Console.Write("ID клиента: ");
            int id = int.Parse(Console.ReadLine());
            if (!customers.ContainsKey(id)) { Console.WriteLine("Нет такого клиента"); return; }
            Customer c = customers[id];
            Console.WriteLine("Текущий: {0}, тел. {1}", c.Name, c.Phone);
            Console.Write("Новое ФИО (Enter - оставить): ");
            string n = Console.ReadLine();
            if (n != "") c.Name = n;
            Console.Write("Новый телефон (Enter - оставить): ");
            string p = Console.ReadLine();
            if (p != "") c.Phone = p;
            Console.WriteLine("Обновлено");
        }

        static void AddOrder()
        {
            Console.Write("ID клиента: ");
            int cid = int.Parse(Console.ReadLine());
            if (!customers.ContainsKey(cid)) { Console.WriteLine("Нет такого клиента"); return; }
            Console.Write("Гос. номер авто: ");
            string pl = Console.ReadLine();
            if (!cars.ContainsKey(pl)) { Console.WriteLine("Нет такого авто"); return; }
            if (cars[pl].DriverId == null) { Console.WriteLine("На авто не назначен водитель"); return; }
            Console.Write("Откуда: ");
            string from = Console.ReadLine();
            Console.Write("Куда: ");
            string to = Console.ReadLine();
            Console.Write("Стоимость: ");
            decimal cost = decimal.Parse(Console.ReadLine());
            int oid = nextOrderId++;
            customs.Add(oid, new Custom(oid, cid, pl, DateTime.Now, from, to, cost));
            Console.WriteLine("Заказ №{0} оформлен", oid);
        }

        static void OrdersByDay()
        {
            Console.Write("Год: "); int y = int.Parse(Console.ReadLine());
            Console.Write("Месяц: "); int mo = int.Parse(Console.ReadLine());
            Console.Write("День: "); int d = int.Parse(Console.ReadLine());
            DateTime target = new DateTime(y, mo, d);
            int cnt = 0;
            decimal total = 0;
            foreach (var o in customs.Values)
                if (o.OrderTime.Date == target.Date) { cnt++; total += o.Cost; }
            Console.WriteLine("За {0}: {1} заказов на сумму {2}", target.ToShortDateString(), cnt, total);
        }

        static void ShowAllOrders()
        {
            Console.WriteLine("Все заказы:");
            foreach (var o in customs.Values)
                Console.WriteLine("  №{0} {1}: {2} ({3} -> {4}), авто {5}, {6} руб.",
                    o.Id, o.OrderTime, customers[o.CustomerId].Name, o.From, o.To, o.CarPlate, o.Cost);
        }

        static void Main()
        {
            drivers.Add(1, new Driver(1, "Сидоров С.С.", "7777 123456"));
            drivers.Add(2, new Driver(2, "Кузнецов К.К.", "7777 654321"));
            cars.Add("А111АА77", new Car("А111АА77", "Hyundai Solaris"));
            cars.Add("В222ВВ77", new Car("В222ВВ77", "Kia Rio"));
            cars["А111АА77"].DriverId = 1;
            cars["В222ВВ77"].DriverId = 2;
            customers.Add(1, new Customer(1, "Иванов И.И.", "+79001112233"));
            customers.Add(2, new Customer(2, "Петров П.П.", "+79004445566"));
            customs.Add(nextOrderId, new Custom(nextOrderId, 1, "А111АА77", DateTime.Now, "ул. Ленина", "Аэропорт", 800m));
            nextOrderId++;
            customs.Add(nextOrderId, new Custom(nextOrderId, 2, "В222ВВ77", DateTime.Now, "Вокзал", "ул. Мира", 450m));
            nextOrderId++;

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить водителя");
                Console.WriteLine("2 - добавить автомобиль");
                Console.WriteLine("3 - назначить водителя на авто");
                Console.WriteLine("4 - добавить клиента");
                Console.WriteLine("5 - редактировать клиента");
                Console.WriteLine("6 - оформить заказ");
                Console.WriteLine("7 - количество заказов за день");
                Console.WriteLine("8 - показать все заказы");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddDriver(); break;
                    case 2: AddCar(); break;
                    case 3: AssignDriver(); break;
                    case 4: AddCustomer(); break;
                    case 5: EditCustomer(); break;
                    case 6: AddOrder(); break;
                    case 7: OrdersByDay(); break;
                    case 8: ShowAllOrders(); break;
                }
            } while (choice != 0);
        }
    }
}
