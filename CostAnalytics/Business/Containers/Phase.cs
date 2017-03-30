using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Phase  : IContainer.IPhase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Product Product { get; set; }
    }
}
