using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface IWarrantySql
    {
        int Add(Cont.Warranty info, SqlTransaction transaction = null);
        void Update(Cont.Warranty info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.Warranty GetByID(int ID);
        Cont.Warranty[] GetAll();
    }//end interface
}//end namespace