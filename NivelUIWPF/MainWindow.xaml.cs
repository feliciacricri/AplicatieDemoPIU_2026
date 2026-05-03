using LibrarieModele;
using LibrarieModele.Enums;
using NivelStocareDate;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NivelUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int LUNGIME_MAXIMA_NUME = 15;

        private IStocareData adminStudenti;
        private readonly List<string> disciplineSelectate = new List<string>();
        private Student studentCurent;

        public Student StudentCurent
        {
            get => studentCurent;
            set
            {
                studentCurent = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            adminStudenti = StocareFactory.GetAdministratorStocare();
            lbFormaFinantare.ItemsSource = FormaFinantareEnum.Toate;
            AfiseazaStudenti();
        }

        private void AfiseazaStudenti()
        {
            List<Student> studenti = adminStudenti.GetStudenti();
            lblNrStudenti.Content = $"Numar studenti: {studenti.Count}";
            dgStudenti.ItemsSource = studenti;
        }

        private void btnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            string nume = txtNume.Text.Trim();
            string prenume = txtPrenume.Text.Trim();
            string sirNote = txtNote.Text.Trim();

            if (!ValideazaDateStudent(nume, prenume, sirNote))
            {
                return;
            }

            Student student = new Student();
            student.Nume = nume;
            student.Prenume = prenume;
            student.ExtrageNote(sirNote);
            student.ProgramSTD = GetProgramSelectat();
            student.Discipline = new List<string>(disciplineSelectate);
            student.FormaFinantare = lbFormaFinantare.SelectedItem as string ?? string.Empty;

            adminStudenti.AddStudent(student);

            AfiseazaStudenti();
        }

        private void btnActualizeaza_Click(object sender, RoutedEventArgs e)
        {
            ReseteazaErori();

            if (string.IsNullOrEmpty(StudentCurent.Nume))
            {
                AfiseazaEroare(txtNume, tbErrNume, "Numele trebuie completat!");
                return;
            }

            adminStudenti.UpdateStudent(StudentCurent);

            AfiseazaStudenti();
            dgStudenti.SelectedItem = null;
            btnActualizeaza.IsEnabled = false;
            tbWarningActualizare.Visibility = Visibility.Collapsed;
        }

        private void btnReseteaza_Click(object sender, RoutedEventArgs e)
        {
            dgStudenti.SelectedItem = null;
            btnActualizeaza.IsEnabled = false;
            tbWarningActualizare.Visibility = Visibility.Collapsed;
            txtNume.Clear();
            txtPrenume.Clear();
            txtNote.Clear();
            rbCalculatoare.IsChecked = true;
            cbPIU.IsChecked = false;
            cbPCLP.IsChecked = false;
            cbPOO.IsChecked = false;
            cbDCE.IsChecked = false;
            cbFizica.IsChecked = false;
            disciplineSelectate.Clear();
            lbFormaFinantare.SelectedIndex = -1;
            ReseteazaErori();
        }

        private void btnCauta_Click(object sender, RoutedEventArgs e)
        {
            string numeCautat = txtCautareNume.Text.Trim();
            List<Student> studentiGasiti = adminStudenti.CautaStudentiDupaNume(numeCautat);
            lblNrStudentiGasiti.Content = $"Numar studenti gasiti: {studentiGasiti.Count}";
            dgStudentiGasiti.ItemsSource = studentiGasiti;
        }

        private void dgStudenti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StudentCurent = dgStudenti.SelectedItem as Student;
            if (StudentCurent == null) return;

            btnActualizeaza.IsEnabled = true;
            tbWarningActualizare.Visibility = Visibility.Visible;
        }

        private void btnMeniuAdauga_Click(object sender, RoutedEventArgs e)
        {
            panelAdauga.Visibility = Visibility.Visible;
            panelCauta.Visibility = Visibility.Collapsed;
        }

        private void btnMeniuCauta_Click(object sender, RoutedEventArgs e)
        {
            panelAdauga.Visibility = Visibility.Collapsed;
            panelCauta.Visibility = Visibility.Visible;
        }

        private ProgramStudiu GetProgramSelectat()
        {
            if (rbAutomatica.IsChecked == true)
                return ProgramStudiu.Automatica;
            if (rbElectronica.IsChecked == true)
                return ProgramStudiu.Electronica;
            if (rbElectrotehnica.IsChecked == true)
                return ProgramStudiu.Electrotehnica;
            if (rbEnergetica.IsChecked == true)
                return ProgramStudiu.Energetica;
            if (rbInginerieEconomica.IsChecked == true)
                return ProgramStudiu.InginerieEconomica;

            // Optiunea implicita este Calculatoare
            return ProgramStudiu.Calculatoare;
        }

        private void Disciplina_CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb == null) return;

            string disciplina = cb.Content?.ToString() ?? string.Empty;

            if (cb.IsChecked == true)
            {
                if (!disciplineSelectate.Contains(disciplina))
                    disciplineSelectate.Add(disciplina);
            }
            else
            {
                disciplineSelectate.Remove(disciplina);
            }
        }

        private bool ValideazaDateStudent(string nume, string prenume, string sirNote)
        {
            ReseteazaErori();

            if (string.IsNullOrEmpty(nume))
            {
                AfiseazaEroare(txtNume, tbErrNume, "Numele trebuie completat!");
                return false;
            }

            if (nume.Length > LUNGIME_MAXIMA_NUME)
            {
                AfiseazaEroare(txtNume, tbErrNume, $"Numele nu poate depasi {LUNGIME_MAXIMA_NUME} caractere!");
                return false;
            }

            if (string.IsNullOrEmpty(prenume))
            {
                AfiseazaEroare(txtPrenume, tbErrPrenume, "Prenumele trebuie completat!");
                return false;
            }

            if (prenume.Length > LUNGIME_MAXIMA_NUME)
            {
                AfiseazaEroare(txtPrenume, tbErrPrenume, $"Prenumele nu poate depasi {LUNGIME_MAXIMA_NUME} caractere!");
                return false;
            }

            if (string.IsNullOrEmpty(sirNote))
            {
                AfiseazaEroare(txtNote, tbErrNote, "Sirul de note trebuie completat!");
                return false;
            }

            return true;
        }

        private void ReseteazaErori()
        {
            AscundeEroare(txtNume, tbErrNume);
            AscundeEroare(txtPrenume, tbErrPrenume);
            AscundeEroare(txtNote, tbErrNote);
        }

        private void AscundeEroare(TextBox textBox, TextBlock tbEroare)
        {
            textBox.ClearValue(Control.BorderBrushProperty);
            textBox.ClearValue(Control.BackgroundProperty);
            tbEroare.Text = string.Empty;
            tbEroare.Visibility = Visibility.Collapsed;
        }

        private void AfiseazaEroare(TextBox textBox, TextBlock tbEroare, string mesaj)
        {
            textBox.BorderBrush = Brushes.Red;
            textBox.Background = new SolidColorBrush(Color.FromRgb(255, 230, 230));
            tbEroare.Text = mesaj;
            tbEroare.Visibility = Visibility.Visible;
            textBox.Focus();
        }
    }
}
