using AprenderHolandes.Data;
using AprenderHolandes.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            if (Configuration.GetValue<bool>("DbInMem"))
            {
                services.AddDbContext<DbContext>(options => options.UseInMemoryDatabase("InstitutoEducativo"));
            }
            else
            {
                services.AddDbContext<DbContextInstituto>(options => options.UseSqlServer(Configuration.GetConnectionString("InstitutoEducativoCS")));
            }
            //Creo tabla intermedia entre Persona y Rol

            services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<DbContextInstituto>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => options.Password.RequireNonAlphanumeric = false);

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                opciones =>
                {
                    opciones.LoginPath = "/Accounts/IniciarSesion";
                    opciones.AccessDeniedPath = "/Accounts/AccesoDenegado";
                });
            services.AddControllersWithViews();
            //services.Configure<SMTPConfig>(Configuration.GetSection("SMTPConfig"));
            //services.AddScoped<IDbInicializador, DbInicializador>();
           // services.AddScoped<IAccountRepository, AccountRepository>();
            //services.AddScoped<IEmailService, EmailService>();
           // services.AddScoped<IUserClaimsPrincipalFactory<Persona>, ApplicationUserClaimsPrincipalFactory>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbContextInstituto miContexto)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {

                var contexto = serviceScope.ServiceProvider.GetRequiredService<DbContextInstituto>();
                if (!Configuration.GetValue<bool>("DbInMem"))
                {
                    miContexto.Database.Migrate();// --> asegura la base de datos y ejecuta todas las migraciones
                }
              //  serviceScope.ServiceProvider.GetService<IDbInicializador>().Seed();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //orden es importante
            //Quien es??
            app.UseAuthentication();
            //Tiene permiso??
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
