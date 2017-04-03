using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface IConfigurationSql
    {
        int Add(Cont.Configuration info, SqlTransaction transaction = null);
        void Update(Cont.Configuration info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.Configuration GetByID(int ID);
        Cont.Configuration[] GetAll();
    }//end interface
}//end namespace