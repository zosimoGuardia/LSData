using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cont = Dell.CostAnalytics.Data.Containers;


namespace Dell.CostAnalytics.Data.Sql.Interfaces
{
    interface IWarrantyCostSql
    {
        int Add(Cont.WarrantyCost info, SqlTransaction transaction = null);
        void Update(Cont.WarrantyCost info, SqlTransaction transaction = null);
        void Delete(int ID, SqlTransaction transaction = null);
        Cont.WarrantyCost GetByID(int ID);
        Cont.WarrantyCost[] GetAll();
    }//end interface
}//end namespace
