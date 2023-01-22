namespace Tent;
using Tent.Logic;
public class Program 
{
   public static void Main(string[] args) {
      var builder = WebApplication.CreateBuilder(args);
      var svc = builder.Services;
      svc.AddScoped<ICrypto, Crypto>()
      .AddRazorPages();
      
      var app = builder.Build();
      ServiceProvider = app.Services;
      if (app.Environment.IsDevelopment())
         app.UseDeveloperExceptionPage();
      app.UseHttpsRedirection()
         .UseStaticFiles()
         .UseRouting()
         .UseAuthorization();
      app.MapRazorPages();
      app.Run();
   }
   public static IServiceProvider ServiceProvider;
}