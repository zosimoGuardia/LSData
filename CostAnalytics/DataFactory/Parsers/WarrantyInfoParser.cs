using Cont = Dell.CostAnalytics.Business.Containers;
using DataAccess;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Dell.CostAnalytics.DataFactory.Parsers
{
    public class WarrantyInfoParser : IDisposable
    {
        #region Constructors
        /// <summary> This constructor takes the warranty file path and sets it as an attribute. </summary>
        /// <param name="warrantyFileName"> File that contains the warranty information. </param>
        public WarrantyInfoParser(string warrantyFilePath)
        {
            this.m_warrantyInfoFilePath = warrantyFilePath;
        } //End constructor (string)
        #endregion

        #region Methods
        /// <summary> Parses through the Excel file, creates a data table with the information we care about, and calls ReadData. </summary>
        public void Parse()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();
            dt.Columns.AddRange(datacols);
            try
            {
                using (SLDocument warrantyXL = new SLDocument(this.m_warrantyInfoFilePath, WARRANTY_WORKBOOK))
                {
                    //DateTime now = DateTime.Now;
                    //string currentMonth =  now.ToString("MMM") + "/" + now.ToString("yy");
                    string currentMonth = "may/17";
                    SLWorksheetStatistics xlProperties = warrantyXL.GetWorksheetStatistics();
                    string currHeader = "";
                    int variantCol = 0;
                    int descriptionCol = 0;
                    int costCol = 0;

                    for (int column = xlProperties.StartColumnIndex; column < xlProperties.EndColumnIndex - 1; column++)
                    {
                        /* First row has merged cells for information types (Description vs. Monthly Costs)
                         * The getCellValueAsString starts at index 1 */
                        if (warrantyXL.GetCellValueAsString(1, column) != "")
                            currHeader = warrantyXL.GetCellValueAsString(1, column).Replace(" ", string.Empty).ToLower();

                        /* Second row is the actual header file. */
                        if (warrantyXL.GetCellValueAsString(2, column).Trim().ToLower() == VARIANT)
                        {
                            variantCol = column;
                            continue;
                        }
                        if (currHeader == DESCRIPTION_HEADER &&
                            warrantyXL.GetCellValueAsString(2, column).Trim().ToLower() == REGION)
                        {
                            descriptionCol = column;
                            continue;
                        }
                        try
                        {
                            if (getDate(currHeader).ToString("MMM/yy").Replace(" ", string.Empty).ToLower() == currentMonth &&
                                warrantyXL.GetCellValueAsString(2, column).ToLower().Trim() == REGION)
                            {
                                costCol = column;
                                break;
                            }
                        }
                        catch (FormatException fe)
                        {
                            System.Diagnostics.Debug.WriteLine("[" + currHeader + "]  " + fe.ToString());
                        }
                        catch (ArgumentOutOfRangeException aoore)
                        {
                            System.Diagnostics.Debug.WriteLine("[" + currHeader + "]  " + aoore.ToString());
                        }
                    }

                    for (int row = xlProperties.StartRowIndex + 2; row < xlProperties.EndRowIndex; row++)
                    {
                        if (warrantyXL.GetCellValueAsString(row, variantCol) != "")
                        {
                            /* Schema -->  ||  Name  |  Region  |  Date  |  Type  |  Description  |  Cost ||  */
                            try
                            {
                                string warrantyName = String.Format("scp_Lati_" + warrantyXL.GetCellValueAsString(row, variantCol).Split(',')[1] + "-min");

                                dt.Rows.Add(warrantyName,
                                            REGION,
                                            getDate(currentMonth),
                                            TYPE,
                                            warrantyXL.GetCellValueAsString(row, descriptionCol),
                                            warrantyXL.GetCellValueAsString(row, costCol));
                            }
                            catch (IndexOutOfRangeException)
                            {
                                System.Diagnostics.Debug.WriteLine("Error parsing " + warrantyXL.GetCellValueAsString(row, variantCol) + ".");
                            }
                        }
                    }
                } //End using statement
            } //End try block
            catch (Exception e)
            {
                throw e;
            }
            

            //Now read filtered data
            foreach (System.Data.DataRow record in dt.Rows)
                this.ReadData(record);
        }//end method


        /// <summary> This method reads the filtered rows from the data table and instantiates the respective container objects. </summary>
        /// <param name="record">A datatable row containing the columns rows we care about per the specified filter criteria. </param>
        private void ReadData(System.Data.DataRow record)
        {
            Handlers.DatabaseObject dbo = new Handlers.DatabaseObject();

            /* Measure */
            Cont.Measure measureInfo = new Business.Containers.Measure()
            {
                Name = MEASURE_TYPE
            };
            dbo.GetMeasure(measureInfo);


            /* SKU */
            Cont.SKU skuInfo = new Business.Containers.SKU()
            {
                Name = Convert.ToString(record["Name"]).Trim(),
                Description = Convert.ToString(record["Description"]),
                Commodity = Convert.ToString(record["Type"]).Trim()
            };
            dbo.GetSKU(skuInfo);


            /* Iteration */
            Cont.Iteration iterationInfo = new Cont.Iteration()
            {
                Measure = measureInfo,
                SKU = skuInfo
            };
            iterationInfo.ID = Business.Handlers.Iteration.Add(iterationInfo);


            /* Cost */
            Cont.Cost costInfo = new Business.Containers.Cost()
            {
                Iteration = iterationInfo,
                Date = Convert.ToDateTime(record["Date"]),
                CurrentCost = Convert.ToDouble(record["Cost"]),
                CostNext1 = Convert.ToDouble(record["Cost"]),
                CostNext2 = Convert.ToDouble(record["Cost"]),
                CostNext3 = Convert.ToDouble(record["Cost"])
            };
            costInfo.ID = Business.Handlers.Cost.Add(costInfo);
        }//end method

        /// <summary> This method converts a column header to a Datetime object. </summary>
        /// <param name="text"> The date in the following format: [mmm] FY[yy] (Ex. May FY18) </param>
        /// <returns> The calendar-year datetime object that corresponds to the param date. </returns>
        public DateTime getDate(string text)
        {
            int year = 0, month = 0;
            Dictionary<string, int> calendar = new Dictionary<string, int>
            {
                {"jan", 1}, {"feb", 2}, {"mar", 3}, {"apr", 4},  {"may", 5},  {"jun", 6},
                {"jul", 7}, {"aug", 8}, {"sep", 9}, {"oct", 10}, {"nov", 11}, {"dec", 12}
            };

            try
            {
                year = Convert.ToInt32("20" + text.Substring(text.Length - 2, 2));
                month = Convert.ToInt32(calendar[text.Substring(0, 3).ToLower()]);

                if (month != 1)             //Convert FY to CY
                    year -= 1;
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                throw aoore;
            }
            catch (FormatException fe)
            {
                throw fe;
            }
            catch (Exception exc)
            {
                throw exc;
            }

            return new DateTime(year, month, 1);
        } //End method


        /// <summary> Disposes object </summary>
        public void Dispose()
        {
            this.Dispose();
        }
        #endregion

        //#region Subclasses
        ///// <summary> Sub-class to store key information about the file. </summary>
        //public class WarrantyInfo
        //{
        //    public DateTime Date { get; set; }
        //    public string Region { get; set; }
        //    public string Name { get; set; }
        //    public string Type { get; set; } 
        //    public string Description { get; set; }
        //    public float Cost { get; set; }
        //}
        //#endregion

        #region Properties
        private string m_warrantyInfoFilePath;
        #endregion

        #region Constants
        private System.Data.DataColumn[] datacols =
        {
            new System.Data.DataColumn("Name"),
            new System.Data.DataColumn("Region"),
            new System.Data.DataColumn("Date"),
            new System.Data.DataColumn("Type"),
            new System.Data.DataColumn("Description"),
            new System.Data.DataColumn("Cost")
        };
        protected string WARRANTY_WORKBOOK = "Summary- EUC";
        protected string VARIANT = "variant";
        protected string DESCRIPTION_HEADER = "term";
        protected string REGION = "dao";
        protected string TYPE = "Warranty";
        protected string MEASURE_TYPE = "Services";
        #endregion
    } //End WarrantyInfoParser class

} //End Namespace
