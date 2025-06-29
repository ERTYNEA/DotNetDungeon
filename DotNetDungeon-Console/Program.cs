using DotNetDungeon_Console.Models;
using DotNetDungeon_Console.Models.Consts;
using DotNetDungeon_Console.Utils;

var worldSettings = JsonUtil.ConvertJsonPathToModelObject(PathsConst.WorldSettingsPath, new WorldSettingsModel());
var dungeonSettings = JsonUtil.ConvertJsonPathToModelObject(PathsConst.DungeonSettingsPath, new DungeonSettings());
var dungeonCharSettings = JsonUtil.ConvertJsonPathToModelObject(PathsConst.DungeonCharSettingsPath, new DungeonCharSettingsModel());

if (!worldSettings.Width.HasValue || !worldSettings.Height.HasValue)
	throw new Exception();

if (!dungeonCharSettings.Air.HasValue || !dungeonCharSettings.Wall.HasValue)
	throw new Exception();

if (!dungeonSettings.RoomProbabilityMin.HasValue || !dungeonSettings.RoomProbabilityMax.HasValue)
	throw new Exception();

int height = worldSettings.Height.Value;
int width = worldSettings.Width.Value;
int minProbability = dungeonSettings.RoomProbabilityMin.Value;
int maxProbability = dungeonSettings.RoomProbabilityMax.Value;
char airChar = dungeonCharSettings.Air.Value;
char wallChar = dungeonCharSettings.Wall.Value;

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

OutputUtil.PrintMatrix(dungeonMatrix);