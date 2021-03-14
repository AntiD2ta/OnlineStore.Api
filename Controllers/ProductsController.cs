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

        /// <summary>
        /// Obtains current products in system. Public endpoint.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /products
        ///
        /// </remarks>
        /// <returns>A list of existing orders.</returns>
        /// <response code="200">Returns the list or orders</response>
        [HttpGet]
        public ActionResult<List<Producto>> GetAll() =>
            _context.Productos.ToList();

        /// <summary>
        /// Obtains a single order by slug. Public endpoint.
        /// </summary>
        /// <param name = "slug" > slug of desired product</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /products/{slug}
        ///
        /// </remarks>
        /// <returns>Product with provided slug.</returns>
        /// <response code="200">Returns the desired product</response>
        /// <response code="404">If no exists a product with provided slug</response>      
        [HttpGet("{slug}")]
        public async Task<ActionResult<Producto>> GetBySlug(string slug)
        {
            var producto = await _context.Productos.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (producto == null) return NotFound();

            return producto;
        }

        /// <summary>
        /// Create a product. Administrators and Vendors access-only.
        /// </summary>
        /// <param name = "model" > Properties of product to be created</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /products
        ///     {
        ///        "nombre": "Martillo",
        ///        "descripcion": "Martillo para martillar",
        ///        "cantidad": 5,
        ///        "slug": "martillo-premium",
        ///        "precio": 30
        ///     }
        ///
        /// </remarks>
        /// <returns>Created product.</returns>
        /// <response code="201">Product created</response>
        /// <response code="400">The is another product in the system with the slug provided</response>
        /// <response code="500">There is a problem obtaining user's credentials</response>     
        [HttpPost]
        [Authorize(Roles = "Administrator,Vendor")]
        public async Task<IActionResult> Create([FromBody] ProductoModel model)
        {
            if (_context.Productos.Any(p => p.Slug == model.Slug))
                return BadRequest();

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

            producto.Usuario = usuario;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBySlug), new { slug = producto.Slug }, producto);
        }

        /// <summary>
        /// Create a product. Administrators and Vendors (product creator) access-only.
        /// </summary>
        /// <param name = "slug" > slug of desired product</param>
        /// <param name = "model" > Updated product</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /products{slug}
        ///     {
        ///        "nombre": "Martillo",
        ///        "descripcion": "Martillo para martillar",
        ///        "cantidad": 10,
        ///        "slug": "martillo-premium",
        ///        "precio": 35
        ///     }
        ///
        /// </remarks>
        /// <response code="204">No Content</response>
        /// <response code="400">The is another product in the system with the slug provided to update</response>
        /// <response code="401">The requester user is not a Administrator or the creator of the indicated product</response>
        /// <response code="500">There is a problem obtaining user's credentials</response>     
        [HttpPut("{slug}")]
        [Authorize(Roles = "Administrator,Vendor")]
        public async Task<IActionResult> Update(string slug, [FromBody] ProductoModel model)
        {
            if (slug != model.Slug && _context.Productos.Any(p => p.Slug == model.Slug))
                return BadRequest();

            if (!_context.Productos.Any(p => p.Slug == slug))
                return BadRequest();

            var (usuario, error) = await GetCurrentUser(User);

            if (usuario == null) return StatusCode(500, error);

            var producto = await _context.Productos.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (User.IsInRole("Administrator") || usuario == producto.Usuario)
            {
                producto.UpdateFromModel(model);

                _context.Entry(producto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Delete a product by slug. Administrators and Vendors (product creator) access-only.
        /// </summary>
        /// <param name = "slug" > slug of desired product</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /products/{slug}
        ///
        /// </remarks>
        /// <response code="204">No Content</response>
        /// <response code="401">The requester user is not a Administrator or the creator of the indicated product</response>
        /// <response code="404">If no exists a product with provided slug</response>
        /// <response code="500">There is a problem obtaining user's credentials</response>    
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

        /// <summary>
        /// Buy a product by slug. Client access-only.
        /// </summary>
        /// <param name = "slug" > slug of desired product</param>
        /// <param name = "model" > amount of product to buy</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /products/{slug}
        ///     {
        ///         "cantidad": 2    
        ///     }
        ///
        /// </remarks>
        /// <returns>An order</returns>
        /// <response code="201">Order created</response>
        /// <response code="400">Invalid amount to buy</response>
        /// <response code="404">If no exists a product with provided slug</response>
        /// <response code="500">There is a problem obtaining user's credentials</response>    
        [HttpPost("{slug}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Buy(string slug, [FromBody] OrdenModel model)
        {
            var producto = await _context.Productos.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (producto == null) return NotFound();

            var (usuario, error) = await GetCurrentUser(User);

            if (usuario == null) return StatusCode(500, error);

            if (model.Cantidad <= 0 || model.Cantidad > producto.Cantidad) return BadRequest();

            var orden = new Orden
            {
                Fecha = DateTime.Now,
                Estado = EstadoOrden.created,
                Usuario = usuario,
                Producto = producto,
                Cantidad = model.Cantidad
            };

            _context.Ordenes.Add(orden);
            await _context.SaveChangesAsync();

            producto.Cantidad -= model.Cantidad;
            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(OrdersController.GetById), new { controller = "orders", id = orden.Id }, orden);
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