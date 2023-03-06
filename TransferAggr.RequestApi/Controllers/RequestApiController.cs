using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferAggr.RequestApi.Data;
using TransferAggr.RequestApi.Dtos;
using TransferAggr.RequestApi.Models;

namespace TransferAggr.RequestApi.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class RequestApiController : ControllerBase
    {
        private readonly RequestContext _context;
        private readonly IMapper _mapper;

        public RequestApiController(RequestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/RequestApi
        [HttpGet]
        [AllowAnonymous]
        [Route("request")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestItems()
        {
            return await _context.RequestItems
                .Include(u => u.From)
                .Include(u => u.To)
                .ToListAsync();
        }

        // GET: api/RequestApi/5
        [HttpGet]
        [AllowAnonymous]
        [Route("request/{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            //var request = await _context.RequestItems.FindAsync(id);
            var request = await _context.RequestItems
                .Include(u => u.From)
                .Include(u => u.To)
                .FirstOrDefaultAsync(p => p.RequestId == id);

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
            request.From = null;
            request.To = null;

            _context.RequestItems.Add(request);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RequestExists(request.RequestId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

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
