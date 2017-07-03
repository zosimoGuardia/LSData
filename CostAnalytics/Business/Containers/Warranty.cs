using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Warranty : IContainer.IWarranty
    {
        #region Constructors
        /// <summary> Default constructor for class Warranty </summary>
        public Warranty() { } //End default constructor

        /** <summary> Constructor that initializes class with specified parameters </summary>
          * <param name="ID"> The Warranty ID </param>
          * <param name="Name"> The warranty name </param>
          * <param name="Description"> The warranty description </param>
          **/
        public Warranty(int ID, string Name, string Description)
        {
            m_ID = ID;
            m_Name = Name;
            m_Description = Description;
        } //End constructor (int, string, string)
        #endregion

        #region Properties
        /// <summary> Property for Warranty ID </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        } //End property ID

        /// <summary> Property for Name </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        } //End property Name

        /// <summary> Description Property </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        } //End property Description

        #endregion

        #region Methods
        /// <summary> Compares two Biz.Container.Warranty objects for equality </summary>
        /// <param name="obj">The Warranty object you want to compare </param>
        /// <returns>True if objects are of the same class and hold the same attribute values; false otherwise. </returns>
        public override bool Equals(object obj)
        {
            if (obj is Warranty)
            {
                Warranty Warranty = (Warranty)obj;
                if (
                    Name != Warranty.Name ||
                    Description != Warranty.Description
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

        /// <summary> Custom Get Hash Code method for Warranty </summary>
        /// <returns> The object's hash code, per the default provider.</returns>
        public override int GetHashCode()
        { return base.GetHashCode(); } //End method
        #endregion

        #region Members
        private int m_ID;
        private string m_Name;
        private string m_Description;
        #endregion

    } //End class Warranty
}//end namespace
