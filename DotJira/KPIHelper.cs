using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotJira
{
    public class KPIHelper
    {

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

        private static void ParseKPIString(Issue issue)
        {

            String[] description = issue.Fields.Description.Split("]");
            foreach (string line in description)
            {
                if (line != null && line.Length > 0) { 
                    string lineWithOutStartBracket = line.Substring(1).Trim();
                    string period = lineWithOutStartBracket.Split(";")[0];
                    string budget = lineWithOutStartBracket.Split(";")[1];

                    if (budget.Contains("budget:"))
                    {
                        budget = budget.Trim().Substring("budget:".Length).Trim();
                    }         
                    string realized = lineWithOutStartBracket.Split(";")[2];
                    if (realized.Contains("realized:"))
                    {
                            realized = realized.Trim().Substring("realized:".Length).Trim();
                    }

                    issue.KPIs.Periods.Add(period);
                    issue.KPIs.Budgets.Add(budget);
                    issue.KPIs.Realized.Add(realized);
                }   
            }
        }
    }
}
