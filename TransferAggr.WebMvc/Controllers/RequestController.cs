using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransferAggr.WebMvc.Models;

namespace TransferAggr.WebMvc.Controllers
{
    [Route("{controller}")]
    public class RequestController : Controller
    {
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
        public IActionResult Create(RequestViewModel item)
        {
            //RedirectToAction(nameof(Details));
            return Ok(item);
        }

        [HttpGet]
        [Route("{Action}/{id:int}")]
        public IActionResult Details(int? id)
        {
            return View(new RequestViewModel());
        }
    }
}
