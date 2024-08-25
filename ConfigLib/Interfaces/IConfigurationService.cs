namespace ConfigLib.Interfaces
{
	public interface IConfigurationService
	{
		Task<T> GetValueAsync<T>(string key);
	}
}
