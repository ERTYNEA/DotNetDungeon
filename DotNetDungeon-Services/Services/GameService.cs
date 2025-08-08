namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Objets;
using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Utils;
using System.Collections.Generic;

public class GameService : IGameService
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
		// Create a 2D char matrix to represent the dungeon level grid
		char[,] dungeonLevelMatrix = new char[height, width];

		// Initialize the dungeon matrix by filling it completely with empty space (nothingChar)
		for (int y = 0; y < height; y++)
			for (int x = 0; x < width; x++)
				dungeonLevelMatrix[y, x] = nothingChar;

		// Create an empty list to store all rooms that will be generated
		List<RoomObject> rooms = new List<RoomObject>();

		// Randomly determine the number of rooms to generate in the level
		int roomsToGenerate = MathUtil.GenerateRandomInteger(roomNumberForLevelMin, roomNumberForLevelMax);

		// Initialize counters to prevent infinite loops if room placement becomes impossible
		int attemptsCount = 0;
		int maxAttempts = roomsToGenerate * 10;

		// Main room generation loop - continues until we have enough rooms or run out of attempts
		while (rooms.Count < roomsToGenerate && attemptsCount < maxAttempts)
		{
			// Increment attempt counter in each iteration
			attemptsCount++;

			// Generates random size and position ensuring the room fits within the dungeon
			int roomHeight = MathUtil.GenerateRandomInteger(roomHeightMin, roomHeightMax);
			int roomWidth = MathUtil.GenerateRandomInteger(roomWidthMin, roomWidthMax);
			int roomY = MathUtil.GenerateRandomInteger(0, height - roomHeight);
			int roomX = MathUtil.GenerateRandomInteger(0, width - roomWidth);

			// Create a new room object with the randomly generated position and dimensions
			RoomObject newRoom = new RoomObject(roomY, roomX, roomHeight, roomWidth);

			// For each existing room, check if the new room overlaps with it
			foreach (RoomObject existingRoom in rooms)
			{
				// Check for internal overlap between rooms
				bool innerOverlap =
					newRoom.X < existingRoom.X + existingRoom.Width &&
					newRoom.X + newRoom.Width > existingRoom.X &&
					newRoom.Y < existingRoom.Y + existingRoom.Height &&
					newRoom.Y + newRoom.Height > existingRoom.Y;

				// If rooms overlap exit the loop
				if (innerOverlap)
					break;
			}

			// Add the new room to the list of rooms
			rooms.Add(newRoom);
		}

		// Fill the dungeon level matrix with the floor (floorChar) for each room
		foreach (RoomObject room in rooms)
			for (int y = room.Y; y < room.Y + room.Height; y++)
				for (int x = room.X; x < room.X + room.Width; x++)
					dungeonLevelMatrix[y, x] = floorChar;

		// Create lists to track room clusters for ensuring dungeon connectivity
		List<RoomObject> remainingRooms = new List<RoomObject>(rooms);
		List<List<RoomObject>> roomsClusters = new List<List<RoomObject>>();

		// Group connected rooms into clusters of interconnected rooms
		while (remainingRooms.Count > 0)
		{
			// Create a new cluster starting with the first available room
			List<RoomObject> currentRoomsCluster = new List<RoomObject>();
			RoomObject startRoom = remainingRooms[0];
			currentRoomsCluster.Add(startRoom);
			remainingRooms.RemoveAt(0);

			// Flag to track whether any room was added to the current cluster in each iteration
			bool addedRoomToCluster;

			// Repeatedly scan for rooms that connect to the current cluster until no more are found
			do
			{
				// Reset flag at the start of each iteration of the cluster growth process
				addedRoomToCluster = false;

				// Iterate through remaining rooms in reverse to safely remove items while iterating
				for (int i = remainingRooms.Count - 1; i >= 0; i--)
				{
					// Get the current room to evaluate for adjacency with the existing cluster
					RoomObject roomToCheck = remainingRooms[i];

					// Compare the room against each room in the current cluster to check for adjacency
					foreach (RoomObject clusterRoom in currentRoomsCluster)
					{
						// Check if rooms are adjacent horizontally with any vertical overlap
						bool horizontalConnection =
							(roomToCheck.X + roomToCheck.Width == clusterRoom.X || clusterRoom.X + clusterRoom.Width == roomToCheck.X) &&
							((roomToCheck.Y >= clusterRoom.Y && roomToCheck.Y < clusterRoom.Y + clusterRoom.Height) ||
							(roomToCheck.Y + roomToCheck.Height > clusterRoom.Y && roomToCheck.Y + roomToCheck.Height <= clusterRoom.Y + clusterRoom.Height) ||
							(clusterRoom.Y >= roomToCheck.Y && clusterRoom.Y < roomToCheck.Y + roomToCheck.Height) ||
							(clusterRoom.Y + clusterRoom.Height > roomToCheck.Y && clusterRoom.Y + clusterRoom.Height <= roomToCheck.Y + roomToCheck.Height));

						// Check if rooms are adjacent vertically with any horizontal overlap
						bool verticalConnection =
							(roomToCheck.Y + roomToCheck.Height == clusterRoom.Y || clusterRoom.Y + clusterRoom.Height == roomToCheck.Y) &&
							((roomToCheck.X >= clusterRoom.X && roomToCheck.X < clusterRoom.X + clusterRoom.Width) ||
							(roomToCheck.X + roomToCheck.Width > clusterRoom.X && roomToCheck.X + roomToCheck.Width <= clusterRoom.X + clusterRoom.Width) ||
							(clusterRoom.X >= roomToCheck.X && clusterRoom.X < roomToCheck.X + roomToCheck.Width) ||
							(clusterRoom.X + clusterRoom.Width > roomToCheck.X && clusterRoom.X + clusterRoom.Width <= roomToCheck.X + roomToCheck.Width));

						// If a connection exists, add the room to the current cluster and mark it for removal
						if (horizontalConnection || verticalConnection)
						{
							currentRoomsCluster.Add(roomToCheck);
							remainingRooms.RemoveAt(i);
							addedRoomToCluster = true;
							break;
						}
					}
				}
			} while (addedRoomToCluster);

			// Add the completed cluster to the list of room clusters
			roomsClusters.Add(currentRoomsCluster);
		}

		// Connect all separate room clusters with L-shaped corridors to ensure full dungeon traversability
		if (roomsClusters.Count > 1)
		{
			List<(int clusterIndex, RoomObject representativeRoom)> clusterRepresentatives = new List<(int, RoomObject)>();

			for (int i = 0; i < roomsClusters.Count; i++)
			{
				clusterRepresentatives.Add((i, roomsClusters[i][0]));
			}

			HashSet<int> connectedClusters = new HashSet<int>();
			connectedClusters.Add(0);

			while (connectedClusters.Count < roomsClusters.Count)
			{
				int closestCluster1 = -1;
				int closestCluster2 = -1;
				int shortestDistance = int.MaxValue;

				foreach (int connectedIndex in connectedClusters)
				{
					for (int i = 0; i < clusterRepresentatives.Count; i++)
					{
						if (connectedClusters.Contains(i))
							continue;

						RoomObject room1 = clusterRepresentatives[connectedIndex].representativeRoom;
						RoomObject room2 = clusterRepresentatives[i].representativeRoom;

						int centerX1 = room1.X + room1.Width / 2;
						int centerY1 = room1.Y + room1.Height / 2;
						int centerX2 = room2.X + room2.Width / 2;
						int centerY2 = room2.Y + room2.Height / 2;

						int distance = Math.Abs(centerX1 - centerX2) + Math.Abs(centerY1 - centerY2);

						if (distance < shortestDistance)
						{
							shortestDistance = distance;
							closestCluster1 = connectedIndex;
							closestCluster2 = i;
						}
					}
				}

				if (closestCluster1 != -1 && closestCluster2 != -1)
				{
					RoomObject room1 = clusterRepresentatives[closestCluster1].representativeRoom;
					RoomObject room2 = clusterRepresentatives[closestCluster2].representativeRoom;

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

					connectedClusters.Add(closestCluster2);
				}
			}
		}

		// TODO - FIX: There's a "small" bug with corridors when they form an "L" shape, as they get blocked by a wall
		// Convert floor tiles to wall tiles if they're adjacent to nothing (nothingChar) or at the edge of the world
		for (int y = 0; y < height; y++)
			for (int x = 0; x < width; x++)
				if (dungeonLevelMatrix[y, x] == floorChar)
					for (int dy = -1; dy <= 1; dy++)
						for (int dx = -1; dx <= 1; dx++)
						{
							// Skip the cell itself
							if (dy == 0 && dx == 0)
								continue;

							// Calculate the coordinates of the adjacent cell
							int newY = y + dy;
							int newX = x + dx;

							// If adjacent cell is outside the dungeon bounds or contains nothing
							if (newY < 0 || newY >= height || newX < 0 || newX >= width ||
								dungeonLevelMatrix[newY, newX] == nothingChar)
								dungeonLevelMatrix[y, x] = wallChar;
						}

		// return the generated dungeon level matrix
		return dungeonLevelMatrix;
	}
}
