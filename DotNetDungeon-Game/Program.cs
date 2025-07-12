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

if (!worldSettings.Width.HasValue || !worldSettings.Height.HasValue)
	throw new Exception();

if (!dungeonCharSettings.Nothing.HasValue || !dungeonCharSettings.Wall.HasValue || !dungeonCharSettings.Floor.HasValue)
	throw new Exception();

if (!dungeonSettings.RoomProbabilityMin.HasValue || !dungeonSettings.RoomProbabilityMax.HasValue)
	throw new Exception();

int height = worldSettings.Height.Value;
int width = worldSettings.Width.Value;
int minProbability = dungeonSettings.RoomProbabilityMin.Value;
int maxProbability = dungeonSettings.RoomProbabilityMax.Value;
char nothingChar = dungeonCharSettings.Nothing.Value;
char wallChar = dungeonCharSettings.Wall.Value;
char floorChar = dungeonCharSettings.Floor.Value;

char[,] dungeonMatrix = gameService.GenerateDungeon(
	height,
	width,
	minProbability,
	maxProbability,
	nothingChar,
	wallChar,
	floorChar);

outputService.PrintMatrix(dungeonMatrix);