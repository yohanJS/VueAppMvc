using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text;
using VueAppMvc.Server.Models;

namespace VueAppMvc.Server.Controllers
{
    public class SendEmail
    {
        private readonly string? _apiKey;

        public SendEmail()
        {
            _apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        }

        public async Task<Response> SendEmailConfirmation(string recipientEmail, string subject, string confirmationLink)
        {
            if (string.IsNullOrEmpty(_apiKey))

                throw new InvalidOperationException("SendGrid API key is not configured.");

            SendGridClient client = new SendGridClient(_apiKey);
            EmailAddress from = new EmailAddress("engfuel@luvoai.com", "engfuel.com");
            EmailAddress to = new EmailAddress(recipientEmail);
            string plainTextContent = $"Please confirm your email by clicking the following link: {confirmationLink}";

            var htmlContentBuilder = new StringBuilder();
            htmlContentBuilder.AppendLine("<div style='font-family: Trebuchet MS, sans-serif; line-height: 1.3; color: #333;'>");
            htmlContentBuilder.AppendLine("<h2 style='color: #2a7ae2;'>Confirm Your Email</h2>");
            htmlContentBuilder.AppendLine("<p>Please confirm your email by clicking the link below:</p>");
            htmlContentBuilder.AppendLine($"<p><a href='{confirmationLink}' style='color: #cfdd51; text-decoration: none;'>Confirm Email</a></p>");
            htmlContentBuilder.AppendLine("</div>");
            htmlContentBuilder.AppendLine("<div style='margin-top: 20px; font-size: 12px; color: #888;'>");
            htmlContentBuilder.AppendLine("<p>If you did not request this, please ignore this email.</p>");
            htmlContentBuilder.AppendLine("</div>");

            var htmlContent = htmlContentBuilder.ToString();

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return await client.SendEmailAsync(msg);
        }
        public async Task<(Response ClientResponse, Response BusinessResponse)> SendEmailBookingDetails(BookFormModel bookFormModel)
        {
            string serviceDate = string.Empty;
            if (!string.IsNullOrEmpty(bookFormModel.Date))
            {
                serviceDate = DateTime.ParseExact(bookFormModel.Date, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture).ToString("d");
            }

            string? apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            SendGridClient client = new SendGridClient(apiKey);

            string displayName = !string.IsNullOrEmpty(bookFormModel.BusinessId)
                ? bookFormModel.BusinessId.Equals("TankAC&HeatingLLC") ? "Tank AC & Heating LLC" : "YohanJS"
                : "BusinessId was empty";

            EmailAddress from = new EmailAddress("engfuel@luvoai.com", displayName);

            // Prepare email content (shared for both emails)
            var htmlContentBuilder = new StringBuilder();
            htmlContentBuilder.AppendLine("<div style='font-family: Trebuchet MS, sans-serif; line-height: 1.3; color: #333;'>");
            htmlContentBuilder.AppendLine("<h2 style='color: #2a7ae2; font-size: 24px; margin-bottom: 20px;'> Booking Details:</h2>");
            htmlContentBuilder.AppendLine($"<p><strong>Service requested:</strong> {bookFormModel.Service}</p>");
            htmlContentBuilder.AppendLine($"<p><strong>Phone:</strong> {bookFormModel.Phone}</p>");
            htmlContentBuilder.AppendLine($"<p><strong>Email:</strong> {bookFormModel.Email}</p>");
            htmlContentBuilder.AppendLine($"<p><strong>Address:</strong> {bookFormModel.Street} {bookFormModel.City} {bookFormModel.State}</p>");
            htmlContentBuilder.AppendLine($"<p><strong>Date and time:</strong> {serviceDate} at {bookFormModel.Time}</p>");
            htmlContentBuilder.AppendLine("</div>");

            var htmlContent = htmlContentBuilder.ToString();

            // Client email
            EmailAddress clientEmail = new EmailAddress(bookFormModel.Email);
            var clientMessage = MailHelper.CreateSingleEmail(from, clientEmail, "Thank you for booking with us!", string.Empty, htmlContent);

            // Business email
            EmailAddress businessEmail = new EmailAddress("yoanvaldes01@icloud.com"); // Replace with your business email
            var businessMessage = MailHelper.CreateSingleEmail(from, businessEmail, "New Booking Received", string.Empty, htmlContent);

            // Send both emails and collect responses
            Response clientResponse = await client.SendEmailAsync(clientMessage);
            Response businessResponse = await client.SendEmailAsync(businessMessage);

            // Return both responses
            return (clientResponse, businessResponse);
        }

    }
}
