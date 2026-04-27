using LibrarieModele;

namespace NivelStocareDate
{
public class AdministrareStudentiFisierText : IStocareData
{
    private const int ID_PRIMUL_STUDENT = 1;
    private const int INCREMENT = 1;
    private string numeFisier;

    public AdministrareStudentiFisierText(string numeFisier)
    {
        this.numeFisier = numeFisier;
        // se incearca deschiderea fisierului in modul OpenOrCreate
        // astfel incat sa fie creat daca nu exista
        Stream streamFisierText = File.Open(numeFisier, FileMode.OpenOrCreate);
        streamFisierText.Close();
    }

    public void AddStudent(Student student)
    {
        student.IdStudent = GetNextIdStudent();

        // instructiunea 'using' va apela la final streamWriterFisierText.Close();
        // al doilea parametru setat la 'true' al constructorului StreamWriter indica
        // modul 'append' de deschidere al fisierului
        using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, true))
        {
            streamWriterFisierText.WriteLine(student.ConversieLaSirPentruFisier());
        }
    }

    public List<Student> GetStudenti()
    {
        List<Student> studenti = new List<Student>();

        // instructiunea 'using' va apela streamReader.Close()
        using (StreamReader streamReader = new StreamReader(numeFisier))
        {
            string linieFisier;

            // citeste cate o linie si creaza un obiect de tip Student
            // pe baza datelor din linia citita
            while ((linieFisier = streamReader.ReadLine()) != null)
            {
                studenti.Add(new Student(linieFisier));
            }
        }

        return studenti;
    }

        public Student GetStudent(string nume, string prenume)
        {
            // instructiunea 'using' va apela streamReader.Close()
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;

                // citeste cate o linie si creaza un obiect de tip Student
                // pe baza datelor din linia citita
                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    Student student = new Student(linieFisier);
                    if (student.Nume.Equals(nume) && student.Prenume.Equals(prenume))
                        return student;
                }
            }

            return null;
        }

        public List<Student> CautaStudentiDupaNume(string nume)
        {
            List<Student> studentiGasiti = new List<Student>();

            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;

                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    Student student = new Student(linieFisier);
                    if (student.Nume != null &&
                        student.Nume.Contains(nume, StringComparison.OrdinalIgnoreCase))
                    {
                        studentiGasiti.Add(student);
                    }
                }
            }

            return studentiGasiti;
        }

        public Student GetStudent(int idStudent)
        {
            // instructiunea 'using' va apela streamReader.Close()
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;

                // citeste cate o linie si creaza un obiect de tip Student
                // pe baza datelor din linia citita
                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    Student student = new Student(linieFisier);
                    if (student.IdStudent == idStudent)
                        return student;
                }
            }

            return null;
        }

        public bool UpdateStudent(Student studentActualizat)
        {
            List<Student> studenti = GetStudenti();
            bool actualizareCuSucces = false;

            //instructiunea 'using' va apela la final swFisierText.Close();
            //al doilea parametru setat la 'false' al constructorului StreamWriter indica modul 'overwrite' de deschidere al fisierului
            using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, false))
            {
                foreach (Student student in studenti)
                {
                    Student studentPentruScrisInFisier = student;
                    //informatiile despre studentul actualizat vor fi preluate din parametrul "studentActualizat"
                    if (student.IdStudent == studentActualizat.IdStudent)
                    {
                        studentPentruScrisInFisier = studentActualizat;
                    }
                    streamWriterFisierText.WriteLine(studentPentruScrisInFisier.ConversieLaSirPentruFisier());
                }
                actualizareCuSucces = true;
            }

            return actualizareCuSucces;
        }

        private int GetNextIdStudent()
        {
            int IdStudent = ID_PRIMUL_STUDENT;

            List<Student> studenti = GetStudenti();

            if (studenti.Count == 0)
            {
                return 1;
            }

            return studenti.Last().IdStudent + INCREMENT;

        }
    }
}
