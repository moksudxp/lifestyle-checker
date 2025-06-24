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

            Assert.That(patientInfo.Age, Is.EqualTo(age));
        }

        [Test]
        public void Age_ShouldReturnValidAge_WhenDateOfBirthIsSameDayAsTodayIn1995()
        {
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = new DateTime(1995, DateTime.Today.Month, DateTime.Today.Day) };

            int age = DateTime.Today.Year - 1995;

            Assert.That(patientInfo.Age, Is.EqualTo(age));
        }

        [Test]
        public void Age_ShouldReturnValidAge_WhenDateOfBirthIsNotOlderThan16()
        {
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = new DateTime(2020, DateTime.Today.Month, DateTime.Today.Day) };

            int age = DateTime.Today.Year - 2020;

            Assert.That(patientInfo.Age, Is.EqualTo(age));
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommmaSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud,Ahmed" };

            Assert.That(patientInfo.Firstname, Is.EqualTo("Moksud"));
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommmaSeparatorAndFirstNameContainsSpace()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud Junior,Ahmed" };

            Assert.That(patientInfo.Firstname, Is.EqualTo("Moksud Junior"));
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommaSeparatorAndSpaceAfterSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud, Ahmed" };

            Assert.That(patientInfo.Firstname, Is.EqualTo("Moksud"));
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommaSeparatorAndSpaceBeforeSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud ,Ahmed" };

            Assert.That(patientInfo.Firstname, Is.EqualTo("Moksud"));
        }  

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsOneCommaSeparatorAndMultipleSpaces()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "   Moksud ,  Ahmed " };

            Assert.That(patientInfo.Firstname, Is.EqualTo("Moksud"));
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsMultipleCommaSeparators()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud,Ahmed,Brian" };

            Assert.That(patientInfo.Firstname, Is.EqualTo("Moksud"));
        }

        [Test]
        public void FirstName_ShouldReturnFirstName_WhenFullNameContainsNoSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud" };

            Assert.That(patientInfo.Firstname, Is.EqualTo("Moksud"));
        }

        [Test]
        public void FirstName_ShouldReturnFullName_WhenFullNameUsesSeparatorDifferentThanComma()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud - Ahmed" };

            Assert.That(patientInfo.Firstname, Is.EqualTo("Moksud - Ahmed"));
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud,Ahmed" };

            Assert.That(patientInfo.Lastname, Is.EqualTo("Ahmed"));
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparatorAndFirstNameContainsSpace()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud Junior,Ahmed" };

            Assert.That(patientInfo.Lastname, Is.EqualTo("Ahmed"));
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparatorAndSpaceAfterSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud, Ahmed" };

            Assert.That(patientInfo.Lastname, Is.EqualTo("Ahmed"));
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparatorAndSpaceBeforeSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud ,Ahmed" };

            Assert.That(patientInfo.Lastname, Is.EqualTo("Ahmed"));
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsOneCommaSeparatorAndMultipleSpaces()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "   Moksud ,  Ahmed " };

            Assert.That(patientInfo.Lastname, Is.EqualTo("Ahmed"));
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsMultipleCommaSeparators()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud,Ahmed,Brian" };

            Assert.That(patientInfo.Lastname, Is.EqualTo("Brian"));
        }

        [Test]
        public void LastName_ShouldReturnLastName_WhenFullNameContainsNoSeparator()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud" };

            Assert.That(patientInfo.Lastname, Is.EqualTo("Moksud"));
        }

        [Test]
        public void LastName_ShouldReturnFullName_WhenFullNameUsesSeparatorDifferentThanComma()
        {
            PatientInfo patientInfo = new PatientInfo { FullName = "Moksud - Ahmed" };

            Assert.That(patientInfo.Lastname, Is.EqualTo("Moksud - Ahmed"));
        }
    }
}
