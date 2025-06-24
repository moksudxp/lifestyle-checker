using LifestyleChecker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace LifestyleChecker.Controllers
{
    public class QuestionnaireController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Start()
        {
            string nhsNumber = TempData["NHSNUmber"].ToString();
            if (string.IsNullOrWhiteSpace(nhsNumber))
                return RedirectToAction("Failure", "ClientInput");

            int age = int.Parse(TempData["Age"].ToString());

            var questionnaire = new Questionnaire
            {
                NHSNumber = nhsNumber,
                Age = age,
                Questions = new List<Question>
                {
                    new Question {ID = 1, Text = "Q1. Do you drink on more than 2 days a week?", ScoreIfYes = 1, ScoreIfNo = 0},
                    new Question {ID = 2, Text = "Q2. Do you smoke?", ScoreIfYes = 1, ScoreIfNo = 0},
                    new Question {ID = 3, Text = "Q3. Do you exercise more than 1 hour per week?", ScoreIfYes = 0, ScoreIfNo = 1}
                }
            };

            return View("Index", questionnaire);
        }

        [HttpPost]
        public IActionResult Submit(Questionnaire model)
        {
            if(!this.AllQuestionsAnswered(model))
            {
                ModelState.AddModelError("", "All questions must be answered.");
                return View("Questionnaire", model);
            }

            int totalScore = this.CalculateScore(model);

            ViewBag.UserName = model.NHSNumber;
            ViewBag.TotalScore = totalScore;

            return View("Result");
        }

        public bool AllQuestionsAnswered(Questionnaire model)
        {
            foreach (var question in model.Questions)
            {
                if (!model.Answers.ContainsKey(question.ID)) return false;
            }

            return true;
        }

        public int CalculateScore(Questionnaire model)
        {
            int totalScore = 0;
            if (model.Questions.Count == 0 || model.Answers.Count == 0) return 0;
            
            foreach (var question in model.Questions)
            {
                if (question == null) continue;
                if (!model.Answers.ContainsKey(question.ID)) continue;

                bool answer =  model.Answers[question.ID];
                int score = 0;

                // Q1
                if (question.ID == 1 && answer)
                {
                    score = model.Age switch
                    {
                        <= 21 => 1,
                        <= 40 => 2,
                        <= 65 => 3,
                        _ => 3
                    };
                }

                // Q2
                else if (question.ID == 2 && answer)
                {
                    score = model.Age switch
                    {
                        <= 21 => 2,
                        <= 40 => 2,
                        <= 65 => 2,
                        _ => 3
                    };
                }

                // Q3 (points awarded for NO)
                else if (question.ID == 3 && !answer)
                {
                    score = model.Age switch
                    {
                        <= 21 => 1,
                        <= 40 => 3,
                        <= 65 => 2,
                        _ => 1
                    };
                }

                totalScore += score;
            }

            return totalScore;
        }
    }
}
