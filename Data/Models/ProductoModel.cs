using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Api.Data.Models
{
    public class ProductoModel
    {
        [Required]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string Descripcion { get; set; }

        //TODO: Try to validate that Cantidad must be >= 0. Maybe CustomValidationAttribute??
        public int Cantidad { get; set; }

        [Required]
        public string Slug { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }
    }
}