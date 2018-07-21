using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
using SillyWonko.Models.Handlers;
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
			services.AddScoped<ICartService, CartService>();
			services.AddScoped<IOrderService, OrderService>();

			services.AddDbContext<WonkoDbContext>(options => 
                     options.UseSqlServer(Configuration.GetConnectionString("ProductionConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("IdentityProd")));

            // Sets up identity services.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = Configuration["OAUTH:Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["OAUTH:Authentication:Google:ClientSecret"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(ApplicationRoles.Administrator));
                options.AddPolicy("Employee", policy => policy.Requirements.Add(new EmployeeEmailRequirement("wonko.com")));

                options.AddPolicy("Golden", policy => policy.Requirements.Add(new CricketRequirement("Golden Cricket Member")));
                options.AddPolicy("Silver", policy => policy.Requirements.Add(new CricketRequirement("Silver Cricket Member")));
                options.AddPolicy("Bronze", policy => policy.Requirements.Add(new CricketRequirement("Bronze Cricket Member")));
            });

            // services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IAuthorizationHandler, EmployeeEmailHandler>();
            services.AddSingleton<IAuthorizationHandler, CricketHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Something went wrong.");
            });
        }
    }
}
