namespace DotNetDungeon_Services.Interfaces;

public interface IJsonService
{
	/// <summary>
	/// Converts a JSON file at the specified path to a model object of type T
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="jsonPath"></param>
	/// <param name="modelObjectEmpty"></param>
	/// <returns>A model object of type T</returns>
	T ConvertJsonPathToModelObject<T>(string jsonPath, T modelObjectEmpty);
}