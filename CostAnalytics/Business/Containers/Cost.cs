using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Cost : IContainer.ICost
    {
        public int ID { get; set; }
        public Iteration Iteration { get; set; }
        public DateTime Date { get; set; }
        public double CurrentCost { get; set; }
        public double CostNext1 { get; set; }
        public double CostNext2 { get; set; }
        public double CostNext3 { get; set; }
    }
}
