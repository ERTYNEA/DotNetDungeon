namespace DotNetDungeon_Services.Interfaces;

public interface IGameService
{
	char[,] GenerateDungeonLevel(
		int height,
		int width,
		int minProbability,
		int maxProbability,
		int roomHeightMin,
		int roomHeightMax,
		int roomWidthMin,
		int roomWidthMax,
		char nothingChar,
		char wallChar,
		char floorChar);
}