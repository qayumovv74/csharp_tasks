using System;
using System.Collections.Generic;

namespace HospitalApp
{
    class Doctor
    {
        private int id;
        private string name;
        private string specialty;

        public Doctor(int id, string name, string specialty)
        {
            this.id = id;
            this.name = name;
            this.specialty = specialty;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public string Specialty { get { return specialty; } }
    }

    class Room
    {
        private int number;
        private int capacity;

        public Room(int number, int capacity)
        {
            this.number = number;
            this.capacity = capacity;
        }

        public int Number { get { return number; } }
        public int Capacity { get { return capacity; } }
    }

    class Patient
    {
        private int id;
        private string name;
        private int roomNumber;
        private int doctorId;
        private string diagnosis;

        public Patient(int id, string name, int roomNumber, int doctorId, string diagnosis)
        {
            this.id = id;
            this.name = name;
            this.roomNumber = roomNumber;
            this.doctorId = doctorId;
            this.diagnosis = diagnosis;
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public int RoomNumber { get { return roomNumber; } set { roomNumber = value; } }
        public int DoctorId { get { return doctorId; } set { doctorId = value; } }
        public string Diagnosis { get { return diagnosis; } set { diagnosis = value; } }
    }

    // Запись о состоянии больного
    class PatState
    {
        public int PatientId;
        public DateTime Date;
        public double Temperature;
        public int Pulse;
        public string Notes;

        public PatState(int patientId, DateTime date, double temperature, int pulse, string notes)
        {
            PatientId = patientId;
            Date = date;
            Temperature = temperature;
            Pulse = pulse;
            Notes = notes;
        }
    }

    class Hospital
    {
        static Dictionary<int, Doctor> doctors;
        static Dictionary<int, Room> rooms;
        static Dictionary<int, Patient> patients;
        static List<PatState> states;

        static Hospital()
        {
            doctors = new Dictionary<int, Doctor>();
            rooms = new Dictionary<int, Room>();
            patients = new Dictionary<int, Patient>();
            states = new List<PatState>();
        }

        static void AddDoctor()
        {
            Console.Write("ID врача: ");
            int id = int.Parse(Console.ReadLine());
            if (doctors.ContainsKey(id)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("ФИО: ");
            string n = Console.ReadLine();
            Console.Write("Специальность: ");
            string s = Console.ReadLine();
            doctors.Add(id, new Doctor(id, n, s));
            Console.WriteLine("Врач добавлен");
        }

        static void AddRoom()
        {
            Console.Write("Номер палаты: ");
            int num = int.Parse(Console.ReadLine());
            if (rooms.ContainsKey(num)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("Вместимость: ");
            int cap = int.Parse(Console.ReadLine());
            rooms.Add(num, new Room(num, cap));
            Console.WriteLine("Палата добавлена");
        }

        static void AddPatient()
        {
            Console.Write("ID больного: ");
            int id = int.Parse(Console.ReadLine());
            if (patients.ContainsKey(id)) { Console.WriteLine("Уже есть"); return; }
            Console.Write("ФИО: ");
            string n = Console.ReadLine();
            Console.Write("Номер палаты: ");
            int rn = int.Parse(Console.ReadLine());
            if (!rooms.ContainsKey(rn)) { Console.WriteLine("Нет такой палаты"); return; }
            Console.Write("ID врача: ");
            int did = int.Parse(Console.ReadLine());
            if (!doctors.ContainsKey(did)) { Console.WriteLine("Нет такого врача"); return; }
            Console.Write("Диагноз: ");
            string d = Console.ReadLine();
            patients.Add(id, new Patient(id, n, rn, did, d));
            Console.WriteLine("Больной добавлен");
        }

        static void AddState()
        {
            Console.Write("ID больного: ");
            int id = int.Parse(Console.ReadLine());
            if (!patients.ContainsKey(id)) { Console.WriteLine("Нет такого больного"); return; }
            Console.Write("Температура: ");
            double t = double.Parse(Console.ReadLine());
            Console.Write("Пульс: ");
            int p = int.Parse(Console.ReadLine());
            Console.Write("Заметки: ");
            string nt = Console.ReadLine();
            states.Add(new PatState(id, DateTime.Now, t, p, nt));
            Console.WriteLine("Состояние записано");
        }

        static void ShowPatientStates()
        {
            Console.Write("ID больного: ");
            int id = int.Parse(Console.ReadLine());
            if (!patients.ContainsKey(id)) { Console.WriteLine("Нет такого больного"); return; }
            Patient p = patients[id];
            Console.WriteLine("Больной: {0}, палата {1}, врач: {2}, диагноз: {3}",
                p.Name, p.RoomNumber, doctors[p.DoctorId].Name, p.Diagnosis);
            Console.WriteLine("История состояний:");
            int cnt = 0;
            foreach (var s in states)
                if (s.PatientId == id)
                {
                    Console.WriteLine("  {0}: t={1}, пульс={2}, {3}", s.Date, s.Temperature, s.Pulse, s.Notes);
                    cnt++;
                }
            if (cnt == 0) Console.WriteLine("  записей нет");
        }

        static void CountInRoom()
        {
            Console.Write("Номер палаты: ");
            int rn = int.Parse(Console.ReadLine());
            if (!rooms.ContainsKey(rn)) { Console.WriteLine("Нет такой палаты"); return; }
            int cnt = 0;
            foreach (var p in patients.Values) if (p.RoomNumber == rn) cnt++;
            Console.WriteLine("В палате {0}: {1} больных (вместимость {2})", rn, cnt, rooms[rn].Capacity);
        }

        static void CountByDoctor()
        {
            Console.Write("ID врача: ");
            int did = int.Parse(Console.ReadLine());
            if (!doctors.ContainsKey(did)) { Console.WriteLine("Нет такого врача"); return; }
            int cnt = 0;
            foreach (var p in patients.Values) if (p.DoctorId == did) cnt++;
            Console.WriteLine("У врача {0}: {1} больных", doctors[did].Name, cnt);
        }

        static void Main()
        {
            doctors.Add(1, new Doctor(1, "Сидоров С.С.", "Терапевт"));
            doctors.Add(2, new Doctor(2, "Кузнецов К.К.", "Хирург"));
            rooms.Add(101, new Room(101, 4));
            rooms.Add(102, new Room(102, 2));
            patients.Add(1, new Patient(1, "Иванов И.И.", 101, 1, "ОРВИ"));
            patients.Add(2, new Patient(2, "Петров П.П.", 101, 1, "Грипп"));
            patients.Add(3, new Patient(3, "Смирнов С.С.", 102, 2, "Аппендицит"));
            states.Add(new PatState(1, DateTime.Now.AddDays(-1), 38.5, 90, "Слабость"));
            states.Add(new PatState(1, DateTime.Now, 37.2, 78, "Улучшение"));

            int choice;
            do
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1 - добавить врача");
                Console.WriteLine("2 - добавить палату");
                Console.WriteLine("3 - добавить больного");
                Console.WriteLine("4 - записать состояние больного");
                Console.WriteLine("5 - параметры состояния больного");
                Console.WriteLine("6 - количество больных в палате");
                Console.WriteLine("7 - количество больных у врача");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");
                if (!int.TryParse(Console.ReadLine(), out choice)) choice = -1;
                switch (choice)
                {
                    case 1: AddDoctor(); break;
                    case 2: AddRoom(); break;
                    case 3: AddPatient(); break;
                    case 4: AddState(); break;
                    case 5: ShowPatientStates(); break;
                    case 6: CountInRoom(); break;
                    case 7: CountByDoctor(); break;
                }
            } while (choice != 0);
        }
    }
}
