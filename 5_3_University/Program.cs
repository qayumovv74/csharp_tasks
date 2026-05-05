using System;
using System.Collections.Generic;

namespace UniversityApp
{
    class Group
    {
        private string code;
        private List<int> studentIds;

        public Group(string code)
        {
            this.code = code;
            this.studentIds = new List<int>();
        }

        public string Code { get { return code; } }
        public List<int> StudentIds { get { return studentIds; } }
    }

    class Student
    {
        private int id;
        private string name;
        private string groupCode;

        public Student(int id, string name, string groupCode)
        {
            this.id = id;
            this.name = name;
            this.groupCode = groupCode;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public string GroupCode { get { return groupCode; } }
    }

    class Course
    {
        private int id;
        private string title;

        public Course(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        public int Id { get { return id; } }
        public string Title { get { return title; } }
    }

    // Запись успеваемости: студент - предмет - оценка
    class Progress
    {
        public int StudentId;
        public int CourseId;
        public int Mark;

        public Progress(int studentId, int courseId, int mark)
        {
            StudentId = studentId;
            CourseId = courseId;
            Mark = mark;
        }
    }

    class University
    {
        static Dictionary<int, Student> students;
        static Dictionary<string, Group> groups;
        static Dictionary<int, Course> courses;
        static List<Progress> progresses;

        static University()
        {
            students = new Dictionary<int, Student>();
            groups = new Dictionary<string, Group>();
            courses = new Dictionary<int, Course>();
            progresses = new List<Progress>();
        }

        static void AddGroup()
        {
            Console.Write("Введите код группы: ");
            string code = Console.ReadLine();
            if (groups.ContainsKey(code)) { Console.WriteLine("Группа уже есть"); return; }
            groups.Add(code, new Group(code));
            Console.WriteLine("Группа добавлена");
        }

        static void AddStudent()
        {
            Console.Write("Введите ID студента: ");
            int id = int.Parse(Console.ReadLine());
            if (students.ContainsKey(id)) { Console.WriteLine("Студент уже есть"); return; }
            Console.Write("Введите ФИО: ");
            string name = Console.ReadLine();
            Console.Write("Введите код группы: ");
            string gc = Console.ReadLine();
            if (!groups.ContainsKey(gc)) { Console.WriteLine("Нет такой группы"); return; }
            students.Add(id, new Student(id, name, gc));
            groups[gc].StudentIds.Add(id);
            Console.WriteLine("Студент добавлен");
        }

        static void AddCourse()
        {
            Console.Write("Введите ID предмета: ");
            int id = int.Parse(Console.ReadLine());
            if (courses.ContainsKey(id)) { Console.WriteLine("Предмет уже есть"); return; }
            Console.Write("Введите название предмета: ");
            string t = Console.ReadLine();
            courses.Add(id, new Course(id, t));
            Console.WriteLine("Предмет добавлен");
        }

        static void AddOrEditMark()
        {
            Console.Write("Введите ID студента: ");
            int sid = int.Parse(Console.ReadLine());
            Console.Write("Введите ID предмета: ");
            int cid = int.Parse(Console.ReadLine());
            Console.Write("Введите оценку (2-5): ");
            int m = int.Parse(Console.ReadLine());
            if (!students.ContainsKey(sid)) { Console.WriteLine("Нет такого студента"); return; }
            if (!courses.ContainsKey(cid)) { Console.WriteLine("Нет такого предмета"); return; }
            if (m < 2 || m > 5) { Console.WriteLine("Оценка должна быть от 2 до 5"); return; }
            // если оценка уже есть - заменим, иначе добавим
            for (int i = 0; i < progresses.Count; i++)
                if (progresses[i].StudentId == sid && progresses[i].CourseId == cid)
                {
                    progresses[i].Mark = m;
                    Console.WriteLine("Оценка обновлена");
                    return;
                }
            progresses.Add(new Progress(sid, cid, m));
            Console.WriteLine("Оценка добавлена");
        }

        static void StudentAverage()
        {
            Console.Write("Введите ID студента: ");
            int sid = int.Parse(Console.ReadLine());
            if (!students.ContainsKey(sid)) { Console.WriteLine("Нет такого студента"); return; }
            int sum = 0, cnt = 0;
            foreach (var p in progresses)
                if (p.StudentId == sid) { sum += p.Mark; cnt++; }
            if (cnt == 0) { Console.WriteLine("У студента нет оценок"); return; }
            Console.WriteLine("Средняя оценка студента {0}: {1:F2}", students[sid].Name, (double)sum / cnt);
        }

        static void GroupAverage()
        {
            Console.Write("Введите код группы: ");
            string gc = Console.ReadLine();
            if (!groups.ContainsKey(gc)) { Console.WriteLine("Нет такой группы"); return; }
            int sum = 0, cnt = 0;
            foreach (var p in progresses)
                if (students.ContainsKey(p.StudentId) && students[p.StudentId].GroupCode == gc)
                { sum += p.Mark; cnt++; }
            if (cnt == 0) { Console.WriteLine("В группе нет оценок"); return; }
            Console.WriteLine("Средняя оценка по группе {0}: {1:F2}", gc, (double)sum / cnt);
        }

        static void Main()
        {
            // Тестовые данные
            groups.Add("ИС-21", new Group("ИС-21"));
            students.Add(1, new Student(1, "Иванов И.И.", "ИС-21"));
            students.Add(2, new Student(2, "Петров П.П.", "ИС-21"));
            groups["ИС-21"].StudentIds.Add(1);
            groups["ИС-21"].StudentIds.Add(2);
            courses.Add(1, new Course(1, "Математика"));
            courses.Add(2, new Course(2, "Программирование"));
            progresses.Add(new Progress(1, 1, 5));
            progresses.Add(new Progress(1, 2, 4));
            progresses.Add(new Progress(2, 1, 3));
            progresses.Add(new Progress(2, 2, 4));

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить группу");
                Console.WriteLine("2 - добавить студента");
                Console.WriteLine("3 - добавить предмет");
                Console.WriteLine("4 - добавить/изменить оценку");
                Console.WriteLine("5 - средняя оценка студента");
                Console.WriteLine("6 - средняя оценка по группе");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddGroup(); break;
                    case 2: AddStudent(); break;
                    case 3: AddCourse(); break;
                    case 4: AddOrEditMark(); break;
                    case 5: StudentAverage(); break;
                    case 6: GroupAverage(); break;
                }
            } while (choice != 0);
        }
    }
}
