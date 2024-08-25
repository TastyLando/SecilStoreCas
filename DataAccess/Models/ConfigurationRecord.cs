using MongoDB.Bson;

namespace DataAccess.Models
{
	public class ConfigurationRecord
	{
		public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
		public string Name { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
		public bool IsActive { get; set; }
		public string ApplicationName { get; set; }
	}
}
