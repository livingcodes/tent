using Tent.Logic;

var builder = WebApplication.CreateBuilder(args);
var svc = builder.Services;
svc.AddScoped<ICrypto, Crypto>()
   .AddRazorPages();

var app = builder.Build();
if (app.Environment.IsDevelopment())
   app.UseDeveloperExceptionPage();
app.UseHttpsRedirection()
   .UseStaticFiles()
   .UseRouting()
   .UseAuthorization();
app.MapRazorPages();
app.Run();