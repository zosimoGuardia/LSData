using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class Region:IContainer.IRegion
    {
        #region Constructors
        /// <summary> Default constructor for the Region class. </summary>
        public Region() { } //End default constructor

        /** <summary> Constructor that instantiates a Region object with specified parameters. </summary>
          * <param name="_id"> The Region ID </param>
          * <param name="_regionName"> The region name </param>
          * <param name="_country"> The country name </param>
          * <param name="_countryCode"> The country code </param>
          **/
        public Region(int _id, string _regionName, string _country, string _countryCode)
        {
            m_ID = _id;
            m_regionName = _regionName;
            m_Country = _country;
            m_countryCode = _countryCode;
        } //End constructor (int, string, string, string)
        #endregion

        #region Members
        private int m_ID;
        private string m_regionName;
        private string m_Country;
        private string m_countryCode;
        #endregion

        #region Properties
        /// <summary> Property for Region ID</summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        } //End property ID

        /// <summary> Property for Region Name</summary>
        public string RegionName
        {
            get { return m_regionName; }
            set { m_regionName = value; }
        } //End property regionName

        /// <summary> Property for country </summary>
        public string Country
        {
            get { return m_Country; }
            set { m_Country = value; }
        } //End property Country

        /// <summary> Property for countryCode</summary>
        public string CountryCode
        {
            get { return m_countryCode; }
            set { m_countryCode = value; }
        } //End property countryCode
        #endregion
    } //End class Region
}
