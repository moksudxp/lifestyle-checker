using LifestyleChecker.Services;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace LifestyleChecker.Models
{
    public class PatientInfo
    {
        static private char _firstNameLastNameSeparator = ',';

        [JsonProperty("nhsNumber")]
        public string NHSNumber { get; set; } = string.Empty;
        [JsonProperty("name")]
        public string FullName { get; set; } = string.Empty;
        [JsonProperty("born")]
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;

        public int Age { get { return CalculatorService.GetAgeFromDateOfBirth(this.DateOfBirth, DateTime.Today); } }

        public string Firstname
        {
            get
            {
                int indexOfSeparator = this.FullName.IndexOf(_firstNameLastNameSeparator);
                
                if (indexOfSeparator == -1) return this.FullName;
                return this.FullName.Substring(0, indexOfSeparator);
            }
        }
        public string Lastname
        {
            get
            {
                int indexOfSeparator = this.FullName.LastIndexOf(_firstNameLastNameSeparator);

                if (indexOfSeparator == -1) return this.FullName;
                return this.FullName.Substring(indexOfSeparator+1, this.FullName.Length-indexOfSeparator-1);
            }
        }
    }
}
