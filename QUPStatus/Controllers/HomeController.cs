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

        public IActionResult Index(String project = "MUSIC", String quarter = null, String issueKey = null, String team = null)
        {
            List<Issue> issues = new();
            QUPViewModel model = new();
            QUPHelper qupHelper = new QUPHelper();

            model.Issues = new();

            if (quarter == null)
            {
                quarter = QuarterCalculator.CurrentQuarter;
            }            
            if (issueKey == null && team == null)
            {
                
               model.Issues = qupHelper.getAllQupIssuesSorted(project, quarter);
            }
            else if (issueKey != null)
            {
                    issues = qupHelper.GetSpecificIssue(issueKey);
                    model.Issues.Add(findIssue(issueKey, issues));                
            }
            else if(team != null)
            {
                model.Issues = qupHelper.GetTeamIssues(project, quarter, team);                
            }
            else
            {
                model.Issues = issues;
            }
            model.Quarter = quarter;
            return View(model);
        }

        private Issue findIssue(string issueKey, List<Issue> issues)
        {
            Issue issue = new();
            issue = issues.Find(i => i.Key.Equals(issueKey)); //search in tribe objectives
            if (issue == null)
            {
                foreach(Issue tribeObj in issues) { 
                    Issue squadObject = tribeObj.Children.Find(i => i.Key.Equals(issueKey)); //search in squad objectives
                    if (squadObject != null)
                    {
                        issue = squadObject;
                        break;
                    }                        
                    if (squadObject == null)
                    {                        
                        foreach(Issue squadObj in tribeObj.Children)
                        {
                            Issue epic = squadObj.Children.Find(i => i.Key.Equals(issueKey)); //search in epics objectives
                            if (epic != null)
                            {
                                issue = epic;
                                break;
                            }
                        }
                    }
                }
            }
            return issue; 
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
