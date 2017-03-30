using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Iteration :IContainer.IIteration
    {
        public int ID { get; set; }
        public Region Region { get; set; }
        public Product Product { get; set; }
        public Configuration Configuration { get; set; }
        public SKU SKU { get; set; }
        public Measure Measure { get; set; }
    }
}
