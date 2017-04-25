using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Cost : IContainer.ICost
    {
        #region Constructors
        /// <summary> Default constructor for class Cost </summary>
        public Cost() { } //End default constructor

        /** <summary> Constructor that initializes class with specified parameters </summary>
          * <param name="ID"> The Cost ID </param>
          * <param name="Iteration"> Iteration ID that references this Cost ID </param>
          * <param name="Date"> Date of Cost information </param>
          * <param name="CurrentCost"> Cost at Date _date </param>
          * <param name="CostNext1"> Next month's predicted cost. </param>
          * <param name="CostNext2"> Predicted costs two months down the line. </param>
          * <param name="CostNext3"> Predicted costs three months down the line. </param>
          **/
        public Cost(int ID, Iteration Iteration, DateTime Date, double CurrentCost,
                    double CostNext1, double CostNext2, double CostNext3)
        {
            m_ID = ID;
            m_Iteration = Iteration;
            m_Date = Date;
            m_CurrentCost = CurrentCost;
            m_CostNext1 = CostNext1;
            m_CostNext2 = CostNext2;
            m_CostNext3 = CostNext3;
        } //End constructor (int, Iteration, Datetime, double, double, double, double)
        #endregion

        #region Properties
        /// <summary> Property for Cost ID </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        } //End property ID

        /// <summary> Property for Iteration </summary>
        public Iteration Iteration
        {
            get
            {
                if (m_Iteration == null)
                    m_Iteration = new Iteration();
                return m_Iteration;
            }
            set { m_Iteration = value; }
        } //End property Iteration

        /// <summary> Date Property </summary>
        public DateTime Date
        {
            get
            {
                if (m_Date == null)
                { return new DateTime(); }
                else
                { return m_Date; }
            }
            set { m_Date = value; }
        } //End property Date

        /// <summary> Property for Current Cost </summary>
        public double CurrentCost
        {
            get { return m_CurrentCost; }
            set { m_CurrentCost = value; }
        } //End Property CurrentCost

        /// <summary> Property for next month's predicted cost </summary>
        public double CostNext1
        {
            get { return m_CostNext1; }
            set { m_CostNext1 = value; }
        } //End property CostPlus1

        /// <summary> Property for two month forecasted cost</summary>
        public double CostNext2
        {
            get { return m_CostNext2; }
            set { m_CostNext2 = value; }
        } //End property CostPlus2

        /// <summary> Property for 3-month predicted cost </summary>
        public double CostNext3
        {
            get { return m_CostNext3; }
            set { m_CostNext3 = value; }
        } //End property CostPlus3
        #endregion

        #region Members
        private int m_ID;
        private Iteration m_Iteration;
        private DateTime m_Date;
        private double m_CurrentCost;
        private double m_CostNext1;
        private double m_CostNext2;
        private double m_CostNext3;
        #endregion

    } //End class Cost
}//end namespace
