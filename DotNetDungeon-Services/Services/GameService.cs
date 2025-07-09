namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Utils;

public class GameService : IGameService
{
	public char[,] GenerateDungeon(int height, int width, int minProbability, int maxProbability, char airChar, char wallChar)
	{
		char[,] dungeonMatrix = new char[height, width];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				char cellChar = airChar;

				int randomValue = MathUtil.GenerateRandomInteger(1, maxProbability);

				if (randomValue <= minProbability)
					cellChar = wallChar;

				dungeonMatrix[y, x] = cellChar;
			}
		}

		return dungeonMatrix;
	}
}
