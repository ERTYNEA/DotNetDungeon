namespace DotNetDungeon_Services.Services;

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

		List<Room> rooms = new List<Room>();

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

			Room newRoom = new Room(roomY, roomX, roomHeight, roomWidth);

			bool isDuplicate = false;
			foreach (Room existingRoom in rooms)
			{
				if (existingRoom.Y == newRoom.Y &&
					existingRoom.X == newRoom.X &&
					existingRoom.Height == newRoom.Height &&
					existingRoom.Width == newRoom.Width)
				{
					isDuplicate = true;
					break;
				}
			}

			bool fitsInMatrix = (roomY + roomHeight <= height) && (roomX + roomWidth <= width);

			if (!isDuplicate && fitsInMatrix)
				rooms.Add(newRoom);
		}

		foreach (Room room in rooms)
			for (int y = room.Y; y < room.Y + room.Height; y++)
				for (int x = room.X; x < room.X + room.Width; x++)
					dungeonLevelMatrix[y, x] = floorChar;

		return dungeonLevelMatrix;
	}

	private class Room
	{
		public int Y { get; }
		public int X { get; }
		public int Height { get; }
		public int Width { get; }

		public Room(int y, int x, int height, int width)
		{
			Y = y;
			X = x;
			Height = height;
			Width = width;
		}
	}
}
