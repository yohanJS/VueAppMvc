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
        public async Task<Response> SendEmailBookingDetails(BookFormModel bookFormModel)
        {
            string? apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            SendGridClient client = new SendGridClient(apiKey);
            string displayName = !string.IsNullOrEmpty(bookFormModel.BusinessId) ? bookFormModel.BusinessId.Equals("TankAC&HeatingLLC") ? "Tank AC & Heating LLC" : "YohanJS" : "BusinessId was empty";
            EmailAddress from = new EmailAddress("engfuel@luvoai.com", displayName);
            // Get Eastern Standard Time zone
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, easternZone);
            string subject = "Thank you for booking with us!";
            EmailAddress to = new EmailAddress(bookFormModel.Email);
            string plainTextContent = "";

            var htmlContentBuilder = new StringBuilder();
            htmlContentBuilder.AppendLine("<div style='font-family: Trebuchet MS, sans-serif; line-height: 1.3; color: #333;'>");
            htmlContentBuilder.AppendLine("<h2 style='color: #2a7ae2; font-size: 24px; margin-bottom: 20px;'> Booking Details:</h2>");
            htmlContentBuilder.AppendLine("<p style='margin: 0; padding: 1px 0;'>");
            htmlContentBuilder.AppendLine("<strong style='color: #333;'>Service requested: </strong>");
            htmlContentBuilder.AppendLine("<span style='color: #333 !important;'>" + bookFormModel.Service + "</span>");
            htmlContentBuilder.AppendLine("<p style='margin: 0; padding: 1px 0;'>");
            htmlContentBuilder.AppendLine("<strong style='color: #333;'>Phone: </strong>");
            htmlContentBuilder.AppendLine("<span style='color: #cfdd51 !important'>" + bookFormModel.Phone + "</span>");
            htmlContentBuilder.AppendLine("</p>");
            htmlContentBuilder.AppendLine("<p style='margin: 0; padding: 1px 0;'>");
            htmlContentBuilder.AppendLine("<strong style='color: #333;'>Email: </strong>");
            htmlContentBuilder.AppendLine("<span style='color: #cfdd51 !important;'>" + bookFormModel.Email + "</span>");
            htmlContentBuilder.AppendLine("</p>");
            htmlContentBuilder.AppendLine("<p style='margin: 0; padding: 1px 0;'>");
            htmlContentBuilder.AppendLine("<strong style='color: #333;'>Address: </strong>");
            htmlContentBuilder.AppendLine("<span style='color: #333 !important;'>" + bookFormModel.Street + bookFormModel.City + bookFormModel.State + bookFormModel.State + "</span>");
            htmlContentBuilder.AppendLine("</p>");
            htmlContentBuilder.AppendLine("<br>");
            htmlContentBuilder.AppendLine("<p style='margin: 0; padding: 1px 0;'>");
            htmlContentBuilder.AppendLine("<strong style='color: #333;'>Date and time service requested for: </strong>");
            htmlContentBuilder.AppendLine("<span style='color: #555 !important;'>" + bookFormModel.Date + "at" + bookFormModel.Time + "</span>");
            htmlContentBuilder.AppendLine("</p>");
            htmlContentBuilder.AppendLine("</div>");

            //FOOTER SECTION
            htmlContentBuilder.AppendLine("<div style='font-family: Trebuchet MS, sans-serif; line-height: 1.3; color: #333; margin-top: 20px; border-top: 1px solid #ccc; padding-top: 20px;'>");
           
            htmlContentBuilder.AppendLine("<br>");
            htmlContentBuilder.AppendLine("<br>");
            htmlContentBuilder.AppendLine("<a href='https://tankac.netlify.app' style='color: #0000FF !important; text-decoration: none !important;'>www.tankack&heat.com</a>");
            htmlContentBuilder.AppendLine("</div>");

            var htmlContent = htmlContentBuilder.ToString();

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            return await client.SendEmailAsync(msg); ;
        }
    }
}
