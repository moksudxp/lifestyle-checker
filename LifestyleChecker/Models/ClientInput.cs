using LifestyleChecker.Common;
using System.ComponentModel.DataAnnotations;

namespace LifestyleChecker.Models
{
    public class ClientInput
    {
        [Required(ErrorMessage = "NHS Number is required")]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "NHS Number must be exactly 10 digits")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "NHS Number must contain only numbers")]

        public string NHSNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
    }
}
