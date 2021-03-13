using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        [Authorize(Roles = "Administrator,Vendor")]
        public async Task<IActionResult> Create([FromBody] ProductoModel model)
        {
            var producto = new Producto
            {
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                Cantidad = model.Cantidad,
                Slug = model.Slug,
                Precio = model.Precio
            };

            var (usuario, error) = await GetCurrentUser(User);

            if (usuario == null) return StatusCode(500, error);

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBySlug), new { slug = producto.Slug }, producto);
        }

        [HttpPut("{slug}")]
        [Authorize(Roles = "Administrator,Vendor")]
        public async Task<IActionResult> Update(string slug, Producto producto)
        {
            var (usuario, error) = await GetCurrentUser(User);

            if (usuario == null) return StatusCode(500, error);

            if (User.IsInRole("Administrator") || usuario == producto.Usuario)
            {
                if (slug != producto.Slug) return BadRequest();

                _context.Entry(producto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{slug}")]
        [Authorize(Roles = "Administrator,Vendor")]
        public async Task<IActionResult> Delete(string slug)
        {
            var (usuario, error) = await GetCurrentUser(User);

            if (usuario == null) return StatusCode(500, error);

            var producto = await _context.Productos.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (producto == null) return NotFound();

            if (User.IsInRole("Administrator") || usuario == producto.Usuario)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("{slug}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Buy(string slug)
        {
            var producto = await _context.Productos.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (producto == null) return NotFound();

            var (usuario, error) = await GetCurrentUser(User);

            if (usuario == null) return StatusCode(500, error);

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

        private async Task<(Usuario, Exception)> GetCurrentUser(ClaimsPrincipal User)
        {
            string nameId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            string userName;
            Usuario usuario;

            try
            {
                userName = User.Claims.Where(c => c.Type == nameId).First().Value;
                usuario = await _context.Users.Where(u => u.UserName == userName).SingleAsync();
            }
            catch (Exception e)
            {
                return (null, e);
            }

            return (usuario, null);
        }
    }
}