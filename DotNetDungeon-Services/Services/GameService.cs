namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Utils;
using System;
using System.Collections.Generic;

public class GameService : IGameService
{
	// REVIEW: Move to the json
	private const int MinSpaceBetweenRooms = 3;

	public char[,] GenerateDungeon(
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
		char floorChar)
	{
		char[,] dungeonMatrix = new char[height, width];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				dungeonMatrix[y, x] = nothingChar;
			}
		}

		List<Room> rooms = new List<Room>();

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				int randomValue = MathUtil.GenerateRandomInteger(1, maxProbability);

				if (randomValue <= minProbability)
				{
					TryCreateRoom(
						dungeonMatrix,
						rooms,
						height,
						width,
						y,
						x,
						roomHeightMin,
						roomHeightMax,
						roomWidthMin,
						roomWidthMax,
						nothingChar,
						wallChar,
						floorChar
					);
				}
			}
		}

		return dungeonMatrix;
	}

	private void TryCreateRoom(
		char[,] dungeonMatrix,
		List<Room> rooms,
		int totalHeight,
		int totalWidth,
		int startY,
		int startX,
		int roomHeightMin,
		int roomHeightMax,
		int roomWidthMin,
		int roomWidthMax,
		char nothingChar,
		char wallChar,
		char floorChar)
	{
		if (dungeonMatrix[startY, startX] != nothingChar)
			return;

		int roomHeight = MathUtil.GenerateRandomInteger(roomHeightMin, roomHeightMax);
		int roomWidth = MathUtil.GenerateRandomInteger(roomWidthMin, roomWidthMax);

		if (startY + roomHeight >= totalHeight || startX + roomWidth >= totalWidth)
			return;

		Room newRoom = new Room(startY, startX, roomHeight, roomWidth);

		foreach (var room in rooms)
		{
			if (RoomsAreTooClosed(room, newRoom))
				return;
		}

		BuildRoom(dungeonMatrix, newRoom, wallChar, floorChar);
		rooms.Add(newRoom);
	}

	private bool RoomsAreTooClosed(Room room1, Room room2)
	{
		int adjustedY = Math.Max(0, room1.Y - MinSpaceBetweenRooms);
		int adjustedX = Math.Max(0, room1.X - MinSpaceBetweenRooms);
		int adjustedHeight = room1.Height + (2 * MinSpaceBetweenRooms);
		int adjustedWidth = room1.Width + (2 * MinSpaceBetweenRooms);

		return !(
			adjustedY + adjustedHeight <= room2.Y ||
			room2.Y + room2.Height <= adjustedY ||
			adjustedX + adjustedWidth <= room2.X ||
			room2.X + room2.Width <= adjustedX
		);
	}

	private void BuildRoom(char[,] dungeonMatrix, Room room, char wallChar, char floorChar)
	{
		for (int y = room.Y; y < room.Y + room.Height; y++)
		{
			for (int x = room.X; x < room.X + room.Width; x++)
			{
				if (y == room.Y || y == room.Y + room.Height - 1 ||
					x == room.X || x == room.X + room.Width - 1)
					dungeonMatrix[y, x] = wallChar;
				else
					dungeonMatrix[y, x] = floorChar;
			}
		}
	}

	// REVIEW: Move to the another file
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
