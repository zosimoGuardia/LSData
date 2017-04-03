using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cont = Dell.CostAnalytics.Data.Containers;

namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface IRegionSql
    {
        int Add(Cont.Region info, SqlTransaction transaction = null);
        void Update(Cont.Region info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.Region GetByID(int ID);
        Cont.Region[] GetAll();
    }//end interface
}//end namespace