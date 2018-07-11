using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SillyWonko.Data;
using SillyWonko.Models;
using SillyWonko.Models.Interfaces;

namespace SillyWonko
{
	// First commit. Second commit.
    public class Startup
    {
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			var builder = new ConfigurationBuilder().AddEnvironmentVariables();
			builder.AddUserSecrets<Startup>();
			// For production.
			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IWarehouse, DevWarehouse>();
			services.AddDbContext<WonkoDbContext>(options => 
                     options.UseSqlServer(Configuration.GetConnectionString("ProductionConnection")));

            //Sets up identity services
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddDbContext<ApplicationDbContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("IdentityLocal")));
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
            });
            //services.AddTransient<IEmailSender, EmailSender>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
            app.UseAuthentication();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Something went wrong.");
            });
        }
    }
}
