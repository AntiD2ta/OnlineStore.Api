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

        [HttpGet]
        public ActionResult<List<Orden>> GetAll() =>
            _context.Ordenes.ToList();

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

        [HttpPatch("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ChangeState(int id, EstadoOrden estado)
        {
            if (estado != EstadoOrden.none)
            {
                var orden = await _context.Ordenes.FindAsync(id);

                if (orden == null) return NotFound();

                _context.Entry(orden).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new ObjectResult(orden);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

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