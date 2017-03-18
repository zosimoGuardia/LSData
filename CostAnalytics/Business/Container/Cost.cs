using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Business.Container
{
    class Cost
    {
        public int ID { get; set; }
        public int IterationID { get; set; }
        public DateTime Date { get; set; }
        public double CurrentCost { get; set; }
        public double CostNext1 { get; set; }
        public double CostNext2 { get; set; }
        public double CostNext3 { get; set; }
    }
}
