using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text;

namespace VueAppMvc.Server.Controllers
{
    public class SendEmail
    {
        private readonly string? _apiKey;

        public SendEmail()
        {
            _apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        }

        public async Task<Response> Execute(string recipientEmail, string subject, string confirmationLink)
        {
            if (string.IsNullOrEmpty(_apiKey))
                throw new InvalidOperationException("SendGrid API key is not configured.");

            SendGridClient client = new SendGridClient(_apiKey);
            EmailAddress from = new EmailAddress("yohangarcia@yohangarcia.com", "www.engfuel.com");
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
    }
}
