using DotNetDungeon_Console.Models;
using DotNetDungeon_Console.Models.Consts;
using DotNetDungeon_Console.Utils;

var worldSettings = JsonUtil.ConvertJsonPathToModelObject(PathsConst.WorldSettingsPath, new WorldSettingsModel());
var dungeonSettings = JsonUtil.ConvertJsonPathToModelObject(PathsConst.DungeonSettingsPath, new DungeonSettingsModel());

if (!worldSettings.Width.HasValue || !worldSettings.Height.HasValue)
	throw new Exception();

if (!dungeonSettings.Air.HasValue)
	throw new Exception();

int height = worldSettings.Height.Value;
int width = worldSettings.Width.Value;
char airChar = dungeonSettings.Air.Value;

char[,] dungeonMatrix = new char[height, width];

for (int y = 0; y < height; y++)
	for (int x = 0; x < width; x++)
		dungeonMatrix[y, x] = airChar;

OutputUtil.PrintMatrix(dungeonMatrix);