using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Data.Containers.Interfaces
{
    interface IWarrantyCost
    {
        int ID { get; set; }
        DateTime Date { get; set; }
        double Cost { get; set; }
        int WarrantyID { get; set; }
        int ConfigurationID { get; set; }
    } //End interface

} //End namespace
