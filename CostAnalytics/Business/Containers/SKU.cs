using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Dell.CostAnalytics.Business.Containers.Interfaces;

namespace Dell.CostAnalytics.Business.Containers
{
    public class SKU:IContainer.ISKU
    {
        #region Constructors
        /// <summary> Default constructor for SKU class. </summary>
        public SKU() { } //End default constructor

        /** <summary> Instantiates a SKU object with specified parameters. </summary>
          * <param name="_id"> The SKU ID. </param>
          * <param name="_name"> The name of the SKU (Ex. 998-CIND) </param>
          * <param name="_description"> The SKU Description. </param>
          * <param name="_commodity"> The SKU Type (SSD, Memory, Base, etc.) </param>
          **/
        public SKU(int _id, string _name, string _description, string _commodity)
        {
            m_ID          = _id;
            m_Name        = _name;
            m_Description = _description;
            m_Commodity   = _commodity;
        } //End SKU constructor (int, string, string, string)
        #endregion

        #region Members
        private int m_ID;
        private string m_Name;
        private string m_Description;
        private string m_Commodity;
        #endregion

        #region Properties
        /// <summary> Property for SKUID </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        } //End property ID

        /// <summary> Property for SKU Name </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        } //End property Name

        /// <summary> Property for SKU description</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        } //End property Description

        /// <summary> Property for SKU Commodity type</summary>
        public string Commodity
        {
            get { return m_Commodity; }
            set { m_Commodity = value; }
        } //End property Commodity
        #endregion
    } //End class SKU
} //End namespace Dell.CostAnalytics.Business.Containers