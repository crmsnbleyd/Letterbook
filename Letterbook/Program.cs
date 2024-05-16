using System.Reflection;
using Letterbook.Adapter.ActivityPub;
using Letterbook.Adapter.Db;
using Letterbook.Adapter.RxMessageBus;
using Letterbook.Api;
using Letterbook.Api.Swagger;
using Letterbook.Core.Models;
using Letterbook.Workers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;

namespace Letterbook;

public class Program
{
	public static void Main(string[] args)
	{
		// Pre initialize Serilog
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
			.Enrich.FromLogContext()
			.Enrich.WithSpan()
			.WriteTo.Console()
			.CreateBootstrapLogger();

		var builder = WebApplication.CreateBuilder(args);

		if (!builder.Environment.IsProduction())
			builder.Configuration.AddUserSecrets<Api.Program>();
		// Register Serilog - Serialized Logging (configured in appsettings.json)
		builder.Host.UseSerilog((context, services, configuration) => configuration
			.Enrich.FromLogContext()
			.Enrich.WithSpan()
			.ReadFrom.Configuration(context.Configuration)
			.ReadFrom.Services(services)
		);

		builder.Services.AddApiProperties(builder.Configuration);
		builder.Services.AddTelemetry();
		builder.Services.AddHealthChecks();
		builder.Services.AddActivityPubClient(builder.Configuration);
		builder.Services.AddServices(builder.Configuration);
		builder.Services.AddPublishers();
		builder.Services.AddRxMessageBus();
		builder.Services.AddIdentity<Account, IdentityRole<Guid>>()
			.AddEntityFrameworkStores<RelationalContext>()
			.AddDefaultTokenProviders()
			.AddDefaultUI();
		builder.Services.AddRazorPages()
			.AddApplicationPart(Assembly.GetAssembly(typeof(Web.Program))!);
		builder.Services.AddMassTransit(bus => bus.AddWorkerBus(builder.Configuration)
			.AddWorkers(builder.Configuration));

		// builder.WebHost.UseUrls(coreOptions.BaseUri().ToString());

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			// Not sure if this works, with mixed Razor/WebApi
			app.UseExceptionHandler("/Error");
		}

		if (!app.Environment.IsProduction())
		{
			app.Use((context, next) =>
			{
				context.Request.EnableBuffering();
				return next();
			});
			app.UseSwaggerConfig();
		}

		// app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseHealthChecks("/healthz");
		app.MapPrometheusScrapingEndpoint();
		app.UseWebFingerScoped();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseSerilogRequestLogging();

		app.MapRazorPages();
		app.UsePathBase(new PathString("/api/v1"));
		app.MapControllers();

		app.Run("http://localhost:5127");
	}
}