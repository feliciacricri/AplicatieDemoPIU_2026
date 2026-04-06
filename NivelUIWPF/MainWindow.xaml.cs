using LibrarieModele;
using NivelStocareDate;
using System.Windows;

namespace NivelUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IStocareData adminStudenti = StocareFactory.GetAdministratorStocare();
            List<Student> studenti = adminStudenti.GetStudenti();
            lblNrStudenti.Content = $"Numar studenti: {studenti.Count}";
            lblStudenti.Content = "Studenti:\n" + string.Join("\n", studenti.Select(s => $"{s.IdStudent}: {s.Nume} {s.Prenume}"));
        }
    }
}
