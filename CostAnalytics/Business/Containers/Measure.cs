using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dell.CostAnalytics.Data.Containers;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Measure : IContainer.IMeasure
    {
        #region Constructors
        /// <summary> Default constructor for Measure </summary>
        public Measure() { } //End default constructor

        /// <summary>
        /// Constructor that initializes a Measure object with specified parameters.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        public Measure(int ID, string Name)
        {
            m_ID = ID;
            m_Name = Name;
        } //End constructor (int, string)
        #endregion

        #region Properties
        /// <summary> Property for Measure ID</summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        } //End property ID

        /// <summary> Property for Measure Name</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        } //End property Name
        #endregion

        #region Members
        private int m_ID;
        private string m_Name;
        #endregion

    } //End class Measure
}//end namespace
