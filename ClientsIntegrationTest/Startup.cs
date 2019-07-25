﻿using Microsoft.Extensions.DependencyInjection;
using ClientsTests.Repositories;
using Microsoft.Extensions.Configuration;
using ClientsTests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ClientsTests
{
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services) => services
			.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Connection")))
			.AddTransient<IClientRepository, ClientRepository>()
			.AddMvc()
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app
				.UseHttpsRedirection()
				.UseMvcWithDefaultRoute();
		}
	}
}