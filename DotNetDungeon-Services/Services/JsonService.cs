namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Services.Interfaces;
using System.Text.Json;

public class JsonService : IJsonService
{
	/// <summary>
	/// Converts a JSON file at the specified path to a model object of type T
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="jsonPath"></param>
	/// <param name="modelObjectEmpty"></param>
	/// <returns>A model object of type T</returns>
	public T ConvertJsonPathToModelObject<T>(string jsonPath, T modelObjectEmpty)
	{
		// Get the full path to the JSON file
		string basePath = AppDomain.CurrentDomain.BaseDirectory;
		string fullPath = Path.Combine(basePath, jsonPath);

		// Check if the JSON file exists
		if (!File.Exists(fullPath))
			throw new Exception();

		// Read the JSON file contents
		string jsonAsString = File.ReadAllText(fullPath);

		// Deserialize the JSON string into the model object
		var modelObjectFull = JsonSerializer.Deserialize<T>(jsonAsString);

		// Return the fully populated model object or the empty instance if deserialization failed
		return modelObjectFull ?? modelObjectEmpty;
	}
}