using LibrarieModele;

namespace NivelStocareDate
{
    //definitia interfetei
    public interface IStocareData
    {
        void AddStudent(Student s);
        List<Student> GetStudenti();
        Student GetStudent(int idStudent);
        Student GetStudent(string nume, string prenume);
        bool UpdateStudent(Student s);
    }
}
