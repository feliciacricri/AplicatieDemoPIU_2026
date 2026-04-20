using LibrarieModele;
using LibrarieModele.Enums;
using NivelStocareDate;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NivelUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int LUNGIME_MAXIMA_NUME = 15;

        private IStocareData adminStudenti;

        public MainWindow()
        {
            InitializeComponent();
            adminStudenti = StocareFactory.GetAdministratorStocare();
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

            adminStudenti.AddStudent(student);

            AfiseazaStudenti();
        }

        private void btnReseteaza_Click(object sender, RoutedEventArgs e)
        {
            txtNume.Clear();
            txtPrenume.Clear();
            txtNote.Clear();
            rbCalculatoare.IsChecked = true;
            ReseteazaErori();
        }

        private ProgramStudiu GetProgramSelectat()
        {
            if (rbAutomatica.IsChecked == true)
                return ProgramStudiu.Automatica;
            if (rbElectronica.IsChecked == true)
                return ProgramStudiu.Electronica;

            // Optiunea implicita este Calculatoare
            return ProgramStudiu.Calculatoare;
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
