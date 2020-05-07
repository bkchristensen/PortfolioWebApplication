using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortfolioWebApplication.Models;

namespace PortfolioWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PurchaseContext>(opt =>
                opt.UseInMemoryDatabase("Purchases"));
            services.AddDbContext<ScriptContext>(opt =>
                //opt.UseInMemoryDatabase("Scripts"));
                //opt.UseSqlServer(@"Server=BRIANCLAPTOP;Database=ArtificialIntelligence;Trusted_Connection=True;"));
                opt.UseSqlServer(@"Server=BRIANCLAPTOP;Database=ArtificialIntelligence;User ID=sa;Password=lakeview828;"));
            services.AddControllersWithViews();
            //services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //    name: "purchases",
                //    pattern: "{controller=Purchases}/{action=Index}/{id?}");
            });

        }
    }
}
