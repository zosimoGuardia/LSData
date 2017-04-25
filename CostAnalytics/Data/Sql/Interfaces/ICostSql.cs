using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface ICostSql
    {
        int Add(Cont.Cost info, SqlTransaction transaction = null);
        void Update(Cont.Cost info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.Cost GetByID(int ID);
        Cont.Cost[] GetAll();
    }//end interface
}//end namespace
