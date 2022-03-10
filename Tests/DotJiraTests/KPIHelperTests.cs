using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotJira;
using NUnit.Framework;

namespace DotJira
{
    class KPIHelperTests
    {
        KPIHelper kPIHelper = new();
        private Issue parent = new();
        private Issue child = new();

        public Issue before()
        {
            parent.Fields = new();
            parent.Fields.Summary = "Parrent Issue";
            child.Fields = new();
            child.Fields.Summary = "Child Issue";
            child.Fields.Description =  "[jan; budget: 0,001; realized:0]" +
                                        "[feb; budget: 0,002; realized: 0]" +
                                        "[mar; budget: 0,003; realized: 0]" +
                                        "[apr; budget: 0,004; realized: 0]" +
                                        "[may; budget: 0,006; realized: 0]" +
                                        "[jun; budget: 0,008; realized: 0]" +
                                        "[jul; budget: 0,008; realized: 0]" +
                                        "[aug; budget: 0,011; realized: 0]" +
                                        "[sep; budget: 0,013; realized: 0]" +
                                        "[oct; budget: 0,015; realized: 0]" +
                                        "[nov; budget: 0,017; realized: 0]" +
                                        "[dec; budget: 0,020; realized: 0]";
            parent.Children.Add(child);
            return parent;
        }
        
        [Test]
        public void ChildIssueSummaryIsParsedSyntaxCorrect()
        {
         Issue parent = before();
        kPIHelper.ParseKPIs(new List<Issue> { parent });

            Assert.AreEqual("may",child.KPIs.Periods[4]); //May 
            Assert.AreEqual("0,008", child.KPIs.Budgets[5]); //June
            Assert.AreEqual("0", child.KPIs.Realized[11]); //December
        }
    }
}
