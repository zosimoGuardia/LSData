using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cont = Dell.CostAnalytics.Business.Containers;
using Handler = Dell.CostAnalytics.Business.Handlers;

namespace Dell.CostAnalytics.DataFactory.Handlers
{
    public class DatabaseObject
    {
        public DatabaseObject()
        {
           if (Measures == null) Measures =  Handler.Measure.GetAll().ToList();
            //TODO: Initialize all other objects with exception of iterations
        }

        //TODO: Implement Get Methods for all other objects, with exception of iterations

        public Cont.Measure GetMeasure(Cont.Measure measure)
        {
            Cont.Measure toReturn = null;

            toReturn = Measures.FirstOrDefault(x => x.Equals(measure));
            if (toReturn == null)
            {
                measure.ID = Handler.Measure.Add(measure);
                Measures.Add(measure);
                toReturn = measure;
            }
            return toReturn;
        }

        static List<Cont.Measure> Measures = null;
        //TODO: declare all other properties as well, with exception of iterations
    }
}
