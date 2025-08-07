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
			bool overlapsWithExistingRoom = false;
			bool connectsWithExistingRoom = false;

			foreach (RoomObject existingRoom in rooms)
			{
				bool innerOverlap = 
					newRoom.X < existingRoom.X + existingRoom.Width - 1 && 
					newRoom.X + newRoom.Width - 1 > existingRoom.X && 
					newRoom.Y < existingRoom.Y + existingRoom.Height - 1 && 
					newRoom.Y + newRoom.Height - 1 > existingRoom.Y;
				
				if (innerOverlap)
				{
					overlapsWithExistingRoom = true;
					break;
				}
				
				bool horizontalConnection = 
					(newRoom.X + newRoom.Width == existingRoom.X || existingRoom.X + existingRoom.Width == newRoom.X) &&
					((newRoom.Y >= existingRoom.Y && newRoom.Y < existingRoom.Y + existingRoom.Height) ||
					(newRoom.Y + newRoom.Height > existingRoom.Y && newRoom.Y + newRoom.Height <= existingRoom.Y + existingRoom.Height) ||
					(existingRoom.Y >= newRoom.Y && existingRoom.Y < newRoom.Y + newRoom.Height) ||
					(existingRoom.Y + existingRoom.Height > newRoom.Y && existingRoom.Y + existingRoom.Height <= newRoom.Y + newRoom.Height));
					
				bool verticalConnection = 
					(newRoom.Y + newRoom.Height == existingRoom.Y || existingRoom.Y + existingRoom.Height == newRoom.Y) &&
					((newRoom.X >= existingRoom.X && newRoom.X < existingRoom.X + existingRoom.Width) ||
					(newRoom.X + newRoom.Width > existingRoom.X && newRoom.X + newRoom.Width <= existingRoom.X + existingRoom.Width) ||
					(existingRoom.X >= newRoom.X && existingRoom.X < newRoom.X + newRoom.Width) ||
					(existingRoom.X + existingRoom.Width > newRoom.X && existingRoom.X + existingRoom.Width <= newRoom.X + newRoom.Width));
				
				if (horizontalConnection || verticalConnection)
				{
					connectsWithExistingRoom = true;
				}
			}

			bool fitsInMatrix = (roomY + roomHeight <= height) && (roomX + roomWidth <= width);

			if (fitsInMatrix && !overlapsWithExistingRoom && (connectsWithExistingRoom || rooms.Count < 2))
				rooms.Add(newRoom);
		}

		foreach (RoomObject room in rooms)
			for (int y = room.Y; y < room.Y + room.Height; y++)
				for (int x = room.X; x < room.X + room.Width; x++)
					dungeonLevelMatrix[y, x] = floorChar;

		List<List<RoomObject>> islands = new List<List<RoomObject>>();
		List<RoomObject> remainingRooms = new List<RoomObject>(rooms);

		while (remainingRooms.Count > 0)
		{
			List<RoomObject> currentIsland = new List<RoomObject>();
			RoomObject startRoom = remainingRooms[0];
			currentIsland.Add(startRoom);
			remainingRooms.RemoveAt(0);

			bool addedRoomToIsland;
			
			do
			{
				addedRoomToIsland = false;
				
				for (int i = remainingRooms.Count - 1; i >= 0; i--)
				{
					RoomObject roomToCheck = remainingRooms[i];
					
					foreach (RoomObject islandRoom in currentIsland)
					{
						bool horizontalConnection = 
							(roomToCheck.X + roomToCheck.Width == islandRoom.X || islandRoom.X + islandRoom.Width == roomToCheck.X) &&
							((roomToCheck.Y >= islandRoom.Y && roomToCheck.Y < islandRoom.Y + islandRoom.Height) ||
							(roomToCheck.Y + roomToCheck.Height > islandRoom.Y && roomToCheck.Y + roomToCheck.Height <= islandRoom.Y + islandRoom.Height) ||
							(islandRoom.Y >= roomToCheck.Y && islandRoom.Y < roomToCheck.Y + roomToCheck.Height) ||
							(islandRoom.Y + islandRoom.Height > roomToCheck.Y && islandRoom.Y + islandRoom.Height <= roomToCheck.Y + roomToCheck.Height));
							
						bool verticalConnection = 
							(roomToCheck.Y + roomToCheck.Height == islandRoom.Y || islandRoom.Y + islandRoom.Height == roomToCheck.Y) &&
							((roomToCheck.X >= islandRoom.X && roomToCheck.X < islandRoom.X + islandRoom.Width) ||
							(roomToCheck.X + roomToCheck.Width > islandRoom.X && roomToCheck.X + roomToCheck.Width <= islandRoom.X + islandRoom.Width) ||
							(islandRoom.X >= roomToCheck.X && islandRoom.X < roomToCheck.X + roomToCheck.Width) ||
							(islandRoom.X + islandRoom.Width > roomToCheck.X && islandRoom.X + islandRoom.Width <= roomToCheck.X + roomToCheck.Width));
						
						if (horizontalConnection || verticalConnection)
						{
							currentIsland.Add(roomToCheck);
							remainingRooms.RemoveAt(i);
							addedRoomToIsland = true;
							break;
						}
					}
				}
			} while (addedRoomToIsland);
			
			islands.Add(currentIsland);
		}

		if (islands.Count > 1)
		{
			List<(int islandIndex, RoomObject representativeRoom)> islandRepresentatives = new List<(int, RoomObject)>();
			
			for (int i = 0; i < islands.Count; i++)
			{
				islandRepresentatives.Add((i, islands[i][0]));
			}
			
			HashSet<int> connectedIslands = new HashSet<int>();
			connectedIslands.Add(0);
			
			while (connectedIslands.Count < islands.Count)
			{
				int closestIsland1 = -1;
				int closestIsland2 = -1;
				int shortestDistance = int.MaxValue;
				
				foreach (int connectedIndex in connectedIslands)
				{
					for (int i = 0; i < islandRepresentatives.Count; i++)
					{
						if (connectedIslands.Contains(i))
							continue;
							
						RoomObject room1 = islandRepresentatives[connectedIndex].representativeRoom;
						RoomObject room2 = islandRepresentatives[i].representativeRoom;
						
						int centerX1 = room1.X + room1.Width / 2;
						int centerY1 = room1.Y + room1.Height / 2;
						int centerX2 = room2.X + room2.Width / 2;
						int centerY2 = room2.Y + room2.Height / 2;
						
						int distance = Math.Abs(centerX1 - centerX2) + Math.Abs(centerY1 - centerY2);
						
						if (distance < shortestDistance)
						{
							shortestDistance = distance;
							closestIsland1 = connectedIndex;
							closestIsland2 = i;
						}
					}
				}
				
				if (closestIsland1 != -1 && closestIsland2 != -1)
				{
					RoomObject room1 = islandRepresentatives[closestIsland1].representativeRoom;
					RoomObject room2 = islandRepresentatives[closestIsland2].representativeRoom;
					
					int centerX1 = room1.X + room1.Width / 2;
					int centerY1 = room1.Y + room1.Height / 2;
					int centerX2 = room2.X + room2.Width / 2;
					int centerY2 = room2.Y + room2.Height / 2;
					
					int corridorWidth = 3;
					int corridorHeight = 3;
					
					int startX = Math.Min(centerX1, centerX2);
					int endX = Math.Max(centerX1, centerX2);
					
					int horizontalY = centerY1;
					horizontalY = Math.Max(horizontalY, corridorHeight / 2);
					horizontalY = Math.Min(horizontalY, height - corridorHeight / 2 - 1);
					
					for (int x = startX; x <= endX; x++)
					{
						for (int y = horizontalY - corridorHeight / 2; y <= horizontalY + corridorHeight / 2; y++)
						{
							if (x >= 0 && x < width && y >= 0 && y < height)
							{
								dungeonLevelMatrix[y, x] = floorChar;
							}
						}
					}
					
					int verticalX = centerX2;
					verticalX = Math.Max(verticalX, corridorWidth / 2);
					verticalX = Math.Min(verticalX, width - corridorWidth / 2 - 1);
					
					int startY = Math.Min(horizontalY, centerY2);
					int endY = Math.Max(horizontalY, centerY2);
					
					for (int y = startY; y <= endY; y++)
					{
						for (int x = verticalX - corridorWidth / 2; x <= verticalX + corridorWidth / 2; x++)
						{
							if (x >= 0 && x < width && y >= 0 && y < height)
							{
								dungeonLevelMatrix[y, x] = floorChar;
							}
						}
					}
					
					connectedIslands.Add(closestIsland2);
				}
			}
		}

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if (dungeonLevelMatrix[y, x] == floorChar)
				{
					for (int dy = -1; dy <= 1; dy++)
					{
						for (int dx = -1; dx <= 1; dx++)
						{
							int newY = y + dy;
							int newX = x + dx;
							
							if (newY >= 0 && newY < height && newX >= 0 && newX < width)
							{
								if (dungeonLevelMatrix[newY, newX] == nothingChar)
								{
									dungeonLevelMatrix[newY, newX] = wallChar;
								}
							}
						}
					}
				}
			}
		}

		return dungeonLevelMatrix;
	}
}
