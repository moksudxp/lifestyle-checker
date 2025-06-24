using LifestyleChecker.Common;
using LifestyleChecker.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace LifestyleChecker.Services
{
    public class JsonScoreCalculator : IScoreCalculator
    {
        private readonly List<ScoringRule>? _rules;
        
        public JsonScoreCalculator(IWebHostEnvironment environment)
        {
            string? path = Path.Combine(environment.ContentRootPath, "scoringRules.json");
            string? json = File.ReadAllText(path);

            try
            {
                _rules = JsonConvert.DeserializeObject<List<ScoringRule>>(json);
            }
            catch (JsonReaderException e)
            {
                _rules = new List<ScoringRule>();
                throw;
            }
        }

        public int CalculateScore(Questionnaire questionnaire)
        {
            int totalScore = 0;

            foreach (var question in questionnaire.Questions)
            {
                if (!questionnaire.Answers.TryGetValue(question.ID, out bool answer))
                    continue;

                ScoringRule? rule = _rules.FirstOrDefault(r =>
                    r.QuestionId == question.ID && r.ExpectedAnswer == answer);

                if (rule == null) continue;

                AgeScoreRange? ageScore = rule.AgeScores.FirstOrDefault(r =>
                    questionnaire.Age >= r.MinAge && questionnaire.Age <= r.MaxAge);

                if (ageScore != null)
                {
                    totalScore += ageScore.Score;
                }
            }

            return totalScore;
        }
    }
}
