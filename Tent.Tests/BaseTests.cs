using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tent.Data;

namespace Tent.Tests
{
    public class BaseTests
    {
        public BaseTests() {
            //this.configuration = new ConfigurationBuilder()
            //    .AddJsonFile(@"c:\code\secrets\Tent\settings.json")
            //    .Build();
            //string connectionString = configuration["connectionString"];

            // i wasn't able to figure out how to construct distributed memory cache
            // the IOptions in particular
            // so i used the service provider to build it
            IServiceCollection services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var distributedCache = serviceProvider.GetService<IDistributedCache>();

            this.db = new Database(
                new SqlConnectionFactory(), 
                new Reader(), 
                new SerializedCached(
                    distributedCache
                )
            );
        }

        protected IConfigurationRoot configuration;
        protected IDatabase db;
    }
}