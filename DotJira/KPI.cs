using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotJira
{
   public class KPI
    {
        internal string budgetsJSON;

        public List<string> Periods { get; set; }
        public List<string> Budgets { get; set; }
        public List<string> Realized { get; set; }
        public string LabelsJSON { get; set; }

        public string BudgetsJSON { get; set; }
        public string RealizedJSON { get; set; }
    }
}
