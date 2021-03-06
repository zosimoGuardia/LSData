﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Configuration: IContainer.IConfiguration
    {
        #region Constructors
        /// <summary> Default constructor for the Configuration class. </summary>
        public Configuration() { }//End default constructor

        /** <summary> Instantiates a configuration object with specified parameters. </summary>
          * <param name="ID"> The configuration ID </param>
          * <param name="Name"> The configuration name. </param>
          * <param name="Type"> The configuration type (Min, Avg, etc.) </param>
          **/
        public Configuration(int ID, string Name, string Type)
        {
            m_ID = ID;
            m_Name = Name;
            m_Type = Type;
        } //End constructor (int, string, string)
        #endregion

        #region Properties
        /// <summary> Property for Configuration ID</summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        } //End Property Configuration ID

        /// <summary> Property for Name </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        } //End property Name

        /// <summary> Property for Configuration Type </summary>
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        } //End property Type
        #endregion

        #region Methods
        /// <summary> Compares two Biz.Container.Configuration objects for equality </summary>
        /// <param name="obj">The Configuration object you want to compare </param>
        /// <returns>True if objects are of the same class and hold the same attribute values; false otherwise. </returns>
        public override bool Equals(object obj)
        {
            if (obj is Configuration)
            {
                Configuration Configuration = (Configuration)obj;
                if (
                    Name != Configuration.Name ||
                    Type != Configuration.Type
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

        /// <summary> Custom Get Hash Code method for Configuration </summary>
        /// <returns> The object's hash code, per the default provider.</returns>
        public override int GetHashCode()
        { return base.GetHashCode(); } //End method
        #endregion

        #region Members
        private int m_ID;
        private string m_Name;
        private string m_Type;
        #endregion
    } //End class Configuration
} //End namespace Dell.CostAnalytics.Business.Containers
