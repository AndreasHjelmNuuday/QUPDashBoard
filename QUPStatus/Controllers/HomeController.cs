using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QUPStatus.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotJira;

namespace QUPStatus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(String project = "MUSIC", String quarter = "2021 Q4" )
        {
            QUPHelper qupHelper = new QUPHelper();
            List<Issue> issues = qupHelper.getAllQupIssuesSorted(project, quarter);            
            return View(issues); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
