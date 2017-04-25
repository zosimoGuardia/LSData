using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface IMeasureSql
    {
        int Add(Cont.Measure info, SqlTransaction transaction = null);
        void Update(Cont.Measure info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.Measure GetByID(int ID);
        Cont.Measure[] GetAll();
    }//end interface
}//end namespace