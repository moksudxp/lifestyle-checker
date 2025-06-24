using LifestyleChecker.Common;
using LifestyleChecker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace LifestyleChecker.Controllers
{
    public class QuestionnaireController : Controller
    {
        private readonly IScoreCalculator _scoreCalculator;

        public QuestionnaireController(IScoreCalculator scoreCalculator)
        {
            _scoreCalculator = scoreCalculator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Start()
        {
            string nhsNumber = TempData["NHSNUmber"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(nhsNumber))
                return RedirectToAction("Failure", "ClientInput");

            int age = int.Parse(TempData["Age"]?.ToString() ?? "0");

            var questionnaire = new Questionnaire
            {
                NHSNumber = nhsNumber,
                Age = age,
                Questions = new List<Question>
                {
                    new Question {ID = 1, Text = "Q1. Do you drink on more than 2 days a week?"},
                    new Question {ID = 2, Text = "Q2. Do you smoke?"},
                    new Question {ID = 3, Text = "Q3. Do you exercise more than 1 hour per week?"}
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

            int totalScore = this._scoreCalculator.CalculateScore(model);

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
    }
}
