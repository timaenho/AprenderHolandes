using AprenderHolandes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AprenderHolandes.Data
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<Persona, Rol>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<Persona> userManager,
           RoleManager<Rol> roleManager, IOptions<IdentityOptions> options)
           : base(userManager, roleManager, options)
        {
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Persona user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.Nombre ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.Apellido ?? ""));
            return identity;
        }

    }
}
