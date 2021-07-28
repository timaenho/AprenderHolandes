using AprenderHolandes.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AprenderHolandes.Repository
{
    public interface IAccountRepository
    {
        Task GenerateForgotPasswordTokenAsync(Persona user);
        Task<Persona> GetUserByEmailAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPassword model);


    }
}