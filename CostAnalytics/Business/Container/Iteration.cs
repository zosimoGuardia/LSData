using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Business.Container
{
    public class Iteration
    {
        public int ID { get; set; }
        public Region Region { get; set; }
        public Product Product { get; set; }
        public Configuration Configuration { get; set; }
        public SKU SKU { get; set; }
        public Measure Measure { get; set; }
    }
}
