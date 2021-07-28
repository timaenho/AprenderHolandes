using AprenderHolandes.Models;
using System.Threading.Tasks;

namespace AprenderHolandes.Servicios
{
    public interface IEmailService
    {
        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
    }
}