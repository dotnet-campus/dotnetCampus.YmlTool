using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace YmlTool
{
    public class FilePathValidation:ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var dir = value?.ToString();
            if (File.Exists(dir))
            {
                return new ValidationResult(true,null);
            }

            return new ValidationResult(false,"Invalid File Path ");
        }
    }

    public class DirectoryPathValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var dir = value?.ToString();
            if (Directory.Exists(dir))
            {
                return new ValidationResult(true, null);
            }

            return new ValidationResult(false, "Invalid Directory Path ");
        }
    }

    public class DirectoryOrFilePathValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var dir = value?.ToString();
            if (File.Exists(dir)||Directory.Exists(dir))
            {
                return new ValidationResult(true, null);
            }

            return new ValidationResult(false, "Invalid Path ");
        }
    }
}
