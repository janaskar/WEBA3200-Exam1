using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

public class DummyEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // This is a placeholder implementation that does nothing.
        // You can leave it empty or log the email that would have been sent (for debugging).
        return Task.CompletedTask;
    }
}