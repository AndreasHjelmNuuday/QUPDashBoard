using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotJira
{
    public class KPIHelper
    {

        private const string BUDGET = "budget:";
        private const string REALIZED = "realized:";

        public void ParseKPIs(List<Issue> issues)
        {
            foreach(Issue parent in issues)
            {
                parent.KPIs = new();
                foreach(Issue child in parent.Children)
                {
                    child.KPIs = new();
                    child.KPIs.Periods = new();
                    child.KPIs.Budgets = new();
                    child.KPIs.Realized = new();
                    ParseKPIString(child);
                }
            }




        }

        private void ParseKPIString(Issue issue)
        {

            if (issue.Fields.Description != null && issue.Fields.Description.Contains("]")){ 
            String[] description = issue.Fields.Description.Split("]");
            foreach (string line in description)
            {
                if (line != null && line.Length > 0)
                {
                    string lineWithOutStartBracket = line.Substring(1);
                    string period = lineWithOutStartBracket.Split(";")[0];
                    string budget = lineWithOutStartBracket.Split(";")[1];

                    if (budget.Contains(BUDGET))
                    {
                        budget = budget.Trim().Substring(BUDGET.Length).Trim();
                    }
                    string realized = lineWithOutStartBracket.Split(";")[2];
                    if (realized.Contains(REALIZED))
                    {
                        realized = realized.Trim().Substring(REALIZED.Length).Trim();
                    }

                    issue.KPIs.Periods.Add(period);
                    issue.KPIs.Budgets.Add(budget);
                    issue.KPIs.Realized.Add(realized);

                    CreatesJSON(issue);
                }
            }   
        }
    }

        private void CreatesJSON(Issue issue)
        {
            this.CreateLabels(issue);
            this.CreateBudgets(issue);
            this.CreateRealized(issue);
        }

        public void CreateLabels(Issue issue)
        {            
            issue.KPIs.LabelsJSON = JsonConvert.SerializeObject(issue.KPIs.Periods);            
        }

        public void CreateBudgets(Issue issue)
        {
            issue.KPIs.BudgetsJSON = JsonConvert.SerializeObject(issue.KPIs.Budgets);
        }

        public void CreateRealized(Issue issue)
        {            
            issue.KPIs.RealizedJSON = JsonConvert.SerializeObject(issue.KPIs.Realized);
        }
    }
}
