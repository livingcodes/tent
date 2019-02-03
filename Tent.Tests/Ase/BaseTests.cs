using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using static Basketcase.Table;

namespace Ase.Tests
{
    [TestClass] public class BaseTests
    {
        protected static Basketcase.IDb db;
        protected static Basketcase.ICache cache;
        
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
            cache = new SerializedCache(distributedCache);

            var connectionString = "server=(LocalDb)\\MSSQLLocalDB; database=Tent; trusted_connection=true;";

            db = new Basketcase.Db(
                new ConnectionFactory(connectionString),
                new Basketcase.Reader(),
                cache,
                new Basketcase.TableName_ClassName()
            );
        }
        
        protected static void createPostTable() {
            var sql = new Table("Post")
                .AddColumn("Id", SqlType.Int, Syntax.Identity(1, 1))
                .AddColumn("Html", SqlType.VarChar(200))
                .End()
                .Sql;
            db.Execute(sql);
        }

        protected void assert(bool condition) {
            Assert.IsTrue(condition);
        }

        public class Post {
            public int Id { get; set; }
            public string Html { get; set; }
        }
    }
}