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
            System.Data.DataTable toReturn = new System.Data.DataTable();

            // Column Headers
            toReturn.Columns.AddRange(new System.Data.DataColumn[]{
                    new System.Data.DataColumn("Name"),
                new System.Data.DataColumn("Region"),
                new System.Data.DataColumn("Date"),
                new System.Data.DataColumn("Type"),
                new System.Data.DataColumn("Description"),
                new System.Data.DataColumn("Cost")
            });
            try
            {
                using (SLDocument slDocument = new SLDocument(this.m_warrantyInfoFilePath, WARRANTY_WORKBOOK))
                {
                    //DateTime now = DateTime.Now;
                    //string currentMonth =  now.ToString("MMM") + "/" + now.ToString("yy");
                    SLWorksheetStatistics slWorksheetStats = slDocument.GetWorksheetStatistics();
                    //string currentMonth = "may/17";
                    DateTime currentDate = DateTime.ParseExact(Path.GetFileNameWithoutExtension(this.m_warrantyInfoFilePath), "yyyyMM", null);
                    string currHeader = "";
                    int variantCol = 0;
                    int descriptionCol = 0;
                    int costCol = 0;

                    for (int column = slWorksheetStats.StartColumnIndex; column <= slWorksheetStats.EndColumnIndex; column++)
                    {
                        /* First row has merged cells for information types (Description vs. Monthly Costs)
                         * The getCellValueAsString starts at index 1 */
                        if (!String.IsNullOrEmpty(slDocument.GetCellValueAsString(1, column).Trim()))
                        {
                            currHeader = slDocument.GetCellValueAsString(1, column).Trim().ToLower();
                        }//end if
                        /* Second row is the actual header file. */
                        if (slDocument.GetCellValueAsString(2, column).Trim().ToLower() == VARIANT)
                        {
                            variantCol = column;
                            continue;
                        }//end if
                        if (currHeader == DESCRIPTION_HEADER &&
                            slDocument.GetCellValueAsString(2, column).Trim().ToLower() == REGION)
                        {
                            descriptionCol = column;
                            continue;
                        }//end if

                        try
                        {
                            if (currentDate.Equals(GetDateFromMMM_FYyy(currHeader)) &&
                                slDocument.GetCellValueAsString(2, column).Trim().ToLower().Equals(REGION))
                            {
                                costCol = column;
                                break;
                            }
                            /*
                            if (getDate(currHeader).ToString("MMM/yy").Replace(" ", string.Empty).ToLower() == currentMonth &&
                                slDocument.GetCellValueAsString(2, column).Trim().ToLower() == REGION)
                            {
                                costCol = column;
                                break;
                            }*/
                        }//end try
                        catch (FormatException fe)
                        {
                            System.Diagnostics.Debug.WriteLine("[" + currHeader + "]  " + fe.ToString());
                        }//end catch
                        catch (ArgumentOutOfRangeException aoore)
                        {
                            System.Diagnostics.Debug.WriteLine("[" + currHeader + "]  " + aoore.ToString());
                        }//end catch
                    }//end for

                    for (int row = slWorksheetStats.StartRowIndex + 2; row < slWorksheetStats.EndRowIndex; row++)
                    {
                        if (slDocument.GetCellValueAsString(row, variantCol) != "")
                        {
                            /* Schema -->  ||  Name  |  Region  |  Date  |  Type  |  Description  |  Cost ||  */
                            try
                            {
                                string warrantyName = String.Format("scp_Lati_" + slDocument.GetCellValueAsString(row, variantCol).Split(',')[1] + "-min");

                                var description = slDocument.GetCellValueAsString(row, descriptionCol);
                                Double cost;
                                Double.TryParse(slDocument.GetCellValueAsString(row, costCol).Replace("$", String.Empty).Replace(",", String.Empty).Trim(), out cost); // Return 0.0 if Cannot Parse Value
                                toReturn.Rows.Add(warrantyName,
                                            REGION,
                                            currentDate.ToString("MM/yy"),
                                            TYPE,
                                            description,
                                            cost
                                            );
                            }//end try
                            catch (IndexOutOfRangeException)
                            {
                                System.Diagnostics.Debug.WriteLine("Error parsing " + slDocument.GetCellValueAsString(row, variantCol) + ".");
                            }//end catch
                        }//end if
                    }
                } //End using statement
            } //End try block
            catch (Exception e)
            {
                throw e;
            }//end catch


            this.ReadData(toReturn);
        }//end method

        
        
        /// <summary> This method reads the filtered rows from the data table and instantiates the respective container objects. </summary>
        /// <param name="record">A datatable row containing the columns rows we care about per the specified filter criteria. </param>
        private void ReadData(System.Data.DataTable dtWarrantyData)
        {
            Handlers.DatabaseObject dbo = new Handlers.DatabaseObject();

            foreach (System.Data.DataRow record in dtWarrantyData.Rows)
            {
                // Warranty
                Cont.Warranty warrantyInfo = new Business.Containers.Warranty()
                {
                    Name = record["Name"].ToString(),
                    Description = record["Description"].ToString()
                };
                dbo.GetWarranty(warrantyInfo);

                //WarrantyCost

                Cont.WarrantyCost warrantyCostInfo = new Business.Containers.WarrantyCost()
                {
                    Date = Convert.ToDateTime(record["Date"]),
                    Cost = Convert.ToDouble(record["Cost"]),
                    Warranty = warrantyInfo,
                    Configuration = null //TODO: Extract Config info from PlatfornConfig File and pass in here
                };
                dbo.GetWarrantyCost(warrantyCostInfo);

            }//end for
        }//end method
    

        /// <summary>
        /// Returns DateTime format from input string of format MMM FYyy
        /// Example: Jul FY18 return 07/2017
        /// </summary>
        /// <param name="MMMM_FYyy"></param>
        /// <returns></returns>
        public DateTime GetDateFromMMM_FYyy(string MMM_FYyy)
        {
            DateTime toReturn;
            string MMMyy = MMM_FYyy.Replace(" fy", String.Empty);
            string[] formats = { "MMMMyy", "MMMyy" };
            if (DateTime.TryParseExact(MMMyy, formats, null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out toReturn)) // Returns  1/1/1 of canot parse string to date
            {
                if(toReturn.Month > 1) // New Fiscal year starts in February
                    toReturn = toReturn.AddYears(-1);
            }// end if
            return toReturn;
        }//end method
     
        /*
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
        */

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

        #region Members
        private string m_warrantyInfoFilePath;
        #endregion

        #region Constants
        private const string WARRANTY_WORKBOOK = "Summary- EUC";
        private const string VARIANT = "variant";
        private const string DESCRIPTION_HEADER = "term";
        private const string REGION = "dao";
        private const string TYPE = "Warranty";
        private const string MEASURE_TYPE = "Services";
        #endregion

    } //End WarrantyInfoParser class

} //End Namespace
