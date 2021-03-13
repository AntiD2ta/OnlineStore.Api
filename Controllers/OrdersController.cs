using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Api.Data;
using OnlineStore.Api.Data.Models;

namespace OnlineStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OnlineStoreContext _context;

        public OrdersController(OnlineStoreContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtains current orders in system. Public endpoint.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /orders
        ///
        /// </remarks>
        /// <returns>A list of existing orders.</returns>
        /// <response code="200">Returns the list or orders</response>
        [HttpGet]
        public ActionResult<List<Orden>> GetAll() =>
            _context.Ordenes.ToList();

        /// <summary>
        /// Obtains a single order by id. Public endpoint.
        /// </summary>
        /// <param name = "id" > id of desired order</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /orders/{id}
        ///
        /// </remarks>
        /// <returns>Order with provided id.</returns>
        /// <response code="200">Returns the desired order</response>
        /// <response code="404">If no exists a order with provided id</response>      
        [HttpGet("{id}")]
        public async Task<ActionResult<Orden>> GetById(int id)
        {
            var orden = await _context.Ordenes.FindAsync(id);

            if (orden == null) return NotFound();

            return orden;
        }

        // [HttpPatch("{id}")]
        // public async Task<IActionResult> ChangeState(int id,
        //     [FromBody] JsonPatchDocument<Orden> patchOrder)
        // {
        //     if (patchOrder != null)
        //     {
        //         var orden = await _context.Ordenes.FindAsync(id);

        //         if (orden == null) return NotFound();

        //         patchOrder.ApplyTo(orden, ModelState);

        //         if (!ModelState.IsValid)
        //         {
        //             return BadRequest(ModelState);
        //         }

        //         _context.Entry(orden).State = EntityState.Modified;
        //         await _context.SaveChangesAsync();

        //         return new ObjectResult(orden);
        //     }
        //     else
        //     {
        //         return BadRequest(ModelState);
        //     }
        // }

        /// <summary>
        /// Edit the state of an order with provided id. Administrators access-only.
        /// </summary>
        /// <param name = "id" > id of desired order</param>
        /// <param name = "state" > State to update</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /orders/{id}
        ///     {
        ///        "state": "confirmed",
        ///     }
        ///
        /// </remarks>
        /// <returns>Order with provided id with state modified.</returns>
        /// <response code="200">Returns the updated order</response>
        /// <response code="400">The given state in request body is not correct</response>
        /// <response code="404">If no exists a order with provided id</response>     
        [HttpPatch("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ChangeState(int id, [FromBody] EstadoOrden state)
        {
            if (state != EstadoOrden.none)
            {
                var orden = await _context.Ordenes.FindAsync(id);

                if (orden == null) return NotFound();

                orden.Estado = state;

                _context.Entry(orden).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new ObjectResult(orden);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Delete an order by id. Administrators access-only.
        /// </summary>
        /// <param name = "id" > id of desired order</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /orders/{id}
        ///
        /// </remarks>
        /// <response code="204">No Content</response>
        /// <response code="400">The order cannot be deleted because its state is confirmed</response>
        /// <response code="404">If no exists a order with provided id</response>     
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var orden = await _context.Ordenes.FindAsync(id);

            if (orden == null) return NotFound();

            if (orden.Estado == EstadoOrden.confirmed) return BadRequest();

            _context.Ordenes.Remove(orden);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}