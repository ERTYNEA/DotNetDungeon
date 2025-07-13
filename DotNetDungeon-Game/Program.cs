using DotNetDungeon_Configs.Consts;
using DotNetDungeon_Models;
using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Services.Services;

IJsonService jsonService = new JsonService();
IGameService gameService = new GameService();
IOutputService outputService = new OutputService();

var worldSettings = jsonService.ConvertJsonPathToModelObject(PathsConst.WorldSettingsPath, new WorldSettingsModel());
var dungeonSettings = jsonService.ConvertJsonPathToModelObject(PathsConst.DungeonSettingsPath, new DungeonSettings());
var dungeonCharSettings = jsonService.ConvertJsonPathToModelObject(PathsConst.DungeonCharSettingsPath, new DungeonCharSettingsModel());

if (!worldSettings.Height.HasValue || !worldSettings.Width.HasValue)
	throw new Exception();

if (!dungeonSettings.RoomNumberForLevelMin.HasValue || !dungeonSettings.RoomNumberForLevelMax.HasValue ||
	!dungeonSettings.RoomHeightMin.HasValue || !dungeonSettings.RoomHeightMax.HasValue ||
	!dungeonSettings.RoomWidthMin.HasValue || !dungeonSettings.RoomWidthMax.HasValue)
	throw new Exception();

if (!dungeonCharSettings.Nothing.HasValue || !dungeonCharSettings.Wall.HasValue || !dungeonCharSettings.Floor.HasValue)
	throw new Exception();

int height = worldSettings.Height.Value;
int width = worldSettings.Width.Value;
int roomNumberForLevelMin = dungeonSettings.RoomNumberForLevelMin.Value;
int roomNumberForLevelMax = dungeonSettings.RoomNumberForLevelMax.Value;
int roomHeightMin = dungeonSettings.RoomHeightMin.Value;
int roomHeightMax = dungeonSettings.RoomHeightMax.Value;
int roomWidthMin = dungeonSettings.RoomWidthMin.Value;
int roomWidthMax = dungeonSettings.RoomWidthMax.Value;
char nothingChar = dungeonCharSettings.Nothing.Value;
char wallChar = dungeonCharSettings.Wall.Value;
char floorChar = dungeonCharSettings.Floor.Value;

char[,] dungeonMatrix = gameService.GenerateDungeonLevel(
	height,
	width,
	roomNumberForLevelMin,
	roomNumberForLevelMax,
	roomHeightMin,
	roomHeightMax,
	roomWidthMin,
	roomWidthMax,
	nothingChar,
	wallChar,
	floorChar);

outputService.PrintMatrix(dungeonMatrix);