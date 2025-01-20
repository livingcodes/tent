namespace Tent.Tests;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Basketcase;
public class BaseTests
{
  public BaseTests() {
    //this.cfg = new ConfigurationBuilder()
    //  .AddJsonFile(@"c:\code\secrets\Tent\settings.json")
    //  .Build();
    //str conStr = cfg["conStr"];

    // i wasn't able to figure out how to construct distributed memory cache
    // the IOptions in particular
    // so i used the service provider to build it
    IServiceCollection svc = new ServiceCollection();
    svc.AddDistributedMemoryCache();
    var svcPrvdr = svc.BuildServiceProvider();
    var distributedCache = svcPrvdr.GetService<IDistributedCache>();
    
    this.db = new Db(
      new Tent.Data.SqlConFct(), 
      new Reader(), 
      new SerializedCached(
        distributedCache
      ),
      new TblNm_ClsNm()
    );
  }

  protected IConfigurationRoot cfg;
  protected IDb db;

  protected void assert(bln condition) {
    Assert.IsTrue(condition);
  }
}