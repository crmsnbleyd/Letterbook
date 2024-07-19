using System.Reflection;
using Letterbook.Adapter.ActivityPub;
using Letterbook.Adapter.Db;
using Letterbook.Adapter.TimescaleFeeds;
using Letterbook.Api;
using Letterbook.Api.Authentication.HttpSignature.DependencyInjection;
using Letterbook.Api.Swagger;
using Letterbook.AspNet;
using Letterbook.Core.Extensions;
using Letterbook.Core.Models;
using Letterbook.Web;
using Letterbook.Workers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
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
			.ReadFrom.Services(services),
			true
		);

		builder.Services.AddOpenTelemetry()
			.AddAspnetTelemetry()
			.AddWorkerTelemetry()
			.AddDbTelemetry()
			.AddClientTelemetry()
			.AddTelemetryExporters();
		builder.Services.AddHealthChecks();
		builder.Services.AddLetterbookCore(builder.Configuration)
			.AddActivityPubClient(builder.Configuration)
			.AddApiProperties(builder.Configuration)
			.AddPublishers()
			.AddDbAdapter(builder.Configuration)
			.AddFeedsAdapter(builder.Configuration)
			.AddWebCookies();
		builder.Services.AddIdentity<Account, IdentityRole<Guid>>(identity => identity.ConfigureIdentity())
			.AddEntityFrameworkStores<RelationalContext>()
			.AddDefaultTokenProviders()
			.AddDefaultUI();
		builder.Services.AddRazorPages()
			.AddApplicationPart(Assembly.GetAssembly(typeof(Web.Program))!);
		builder.Services.AddAuthorization(options =>
		{
			options.AddWebAuthzPolicy();
			options.AddpiAuthzPolicy();
		});
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
		app.UseStaticFiles(new StaticFileOptions
		{
			FileProvider = new ManifestEmbeddedFileProvider(Assembly.GetAssembly(typeof(Web.Program))!, "wwwroot"),

		});

		app.UseHealthChecks("/healthz");
		app.MapPrometheusScrapingEndpoint();
		app.UseWebFingerScoped();

		app.UseHttpSignatureVerification();
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseWhen(ProfileIdentityPaths, applicationBuilder =>
		{
			applicationBuilder.UseMiddleware<ProfileIdentityMiddleware>();
		});

		app.UseSerilogRequestLogging();

		app.MapRazorPages();
		app.UsePathBase(new PathString("/api/v1"));
		app.MapControllers();

		app.Run("http://localhost:5127");

		return;

		static bool ProfileIdentityPaths(HttpContext context)
		{
			return !context.Request.Path.StartsWithSegments("/Identity")
			       // TODO: prefix with /ap/v1
			       // Need to use url generators integrated with routing, instead of just a bunch of magic strings
			       && !context.Request.Path.StartsWithSegments("/actor")
			       && !context.Request.Path.StartsWithSegments("/object");
		}
	}
}