using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers.Interfaces
{
    interface IWarranty
    {
        int ID { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
