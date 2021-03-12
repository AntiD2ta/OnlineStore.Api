using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Api.Data.Models
{
    public class Usuario : IdentityUser
    {
        public string Nombre { get; set; }

        public string Apellido { get; set; }

    }
}