using LifestyleChecker.Controllers;
using LifestyleChecker.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifestyleChecker.Tests.Controllers
{
    [TestFixture]
    public class QuestionnaireControllerTests
    {
        private QuestionnaireController _controller;

        [SetUp]
        public void Setup()
        {
            this._controller = new QuestionnaireController();
        }

        [TearDown]
        public void Dispose()
        {
            this._controller.Dispose();
        }

        [Test]
        public void AllQuestionsAnswered_ShouldReturnTrue_WhenAllQuestionsHaveBeenAnswered()
        {
            Questionnaire questionnnaire = new Questionnaire();
            questionnnaire.Questions = new List<Question>() { new Question { ID = 1, Text = "Q1" }, new Question { ID = 2, Text = "Q2" }, new Question { ID = 3, Text = "Q3" } };
            questionnnaire.Answers = new Dictionary<int, bool>() { { 1, true }, { 2, true }, { 3, false } };

            Assert.IsTrue(this._controller.AllQuestionsAnswered(questionnnaire));
        }

        [Test]
        public void AllQuestionsAnswered_ShouldReturnFalse_WhenOneQuestionHasNotBeenAnswered()
        {
            Questionnaire questionnnaire = new Questionnaire();
            questionnnaire.Questions = new List<Question>() { new Question { ID = 1, Text = "Q1" }, new Question { ID = 2, Text = "Q2" }, new Question { ID = 3, Text = "Q3" } };
            questionnnaire.Answers = new Dictionary<int, bool>() { { 1, true }, { 2, true }, };

            Assert.IsFalse(this._controller.AllQuestionsAnswered(questionnnaire));
        }

        [Test]
        public void AllQuestionsAnswered_ShouldReturnFalse_WhenTwoQuestionsHasNotBeenAnswered()
        {
            Questionnaire questionnnaire = new Questionnaire();
            questionnnaire.Questions = new List<Question>() { new Question { ID = 1, Text = "Q1" }, new Question { ID = 2, Text = "Q2" }, new Question { ID = 3, Text = "Q3" } };
            questionnnaire.Answers = new Dictionary<int, bool>() { { 1, true }, };

            Assert.IsFalse(this._controller.AllQuestionsAnswered(questionnnaire));
        }

        [Test]
        public void AllQuestionsAnswered_ShouldReturnFalse_WhenThreeQuestionsHasNotBeenAnswered()
        {
            Questionnaire questionnnaire = new Questionnaire();
            questionnnaire.Questions = new List<Question>() { new Question { ID = 1, Text = "Q1" }, new Question { ID = 2, Text = "Q2" }, new Question { ID = 3, Text = "Q3" } };
            questionnnaire.Answers = new Dictionary<int, bool>() { };

            Assert.IsFalse(this._controller.AllQuestionsAnswered(questionnnaire));
        }

        [Test]
        public void CalculateScore_ShouldReturn0_WhenQuestionnaireIsEmpty()
        {
            Questionnaire questionnnaire = new Questionnaire();
            questionnnaire.Questions = new List<Question>() { };
            questionnnaire.Answers = new Dictionary<int, bool>() { };

            Assert.That(this._controller.CalculateScore(questionnnaire), Is.EqualTo(0));
        }

        [Test]
        public void CalculateScore_ShouldReturn0_WhenQuestionnaireContainsOnlyNullObject()
        {
            Questionnaire questionnnaire = new Questionnaire();
            questionnnaire.Questions = new List<Question>() { new Question { } };
            questionnnaire.Answers = new Dictionary<int, bool>() { };

            Assert.That(this._controller.CalculateScore(questionnnaire), Is.EqualTo(0));
        }

        [Test]
        public void CalculateScore_Q1_Yes_AgeUnder21_Returns1()
        {
            var model = new Questionnaire
            {
                Age = 20,
                Questions = new List<Question>
                {
                    new Question { ID = 1 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 1, true }
                }
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculateScore_Q1_Yes_AgeBetween22And40_Returns2()
        {
            var model = new Questionnaire
            {
                Age = 35,
                Questions = new List<Question>
                {
                    new Question { ID = 1 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 1, true }
                }
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CalculateScore_Q1_Yes_AgeBetween41And65_Returns3()
        {
            var model = new Questionnaire
            {
                Age = 60,
                Questions = new List<Question>
                {
                    new Question { ID = 1 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 1, true }
                }
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void CalculateScore_Q2_Yes_AgeUnder65_Returns2()
        {
            var model = new Questionnaire
            {
                Age = 40,
                Questions = new List<Question>
                {
                    new Question { ID = 2 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 2, true }
                }
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CalculateScore_Q2_Yes_AgeOver65_Returns3()
        {
            var model = new Questionnaire
            {
                Age = 70,
                Questions = new List<Question>
                {
                    new Question { ID = 2 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 2, true }
                }
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void CalculateScore_Q3_No_AgeUnder21_Returns1()
        {
            var model = new Questionnaire
            {
                Age = 18,
                Questions = new List<Question>
                {
                    new Question { ID = 3 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 3, false }
                }
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculateScore_Q3_No_AgeBetween22And40_Returns3()
        {
            var model = new Questionnaire
            {
                Age = 30,
                Questions = new List<Question>
                {
                    new Question { ID = 3 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 3, false }
                }
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void CalculateScore_Q3_No_AgeBetween41And65_Returns2()
        {
            var model = new Questionnaire
            {
                Age = 55,
                Questions = new List<Question>
                {
                    new Question { ID = 3 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 3, false }
                }
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CalculateScore_Q3_No_AgeOver65_Returns1()
        {
            var model = new Questionnaire
            {
                Age = 75,
                Questions = new List<Question>
                {
                    new Question { ID = 3 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 3, false }
                }
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CalculateScore_NoAnswersOrQuestions_Returns0()
        {
            var model = new Questionnaire
            {
                Age = 50,
                Questions = new List<Question>(),
                Answers = new Dictionary<int, bool>()
            };

            var result = _controller.CalculateScore(model);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Submit_AllQuestionsAnswered_ReturnsResultViewWithCorrectScore()
        {
            // Arrange
            var model = new Questionnaire
            {
                NHSNumber = "1234567890",
                Age = 30,
                Questions = new List<Question>
                {
                    new Question { ID = 1 },
                    new Question { ID = 2 },
                    new Question { ID = 3 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 1, true },  // Q1 yes -> 2 points
                    { 2, true },  // Q2 yes -> 2 points
                    { 3, false }  // Q3 no -> 3 points
                }
            };

            // Act
            var result = _controller.Submit(model) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Result"));
            Assert.That(_controller.ViewBag.UserName, Is.EqualTo("1234567890"));
            Assert.That(_controller.ViewBag.TotalScore, Is.EqualTo(7));
        }

        [Test]
        public void Submit_MissingAnswers_ReturnsQuestionnaireViewWithModelError()
        {
            // Arrange
            var model = new Questionnaire
            {
                NHSNumber = "9999999999",
                Age = 30,
                Questions = new List<Question>
                {
                    new Question { ID = 1 },
                    new Question { ID = 2 },
                    new Question { ID = 3 }
                },
                Answers = new Dictionary<int, bool>
                {
                    { 1, true },  // Missing Q2 and Q3
                }
            };

            // Act
            var result = _controller.Submit(model) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Questionnaire"));
            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(_controller.ModelState[string.Empty].Errors.Count, Is.EqualTo(1));
            Assert.That(_controller.ModelState[string.Empty].Errors[0].ErrorMessage, Is.EqualTo("All questions must be answered."));
        }

        [Test]
        public void Submit_EmptyModel_ReturnsZeroScore()
        {
            var model = new Questionnaire
            {
                NHSNumber = "0000000000",
                Age = 45,
                Questions = new List<Question>(),
                Answers = new Dictionary<int, bool>()
            };

            var result = _controller.Submit(model) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Result"));
            Assert.That(_controller.ViewBag.TotalScore, Is.EqualTo(0));
        }
    }
}
