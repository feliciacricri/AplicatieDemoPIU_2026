using LibrarieModele;
using NivelStocareDate;

namespace EvidentaStudenti
{
    class Program
    {
        public static void Main()
        {
            IStocareData adminStudenti = StocareFactory.GetAdministratorStocare();

            Student? studentNou = null;
            string optiune;

            List<Student> studenti = new List<Student>();


            do
            {
                Console.WriteLine("C. Citire informatii student de la tastatura");
                Console.WriteLine("I. Afisarea informatiilor despre ultimul student introdus");
                Console.WriteLine("A. Afisare studenti din lista");
                Console.WriteLine("Z. Afisare studenti fara note din lista");
                Console.WriteLine("S. Salvare student in lista");
                Console.WriteLine("X. Inchidere program");

                Console.WriteLine("Alegeti o optiune");
                optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

                switch (optiune)
                {
                    case "C":
                        studentNou = CitireStudentTastatura();
                        break;

                    case "I":
                        AfisareStudent(studentNou);
                        break;

                    case "A":
                        studenti = adminStudenti.GetStudenti();
                        AfisareStudenti(studenti);
                        break;
                    case "Z":
                        studenti = adminStudenti.GetStudenti();
                        AfisareStudentiFaraNote(studenti);
                        break;

                    case "S":
                        if (studentNou != null)
                        {
                            adminStudenti.AddStudent(studentNou);
                            Console.WriteLine("Student salvat.");
                        }
                        else
                        {
                            Console.WriteLine("Studentul nu a fost initializat");
                        }
                        break;

                    case "X":
                        Console.WriteLine("Aplicatia va fi inchisa");
                        return;

                    default:
                        Console.WriteLine("Optiune inexistenta");
                        break;
                }

            } while (optiune.ToUpper() != "X");

            Console.ReadKey();
        }

        public static Student CitireStudentTastatura()
        {
            Console.WriteLine("Introduceti numele");
            string nume = Console.ReadLine();

            Console.WriteLine("Introduceti prenumele");
            string prenume = Console.ReadLine();

            Student student = new Student(0, nume, prenume);

            Console.WriteLine("Introduceti numarul de note:");
            int.TryParse(Console.ReadLine(), out int nrNote);

            int[] note = new int[nrNote];
            for (int i = 0; i < nrNote; i++)
            {
                Console.Write($"Nota {i + 1}: ");
                bool rezultat = int.TryParse(Console.ReadLine(), out int nota);
                if (rezultat)
                {
                    note[i] = nota;
                }
                else
                {
                    Console.WriteLine("Nota invalida, se va seta la 0.");
                    note[i] = 0;
                }
            }
            student.SetNote(note);

            return student;
        }

        public static void AfisareStudent(Student? student)
        {
            Console.WriteLine(student?.Info());
        }

        public static void AfisareStudenti(List<Student> studenti)
        {
            Console.WriteLine("Studentii sunt:");

            foreach (Student student in studenti)
            {
                AfisareStudent(student);
            }
        }

        public static void AfisareStudentiFaraNote(List<Student> studenti)
        {
            Console.WriteLine("Studentii sunt:");

            var studentiSelectati = studenti
                                    .Where(student => student.GetNote().Length < 2);

            foreach (Student student in studentiSelectati)
            {
                AfisareStudent(student);
            }
        }
    }
}
