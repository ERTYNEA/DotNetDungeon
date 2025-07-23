namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Objets;
using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Utils;
using System.Collections.Generic;

public class GameService : IGameService
{
	public char[,] GenerateDungeonLevel(
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
		char floorChar)
	{
		char[,] dungeonLevelMatrix = new char[height, width];

		for (int y = 0; y < height; y++)
			for (int x = 0; x < width; x++)
				dungeonLevelMatrix[y, x] = nothingChar;

		List<RoomObject> rooms = new List<RoomObject>();

		int roomsToGenerate = MathUtil.GenerateRandomInteger(roomNumberForLevelMin, roomNumberForLevelMax);

		int attemptsCount = 0;
		int maxAttempts = roomsToGenerate * 10;

		while (rooms.Count < roomsToGenerate && attemptsCount < maxAttempts)
		{
			attemptsCount++;

			int roomHeight = MathUtil.GenerateRandomInteger(roomHeightMin, roomHeightMax);
			int roomWidth = MathUtil.GenerateRandomInteger(roomWidthMin, roomWidthMax);

			int roomY = MathUtil.GenerateRandomInteger(0, height - roomHeight);
			int roomX = MathUtil.GenerateRandomInteger(0, width - roomWidth);

			RoomObject newRoom = new RoomObject(roomY, roomX, roomHeight, roomWidth);

			bool[,] occupiedArea = new bool[height, width];

			foreach (RoomObject existingRoom in rooms)
			{
				for (int y = existingRoom.Y; y < existingRoom.Y + existingRoom.Height; y++)
					for (int x = existingRoom.X; x < existingRoom.X + existingRoom.Width; x++)
						occupiedArea[y, x] = true;
			}

			bool addsNewArea = false;

			for (int y = newRoom.Y; y < newRoom.Y + newRoom.Height; y++)
			{
				for (int x = newRoom.X; x < newRoom.X + newRoom.Width; x++)
				{
					if (!occupiedArea[y, x])
					{
						addsNewArea = true;
						break;
					}
				}

				if (addsNewArea)
					break;
			}

			bool fitsInMatrix = (roomY + roomHeight <= height) && (roomX + roomWidth <= width);

			if (addsNewArea && fitsInMatrix)
				rooms.Add(newRoom);
		}

		foreach (RoomObject room in rooms)
			for (int y = room.Y; y < room.Y + room.Height; y++)
				for (int x = room.X; x < room.X + room.Width; x++)
					dungeonLevelMatrix[y, x] = floorChar;

		return dungeonLevelMatrix;
	}
}
