using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface IPhaseSql
    {
        int Add(Cont.Phase info, SqlTransaction transaction = null);
        void Update(Cont.Phase info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.Phase GetByID(int ID);
        Cont.Phase[] GetAll();
    }//end interface
}//end namespace