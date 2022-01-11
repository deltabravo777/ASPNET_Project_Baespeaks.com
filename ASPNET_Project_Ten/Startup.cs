using System.Diagnostics;
using ASPNET_Project_Eleven.Models;
using ASPNET_Project_Eleven.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_Project_Eleven
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
            var connection = Configuration["Production:SqliteConnectionStringSQLite"];

            string secondConnectionString = System.Environment.CurrentDirectory.ToString();
            secondConnectionString = Path.Combine(secondConnectionString, "MyDatabase.db;Foreign Keys=False");
            secondConnectionString = "Data Source=file:///" + secondConnectionString;
            services.AddDbContextPool<AppDBContext>(options => options.UseSqlite(secondConnectionString));
            
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
                options.SuppressAsyncSuffixInActionNames = false;
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).AddXmlSerializerFormatters();
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied"); ;
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();
            services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(1));
            
            services.AddAuthorization(options => {
                options.AddPolicy("ManagerLevelPolicy",
                    policy => policy.AddRequirements(new ManagerClaimsRequirement()
                ));
                options.AddPolicy("AdminLevelPolicy",
                    policy => policy.AddRequirements(new AdminClaimsRequirement()
                ));
                options.AddPolicy("ExecutiveLevelPolicy",
                    policy => policy.AddRequirements(new ExecutiveClaimsRequirement()
                ));
                options.InvokeHandlersAfterFailure = true;
            });
            services.AddSingleton<IAuthorizationHandler, ManagerClaimHandler>();
            services.AddSingleton<IAuthorizationHandler, AdminClaimHandler>();
            services.AddSingleton<IAuthorizationHandler, ExecutiveClaimHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>();

            services.AddScoped<IDatabaseRepository, SQLDatabaseRepository>();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}

