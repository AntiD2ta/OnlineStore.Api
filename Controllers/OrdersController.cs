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

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> ChangeState(int id, EstadoOrden estado)
        {
            var orden = await _context.Ordenes.FindAsync(id);

            if (orden == null) return NotFound();

            
        }
    }
}