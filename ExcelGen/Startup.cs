using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ExcelGen.Domain.Interfaces;
using ExcelGen.Domain.Managers;
using ExcelGen.Repository;
using ExcelGen.Repository.Interfaces;
using ExcelGen.Repository.Repositories;
using ExcelGen.Repository.AuthorizationData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ExcelGen
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

            services.AddControllersWithViews();

            #region DependencyInjection

            services.AddTransient<ICategoryManager, CategoryManager>();
            services.AddTransient<IIncomeManager, IncomeManager>();
            services.AddTransient<IPurchaseManager, PurchaseManager>();
            services.AddTransient<IReportingManager, ReportingManager>();
            services.AddTransient<IPurchaseRepository, PurchasesRepository>();
            services.AddTransient<IIncomeRepository, IncomeRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IAccountManager, AccountManager>();
            services.AddTransient<IAccessRepository, AccessRepository>();
            services.AddTransient<IAccessManager, AccessManager>();

            #endregion
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });



            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            // Adding Authentication
            services.AddAuthentication()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options => {
                        options.Cookie.HttpOnly = false;
                        Configuration.Bind("CookieSettings", options);
                    });
            services.AddAuthorization();

            services.AddDbContext<DatabaseContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ExcelGenContext")));
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
