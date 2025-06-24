using LifestyleChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifestyleChecker.Tests.Models
{
    [TestFixture]
    public class PatientInfoTests
    {
        [Test]
        public void Age_ShouldReturnValidAge_WhenDateOfBirthOlderThan1900()
        {
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = new DateTime(1850, 1, 1) };

            int age = DateTime.Today.Year - 1850;

            Assert.AreEqual(age, patientInfo.Age);
        }

        [Test]
        public void Age_ShouldReturnValidAge_WhenDateOfBirthIsSameDayAsTodayIn1995()
        {
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = new DateTime(1995, DateTime.Today.Month, DateTime.Today.Day) };

            int age = DateTime.Today.Year - 1995;

            Assert.AreEqual(age, patientInfo.Age);
        }

        [Test]
        public void Age_ShouldReturnValidAge_WhenDateOfBirthIsNotOlderThan16()
        {
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = new DateTime(2020, DateTime.Today.Month, DateTime.Today.Day) };

            int age = DateTime.Today.Year - 2020;

            Assert.AreEqual(age, patientInfo.Age);
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommmaSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud,Ahmed" };

            Assert.AreEqual("Moksud", patientInfo.Firstname);
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommmaSeparatorAndFirstNameContainsSpace()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud Junior,Ahmed" };

            Assert.AreEqual("Moksud Junior", patientInfo.Firstname);
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommaSeparatorAndSpaceAfterSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud, Ahmed" };

            Assert.AreEqual("Moksud", patientInfo.Firstname);
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommaSeparatorAndSpaceBeforeSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud ,Ahmed" };

            Assert.AreEqual("Moksud ", patientInfo.Firstname);
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommaSeparatorAndMultipleSpaces()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "   Moksud ,  Ahmed " };

            Assert.AreEqual("   Moksud ", patientInfo.Firstname);
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsMultipleCommaSeparators()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud,Ahmed,Brian" };

            Assert.AreEqual("Moksud", patientInfo.Firstname);
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsNoSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud" };

            Assert.AreEqual("Moksud", patientInfo.Firstname);
        }

        [Test]
        public void FirstName_ShouldReturnFullName_WhenFullNameUsesSeparatorDifferentThanComma()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud - Ahmed" };

            Assert.AreEqual("Moksud - Ahmed", patientInfo.Firstname);
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud,Ahmed" };

            Assert.AreEqual("Ahmed", patientInfo.Lastname);
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparatorAndFirstNameContainsSpace()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud Junior,Ahmed" };

            Assert.AreEqual("Ahmed", patientInfo.Lastname);
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparatorAndSpaceAfterSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud, Ahmed" };

            Assert.AreEqual(" Ahmed", patientInfo.Lastname);
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparatorAndSpaceBeforeSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud ,Ahmed" };

            Assert.AreEqual("Ahmed", patientInfo.Lastname);
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparatorAndMultipleSpaces()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "   Moksud ,  Ahmed " };

            Assert.AreEqual("  Ahmed ", patientInfo.Lastname);
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsMultipleCommaSeparators()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud,Ahmed,Brian" };

            Assert.AreEqual("Brian", patientInfo.Lastname);
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsNoSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud" };

            Assert.AreEqual("Moksud", patientInfo.Lastname);
        }

        [Test]
        public void LastName_ShouldReturnFullName_WhenFullNameUsesSeparatorDifferentThanComma()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud - Ahmed" };

            Assert.AreEqual("Moksud - Ahmed", patientInfo.Lastname);
        }
    }
}
