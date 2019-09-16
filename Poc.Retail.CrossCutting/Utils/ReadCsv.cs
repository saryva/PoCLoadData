using CsvHelper;
using CsvHelper.Configuration;
using Poc.Retail.Domain.Entities;
using System.Collections.Generic;
using System.IO;

namespace Poc.Retail.CrossCutting.Utils
{
    public static class ReadCsv
    {
        /// <summary>
        /// Read file in csv format
        /// </summary>
        /// <returns>IEnumerable with data</returns>
        public static IEnumerable<StockDetail> GetDataToFile(string url)
        {            
            TextReader reader = new StreamReader(url);
            var csvReader = new CsvReader(reader);            
            csvReader.Configuration.RegisterClassMap<StockDetailMap>();            

            var records = csvReader.GetRecords<StockDetail>();
            return records;          
        }

        /// <summary>
        /// Configure map
        /// </summary>
        public sealed class StockDetailMap : ClassMap<StockDetail>
        {            
            public StockDetailMap()
            {
                AutoMap();
                Map(m => m.IsImport).Ignore();
                Map(m => m.Id).Ignore();
            }
        }
    }
}