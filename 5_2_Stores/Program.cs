using System;
using System.Collections.Generic;

namespace StoresApp
{
    // Товар
    class Goods
    {
        private int id;
        private string name;
        private decimal price;
        private string unit;

        public Goods(int id, string name, decimal price, string unit)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.unit = unit;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } set { name = value; } }
        public decimal Price { get { return price; } set { price = value; } }
        public string Unit { get { return unit; } set { unit = value; } }
    }

    // Склад
    class Store
    {
        private int id;
        private string address;
        private string keeperName;

        public Store(int id, string address)
        {
            this.id = id;
            this.address = address;
            this.keeperName = "";
        }

        public int Id { get { return id; } }
        public string Address { get { return address; } set { address = value; } }
        public string KeeperName { get { return keeperName; } set { keeperName = value; } }
    }

    // Кладовщик
    class Storekeeper
    {
        private string name;
        private int storeId;

        public Storekeeper(string name)
        {
            this.name = name;
            this.storeId = -1;
        }

        public string Name { get { return name; } }
        public int StoreId { get { return storeId; } set { storeId = value; } }
    }

    // Поступившие товары (товар на складе с количеством)
    class GoodsInStore
    {
        public int GoodsId;
        public int StoreId;
        public int Quantity;

        public GoodsInStore(int goodsId, int storeId, int quantity)
        {
            GoodsId = goodsId;
            StoreId = storeId;
            Quantity = quantity;
        }
    }

    class Storage
    {
        static Dictionary<int, Goods> goods;
        static Dictionary<int, Store> stores;
        static Dictionary<string, Storekeeper> keepers;
        static List<GoodsInStore> goodsInStores;

        static Storage()
        {
            goods = new Dictionary<int, Goods>();
            stores = new Dictionary<int, Store>();
            keepers = new Dictionary<string, Storekeeper>();
            goodsInStores = new List<GoodsInStore>();
        }

        static void AddGoods()
        {
            Console.Write("Введите ID товара: ");
            int id = int.Parse(Console.ReadLine());
            if (goods.ContainsKey(id)) { Console.WriteLine("Товар уже есть"); return; }
            Console.Write("Введите название: ");
            string name = Console.ReadLine();
            Console.Write("Введите цену: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Введите единицу измерения: ");
            string unit = Console.ReadLine();
            goods.Add(id, new Goods(id, name, price, unit));
            Console.WriteLine("Товар добавлен");
        }

        static void AddStore()
        {
            Console.Write("Введите ID склада: ");
            int id = int.Parse(Console.ReadLine());
            if (stores.ContainsKey(id)) { Console.WriteLine("Склад уже есть"); return; }
            Console.Write("Введите адрес склада: ");
            string addr = Console.ReadLine();
            stores.Add(id, new Store(id, addr));
            Console.WriteLine("Склад добавлен");
        }

        static void AddKeeper()
        {
            Console.Write("Введите ФИО кладовщика: ");
            string name = Console.ReadLine();
            if (keepers.ContainsKey(name)) { Console.WriteLine("Кладовщик уже есть"); return; }
            keepers.Add(name, new Storekeeper(name));
            Console.WriteLine("Кладовщик добавлен");
        }

        static void LinkKeeperStore()
        {
            Console.Write("Введите ФИО кладовщика: ");
            string name = Console.ReadLine();
            Console.Write("Введите ID склада: ");
            int sid = int.Parse(Console.ReadLine());
            if (!keepers.ContainsKey(name)) { Console.WriteLine("Нет такого кладовщика"); return; }
            if (!stores.ContainsKey(sid)) { Console.WriteLine("Нет такого склада"); return; }
            keepers[name].StoreId = sid;
            stores[sid].KeeperName = name;
            Console.WriteLine("Связь установлена");
        }

        static void AddGoodsToStore()
        {
            Console.Write("Введите ID товара: ");
            int gid = int.Parse(Console.ReadLine());
            Console.Write("Введите ID склада: ");
            int sid = int.Parse(Console.ReadLine());
            Console.Write("Введите количество: ");
            int qty = int.Parse(Console.ReadLine());
            if (!goods.ContainsKey(gid)) { Console.WriteLine("Нет такого товара"); return; }
            if (!stores.ContainsKey(sid)) { Console.WriteLine("Нет такого склада"); return; }
            goodsInStores.Add(new GoodsInStore(gid, sid, qty));
            Console.WriteLine("Товар поступил на склад");
        }

        static void EditGoods()
        {
            Console.Write("Введите ID товара: ");
            int id = int.Parse(Console.ReadLine());
            if (!goods.ContainsKey(id)) { Console.WriteLine("Нет такого товара"); return; }
            Goods g = goods[id];
            Console.WriteLine("Текущее: {0}, цена {1}, ед. {2}", g.Name, g.Price, g.Unit);
            Console.Write("Новое название (Enter - оставить): ");
            string n = Console.ReadLine();
            if (n != "") g.Name = n;
            Console.Write("Новая цена (Enter - оставить): ");
            string p = Console.ReadLine();
            if (p != "") g.Price = decimal.Parse(p);
            Console.Write("Новая ед. изм. (Enter - оставить): ");
            string u = Console.ReadLine();
            if (u != "") g.Unit = u;
            Console.WriteLine("Обновлено");
        }

        static void ShowGoods()
        {
            Console.Write("Введите ID товара: ");
            int id = int.Parse(Console.ReadLine());
            if (!goods.ContainsKey(id)) { Console.WriteLine("Нет такого товара"); return; }
            Goods g = goods[id];
            Console.WriteLine("ID: {0}, Название: {1}, Цена: {2}, Ед.: {3}", g.Id, g.Name, g.Price, g.Unit);
            Console.WriteLine("Размещение по складам:");
            foreach (var gs in goodsInStores)
                if (gs.GoodsId == id)
                    Console.WriteLine("  Склад {0} ({1}): {2} {3}",
                        gs.StoreId, stores[gs.StoreId].Address, gs.Quantity, g.Unit);
        }

        static void ShowStore()
        {
            Console.Write("Введите ID склада: ");
            int id = int.Parse(Console.ReadLine());
            if (!stores.ContainsKey(id)) { Console.WriteLine("Нет такого склада"); return; }
            Store s = stores[id];
            Console.WriteLine("ID: {0}, Адрес: {1}, Кладовщик: {2}",
                s.Id, s.Address, s.KeeperName == "" ? "не назначен" : s.KeeperName);
            Console.WriteLine("Товары на складе:");
            foreach (var gs in goodsInStores)
                if (gs.StoreId == id)
                    Console.WriteLine("  {0}: {1} {2}",
                        goods[gs.GoodsId].Name, gs.Quantity, goods[gs.GoodsId].Unit);
        }

        static void Main()
        {
            // Тестовые данные
            goods.Add(1, new Goods(1, "Гвозди", 50m, "кг"));
            goods.Add(2, new Goods(2, "Доски", 200m, "шт"));
            stores.Add(1, new Store(1, "ул. Ленина, 1"));
            stores.Add(2, new Store(2, "ул. Мира, 5"));
            keepers.Add("Сидоров", new Storekeeper("Сидоров"));
            keepers["Сидоров"].StoreId = 1;
            stores[1].KeeperName = "Сидоров";
            goodsInStores.Add(new GoodsInStore(1, 1, 100));
            goodsInStores.Add(new GoodsInStore(2, 1, 50));

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить товар");
                Console.WriteLine("2 - добавить склад");
                Console.WriteLine("3 - добавить кладовщика");
                Console.WriteLine("4 - связать кладовщика и склад");
                Console.WriteLine("5 - поступление товара на склад");
                Console.WriteLine("6 - редактировать товар");
                Console.WriteLine("7 - информация о товаре");
                Console.WriteLine("8 - информация о складе");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddGoods(); break;
                    case 2: AddStore(); break;
                    case 3: AddKeeper(); break;
                    case 4: LinkKeeperStore(); break;
                    case 5: AddGoodsToStore(); break;
                    case 6: EditGoods(); break;
                    case 7: ShowGoods(); break;
                    case 8: ShowStore(); break;
                }
            } while (choice != 0);
        }
    }
}
