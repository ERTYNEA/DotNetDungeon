using DotNetDungeon_Configs.Consts;
using DotNetDungeon_Models;
using DotNetDungeon_Objets;
using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Services.Services;
using System.Drawing;

IJsonService jsonService = new JsonService();
IGameService gameService = new GameService();
IOutputService outputService = new OutputService(Console.Out);

var worldSettings = jsonService.ConvertJsonPathToModelObject(PathsConst.WorldSettingsPath, new WorldSettingsModel());
var dungeonSettings = jsonService.ConvertJsonPathToModelObject(PathsConst.DungeonSettingsPath, new DungeonSettingsModel());
var dungeonCharSettings = jsonService.ConvertJsonPathToModelObject(PathsConst.DungeonCharSettingsPath, new DungeonCharSettingsModel());

if (!worldSettings.Height.HasValue || !worldSettings.Width.HasValue)
	throw new Exception();

if (!dungeonSettings.RoomNumberForLevelMin.HasValue || !dungeonSettings.RoomNumberForLevelMax.HasValue ||
	!dungeonSettings.RoomHeightMin.HasValue || !dungeonSettings.RoomHeightMax.HasValue ||
	!dungeonSettings.RoomWidthMin.HasValue || !dungeonSettings.RoomWidthMax.HasValue)
	throw new Exception();

// Validate DungeonCharSettings
if (dungeonCharSettings.Nothing == null || dungeonCharSettings.Wall == null || dungeonCharSettings.Floor == null)
	throw new Exception("Missing dungeon character settings");

// Validate required character settings fields
if (string.IsNullOrEmpty(dungeonCharSettings.Nothing.CharacterChar) || 
    string.IsNullOrEmpty(dungeonCharSettings.Nothing.CharacterColorText) || 
    string.IsNullOrEmpty(dungeonCharSettings.Nothing.CharacterColorBackground))
	throw new Exception("Missing Nothing character settings");

if (string.IsNullOrEmpty(dungeonCharSettings.Wall.CharacterChar) || 
    string.IsNullOrEmpty(dungeonCharSettings.Wall.CharacterColorText) || 
    string.IsNullOrEmpty(dungeonCharSettings.Wall.CharacterColorBackground))
	throw new Exception("Missing Wall character settings");

if (string.IsNullOrEmpty(dungeonCharSettings.Floor.CharacterChar) || 
    string.IsNullOrEmpty(dungeonCharSettings.Floor.CharacterColorText) || 
    string.IsNullOrEmpty(dungeonCharSettings.Floor.CharacterColorBackground))
	throw new Exception("Missing Floor character settings");

// Validate that color values are valid for Colorful.Console
Color nothingTextColor, nothingBgColor, wallTextColor, wallBgColor, floorTextColor, floorBgColor;

try
{
    nothingTextColor = Color.FromName(dungeonCharSettings.Nothing.CharacterColorText);
    nothingBgColor = Color.FromName(dungeonCharSettings.Nothing.CharacterColorBackground);
    wallTextColor = Color.FromName(dungeonCharSettings.Wall.CharacterColorText);
    wallBgColor = Color.FromName(dungeonCharSettings.Wall.CharacterColorBackground);
    floorTextColor = Color.FromName(dungeonCharSettings.Floor.CharacterColorText);
    floorBgColor = Color.FromName(dungeonCharSettings.Floor.CharacterColorBackground);
}
catch (Exception)
{
    throw new Exception("Invalid color name in settings");
}

int height = worldSettings.Height.Value;
int width = worldSettings.Width.Value;
int roomNumberForLevelMin = dungeonSettings.RoomNumberForLevelMin.Value;
int roomNumberForLevelMax = dungeonSettings.RoomNumberForLevelMax.Value;
int roomHeightMin = dungeonSettings.RoomHeightMin.Value;
int roomHeightMax = dungeonSettings.RoomHeightMax.Value;
int roomWidthMin = dungeonSettings.RoomWidthMin.Value;
int roomWidthMax = dungeonSettings.RoomWidthMax.Value;

// Create TitleObjects for each tile type
TitleObject nothingTitleObject = new TitleObject
{
    CharacterChar = dungeonCharSettings.Nothing.CharacterChar[0],
    CharacterColorText = nothingTextColor,
    CharacterColorBackground = nothingBgColor
};

TitleObject wallTitleObject = new TitleObject
{
    CharacterChar = dungeonCharSettings.Wall.CharacterChar[0],
    CharacterColorText = wallTextColor,
    CharacterColorBackground = wallBgColor
};

TitleObject floorTitleObject = new TitleObject
{
    CharacterChar = dungeonCharSettings.Floor.CharacterChar[0],
    CharacterColorText = floorTextColor,
    CharacterColorBackground = floorBgColor
};

TitleObject[,] dungeonMatrix = gameService.GenerateDungeonLevel(
	height,
	width,
	roomNumberForLevelMin,
	roomNumberForLevelMax,
	roomHeightMin,
	roomHeightMax,
	roomWidthMin,
	roomWidthMax,
	nothingTitleObject,
	wallTitleObject,
	floorTitleObject);

outputService.PrintMatrix(dungeonMatrix);