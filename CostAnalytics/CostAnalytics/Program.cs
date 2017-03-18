using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;

namespace CostAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            string flcPath = @"C:\Users\zosimo_guardia\Documents\Dell\Project Draco\Cost Analytics\FLC\FLC_EXTRACT_EUC_SC_20170307080355.csv";
            //string warPath = @"C:\Users\zosimo_guardia\Documents\Dell\Project Draco\Cost Analytics\Warranty\201703.xlsx";
            var dicPath = @"C:\Users\zosimo_guardia\Documents\Dell\Project Draco\Cost Analytics\costAnalytics\consolidated.txt";
            Stopwatch timer = Stopwatch.StartNew();
            /** Extract Data from important files by calling the respective methods.
             *    flcExtract(String path) parses through the FLC_EXTRACT CSV and extracts all product related information, including all costs
             *                            except warranty information.
             *    warExtract(String path) parses through the Warranty Cost XLSX provided by PG Finance and gets the missing warranty data.
             *    dicExtract(String path) reads a multiple delimited text file that allows us to standardize product names and filter retrieved
             *                            data by product and config types.
             **/
            
            Program instance = new Program();
            Support dictionary = new Support(dicPath);
            Console.WriteLine("Dictionary read in {0}ms", timer.ElapsedMilliseconds);

            DataTable sustainData = instance.flcExtract(flcPath, dictionary);
            Console.WriteLine("FLC_Extract parsed in {0}ms", timer.ElapsedMilliseconds);
            timer.Stop();
        }
        /** <summary>
          * This method goes through the FLC_Extract in CSV form, filters out rows and columns we don't care about, and returns
          * a DataTable we can then consolidate and update to the Database.
          * Still under discussion whether we should update it to consolidate in-method...
          * </summary>
          * <value name="path">The path to the FLC_Extract file.</value>
          * <value name="dictionary">A Support object containing standardized product names and configs we need to report on.</value>
          * <returns name="sustainingData">A DataTable containing the relevant excerpt of FLC_Extract data.</returns>
          **/
        public DataTable flcExtract(string path, Support dictionary)
        {
            var FS = ";";
            string[] selectedHeaders = {    @"Region", "Country", "CCN", "LOB", "Platform", "Configuration Type", "Config Name",
                                            "SKU", "SKU Description", "Commodity", "Total Measure Name", "Measure Detail", "MOD"};
            Regex expression = new Regex(@"^(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec).*Mth 1$");
            Dictionary<string, int> importantFields = new Dictionary<string, int>();
            DataTable sustainData = new DataTable("flcExtract");
            string fecha = "";

            //## Start reading CSV ##
            Console.WriteLine("Trying to read CSV File...");
            using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(path))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(FS);
                parser.TrimWhiteSpace = true;
                int row = 1;

                while (!parser.EndOfData)
                {
                    string[] line = parser.ReadFields();

                    //Assuming the first row contains the headers
                    if (row == 1)
                    {  
                        //Find indexes for columns we're interested in
                        foreach (string importantColumn in selectedHeaders)
                        {
                            int fieldNo = Array.IndexOf(line, importantColumn);
                            if (fieldNo != -1)
                            {   importantFields.Add(importantColumn, Array.IndexOf(line,importantColumn));   }
                            else
                            {   throw new IndexOutOfRangeException(importantColumn + " not found in row " + row + "."); }
                        }

                        //Now get the cost information. Subsequent months are assumed to be next to current month data.
                        foreach (string column in line)
                        {
                            if (expression.IsMatch(column))
                            {
                                fecha = expression.Match(column).ToString();
                                Console.WriteLine("Processing file for {0}", fecha);
                                importantFields.Add("thisMonth", Array.IndexOf(line, column));
                                importantFields.Add("nextMonth", Array.IndexOf(line, column) + 1);
                                importantFields.Add("monthPlus2",Array.IndexOf(line, column) + 2);
                                importantFields.Add("monthPlus3",Array.IndexOf(line, column) + 3);
                            }
                        }

                        //Finally, create the DataTable columns, handling the special case for cost.
                        foreach (string column in importantFields.Keys.ToList())
                        {
                            if (column == "thisMonth" || column == "nextMonth" ||
                               column == "monthPlus2" || column == "monthPlus3")
                            { sustainData.Columns.Add(column); }
                            else
                            { sustainData.Columns.Add(line[importantFields[column]]); }
                        }
                    }
                    else
                    {
                        //Variables for readability
                        string region     = line[importantFields["CCN"]];
                        string lob        = line[importantFields["LOB"]];
                        string configName = line[importantFields["Config Name"]];

                        /** Now, filter out records we don't care about before adding importantFields to the DataTable.
                          * Two IF statements allow us to check for available fields before we spend time parsing costs to float,
                          * and ensuring they have data.
                          **/
                        try
                        {
                            if (region  == "DAO" &&
                                lob     == "Dell Latitude" &&
                                dictionary.confValid(configName))
                            {
                                float currCost = 0;
                                float costPlus1 = 0;
                                float costPlus2 = 0;
                                float costPlus3 = 0;
                                if (float.TryParse(line[importantFields["thisMonth"]], out currCost) &&
                                    float.TryParse(line[importantFields["nextMonth"]], out costPlus1) &&
                                    float.TryParse(line[importantFields["monthPlus2"]], out costPlus2) &&
                                    float.TryParse(line[importantFields["monthPlus3"]], out costPlus3) &&
                                    currCost != 0 && costPlus1 != 0 && costPlus2 != 0 && costPlus3 != 0)
                                {
                                    DataRow rowData = sustainData.NewRow();
                                    foreach (string column in importantFields.Keys.ToList())
                                    { rowData[column] = line[importantFields[column]]; }
                                    sustainData.Rows.Add(rowData);
                                }

                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            Console.WriteLine("R{0}: Error while looking up key in importantFields. Details below:", row);
                            Console.WriteLine("[importantFields]");
                            foreach (var key in importantFields.Keys.ToList())
                            { Console.WriteLine("Key: {0}\t\tValue: {1}", key, importantFields[key]); }
                            Console.WriteLine("\n[Record]");
                            foreach (string field in line)
                            { Console.Write(field + ";"); }
                        }
                    }
                    row++;
                }
                Console.WriteLine("Done. Processed {0} rows.\n", row);
            }
            return sustainData;
        }
        public void warExtract(string path)
        {
            //DataTable productData = new DataTable flcExtract(flcPath);


            //SLDocument sl = new SLDocument();

            //// set a boolean at "A1"
            //sl.SetCellValue("A1", true);

            //// set at row 2, columns 1 through 20, a value that's equal to the column index
            //for (int i = 1; i <= 20; ++i) sl.SetCellValue(2, i, i);

            //// set the value of PI
            //sl.SetCellValue("B3", 3.14159);

            //// set the value of PI at row 4, column 2 (or "B4") in string form.
            //// use this when you already have numeric data in string form and don't
            //// want to parse it to a double or float variable type
            //// and then set it as a value.
            //// Note that "3,14159" is invalid. Excel (or Open XML) stores numerals in
            //// invariant culture mode. Frankly, even "1,234,567.89" is invalid because
            //// of the comma. If you can assign it in code, then it's fine, like so:
            //// double fTemp = 1234567.89;
            //sl.SetCellValueNumeric(4, 2, "3.14159");

            //// normal string data
            //sl.SetCellValue("C6", "This is at C6!");

            //// typical XML-invalid characters are taken care of,
            //// in particular the & and < and >
            //sl.SetCellValue("I6", "Dinner & Dance costs < $10");

            //// this sets a cell formula
            //// Note that if you want to set a string that starts with the equal sign,
            //// but is not a formula, prepend a single quote.
            //// For example, "'==" will display 2 equal signs
            //sl.SetCellValue(7, 3, "=SUM(A2:T2)");

            //// if you need cell references and cell ranges *really* badly, consider the SLConvert class.
            //sl.SetCellValue(SLConvert.ToCellReference(7, 4), string.Format("=SUM({0})", SLConvert.ToCellRange(2, 1, 2, 20)));

            //// dates need the format code to be displayed as the typical date.
            //// Otherwise it just looks like a floating point number.
            //sl.SetCellValue("C8", new DateTime(3141, 5, 9));
            //SLStyle style = sl.CreateStyle();
            //style.FormatCode = "d-mmm-yyyy";
            //sl.SetCellStyle("C8", style);

            //sl.SetCellValue(8, 6, "I predict this to be a significant date. Why, I do not know...");

            //sl.SetCellValue(9, 4, 456.123789);
            //// we don't have to create a new SLStyle because
            //// we only used the FormatCode property
            //style.FormatCode = "0.000%";
            //sl.SetCellStyle(9, 4, style);

            //sl.SetCellValue(9, 6, "Perhaps a phenomenal growth in something?");

            //var path = "C:\\Users\\Dominic_Bett\\Desktop\\HelloWorld.xlsx";
            //sl.SaveAs("C:\\Users\\Dominic_Bett\\Desktop\\HelloWorld.xlsx");

            //Console.WriteLine("End of program");
            //Console.WriteLine($"File has been saved to {path}");
            //Console.ReadLine();
        }
    }

    /** <summary>
      * This class *could* be replaced by a function that reads from a CSV, but unsure about
      * resource optimization (open file everytime it's called).
      * </summary>
      * <param name="path"> Contains the path to the file containing product translation and valid configs.
      *                     Expected file format:  FLC_Extract Name : Marketing Name : Valid Configuration 1 [ | Valid Configuration 2 | ... ]
      * </param>
      **/
    class Support
    {
        private Dictionary<string, string> mktName = new Dictionary<string, string>();
        private List<string> validConfigs = new List<string>();
        public Support(string path)
        {
            try
            {
                var file = File.ReadLines(path);
                foreach (string line in file)
                {
                    //Takes care of carriage return without making dictionary.txt unreadable.
                    line.Replace(Environment.NewLine, "");

                    mktName.Add(line.Split(':')[0], line.Split(':')[1]);
                    foreach (string config in line.Split(':')[2].Split('|'))
                    { validConfigs.Add(config); }
                }
                //Apparently, this is not necessary when using ReadLines because it disposes it under the hood
                //file.dispose();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found. Make sure that the dictionary file exists in the path.");
                Console.WriteLine(path);
                Environment.Exit(-1);
            }
            catch (IOException ioe)
            {
                Console.WriteLine("There was an error opening the file. Printing for debug: ");
                Console.WriteLine("Error: {}" + ioe.ToString());
                throw ioe;
            }
        }

        /** <summary>
          * This method translates PG Finance product names to the standardized "CodeName,Model" format.
          * </summary>
          * <value name="product">The product name as shown in the FLC_Extract file.</value>
          * <returns name="stdProductName">The product name in the standardized format.</returns>
          **/
        internal string translate(string product)
        {
            try
            { return mktName[product]; }
            catch (KeyNotFoundException knfe)
            {
                Console.WriteLine("No match found for entry {0}", product);
                throw knfe;
            }
        }

        /** <summary>
          * This method validates whether a config is valid for reporting purposes.
          * </summary>
          * <value name="config">The product's configuration name as shown in the FLC_Extract file.</value>
          * <returns>Boolean showing if the configuration is valid.</returns>
          **/
        internal Boolean confValid(string config)
        { return validConfigs.Contains(config); } 
    }
}
