namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Services.Interfaces;
using System.Text.Json;

public class JsonService : IJsonService
{
	public T ConvertJsonPathToModelObject<T>(string jsonPath, T modelObjectEmpty)
	{
		if (!File.Exists(jsonPath))
			throw new Exception();

		string jsonAsString = File.ReadAllText(jsonPath);
		var modelObjectFull = JsonSerializer.Deserialize<T>(jsonAsString);

		return modelObjectFull ?? modelObjectEmpty;
	}
}