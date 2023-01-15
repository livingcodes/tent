using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Tent;
public class Startup {
    public void ConfigureServices(IServiceCollection services) {
        services.AddScoped<Logic.ICrypto, Logic.Crypto>();
        services.AddRazorPages();
    }

    // configures the http request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseExceptionHandler("/Error");

        app .UseStaticFiles()
            .UseRouting()
            .UseEndpoints(x => x.MapRazorPages());
    }
}