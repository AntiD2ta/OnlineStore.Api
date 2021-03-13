using System;
using System.ComponentModel.DataAnnotations;
using OnlineStore.Api.Data.Models;
using System.Collections.Generic;

namespace OnlineStore.Api.Data.Models
{
    public enum EstadoOrden
    {
        none,
        created,
        confirmed,
        canceled
    }

    public class Orden
    {
        [Required]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public EstadoOrden Estado { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual Producto Producto { get; set; }
    }
}