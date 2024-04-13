using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Lyrical_Typing_Game
{
    public static class CsvCrud<T> where T : class
    {

        /// <summary>
        /// Appends to a csv file, or creates one and writes to it if one does not exist
        /// </summary>
        /// <param name="path">Path to csv file</param>
        /// <param name="rows">Rows written to csv file</param>
        /// <exception cref="Exception">Thrown if file does not have the csv extension</exception>
        public static void Append(string path, List<T> rows)
        {

            if (!path.EndsWith(".csv"))
            {
                throw new Exception("Incorrect file type; must be .csv");
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(rows);
            }
        }

        /// <summary>
        /// Replaces all matched rows in a csv file with a specified row
        /// </summary>
        /// <param name="path">Path to csv file</param>
        /// <param name="updatedRows">Match of rows being updated</param>
        /// <param name="newRow">Value to replace updatedElements</param>
        /// <exception cref="Exception">Thrown if file does not have the csv extension</exception>
        public static void Update(string path, Predicate<T> updatedRows, T newRow)
        {

            if (!path.EndsWith(".csv"))
            {
                throw new Exception("Incorrect file type; must be .csv");
            }

            var records = Read(path);

            List<T> recordsList = records.ToList();
            var listOfUpdated = recordsList.FindAll(updatedRows);
            var indexList = new List<int>();
            listOfUpdated.ForEach(x => indexList.Add(recordsList.FindIndex(y => y.Equals(x))));

            recordsList.RemoveAll(updatedRows);

            for (int i = 0; i < listOfUpdated.Count; i++)
            {
                recordsList.Insert(indexList[i], newRow);
            }

            Delete(path);

            Append(path, recordsList);
        }

        /// <summary>
        /// Deletes a csv file
        /// </summary>
        /// <param name="path">Path to csv file</param>
        /// <exception cref="Exception">Thrown if file does not have the csv extension</exception>
        public static void Delete(string path) 
        {

            if (!path.EndsWith(".csv"))
            {
                throw new Exception("Incorrect file type; must be .csv");
            }

            File.Delete(path);
        }

        /// <summary>
        /// Reads a specified csv file
        /// </summary>
        /// <param name="path">Path to csv file</param>
        /// <returns>Rows of type T</returns>
        /// <exception cref="Exception">Thrown if file does not have the csv extension</exception>
        public static IEnumerable<T> Read(string path)
        {

            if (!path.EndsWith(".csv"))
            {
                throw new Exception("Incorrect file type; must be .csv");
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            using(var reader = new StreamReader(path))
            using(var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<T>();
            }
        }
    }
}
