using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Business.Container
{
    class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LOB { get; set; }
        public int Model { get; set; }
        public string Variant { get; set; }
    }
}
