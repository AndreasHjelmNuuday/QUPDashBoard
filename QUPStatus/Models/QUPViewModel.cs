using DotJira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QUPStatus.Models
{
    public class QUPViewModel
    {
        public List<Issue> issues { get; set; }
        public String parentKey { get; set; }

    }
}
