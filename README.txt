NHS Number Validation: https://digital.nhs.uk/services/personal-demographics-service/nhs-number
Initially I put a validation that NHS Number should be exact 10 digits as I thought this is how the reality is, but looking at the API results, it seems that 9 is also valid? I removed that "validation" in order to avoid interfering with any 
testing you have, but ideally we would want a validation like this if this was real production.

In terms of API result, I did make some assumptions that API returns correct and valid results, and always d.o.b. in the same "format".

API Key: This has been set through User Secrets. Prior to running the project, please perform the following:
1 - Enable the functionality through Developer PowerShell in Visual Studio (make sure you are in the root folder where .csproj file lives) through the following command: dotnet user-secrets init
2 - Set the secret through the following command: dotnet user-secrets set "ApiKey" "ReplaceThisWithYourApiKey"

lifestyle-checker
This is a simple web application which allows the user to use a questionnaire to get some lifestyle advices. It communicates through an API in order to retrieve NHS information.