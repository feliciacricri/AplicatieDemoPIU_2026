using LibrarieModele;
using LibrarieModele.Enums;
using NivelStocareDate;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace NivelUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IStocareData adminStudenti;
        private Student studentCurent = new Student();

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
            dgStudenti.ItemsSource = studenti;
        }

        private void btnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (!EsteFormularValid()) return;

            Student student = new Student();
            student.Nume = txtNume.Text.Trim();
            student.Prenume = txtPrenume.Text.Trim();
            student.ExtrageNote(txtNote.Text.Trim());
            student.ProgramSTD = GetProgramSelectat();
            student.Discipline = GetDisciplineBifate();
            student.FormaFinantare = lbFormaFinantare.SelectedItem as string ?? string.Empty;

            adminStudenti.AddStudent(student);

            AfiseazaStudenti();
        }

        private void btnActualizeaza_Click(object sender, RoutedEventArgs e)
        {
            if (!EsteFormularValid()) return;

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
            lbFormaFinantare.SelectedIndex = -1;
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

        private List<string> GetDisciplineBifate()
        {
            List<string> lista = new List<string>();
            if (cbPIU.IsChecked == true) lista.Add(cbPIU.Content.ToString());
            if (cbPCLP.IsChecked == true) lista.Add(cbPCLP.Content.ToString());
            if (cbPOO.IsChecked == true) lista.Add(cbPOO.Content.ToString());
            if (cbDCE.IsChecked == true) lista.Add(cbDCE.Content.ToString());
            if (cbFizica.IsChecked == true) lista.Add(cbFizica.Content.ToString());
            return lista;
        }

        private bool EsteFormularValid()
        {
            foreach (var textBox in new[] { txtNume, txtPrenume, txtNote })
            {
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
                if (Validation.GetHasError(textBox))
                {
                    textBox.Focus();
                    return false;
                }
            }
            return true;
        }
    }
}
