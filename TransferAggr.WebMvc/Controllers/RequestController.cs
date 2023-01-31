using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TransferAggr.WebMvc.Models;

namespace TransferAggr.WebMvc.Controllers
{
    [Route("{controller}")]
    public class RequestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RequestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route("{Action}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("{Action}")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{Action}")]
        public async Task<IActionResult> Create(RequestViewModel item)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://host.docker.internal:5000/item", item);
                response.EnsureSuccessStatusCode();
                RequestViewModel newItem = await response.Content.ReadFromJsonAsync<RequestViewModel>();
                /*
                return RedirectToRoute(new
                {
                    controller = "Item",
                    action = "Details",
                    id = newItem.Id
                });
                */
                RedirectToAction(nameof(Details));
            }
            return View("Error");
        }

        [HttpGet]
        [Route("{Action}/{id:int}")]
        public IActionResult Details(int? id)
        {
            return View(new RequestViewModel());
        }
    }
}
