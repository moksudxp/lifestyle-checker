using LifestyleChecker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace LifestyleChecker.Controllers
{
    public class ClientInputController : Controller
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(Models.ClientInput model)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("Success", model);
            }
            else
            {
                return RedirectToAction("Failure");
            }
        }

        public ActionResult Failure()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }

        public async Task<ActionResult> Success(Models.ClientInput model)
        {
            HttpResponseMessage response;

            try
            {
                response = await GetRequestFromAPI(model.NHSNumber);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = "Error retrieving API Data: " + ex.Message;
                return View("Error");
            }


            return this.ExecuteBusinessLogic(model, response);
        }

        public ActionResult ExecuteBusinessLogic(ClientInput model, HttpResponseMessage response)
        {
            //400 - When the NHS Number doesn't exist in the server
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) return View("Error", new ErrorViewModel { ErrorMessage = "Your details could not be found", DetailedErrorDescription = "API cannot find Key"});

            string apiResponseResult = response.Content.ReadAsStringAsync().Result.ToString();

            PatientInfo patientInfo;
            try
            {
                patientInfo = JsonConvert.DeserializeObject<PatientInfo>(apiResponseResult, new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" });
            }
            catch(JsonReaderException e)
            {
                return View("Error", new ErrorViewModel {DetailedErrorDescription = $"Unable to read from JSON: {e.Message}" });
            }

            if (!this.ValidateClientInputAgainstAPIDetails(model, patientInfo)) return View("Error", new ErrorViewModel { ErrorMessage = "Your details could not be found", DetailedErrorDescription = "Data don't match between user input and API" });

            if (patientInfo.Age < 16) return View("Error", new ErrorViewModel { ErrorMessage = "You are not eligible for this service" });

            this.TempData["NHSNumber"] = patientInfo.NHSNumber;
            this.TempData["Age"] = patientInfo.Age;

            return RedirectToAction("Start", "Questionnaire");
        }

        public bool ValidateClientInputAgainstAPIDetails(ClientInput clientInput, PatientInfo patientInfo)
        {
            if (clientInput.NHSNumber != patientInfo.NHSNumber) return false;
            if (clientInput.Surname != patientInfo.Lastname) return false;
            if (clientInput.DateOfBirth != patientInfo.DateOfBirth) return false;

            return true;
        }

        private async Task<HttpResponseMessage> GetRequestFromAPI(string nhsNumber)
        {
            string url = $"https://al-tech-test-apim.azure-api.net/tech-test/t2/patients/{nhsNumber}";
            string apiKeyHeaderName = "Ocp-Apim-Subscription-Key";
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            _httpClient.DefaultRequestHeaders.Add(apiKeyHeaderName, config["ApiKey"]);
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            return response;
        }
    }
}
