using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotJira
{
    public class QuarterCalculator
    {
        
        public static string currentQuarter
        {
            get
            {
                int year = DateTime.Today.Year;
                int quarter = (int)Math.Ceiling(DateTime.Today.Month / 3m);

                return string.Format("{0} Q{1}", year, quarter);
            }
        }
    }
}
