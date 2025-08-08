namespace DotNetDungeon_Services.Interfaces;

public interface IGameService
{
	/// <summary>
	/// Procedurally generates a dungeon level with rooms, corridors, and walls
	/// </summary>
	/// <param name="height">Height of the dungeon level (world)</param>
	/// <param name="width">Width of the dungeon level (world)</param>
	/// <param name="roomNumberForLevelMin">Minimum number of rooms for the level</param>
	/// <param name="roomNumberForLevelMax">Maximum number of rooms for the level</param>
	/// <param name="roomHeightMin">Minimum height of rooms</param>
	/// <param name="roomHeightMax">Maximum height of rooms</param>
	/// <param name="roomWidthMin">Minimum width of rooms</param>
	/// <param name="roomWidthMax">Maximum width of rooms</param>
	/// <param name="nothingChar">Character representing empty space</param>
	/// <param name="wallChar">Character representing walls</param>
	/// <param name="floorChar">Character representing floors</param>
	/// <returns>A 2D char matrix representing the generated dungeon</returns>
	char[,] GenerateDungeonLevel(
		int height,
		int width,
		int roomNumberForLevelMin,
		int roomNumberForLevelMax,
		int roomHeightMin,
		int roomHeightMax,
		int roomWidthMin,
		int roomWidthMax,
		char nothingChar,
		char wallChar,
		char floorChar);
}