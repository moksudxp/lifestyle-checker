namespace LifestyleChecker.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorMessage { get; set; } = "An error occurred while processing your request.";

        public string DetailedErrorDescription { get; set; } = string.Empty;
    }
}
