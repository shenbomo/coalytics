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
using BackendServiceDispatcher.Services;
using BackendServiceDispatcher.Extensions;
using FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

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
            string connectionString;
            if (HostingEnvironment.IsDevelopment())
            {
                connectionString = "DevelopmentSqliteConnection";
            }
            else
            {
                connectionString = "ProductionSqlServerConnection";
            }
            services
                .AddDbContext<ApplicationDbContext>(
                    options =>
                        options.UseSqlite(Configuration.GetConnectionString(connectionString)));
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();
            services
                .AddMvc()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddSingleton(Configuration);
            services.AddTransient<IServiceProvider>(instance => services.BuildServiceProvider());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", 
                    new Info
                    {
                        Title = "Coalytics API",
                        Version = "v1",
                        Description = "APIs for Coalytics",
                        TermsOfService = "None",
                        Contact = new Contact { Name = "Bomo Shen", Email = "sxxxxxxx@gmail.com", Url = "https://a.com/"},
                        License = new License { Name = "None", Url = "https://license.com/"}
                    });
                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "BackendServiceDispatcher.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Order matters when registering middlewares
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();            
            app.UseSwagger(c => 
            {
                c.RouteTemplate = "api/docs/{documentName}/apis.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/docs/v1/apis.json", "Coalytics API");
                c.RoutePrefix = "api/docs";
            });
            app.UseStaticFiles();
            app.UseAntiforgeryForAngularJs();            
            app.UseMvc();            
        }
    }
}
