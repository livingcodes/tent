using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Basketcase;
namespace Tent.Tests;
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

      this.db = new Db(
         new Tent.Data.SqlConnectionFactory(), 
         new Reader(), 
         new SerializedCached(
            distributedCache
         ),
         new TableName_ClassName()
      );
   }

   protected IConfigurationRoot configuration;
   protected IDb db;

   protected void assert(bool condition) {
      Assert.IsTrue(condition);
   }
}