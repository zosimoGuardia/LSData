using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cont = Dell.CostAnalytics.Data.Containers;


namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface IProduct
    {
        int Add(Cont.Product info, SqlTransaction transaction = null);
        void Update(Cont.Product info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.Product GetByID(int ID);
        Cont.Product[] GetAll();
    }//end interface
}//end namespace
