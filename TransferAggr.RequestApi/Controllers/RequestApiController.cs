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
    public class RequestApiController : ControllerBase
    {
        private readonly RequestContext _context;

        public RequestApiController(RequestContext context)
        {
            _context = context;
        }

        // GET: api/RequestApi
        [HttpGet]
        [Route("request")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestItems()
        {
            return await _context.RequestItems.ToListAsync();
        }

        // GET: api/RequestApi/5
        [HttpGet]
        [Route("request/{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.RequestItems.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/RequestApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("request/{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.RequestId)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/RequestApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("request")]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.RequestItems.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.RequestId }, request);
        }

        // DELETE: api/RequestApi/5
        [HttpDelete]
        [Route("request/{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.RequestItems.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.RequestItems.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.RequestItems.Any(e => e.RequestId == id);
        }
    }
}
