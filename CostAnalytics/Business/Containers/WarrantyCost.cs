using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class WarrantyCost : IContainer.IWarrantyCost
    {
        #region Constructors
        /// <summary> Default constructor for class WarrantyCost </summary>
        public WarrantyCost() { } //End default constructor

        /// <summary> Constructor that initializes class with pre-specified members. </summary>
        /// <param name="ID"> The Warranty ID </param>
        /// <param name="Date"> The data these costs were effective. </param>
        /// <param name="Cost"> The cost of warranty. </param>
        /// <param name="Warranty"> The Warranty object that holds the type of warranty. </param>
        /// <param name="Configuration"> The Configuration object that holds the config we're applying this warranty cost to. </param>
        public WarrantyCost(int ID, DateTime Date, double Cost, Warranty Warranty, Configuration Configuration)
        {
            m_ID = ID;
            m_Date = Date;
            m_Cost = Cost;
            m_Warranty = Warranty;
            m_Configuration = Configuration;
        } //End constructor (int, Datetime, double, Warranty, Configuration)
        #endregion

        #region Properties
        /// <summary> Property for WarrantyCost ID </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        } //End property ID

        /// <summary> Property for Date </summary>
        public DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        } //End property Date

        /// <summary> Property for Cost </summary>
        public double Cost
        {
            get { return m_Cost; }
            set { m_Cost = value; }
        } //End property Cost

        /// <summary> Property for Warranty </summary>
        public Warranty Warranty
        {
            get
            {
                if (m_Warranty == null)
                    m_Warranty = new Warranty();
                return m_Warranty;
            }
            set { m_Warranty = value; }
        } //End property Product

        /// <summary> Property for Configuration </summary>
        public Configuration Configuration
        {
            get
            {
                if (m_Configuration == null)
                    m_Configuration = new Configuration();
                return m_Configuration;
            }
            set { m_Configuration = value; }
        } //End property Configuration
        #endregion


        #region Methods
        /// <summary> Compares two Biz.Container.WarrantyCost objects for equality </summary>
        /// <param name="obj"> The WarrantyCost object you want to compare </param>
        /// <returns>True if objects are of the same class and hold the same attribute values; false otherwise. </returns>
        public override bool Equals(object obj)
        {
            if (obj is WarrantyCost)
            {
                WarrantyCost WarrantyCost = (WarrantyCost)obj;
                if (
                    ID != WarrantyCost.ID ||
                    Date != WarrantyCost.Date ||
                    Cost != WarrantyCost.Cost ||
                    Warranty.ID != WarrantyCost.Warranty.ID ||
                    Configuration.ID != WarrantyCost.Configuration.ID
                    )
                    return false;
                else
                    return true;
            }//endif
            else
            {
                return false;
            }//end else
        }//end method

        /// <summary> Custom Get Hash Code method for WarrantyCost </summary>
        /// <returns> The object's hash code, per the default provider.</returns>
        public override int GetHashCode()
        { return base.GetHashCode(); } //End method
        #endregion

        #region Members
        private int m_ID;
        private DateTime m_Date;
        private double m_Cost;
        private Warranty m_Warranty;
        private Configuration m_Configuration;
        #endregion

    } //End class WarrantyCost
}//end namespace
