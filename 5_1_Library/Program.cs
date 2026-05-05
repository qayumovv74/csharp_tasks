using System;
using System.Collections.Generic;

namespace LibraryApp
{
    // Книга
    class Book
    {
        private int id;
        private string title;
        private string author;
        private int totalCopies;
        private int issuedCopies;

        public Book(int id, string title, string author, int totalCopies)
        {
            this.id = id;
            this.title = title;
            this.author = author;
            this.totalCopies = totalCopies;
            this.issuedCopies = 0;
        }

        public int Id { get { return id; } }
        public string Title { get { return title; } }
        public string Author { get { return author; } }
        public int TotalCopies { get { return totalCopies; } }
        public int IssuedCopies { get { return issuedCopies; } }
        public int AvailableCopies { get { return totalCopies - issuedCopies; } }

        public bool Issue()
        {
            if (AvailableCopies > 0) { issuedCopies++; return true; }
            return false;
        }

        public bool Return()
        {
            if (issuedCopies > 0) { issuedCopies--; return true; }
            return false;
        }
    }

    // Читатель
    class Reader
    {
        private int id;
        private string name;
        private List<int> issuedBooks;

        public Reader(int id, string name)
        {
            this.id = id;
            this.name = name;
            this.issuedBooks = new List<int>();
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public List<int> IssuedBooks { get { return issuedBooks; } }
    }

    // Запись о выдаче
    class IssBook
    {
        public int BookId;
        public int ReaderId;
        public DateTime IssueDate;

        public IssBook(int bookId, int readerId, DateTime issueDate)
        {
            BookId = bookId;
            ReaderId = readerId;
            IssueDate = issueDate;
        }
    }

    // Запись о возврате
    class RetBook
    {
        public int BookId;
        public int ReaderId;
        public DateTime ReturnDate;

        public RetBook(int bookId, int readerId, DateTime returnDate)
        {
            BookId = bookId;
            ReaderId = readerId;
            ReturnDate = returnDate;
        }
    }

    class Library
    {
        static Dictionary<int, Book> books;
        static Dictionary<int, Reader> readers;
        static List<IssBook> issBooks;
        static List<RetBook> retBooks;

        static Library()
        {
            books = new Dictionary<int, Book>();
            readers = new Dictionary<int, Reader>();
            issBooks = new List<IssBook>();
            retBooks = new List<RetBook>();
        }

        static void AddBook()
        {
            Console.Write("Введите ID книги: ");
            int id = int.Parse(Console.ReadLine());
            if (books.ContainsKey(id)) { Console.WriteLine("Книга с таким ID уже есть"); return; }
            Console.Write("Введите название: ");
            string title = Console.ReadLine();
            Console.Write("Введите автора: ");
            string author = Console.ReadLine();
            Console.Write("Введите количество экземпляров: ");
            int copies = int.Parse(Console.ReadLine());
            books.Add(id, new Book(id, title, author, copies));
            Console.WriteLine("Книга добавлена");
        }

        static void AddReader()
        {
            Console.Write("Введите ID читателя: ");
            int id = int.Parse(Console.ReadLine());
            if (readers.ContainsKey(id)) { Console.WriteLine("Читатель с таким ID уже есть"); return; }
            Console.Write("Введите ФИО читателя: ");
            string name = Console.ReadLine();
            readers.Add(id, new Reader(id, name));
            Console.WriteLine("Читатель добавлен");
        }

        static void IssueBook()
        {
            Console.Write("Введите ID книги: ");
            int bid = int.Parse(Console.ReadLine());
            Console.Write("Введите ID читателя: ");
            int rid = int.Parse(Console.ReadLine());
            if (!books.ContainsKey(bid)) { Console.WriteLine("Нет такой книги"); return; }
            if (!readers.ContainsKey(rid)) { Console.WriteLine("Нет такого читателя"); return; }
            if (books[bid].Issue())
            {
                readers[rid].IssuedBooks.Add(bid);
                issBooks.Add(new IssBook(bid, rid, DateTime.Now));
                Console.WriteLine("Книга выдана");
            }
            else Console.WriteLine("Нет свободных экземпляров");
        }

        static void ReturnBook()
        {
            Console.Write("Введите ID книги: ");
            int bid = int.Parse(Console.ReadLine());
            Console.Write("Введите ID читателя: ");
            int rid = int.Parse(Console.ReadLine());
            if (!books.ContainsKey(bid)) { Console.WriteLine("Нет такой книги"); return; }
            if (!readers.ContainsKey(rid)) { Console.WriteLine("Нет такого читателя"); return; }
            if (!readers[rid].IssuedBooks.Contains(bid)) { Console.WriteLine("Эта книга не у этого читателя"); return; }
            books[bid].Return();
            readers[rid].IssuedBooks.Remove(bid);
            retBooks.Add(new RetBook(bid, rid, DateTime.Now));
            Console.WriteLine("Книга возвращена");
        }

        static void ShowBook()
        {
            Console.Write("Введите ID книги: ");
            int id = int.Parse(Console.ReadLine());
            if (!books.ContainsKey(id)) { Console.WriteLine("Нет такой книги"); return; }
            Book b = books[id];
            Console.WriteLine("ID: {0}, Название: {1}, Автор: {2}, Всего: {3}, Выдано: {4}, Доступно: {5}",
                b.Id, b.Title, b.Author, b.TotalCopies, b.IssuedCopies, b.AvailableCopies);
        }

        static void ShowReader()
        {
            Console.Write("Введите ID читателя: ");
            int id = int.Parse(Console.ReadLine());
            if (!readers.ContainsKey(id)) { Console.WriteLine("Нет такого читателя"); return; }
            Reader r = readers[id];
            Console.WriteLine("ID: {0}, ФИО: {1}", r.Id, r.Name);
            Console.Write("Книги на руках: ");
            if (r.IssuedBooks.Count == 0) Console.WriteLine("нет");
            else
            {
                foreach (int bid in r.IssuedBooks)
                    Console.Write(books[bid].Title + "; ");
                Console.WriteLine();
            }
        }

        static void Main()
        {
            // Тестовые данные
            books.Add(1, new Book(1, "Война и мир", "Толстой Л.Н.", 3));
            books.Add(2, new Book(2, "Преступление и наказание", "Достоевский Ф.М.", 2));
            readers.Add(1, new Reader(1, "Иванов И.И."));
            readers.Add(2, new Reader(2, "Петров П.П."));

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить книгу");
                Console.WriteLine("2 - добавить читателя");
                Console.WriteLine("3 - выдать книгу");
                Console.WriteLine("4 - вернуть книгу");
                Console.WriteLine("5 - информация о книге");
                Console.WriteLine("6 - информация о читателе");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddBook(); break;
                    case 2: AddReader(); break;
                    case 3: IssueBook(); break;
                    case 4: ReturnBook(); break;
                    case 5: ShowBook(); break;
                    case 6: ShowReader(); break;
                }
            } while (choice != 0);
        }
    }
}
