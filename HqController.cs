using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using www.Models;

namespace www.HqControllerControllers
{
    public class HqController : Controller
    {
        IDistributedCache _redis = null;
        public HqController(IDistributedCache cache)
        {
            _redis = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            var key = "aspnet:{Staff}:bio";
            var bio = _redis.GetString(key);
            if (string.IsNullOrWhiteSpace(bio))
            {
                bio = "John"; // here we call "expensive" or "slow" web service...

                _redis.SetString(key, bio);
            }

            HttpContext.Session.Set("HqSessionKey",
                Encoding.UTF8.GetBytes("Redis session in progress"));

            ViewData["Message"] = $"About Staff {bio + " "}";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Retrieving Contact Details....";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
