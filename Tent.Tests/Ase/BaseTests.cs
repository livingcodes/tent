using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Ase.Tests
{
    [TestClass] public class BaseTests
    {
        protected static IDb db;
        
        protected static void initialize() {
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

            var connectionString = "server=(LocalDb)\\MSSQLLocalDB; database=Tent; trusted_connection=true;";

            db = new Db(
                new ConnectionFactory(connectionString),
                new Reader(),
                new SerializedCache(distributedCache),
                new TableName_ClassName()
            );
        }

        protected void assert(bool condition) {
            Assert.IsTrue(condition);
        }
    }
}