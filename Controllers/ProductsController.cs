using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Api.Data;
using OnlineStore.Api.Data.Models;

namespace OnlineStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly OnlineStoreContext _context;

        public ProductsController(OnlineStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Producto>> GetAll() =>
            _context.Productos.ToList();

        [HttpGet("{slug}")]
        public async Task<ActionResult<Producto>> GetBySlug(string slug)
        {
            var producto = await _context.Productos.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (producto == null) return NotFound();

            return producto;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBySlug), new { id = producto.Id }, producto);
        }

        [HttpPut("{slug}")]
        [Authorize]
        public async Task<IActionResult> Update(string slug, Producto producto)
        {
            if (slug != producto.Slug) return BadRequest();

            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{slug}")]
        [Authorize]
        public async Task<IActionResult> Delete(string slug)
        {
            var producto = await _context.Productos.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (producto == null) return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{slug}")]
        [Authorize]
        public async Task<IActionResult> Buy(string slug)
        {
            var producto = await _context.Productos.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (producto == null) return NotFound();

            var usuario = await _context.Users.FindAsync(User.Identity.Name);

            if (usuario == null) return StatusCode(500);

            var orden = new Orden
            {
                Fecha = DateTime.Now,
                Estado = EstadoOrden.created,
                Usuario = usuario,
                Producto = producto
            };

            _context.Ordenes.Add(orden);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(OrdersController.GetById), new { id = orden.Id }, orden);
        }
    }
}