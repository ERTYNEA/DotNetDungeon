namespace DotNetDungeon_Services.Interfaces;

public interface IJsonService
{
	T ConvertJsonPathToModelObject<T>(string jsonPath, T modelObjectEmpty);
}