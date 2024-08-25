using DataAccess.Interfaces;
using System.Collections.Concurrent;
using ConfigLib.Interfaces;
using DataAccess.Models;

namespace ConfigLib.Services
{
	public class ConfigurationService : IConfigurationService
	{
		private readonly IConfigurationRepository _configurationRepository;
		private readonly string _applicationName;
		private readonly int _refreshTimerIntervalInMs;
		private readonly ConcurrentDictionary<string, ConfigurationRecord> _configurations;
		private Timer _refreshTimer;

		public ConfigurationService(IConfigurationRepository configurationRepository, string applicationName, int refreshTimerIntervalInMs)
		{
			_configurationRepository = configurationRepository;
			_applicationName = applicationName;
			_refreshTimerIntervalInMs = refreshTimerIntervalInMs;
			_configurations = new ConcurrentDictionary<string, ConfigurationRecord>();

			LoadConfigurations().Wait();
			StartRefreshTimer();
		}

		private void StartRefreshTimer()
		{
			_refreshTimer = new Timer(async _ => await LoadConfigurations(), null, 0, _refreshTimerIntervalInMs);
		}

		private async Task<List<ConfigurationRecord>> LoadConfigurations()
		{
			List<ConfigurationRecord> configurationRecords = new List<ConfigurationRecord>();
			try
			{
				var configurations = await _configurationRepository.GetActiveConfigurationsAsync(_applicationName);

				foreach (var config in configurations)
				{
					if (!_configurations.ContainsKey(config.Name))
					{
						configurationRecords.Add(config);
					}

					_configurations[config.Name] = config;
				}

				return configurationRecords;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Konfigürasyonları yükleme işlemi başarısız.", ex);
			}
		}

		public async Task<T> GetValueAsync<T>(string name)
		{
			if (_configurations.ContainsKey(name))
			{
				var value = _configurations[name];

				return (T)Convert.ChangeType(value, typeof(T));
			}

			throw new KeyNotFoundException($"'{name}' bulunamadı.");
		}
	}
}
