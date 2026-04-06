using NivelStocareDate;
using System.Configuration;
using System.IO;

namespace NivelUIWPF
{
    public static class StocareFactory
    {
        private const string FORMAT_SALVARE = "FormatSalvare";
        private const string NUME_FISIER = "NumeFisier";

        public static IStocareData GetAdministratorStocare()
        {
            string formatSalvare = ConfigurationManager.AppSettings[FORMAT_SALVARE] ?? "";

            string numeFisier = ConfigurationManager.AppSettings[NUME_FISIER] ?? "";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName ?? "";
            // setare locatie fisier in directorul corespunzator solutiei
            // astfel incat datele din fisier sa poata fi utilizate si de alte proiecte
            string caleCompletaFisier = locatieFisierSolutie + "\\" + numeFisier;

            
            if (formatSalvare != null)
            {
                switch (formatSalvare)
                {
                    default:
                    case "memorie":
                        return new AdministrareStudentiMemorie();
                    case "txt":
                        return new AdministrareStudentiFisierText(caleCompletaFisier + "." + formatSalvare);
                }
            }

            return null;
        }
    }
}
