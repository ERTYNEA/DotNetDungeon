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
	/// <param name="nothingTitleObject">TitleObject representing empty space</param>
	/// <param name="wallTitleObject">TitleObject representing walls</param>
	/// <param name="floorTitleObject">TitleObject representing floors</param>
	/// <returns>A 2D TitleObject matrix representing the generated dungeon</returns>
	public TitleObject[,] GenerateDungeonLevel(
		int height,
		int width,
		int roomNumberForLevelMin,
		int roomNumberForLevelMax,
		int roomHeightMin,
		int roomHeightMax,
		int roomWidthMin,
		int roomWidthMax,
		TitleObject nothingTitleObject,
		TitleObject wallTitleObject,
		TitleObject floorTitleObject)
	{
		// Create a 2D TitleObject matrix to represent the dungeon level grid
		TitleObject[,] dungeonLevelMatrix = new TitleObject[height, width];

		// Initialize the dungeon matrix by filling it completely with empty space (nothingTitleObject)
		for (int y = 0; y < height; y++)
			for (int x = 0; x < width; x++)
				dungeonLevelMatrix[y, x] = nothingTitleObject;

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

		// Fill the dungeon level matrix with the floor (floorTitleObject) for each room
		foreach (RoomObject room in rooms)
			for (int y = room.Y; y < room.Y + room.Height; y++)
				for (int x = room.X; x < room.X + room.Width; x++)
					dungeonLevelMatrix[y, x] = floorTitleObject;

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
			// Create a list to hold representatives for each cluster
			List<(int clusterIndex, RoomObject representativeRoom)> clusterRepresentatives = new List<(int, RoomObject)>();

			// Initialize representatives with the first room of each cluster
			for (int i = 0; i < roomsClusters.Count; i++)
				clusterRepresentatives.Add((i, roomsClusters[i][0]));

			// Create a set to track connected clusters
			HashSet<int> connectedClusters = new HashSet<int>();
			connectedClusters.Add(0);

			// Continue until all clusters are connected
			while (connectedClusters.Count < roomsClusters.Count)
			{
				// Initialize variables to track the closest clusters and their distance
				int closestCluster1 = -1;
				int closestCluster2 = -1;
				int shortestDistance = int.MaxValue;

				// Find the closest pair of clusters
				foreach (int connectedIndex in connectedClusters)
				{
					// Compare with all other clusters
					for (int i = 0; i < clusterRepresentatives.Count; i++)
					{
						// Skip already connected clusters
						if (connectedClusters.Contains(i))
							continue;

						// Get the representative rooms for both clusters
						RoomObject room1 = clusterRepresentatives[connectedIndex].representativeRoom;
						RoomObject room2 = clusterRepresentatives[i].representativeRoom;

						// Calculate the center points of both rooms
						int centerX1 = room1.X + room1.Width / 2;
						int centerY1 = room1.Y + room1.Height / 2;
						int centerX2 = room2.X + room2.Width / 2;
						int centerY2 = room2.Y + room2.Height / 2;

						// Calculate the distance between the two center points
						int distance = Math.Abs(centerX1 - centerX2) + Math.Abs(centerY1 - centerY2);

						// Check if this pair is the closest so far
						if (distance < shortestDistance)
						{
							shortestDistance = distance;
							closestCluster1 = connectedIndex;
							closestCluster2 = i;
						}
					}
				}

				// If a closest pair was found, connect the clusters with a corridor
				if (closestCluster1 != -1 && closestCluster2 != -1)
				{
					// Get the representative rooms for both clusters
					RoomObject room1 = clusterRepresentatives[closestCluster1].representativeRoom;
					RoomObject room2 = clusterRepresentatives[closestCluster2].representativeRoom;

					// Calculate the center points of both rooms
					int centerX1 = room1.X + room1.Width / 2;
					int centerY1 = room1.Y + room1.Height / 2;
					int centerX2 = room2.X + room2.Width / 2;
					int centerY2 = room2.Y + room2.Height / 2;

					// Default corridor size
					int corridorWidth = 3;
					int corridorHeight = 3;

					// Calculate the horizontal corridor position
					int horizontalStartX = Math.Min(centerX1, centerX2);
					int horizontalEndX = Math.Max(centerX1, centerX2);
					int horizontalY = centerY1;
					horizontalY = Math.Max(horizontalY, corridorHeight / 2);
					horizontalY = Math.Min(horizontalY, height - corridorHeight / 2 - 1);

					// Create the horizontal corridor
					for (int x = horizontalStartX; x <= horizontalEndX; x++)
						for (int y = horizontalY - corridorHeight / 2; y <= horizontalY + corridorHeight / 2; y++)
							if (x >= 0 && x < width && y >= 0 && y < height)
								dungeonLevelMatrix[y, x] = floorTitleObject;

					// Calculate the vertical corridor position
					int verticalStartY = Math.Min(centerY1, centerY2);
					int verticalEndY = Math.Max(centerY1, centerY2);
					int verticalX = centerX2;
					verticalX = Math.Max(verticalX, corridorWidth / 2);
					verticalX = Math.Min(verticalX, width - corridorWidth / 2 - 1);

					// Create the vertical corridor
					for (int y = verticalStartY; y <= verticalEndY; y++)
						for (int x = verticalX - corridorWidth / 2; x <= verticalX + corridorWidth / 2; x++)
							if (x >= 0 && x < width && y >= 0 && y < height)
								dungeonLevelMatrix[y, x] = floorTitleObject;

					// Fill the corner area
					int cornerSize = Math.Max(corridorWidth, corridorHeight);
					for (int y = horizontalY - cornerSize / 2; y <= horizontalY + cornerSize / 2; y++)
						for (int x = verticalX - cornerSize / 2; x <= verticalX + cornerSize / 2; x++)
							if (x >= 0 && x < width && y >= 0 && y < height)
								dungeonLevelMatrix[y, x] = floorTitleObject;

					// Connect the clusters
					connectedClusters.Add(closestCluster2);
				}
			}
		}

		// Convert floor tiles to wall tiles if they're adjacent to nothing (nothingTitleObject) or at the edge of the world
		for (int y = 0; y < height; y++)
			for (int x = 0; x < width; x++)
				if (dungeonLevelMatrix[y, x].CharacterChar == floorTitleObject.CharacterChar)
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
							dungeonLevelMatrix[newY, newX].CharacterChar == nothingTitleObject.CharacterChar)
								dungeonLevelMatrix[y, x] = wallTitleObject;
						}

		// return the generated dungeon level matrix
		return dungeonLevelMatrix;
	}
}
