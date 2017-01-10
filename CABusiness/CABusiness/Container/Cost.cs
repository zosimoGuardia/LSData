using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CABusiness.Container
{
    class Cost
    {
        public int ID { get; set; }
        public int SKUID { get; set; }
        public int RegionID { get; set; }
        public DateTime Date { get; set; }
        public double CurrentCost { get; set; }
        public double Next1Cost { get; set; }
        public double Next2Cost { get; set; }
        public double Next3Cost { get; set; }
    }
}
