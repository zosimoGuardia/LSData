using System;
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
          * <param name="_id"> The configuration ID </param>
          * <param name="_name"> The configuration name. </param>
          * <param name="_type"> The configuration type (Min, Avg, etc.) </param>
          **/
        public Configuration(int _id, string _name, string _type)
        {
            m_ID = _id;
            m_Name = _name;
            m_Type = _type;
        } //End constructor (int, string, string)
        #endregion

        #region Members
        private int m_ID;
        private string m_Name;
        private string m_Type;
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
    } //End class Configuration
} //End namespace Dell.CostAnalytics.Business.Containers
