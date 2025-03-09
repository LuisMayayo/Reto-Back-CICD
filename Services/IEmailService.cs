namespace EcommerceBackend.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        
        Task SendOrderConfirmationAsync(int pedidoId, string userEmail, string userName);
    }
}
