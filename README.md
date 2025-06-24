# Lifestyle Checker
This is a simple web application made in ASP.NET Core MVC which provides the user with some simple health questions and a recommendation based on their choices.
The application is made to be moduler, testable and configurable for different scoring strategies.
It communicates through an API in order to retrieve NHS information, which are then compared to the user input.

# API Key
This has been set through User Secrets. Prior to running the project, please perform the following:
1 - Enable the functionality through Developer PowerShell in Visual Studio (make sure you are in the root folder where .csproj file lives) through the following command: dotnet user-secrets init
2 - Set the secret through the following command: dotnet user-secrets set "ApiKey" "ReplaceThisWithYourApiKey"

# Validation
## NHS Number Validation
NHS Number Validation: https://digital.nhs.uk/services/personal-demographics-service/nhs-number
Initially I put a validation that NHS Number should be exact 10 digits as I thought this is how the reality is, but looking at the API results, it seems that 9 is also valid? I removed that "validation" in order to avoid interfering with any 
testing you have, but ideally we would want a validation like this if this was real production.

## Validation of comma, spaces and special characters
I kept the validation minimal and made the assumption to trust the information received from the API.
Rather than overcomplicating the controller with excessive validation, as the information is "supposed" to come from a trustworthy upstream system (e.g. NHS Database).
The following assumptions have been made:
1 - The API always returns Date of birth in the same standard format
2 - The API always returns the fullname in the format "surname, name"

# Scoring
The scoring strategy has been refactored in a 2nd instance in order to allow dynamic change of rules through a JSON file (e.g. age brackets, question weight) without the need of re-compiling the application.

# Testing
I performed manual tests in order to ensure application is running as expected (during development phases). Some of these could be implemented as Integration Tests.
I focused on unit tests are they are deterministic and easily runnable.
All tests are within the same testing Project, in order to keep them separate and modular.

# Future improvements
- Create a separate service for the scoringrules, which will be injected through API or remote config service, instead of being a direct JSON file
- Create an editor for the score file, which allows administrators to easily edit the scoring system within a Web Interface, without the need of changing the JSON file directly
- Include validation for NHS Number (use 10th digit for check)
- Add a database to save and persist questions
- Add model to create different questionnaires and questions depending on factors like age, etc...


