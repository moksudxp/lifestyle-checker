namespace LifestyleChecker.Models
{
    public class Questionnaire
    {
        public string NHSNumber { get; set; }
        public int Age { get; set; }
        public List<Question> Questions { get;set; }
        public Dictionary<int, bool> Answers { get; set; } = new();
    }
}
