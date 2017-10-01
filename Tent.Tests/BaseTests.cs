using Microsoft.Extensions.Configuration;
using System.IO;
using Tent.Data;

namespace Tent.Tests
{
    public class BaseTests
    {
        public BaseTests() {
            this.configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"c:\code\secrets\Tent\settings.json")
                .Build();
            string connectionString = configuration["connectionString"];
            this.db = new Database(connectionString);
        }

        protected IConfigurationRoot configuration;
        protected IDatabase db;
    }
}
