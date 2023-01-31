using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferAggr.GeocodeApi.Data;
using TransferAggr.GeocodeApi.Models;

namespace TransferAggr.GeocodeApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PlaceApiController : ControllerBase
    {
        private readonly PlaceContext _context;

        public PlaceApiController(PlaceContext context)
        {
            _context = context;
        }

        // GET: api/PlaceApi
        [HttpGet]
        [Route("place")]
        public async Task<ActionResult<IEnumerable<Place>>> GetItems()
        {
            return await _context.Items.Take(100).ToListAsync();
        }

        [HttpGet]
        [Route("place/address/{name}")]
        public async Task<ActionResult<IEnumerable<Place>>> GetItemsByAddress(string name)
        {
            return await _context.Items.Where(p => EF.Functions.Like(p.Address, $"%{name}%"))
                .Take(10)
                .ToListAsync();
        }

        // GET: api/PlaceApi/5
        [HttpGet]
        [Route("place/{id}")]
        public async Task<ActionResult<Place>> GetPlace(string id)
        {
            var place = await _context.Items.FindAsync(id);

            if (place == null)
            {
                return NotFound();
            }

            return place;
        }

        // PUT: api/PlaceApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("place/{id}")]
        public async Task<IActionResult> PutPlace(string id, Place place)
        {
            if (id != place.GUID)
            {
                return BadRequest();
            }

            _context.Entry(place).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PlaceApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("place")]
        public async Task<ActionResult<Place>> PostPlace(Place place)
        {
            _context.Items.Add(place);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PlaceExists(place.GUID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPlace", new { id = place.GUID }, place);
        }

        // DELETE: api/PlaceApi/5
        [HttpDelete]
        [Route("place/{id}")]
        public async Task<IActionResult> DeletePlace(string id)
        {
            var place = await _context.Items.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            _context.Items.Remove(place);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaceExists(string id)
        {
            return _context.Items.Any(e => e.GUID == id);
        }
    }
}
