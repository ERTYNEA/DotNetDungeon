using DotNetDungeon_Game.Models;
using DotNetDungeon_Game.Models.Consts;
using DotNetDungeon_Services.Services;
using DotNetDungeon_Utils;

var worldSettings = JsonService.ConvertJsonPathToModelObject(PathsConst.WorldSettingsPath, new WorldSettingsModel());
var dungeonSettings = JsonService.ConvertJsonPathToModelObject(PathsConst.DungeonSettingsPath, new DungeonSettings());
var dungeonCharSettings = JsonService.ConvertJsonPathToModelObject(PathsConst.DungeonCharSettingsPath, new DungeonCharSettingsModel());

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

		int randomValue = MathUtil.GenerateRandomInteger(1, maxProbability); // REVIEW: Delete - use a service and delete the reference Utils from the csproj.

		if (randomValue <= minProbability)
			cellChar = wallChar;

		dungeonMatrix[y, x] = cellChar;
	}
}

OutputService.PrintMatrix(dungeonMatrix);