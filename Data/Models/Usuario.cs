using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Api.Data.Models
{
    public class Usuario : IdentityUser
    {
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Role { get; set; }

        public static string Admin = "Administrator";

        public static string Vendor = "Vendor";

        public static string Client = "Client";
    }
}