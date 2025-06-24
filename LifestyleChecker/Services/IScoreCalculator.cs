using LifestyleChecker.Models;

namespace LifestyleChecker.Common
{
    public interface IScoreCalculator
    {
        int CalculateScore(Questionnaire questionnaire);
    }
}
