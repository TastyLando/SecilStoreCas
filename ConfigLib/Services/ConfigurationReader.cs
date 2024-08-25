using ConfigLib.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;

namespace ConfigLib.Services
{
	public class ConfigurationReader : IConfigurationService
	{
		private readonly IConfigurationService _configurationService;

		public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
		{
			var repository = new MongoConfigurationRepository(connectionString, "ConfigDB", "ConfigurationRecords");
			_configurationService = new ConfigurationService(repository, applicationName, refreshTimerIntervalInMs);
		}
		public async Task<T> GetValueAsync<T>(string key)
		{
			return await _configurationService.GetValueAsync<T>(key);
		}
		public Task<List<ConfigurationRecord>> Init()
		{
			throw new NotImplementedException();
		}
	}
}
