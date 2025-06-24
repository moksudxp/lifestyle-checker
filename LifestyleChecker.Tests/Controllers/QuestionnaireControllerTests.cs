using LifestyleChecker.Controllers;
using LifestyleChecker.Models;
using LifestyleChecker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
        private const string _rulesJson = @"
        [
            {
                ""questionId"": 1,
                ""expectedAnswer"": true,
                ""ageScores"": [
                    { ""minAge"": 0, ""maxAge"": 21, ""score"": 1 },
                    { ""minAge"": 22, ""maxAge"": 40, ""score"": 2 },
                    { ""minAge"": 41, ""maxAge"": 120, ""score"": 3 }
                ]
            },
            {
                ""questionId"": 2,
                ""expectedAnswer"": true,
                ""ageScores"": [
                    { ""minAge"": 0, ""maxAge"": 65, ""score"": 2 },
                    { ""minAge"": 66, ""maxAge"": 120, ""score"": 3 }
                ]
            },
            {
                ""questionId"": 3,
                ""expectedAnswer"": false,
                ""ageScores"": [
                    { ""minAge"": 0, ""maxAge"": 21, ""score"": 1 },
                    { ""minAge"": 22, ""maxAge"": 40, ""score"": 3 },
                    { ""minAge"": 41, ""maxAge"": 65, ""score"": 2 },
                    { ""minAge"": 66, ""maxAge"": 120, ""score"": 1 }
                ]
            }
        ]";

        private QuestionnaireController _controller;
        private string _tempJsonPath;
        private JsonScoreCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            _tempJsonPath = Path.GetTempFileName();
            File.WriteAllText(_tempJsonPath, _rulesJson);

            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.ContentRootPath).Returns(Path.GetDirectoryName(_tempJsonPath)!);

            // Rename the temp file to scoringRules.json to match expectations
            var fullJsonPath = Path.Combine(Path.GetDirectoryName(_tempJsonPath)!, "scoringRules.json");
            File.Move(_tempJsonPath, fullJsonPath, true);

            _calculator = new JsonScoreCalculator(mockEnv.Object);

            _controller = new QuestionnaireController(_calculator);
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
