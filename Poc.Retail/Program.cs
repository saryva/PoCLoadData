using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Poc.Retail.CrossCutting.Utils;
using Poc.Retail.Domain.Entities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Poc.Retail
{
    class Program
    {
        static void Main(string[] args)
        {            
            try
            {
                MainAsync().Wait();               
            }            
            catch(Exception ex)
            {             
                Console.WriteLine("\r\nError in process.");
                Console.WriteLine($"\r\nError: {ex}");                
            }
            
            Console.WriteLine("\r\nPress any key to continue ...");
            Console.Read();
        }

        private static async Task MainAsync()
        {            
            /// Configuration 
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();            

            await AzureConnect.DownloadBlobAnonymously(configuration["FileUrl"], configuration["TempFileUrl"]);
            var data = ReadCsv.GetDataToFile(configuration["TempFileUrl"]);
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<RetailDbContext>();
            optionsBuilder.UseSqlServer(connectionString, opts=>opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

            using (var context = new RetailDbContext(optionsBuilder.Options))
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var haveImport = context.StockDetail.FirstOrDefault(s => s.IsImport == true);

                        if (haveImport != null)
                        {                            
                            const string query = "DELETE FROM [dbo].[StockDetail] WHERE [IsImport]=1";
                            context.Database.ExecuteSqlCommand(query);
                            context.SaveChanges();                          
                        }  
                        
                        var dataToSave = data.Select(x => { x.IsImport = true; return x; });
                        await context.BulkInsertAsync(dataToSave.ToList(), new BulkConfig { BatchSize = 100000 });

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();                        
                        throw ex;
                    }
                }
            }
        }
    }
}
