using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface IIterationSql
    {
        int Add(Cont.Iteration info, SqlTransaction transaction = null);
        void Update(Cont.Iteration info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.Iteration GetByID(int ID);
        Cont.Iteration[] GetAll();
    }//end interface
}//end namespace