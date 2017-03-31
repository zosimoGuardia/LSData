using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Measure : IContainer.IMeasure
    {
        #region Constructors
        /// <summary> Default constructor for Measure </summary>
        public Measure() { } //End default constructor

        /** <summary> Constructor that initializes a Measure object with specified parameters. </summary>
          * <param name="_id"> The Measure ID </param>
          * <param name="_name"> The name of the Cost Adder </param>
          **/
        public Measure(int _id, string _name)
        {
            m_ID = _id;
            m_Name = _name;
        } //End constructor (int, string)
        #endregion

        #region Members
        private int m_ID;
        private string m_Name;
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

    } //End class Measure
}
