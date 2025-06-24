using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifestyleChecker.Models;
using LifestyleChecker.Services;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System.Text.Json;
using Moq;

namespace LifestyleChecker.Tests.Services
{
    [TestFixture]
    public class JsonScoreCalculatorTests
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

        private const string _rulesJsonMods = @"
        [
            {
                ""questionId"": 1,
                ""expectedAnswer"": true,
                ""ageScores"": [
                    { ""minAge"": 0, ""maxAge"": 21, ""score"": 20 },
                    { ""minAge"": 22, ""maxAge"": 40, ""score"": 20 },
                    { ""minAge"": 41, ""maxAge"": 120, ""score"": 20 }
                ]
            },
            {
                ""questionId"": 2,
                ""expectedAnswer"": true,
                ""ageScores"": [
                    { ""minAge"": 0, ""maxAge"": 65, ""score"": 20 },
                    { ""minAge"": 66, ""maxAge"": 120, ""score"": 20 }
                ]
            },
            {
                ""questionId"": 3,
                ""expectedAnswer"": false,
                ""ageScores"": [
                    { ""minAge"": 0, ""maxAge"": 21, ""score"": 20 },
                    { ""minAge"": 22, ""maxAge"": 40, ""score"": 20 },
                    { ""minAge"": 41, ""maxAge"": 65, ""score"": 20 },
                    { ""minAge"": 66, ""maxAge"": 120, ""score"": 20 }
                ]
            }
        ]";

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
        }

        [TearDown]
        public void Cleanup()
        {
            File.Delete(Path.Combine(Path.GetDirectoryName(_tempJsonPath)!, "scoringRules.json"));
        }

        [Test]
        public void Calculates_Correct_Score_For_Question1_Age_25_AnswerYes()
        {
            var questionnaire = CreateQuestionnaire(25, new Dictionary<int, bool> { { 1, true } });
            var result = _calculator.CalculateScore(questionnaire);
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void Calculates_Correct_Score_For_Question2_Age_70_AnswerYes()
        {
            var questionnaire = CreateQuestionnaire(70, new Dictionary<int, bool> { { 2, true } });
            var result = _calculator.CalculateScore(questionnaire);
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Calculates_Correct_Score_For_Question3_Age_30_AnswerNo()
        {
            var questionnaire = CreateQuestionnaire(30, new Dictionary<int, bool> { { 3, false } });
            var result = _calculator.CalculateScore(questionnaire);
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Calculates_Correct_Score_For_Question1_Age_25_AnswerYes_ModifiedRules()
        {
            var _tempJsonPath_ModifiedRules = Path.GetTempFileName();
            File.WriteAllText(_tempJsonPath_ModifiedRules, _rulesJsonMods);

            var mockEnv_Modified = new Mock<IWebHostEnvironment>();
            mockEnv_Modified.Setup(e => e.ContentRootPath).Returns(Path.GetDirectoryName(_tempJsonPath_ModifiedRules)!);

            var fullJsonPath_Modified = Path.Combine(Path.GetDirectoryName(_tempJsonPath_ModifiedRules)!, "scoringRules.json");
            File.Move(_tempJsonPath_ModifiedRules, fullJsonPath_Modified, true);

            var _calculator_Modified = new JsonScoreCalculator(mockEnv_Modified.Object);

            var questionnaire = CreateQuestionnaire(25, new Dictionary<int, bool> { { 1, true } });
            var result = _calculator_Modified.CalculateScore(questionnaire);
            Assert.That(result, Is.EqualTo(20));
        }

        [Test]
        public void Returns_Zero_If_Answers_Are_Empty()
        {
            var questionnaire = CreateQuestionnaire(30, new Dictionary<int, bool>());
            var result = _calculator.CalculateScore(questionnaire);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Ignores_Unmatched_Answers()
        {
            var questionnaire = CreateQuestionnaire(30, new Dictionary<int, bool> { { 99, true } });
            var result = _calculator.CalculateScore(questionnaire);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Sums_Multiple_Questions_Correctly()
        {
            var answers = new Dictionary<int, bool>
            {
                { 1, true },
                { 2, true },
                { 3, false }
            };
            var questionnaire = CreateQuestionnaire(30, answers);
            var result = _calculator.CalculateScore(questionnaire);
            Assert.That(result, Is.EqualTo(2 + 2 + 3)); // Total: 7
        }

        private Questionnaire CreateQuestionnaire(int age, Dictionary<int, bool> answers)
        {
            return new Questionnaire
            {
                Age = age,
                Answers = answers,
                Questions = answers.Select(kvp => new Question
                {
                    ID = kvp.Key,
                    Text = $"Question {kvp.Key}"
                }).ToList()
            };
        }
    }
}
