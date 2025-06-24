namespace LifestyleChecker.Services
{
    public static class CalculatorService
    {
        public static int GetAgeFromDateOfBirth(DateTime dob, DateTime today)
        {
            int age = today.Year - dob.Year;

            // Adjusting if birthday hasn't occured yet this year
            if (dob.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}