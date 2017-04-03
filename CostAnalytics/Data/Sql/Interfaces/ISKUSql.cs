using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface ISKUSql
    {
        int Add(Cont.SKU info, SqlTransaction transaction = null);
        void Update(Cont.SKU info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.SKU GetByID(int ID);
        Cont.SKU[] GetAll();
    }//end interface
}//end namespace