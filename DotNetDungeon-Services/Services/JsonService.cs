namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Services.Interfaces;
using System.Text.Json;

public class JsonService : IJsonService
{
	public T ConvertJsonPathToModelObject<T>(string jsonPath, T modelObjectEmpty)
	{
		string basePath = AppDomain.CurrentDomain.BaseDirectory;
		string fullPath = Path.Combine(basePath, jsonPath);
		
		if (!File.Exists(fullPath))
			throw new Exception();

		string jsonAsString = File.ReadAllText(fullPath);
		var modelObjectFull = JsonSerializer.Deserialize<T>(jsonAsString);

		return modelObjectFull ?? modelObjectEmpty;
	}
}