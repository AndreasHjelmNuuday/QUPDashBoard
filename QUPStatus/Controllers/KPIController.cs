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
    public class KPIController : Controller
    {
        private readonly ILogger<KPIController> _logger;
        
        public KPIController(ILogger<KPIController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string issueKey)
        {
            QUPViewModel model = new();             
            QUPHelper qupHelper = new();
            List<Issue> issues = qupHelper.GetSpecificIssue(issueKey, false);
            KPIHelper.parseKPIs(issues);
            model.Issues.Add(QUPHelper.findIssue(issueKey, issues));           
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
