using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Api.Data;
using OnlineStore.Api.Data.Models;
using System;
using System.Linq;

namespace ContosoPets.Api.Data
{
    public static class SeedData
    {
        public async static void Initialize(OnlineStoreContext context, UserManager<Usuario> userManager)
        {
            if (!context.Users.Any())
            {
                Usuario[] users = new Usuario[]{
                    new Usuario
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        Nombre = "Alan",
                        Apellido = "Brito",
                        Role = Usuario.Admin,
                    },
                   new Usuario
                   {
                       Email = "vendor@gmail.com",
                       UserName = "vendor",
                       SecurityStamp = Guid.NewGuid().ToString(),
                       Nombre = "Elba",
                       Apellido = "Lazo",
                       Role = Usuario.Vendor,
                   },
                   new Usuario
                   {
                       Email = "client@gmail.com",
                       UserName = "client",
                       SecurityStamp = Guid.NewGuid().ToString(),
                       Nombre = "Elsa",
                       Apellido = "Polindo",
                       Role = Usuario.Client,
                   }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Test@123");
                    context.SaveChanges();
                }

            }
        }
    }
}