using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TransferAggr.WebMvc.Models;

namespace TransferAggr.WebMvc.Controllers
{
    [Route("{controller}")]
    public class RequestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public RequestController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;

            _baseUrl = $"{configuration.GetValue<string>("ApiUrl")}";
        }

        [HttpGet]
        [Route("{Action}")]
        public async Task<IActionResult>Index()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"{_baseUrl}/api/request/");
            response.EnsureSuccessStatusCode();
            IEnumerable<Request> Items = await response.Content.ReadFromJsonAsync<IEnumerable<Request>>();


            return View(Items);

            //return View();
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
        public async Task<IActionResult> Create(Request item)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.PostAsJsonAsync($"{_baseUrl}/api/request", item);
                response.EnsureSuccessStatusCode();
                Request newItem = await response.Content.ReadFromJsonAsync<Request>();
                return RedirectToRoute(new
                {
                    controller = "Request",
                    action = "Details",
                    id = newItem.RequestId
                });
            }
            return View("Error");
        }

        [HttpGet]
        [Route("{Action}/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"{_baseUrl}/api/request/{id}");
            response.EnsureSuccessStatusCode();
            Request Item = await response.Content.ReadFromJsonAsync<Request>();
            

            return View(Item);
        }
    }
}
