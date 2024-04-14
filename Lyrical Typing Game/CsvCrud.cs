using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <param name="config">Write config</param>
        /// <param name="rows">Rows written to csv file</param>
        /// <exception cref="Exception">Thrown if file does not have the csv extension</exception>
        public static void Append(string path, CsvConfiguration config, List<T> rows)
        {

            if (!path.EndsWith(".csv"))
            {
                throw new Exception("Incorrect file type; must be .csv");
            }
            
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
        /// <param name="config">Read/Write config</param>
        /// <param name="updatedRows">Match of rows being updated</param>
        /// <param name="newRow">Value to replace updatedElements</param>
        /// <exception cref="Exception">Thrown if file does not have the csv extension</exception>
        public static void Update(string path, CsvConfiguration config, Predicate<T> updatedRows, T newRow)
        {

            if (!path.EndsWith(".csv"))
            {
                throw new Exception("Incorrect file type; must be .csv");
            }

            var records = Read(path, config);

            if (records.Any())
            {
                var listOfUpdated = records.FindAll(updatedRows);
                var indexList = new List<int>();
                if (listOfUpdated.Any())
                {
                    listOfUpdated.ForEach(x => indexList.Add(records.FindIndex(y => y.Equals(x))));
                    records.RemoveAll(updatedRows);

                    for (int i = 0; i < listOfUpdated.Count; i++)
                    {
                        records.Insert(indexList[i], newRow);
                    }
                } else
                {
                    records = records.Append(newRow).ToList();
                }

                Delete(path);
            } else
            {
                records = records.Append(newRow).ToList();
            }


            Append(path, config, records);
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
        /// <param name="config">Reader config</param>
        /// <returns>Rows of type T</returns>
        /// <exception cref="Exception">Thrown if file does not have the csv extension</exception>
        public static List<T> Read(string path, CsvConfiguration config)
        {

            if (!path.EndsWith(".csv"))
            {
                throw new Exception("Incorrect file type; must be .csv");
            }
            
            if (!File.Exists(path))
            {
                return new List<T>();
            }

            using(var reader = new StreamReader(path))
            using(var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<T>().ToList();
            }
        }
    }
}
