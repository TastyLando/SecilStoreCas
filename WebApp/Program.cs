using DataAccess.Interfaces;
using ConfigLib.Interfaces;
using ConfigLib.Services;
using DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

builder.Services.AddSingleton<IConfigurationService>(provider =>
{
	var configuration = provider.GetRequiredService<IConfiguration>();

	var applicationName = configuration["AppSettings:ApplicationName"];
	var connectionString = configuration["AppSettings:ConnectionString"];
	var refreshInterval = int.Parse(configuration["AppSettings:RefreshInterval"]);

	var reader = new ConfigurationReader(applicationName, connectionString, refreshInterval);

	return reader;
});

builder.Services.AddScoped<IConfigurationRepository>(provider =>
{
	var configuration = provider.GetRequiredService<IConfiguration>();

	var databaseName = configuration["AppSettings:DatabaseName"];
	var connectionString = configuration["AppSettings:ConnectionString"];
	var collectionName = configuration["AppSettings:CollectionName"];

	var mongoConfig = new MongoConfigurationRepository(connectionString, databaseName, collectionName);

	return mongoConfig;
});

builder.Services.AddScoped<IConfigurationService>(provider =>
{
	var configuration = provider.GetRequiredService<IConfiguration>();
	
	var applicationName = configuration["AppSettings:ApplicationName"];
	var connectionString = configuration["AppSettings:ConnectionString"];
	var refreshInterval = int.Parse(configuration["AppSettings:RefreshInterval"]);
	var databaseName = configuration["AppSettings:DatabaseName"];
	var collectionName = configuration["AppSettings:CollectionName"];

	var mongoConfig = new MongoConfigurationRepository(connectionString, databaseName, collectionName);
	var configurationService = new ConfigurationService(mongoConfig, applicationName, refreshInterval);

	return configurationService;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Configuration}/{action=Index}/{id?}");

app.Run();
