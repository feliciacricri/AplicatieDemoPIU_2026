using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareStudentiMemorie : IStocareData
    {
        private List<Student> studenti;

        public AdministrareStudentiMemorie()
        {
            studenti = new List<Student>();
        }

        public void AddStudent(Student student)
        {
            student.IdStudent = GetNextIdStudent();
            studenti.Add(student);
        }

        public List<Student> GetStudenti()
        {
            return studenti;
        }

        public Student? GetStudent(int idStudent)
        {
            foreach (Student student in studenti)
            {
                if (student.IdStudent == idStudent)
                {
                    return student;
                }
            }

            return null;
        }

        public Student? GetStudent(string nume, string prenume)
        {
            return studenti?.FirstOrDefault(student =>
                student.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase) &&
                student.Prenume.Equals(prenume, StringComparison.OrdinalIgnoreCase)
            );
        }

        public List<Student> CautaStudentiDupaNume(string nume)
        {
            return studenti
                .Where(student => student.Nume != null &&
                                  student.Nume.Contains(nume, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public bool UpdateStudent(Student s)
        {
            throw new Exception("Optiunea UpdateStudent nu este implementata");
        }

        public int GetNextIdStudent()
        {
            if (studenti.Count == 0)
            {
                return 1;
            }

            return studenti.Last().IdStudent + 1;
        }
    }
}