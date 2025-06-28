namespace DotNetDungeon_Console.Utils;

using System.Text.Json;

public static class JsonUtil
{
	public static T ConvertJsonPathToModelObject<T>(string jsonPath, T modelObjectEmpty)
	{
		if (!File.Exists(jsonPath))
			throw new Exception();

		string jsonAsString = File.ReadAllText(jsonPath);
		var modelObjectFull = JsonSerializer.Deserialize<T>(jsonAsString);

		return modelObjectFull ?? modelObjectEmpty;
	}
}