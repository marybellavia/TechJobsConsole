using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

namespace TechJobsConsole
{
    class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            return AllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }
            return values;
        }

        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            // creating dictionary for return "jobs"
            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();
            // lopping through the dictionary row
            foreach (Dictionary<string, string> row in AllJobs)
            {
                // accessing the column key of the dictionary from parameter to check dictionary for value
                string aValue = row[column];

                if (aValue.Contains(value))
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded)
            {
                return;
            }

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();

        }

        /*
        * needs to take a parameter that is a string value, the user's search term
        */
        public static List<Dictionary<string, string>> FindByValue(string value)
        {
            // load data, if not already loaded -- how I get the AllJobs to have values from job_data.cvs
            LoadData();

            // making a new dictionary to hold jobs with search term
            List<Dictionary<string, string>> searchJobs = new List<Dictionary<string, string>>();

            // outer loop to get row -- which I will use in nested loop to search through all columns.
            foreach (Dictionary<string, string> row in AllJobs)
            {
                // nested loop to iterate through all valid columns; assignment requirement in case we add columns 
                foreach (KeyValuePair<string, string> kvp in row)
                {
                    // checking if by column to see if anything matched the search term given by the user "value"
                    // used ToUpper() to make search case insensitive
                    if (row[kvp.Key].ToUpper().Contains(value.ToUpper()))
                    {
                        // adding job that matches search term to new dictionary
                        searchJobs.Add(row);
                        //breaking the loop so it won't return doubles
                        break;
                    }
                }
             }

            if (searchJobs.Count == 0)
            {
                string noResults = "That search does not return any results. Please try again!";
                Console.WriteLine(noResults);
            }

            return searchJobs;
        }   
    }

}
