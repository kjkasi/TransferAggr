using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TransferAggr.GeocodeApi.Data;

namespace TransferAggr.GeocodeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlaceApiController : ControllerBase
    {
        private readonly PlaceContext _context;

        public PlaceApiController(PlaceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPlacesAsync()
        {
            var items = await _context.Items.Take(100).ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        public async Task<ActionResult> SearchPlaceAsync(string searchText)
        {
            var items = await _context.Items.Where(x => EF.Functions.Like(x.StreetParsed, $"%{searchText}%")).Take(10).ToListAsync();
            return Ok(items);
        }
    }
}
