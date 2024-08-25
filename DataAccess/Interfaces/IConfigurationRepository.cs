using DataAccess.Models;
using MongoDB.Bson;

namespace DataAccess.Interfaces
{
	public interface IConfigurationRepository
	{
		Task<IEnumerable<ConfigurationRecord>> GetActiveConfigurationsAsync(string applicationName);
		Task<ConfigurationRecord> GetConfigurationAsync(ObjectId id);
		Task AddOrUpdateConfigurationAsync(ConfigurationRecord record);
		Task DeleteConfigurationAsync(ObjectId id);
	}
}
