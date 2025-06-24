namespace LifestyleChecker.Models
{
    public class ScoringRule
    {
        public int QuestionId { get; set; }
        public bool ExpectedAnswer { get; set; }
        public List<AgeScoreRange> AgeScores { get; set; }
    }

    public class AgeScoreRange
    {
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int Score { get; set; }
    }
}
