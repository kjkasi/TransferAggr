using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferAggr.RequestApi.Data;
using TransferAggr.RequestApi.Models;

namespace TransferAggr.RequestApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PlaceApiController : ControllerBase
    {
        private readonly RequestContext _context;

        public PlaceApiController(RequestContext context)
        {
            _context = context;
        }

        // GET: api/PlaceApi
        [HttpGet]
        [Route("place")]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaceItem()
        {
            return await _context.PlaceItem.Take(1000).ToListAsync();
        }

        // GET: api/PlaceApi/5
        [HttpGet]
        [Route("place/{id}")]
        public async Task<ActionResult<Place>> GetPlace(string id)
        {
            var place = await _context.PlaceItem.FindAsync(id);

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
            _context.PlaceItem.Add(place);
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
            var place = await _context.PlaceItem.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            _context.PlaceItem.Remove(place);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaceExists(string id)
        {
            return _context.PlaceItem.Any(e => e.GUID == id);
        }
    }
}
