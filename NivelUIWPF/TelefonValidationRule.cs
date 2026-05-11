using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace NivelUIWPF
{
    public class TelefonValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string telefon = value as string;

            if (string.IsNullOrWhiteSpace(telefon))
                return new ValidationResult(false, "Telefonul este obligatoriu");

            if (telefon.Length != 10)
                return new ValidationResult(false, "Telefonul trebuie sa aiba 10 cifre");

            if (!telefon.All(char.IsDigit))
                return new ValidationResult(false, "Telefonul trebuie sa contina doar cifre");

            return ValidationResult.ValidResult;
        }
    }
}
