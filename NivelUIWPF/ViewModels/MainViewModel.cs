using LibrarieModele;
using LibrarieModele.Enums;
using NivelStocareDate;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NivelUIWPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IStocareData adminStudenti;
        private Student studentCurent = new Student();
        private Student studentSelectat;
        private string numeCautat = string.Empty;
        private string mesajNrStudentiGasiti = string.Empty;

        public MainViewModel()
        {
            adminStudenti = StocareFactory.GetAdministratorStocare();

            Studenti = new ObservableCollection<Student>(adminStudenti.GetStudenti());
            StudentiGasiti = new ObservableCollection<Student>();

            SalveazaCommand = new RelayCommand(Salveaza, PoateSalva);
            ActualizeazaCommand = new RelayCommand(Actualizeaza, PoateActualiza);
            ReseteazaCommand = new RelayCommand(Reseteaza);
            CautaCommand = new RelayCommand(Cauta);
        }

        public ObservableCollection<Student> Studenti { get; }
        public ObservableCollection<Student> StudentiGasiti { get; }
        public IEnumerable<string> FormeFinantare => FormaFinantareEnum.Toate;

        public ICommand SalveazaCommand { get; }
        public ICommand ActualizeazaCommand { get; }
        public ICommand ReseteazaCommand { get; }
        public ICommand CautaCommand { get; }

        public Student StudentCurent
        {
            get => studentCurent;
            set { studentCurent = value; OnPropertyChanged(); }
        }

        public Student StudentSelectat
        {
            get => studentSelectat;
            set
            {
                studentSelectat = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EsteStudentSelectat));
                if (studentSelectat != null)
                    StudentCurent = studentSelectat;
            }
        }

        public bool EsteStudentSelectat => studentSelectat != null;

        public string NumeCautat
        {
            get => numeCautat;
            set { numeCautat = value; OnPropertyChanged(); }
        }

        public string MesajNrStudentiGasiti
        {
            get => mesajNrStudentiGasiti;
            set { mesajNrStudentiGasiti = value; OnPropertyChanged(); }
        }

        private bool PoateSalva()
        {
            return StudentCurent != null && StudentCurent.EsteValid;
        }

        private void Salveaza()
        {
            Student student = new Student();
            student.Nume = StudentCurent.Nume?.Trim();
            student.Prenume = StudentCurent.Prenume?.Trim();
            student.ExtrageNote(StudentCurent.NoteAfisare?.Trim() ?? string.Empty);
            student.ProgramSTD = StudentCurent.ProgramSTD;
            student.Discipline = new List<string>(StudentCurent.Discipline ?? new List<string>());
            student.FormaFinantare = StudentCurent.FormaFinantare ?? string.Empty;

            adminStudenti.AddStudent(student);
            ReincarcaStudenti();
        }

        private bool PoateActualiza()
        {
            return EsteStudentSelectat && StudentCurent != null && StudentCurent.EsteValid;
        }

        private void Actualizeaza()
        {
            adminStudenti.UpdateStudent(StudentCurent);
            ReincarcaStudenti();
            StudentSelectat = null;
            StudentCurent = new Student();
        }

        private void Reseteaza()
        {
            StudentSelectat = null;
            StudentCurent = new Student();
        }

        private void Cauta()
        {
            List<Student> gasiti = adminStudenti.CautaStudentiDupaNume((NumeCautat ?? string.Empty).Trim());
            StudentiGasiti.Clear();
            foreach (Student s in gasiti)
                StudentiGasiti.Add(s);
            MesajNrStudentiGasiti = $"Numar studenti gasiti: {gasiti.Count}";
        }

        private void ReincarcaStudenti()
        {
            Studenti.Clear();
            foreach (Student s in adminStudenti.GetStudenti())
                Studenti.Add(s);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
