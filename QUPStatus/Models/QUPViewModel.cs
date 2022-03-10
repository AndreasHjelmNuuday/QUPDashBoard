using DotJira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QUPStatus.Models
{
    public class QUPViewModel
    {

        public QUPViewModel()
        {
            this.Issues = new();
        }
        public List<Issue> Issues { get; set; }
        public String ParentKey { get; set; }

        public string Quarter { get; set; }

    }
}
