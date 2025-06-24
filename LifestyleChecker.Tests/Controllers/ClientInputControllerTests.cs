using LifestyleChecker.Controllers;
using LifestyleChecker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifestyleChecker.Tests.Controllers
{
    [TestFixture]
    public class ClientInputControllerTests
    {
        [Test]
        public void BusinessLogic_ShouldReturnRelevantErrorMessage_WhenNHSNumberNotFoundByAPI()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            HttpResponseMessage response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest };

            ActionResult returnResult = controller.ExecuteBusinessLogic(clientInput, response);
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)returnResult;
            string errorMessage = ((ErrorViewModel)viewResult.Model).ErrorMessage;
            string detailedErrorMessage = ((ErrorViewModel)viewResult.Model).DetailedErrorDescription;

            Assert.AreEqual("Your details could not be found", errorMessage);
            Assert.AreEqual("API cannot find Key", detailedErrorMessage);
        }

        [Test]
        public void BusinessLogic_ShouldReturnBadStatusCode_WhenNHSNumberNotFoundByAPI()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            HttpResponseMessage response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest };

            ActionResult returnResult = controller.ExecuteBusinessLogic(clientInput, response);
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)returnResult;

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void BusinessLogic_ShouldReturnErrprView_WhenNHSNumberNotFoundByAPI()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = DateTime.Now, NHSNumber = "1111", FullName = "Ahmed,Moksud" };
            HttpResponseMessage response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest };

            ActionResult returnResult = controller.ExecuteBusinessLogic(clientInput, response);
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)returnResult;

            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [Test]
        public void BusinessLogic_ShouldReturnRelevantErrorMessage_WhenDetailsDontMatch()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = DateTime.Now, NHSNumber = "1111", FullName = "Ahmed,Moksud" };
            HttpResponseMessage response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            response.Content = new StringContent("{\"nhsNumber\":\"111222333\",\"name\":\"DOE, John\",\"born\":\"14-01-2007\"}");

            ActionResult returnResult = controller.ExecuteBusinessLogic(clientInput, response);
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)returnResult;
            string errorMessage = ((ErrorViewModel)viewResult.Model).ErrorMessage;
            string detailedErrorMessage = ((ErrorViewModel)viewResult.Model).DetailedErrorDescription;

            Assert.AreEqual("Your details could not be found", errorMessage);
            Assert.AreEqual("Data don't match between user input and API", detailedErrorMessage);
        }

        [Test]
        public void BusinessLogic_ShouldReturnErrorView_WhenDetailsDontMatch()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = DateTime.Now, NHSNumber = "1111", FullName = "Ahmed,Moksud" };
            HttpResponseMessage response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest };

            ActionResult returnResult = controller.ExecuteBusinessLogic(clientInput, response);
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)returnResult;

            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [Test]
        public void BusinessLogic_ShouldThrowException_WhenInvalidApiResult()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = DateTime.Now, NHSNumber = "1111", FullName = "Ahmed,Moksud" };
            HttpResponseMessage response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            response.Content = new StringContent("notJsonFormattedString");

            ActionResult returnResult = controller.ExecuteBusinessLogic(clientInput, response);
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)returnResult;
            string errorMessage = ((ErrorViewModel)viewResult.Model).ErrorMessage;
            string detailedErrorMessage = ((ErrorViewModel)viewResult.Model).DetailedErrorDescription;

            Assert.Throws(Is.TypeOf<JsonReaderException>(), () => throw new JsonReaderException());
        }

        [Test]
        public void ValidateClientInputAgainstAPIDetails_ShouldReturnFalse_WhenNHSNumbersDontMatch()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = DateTime.Now, NHSNumber = "1121", FullName = "Ahmed,Moksud" };

            bool result = controller.ValidateClientInputAgainstAPIDetails(clientInput, patientInfo);

            Assert.IsFalse(result, "This should fail _When NHS Numbers are different");
        }

        [Test]
        public void ValidateClientInputAgainstAPIDetails_ShouldReturnFalse_WhenSurnameDontMatch()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = DateTime.Now, NHSNumber = "1111", FullName = "Ahmed,Moksud" };

            bool result = controller.ValidateClientInputAgainstAPIDetails(clientInput, patientInfo);

            Assert.IsFalse(result, "This should fail _When Surname are different");
        }

        [Test]
        public void ValidateClientInputAgainstAPIDetails_ShouldReturnFalse_WhenDateOfBirthDontMatch()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = DateTime.Now.AddDays(2), NHSNumber = "1111", FullName = "Moksud,Ahmed" };

            bool result = controller.ValidateClientInputAgainstAPIDetails(clientInput, patientInfo);

            Assert.IsFalse(result, "This should fail _When Date Of Birth are different");
        }

        public void ValidateClientInputAgainstAPIDetails_ShouldReturnTrue_WhenAllDetailsMatching()
        {
            ClientInputController controller = new ClientInputController();
            ClientInput clientInput = new ClientInput { DateOfBirth = DateTime.Now, NHSNumber = "1111", Surname = "Ahmed" };
            PatientInfo patientInfo = new PatientInfo { DateOfBirth = DateTime.Now, NHSNumber = "1111", FullName = "Moksud,Ahmed" };

            bool result = controller.ValidateClientInputAgainstAPIDetails(clientInput, patientInfo);

            Assert.IsTrue(result, "This should return true _When all details are matching");
        }
    }
}
