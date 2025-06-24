namespace LifestyleChecker.Models
{
    public class Question
    {
        public int ID { get; set; }
        public string Text {  get; set; }
        public int ScoreIfYes { get; set; } = 1;
        public int ScoreIfNo { get; set; } = 0;
    }
}
