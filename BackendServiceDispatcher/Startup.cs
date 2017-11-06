using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackendServiceDispatcher.Data;
using Microsoft.EntityFrameworkCore;
using BackendServiceDispatcher.Models.AccountEntities;
using Microsoft.AspNetCore.Identity;

namespace BackendServiceDispatcher
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                services
                    .AddDbContext<ApplicationDbContext>(
                    options =>
                    options.UseSqlite(Configuration.GetConnectionString("DevelopmentSqliteConnection")));
            }
            else if (HostingEnvironment.IsProduction())
            {
                services
                    .AddDbContext<ApplicationDbContext>(
                    options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ProductionSqlServerConnection")));
            }
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            services.AddSingleton(Configuration);
            services.AddTransient<IServiceProvider>(instance => services.BuildServiceProvider());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();

        }
    }
}
