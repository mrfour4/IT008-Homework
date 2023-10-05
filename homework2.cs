namespace Homewrok_2
{
    internal class Program
    {
        class Student
        {
            public string IdCourse { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public double MathScore { get; set; }
            public double LiteratureScore { get; set; }
            public double EnglishScore { get; set; }
            public double AverageScore
            {
                get => Math.Round((MathScore + LiteratureScore + EnglishScore) / 3, 2);
            }

            public Student(string idCourse, string id, string name, double mathScore, double literatureScore, double englishScore)
            {
                IdCourse = idCourse;
                Id = id;
                Name = name;
                MathScore = mathScore;
                LiteratureScore = literatureScore;
                EnglishScore = englishScore;
            }

            public override string ToString()
            {
                return $"{IdCourse}, {Id}, {Name}, {MathScore}, {LiteratureScore}, {EnglishScore}, {AverageScore}";
            }
        }

        class ManageStudents
        {
            private List<Student> ListStudents;

            public ManageStudents()
            {
                ListStudents = new List<Student>();
            }

            public void AddStudent(Student student)
            {
                ListStudents.Add(student);
            }

            public void RemoveStudent(string id)
            {
                foreach (var student in ListStudents)
                {
                    if (student.Id == id)
                    {
                        ListStudents.Remove(student);
                        return;
                    }
                }
            }

            public Student SearchStudent(string idOrName)
            {
                foreach (Student student in ListStudents)
                {
                    if (student.Id == idOrName || student.Name == idOrName)
                    {
                        return student;
                    }
                }

                return null;
            }

            public List<Student> TopStudent(int n, bool top = false)
            {
                var topStudents = ListStudents.OrderByDescending(student => student.AverageScore);
                if (!top)
                {
                    var result = topStudents.TakeLast(n).ToList();
                    result.Reverse();
                    return result;
                }

                return topStudents.Take(n).ToList();
            }

            public void PrintList()
            {
                foreach (var student in ListStudents)
                {
                    Console.WriteLine(student.ToString());
                }
                Console.WriteLine();
            }

            public void SaveData(string path)
            {
                try
                {
                    using (var writer = new StreamWriter(path))
                    {
                        foreach (var student in ListStudents)
                        {
                            writer.WriteLine(student);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            public bool LoadData(string path)
            {
                if (!File.Exists(path)) return false;
                try
                {
                    using (var reader = new StreamReader(path))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] data = line.Split(',').Select(s => s.Trim()).ToArray();
                            ListStudents.Add(new Student(data[0], data[1], data[2], double.Parse(data[3]), double.Parse(data[4]), double.Parse(data[5])));
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            public void ExportData(string idCourse, string path)
            {
                try
                {
                    using (var writer = new StreamWriter(path))
                    {
                        writer.WriteLine($"STT, Ma lop, MSSV, Ten, Diem Toan, Diem Van, Diem Anh, DTB");

                        int i = 0;
                        foreach (var student in ListStudents)
                        {
                            if (idCourse == "ALL" || student.IdCourse == idCourse)
                            {
                                writer.WriteLine($"{++i}, {student}");

                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        static void printList(List<Student> list)
        {

            foreach (var student in list)
            {
                Console.WriteLine(student.ToString());
            }

        }

        static void Main(string[] args)
        {
            string action = args[0].ToLower();

            ManageStudents manage = new ManageStudents();

            const string PATH = "listStudent.csv";
            manage.LoadData(PATH);

            switch (action)
            {
                case "all":
                    manage.PrintList();
                    break;

                case "add":
                    manage.AddStudent(new Student(args[1], args[2], args[3], double.Parse(args[4]), double.Parse(args[5]), double.Parse(args[6])));
                    break;

                case "remove":
                    manage.RemoveStudent(args[1]);
                    break;

                case "search":
                    Student student = manage.SearchStudent(args[1]);
                    Console.WriteLine(student);
                    break;

                case "top":
                    var list = manage.TopStudent(int.Parse(args[1]), bool.Parse(args[2]));
                    printList(list);
                    break;

                case "export":
                    manage.ExportData(args[1].ToUpper(), args[2]);
                    break;

                default:
                    Console.WriteLine("Action exception");
                    break;
            }

            manage.SaveData(PATH);
        }
    }
}