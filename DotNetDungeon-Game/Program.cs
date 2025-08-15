using DotNetDungeon_Configs.Consts;
using DotNetDungeon_Models;
using DotNetDungeon_Objets;
using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Services.Services;
using Spectre.Console;

IJsonService jsonService = new JsonService();
IGameService gameService = new GameService();
IOutputService outputService = new OutputService();

var worldSettings = jsonService.ConvertJsonPathToModelObject(PathsConst.WorldSettingsPath, new WorldSettingsModel());
var dungeonSettings = jsonService.ConvertJsonPathToModelObject(PathsConst.DungeonSettingsPath, new DungeonSettingsModel());
var dungeonCharSettings = jsonService.ConvertJsonPathToModelObject(PathsConst.DungeonCharSettingsPath, new DungeonCharSettingsModel());

if (!worldSettings.Height.HasValue || !worldSettings.Width.HasValue)
	throw new Exception();

if (!dungeonSettings.RoomNumberForLevelMin.HasValue || !dungeonSettings.RoomNumberForLevelMax.HasValue ||
	!dungeonSettings.RoomHeightMin.HasValue || !dungeonSettings.RoomHeightMax.HasValue ||
	!dungeonSettings.RoomWidthMin.HasValue || !dungeonSettings.RoomWidthMax.HasValue)
	throw new Exception();

if (dungeonCharSettings.Nothing == null ||
	string.IsNullOrEmpty(dungeonCharSettings.Nothing.CharacterChar) ||
	string.IsNullOrEmpty(dungeonCharSettings.Nothing.CharacterColorText) ||
	string.IsNullOrEmpty(dungeonCharSettings.Nothing.CharacterColorBackground))
	throw new Exception();

if (dungeonCharSettings.Wall == null ||
	string.IsNullOrEmpty(dungeonCharSettings.Wall.CharacterChar) ||
	string.IsNullOrEmpty(dungeonCharSettings.Wall.CharacterColorText) ||
	string.IsNullOrEmpty(dungeonCharSettings.Wall.CharacterColorBackground))
	throw new Exception();

if (dungeonCharSettings.Floor == null ||
	string.IsNullOrEmpty(dungeonCharSettings.Floor.CharacterChar) ||
	string.IsNullOrEmpty(dungeonCharSettings.Floor.CharacterColorText) ||
	string.IsNullOrEmpty(dungeonCharSettings.Floor.CharacterColorBackground))
	throw new Exception();

int height = worldSettings.Height.Value;
int width = worldSettings.Width.Value;
int roomNumberForLevelMin = dungeonSettings.RoomNumberForLevelMin.Value;
int roomNumberForLevelMax = dungeonSettings.RoomNumberForLevelMax.Value;
int roomHeightMin = dungeonSettings.RoomHeightMin.Value;
int roomHeightMax = dungeonSettings.RoomHeightMax.Value;
int roomWidthMin = dungeonSettings.RoomWidthMin.Value;
int roomWidthMax = dungeonSettings.RoomWidthMax.Value;

Color nothingCharacterColorText, nothingCharacterColorBackground, wallCharacterColorText, wallCharacterColorBackground, floorCharacterColorText, floorCharacterColorBackground;

try
{
	nothingCharacterColorText = outputService.HexadecimalStringToColor(dungeonCharSettings.Nothing.CharacterColorText);
	nothingCharacterColorBackground = outputService.HexadecimalStringToColor(dungeonCharSettings.Nothing.CharacterColorBackground);
	wallCharacterColorText = outputService.HexadecimalStringToColor(dungeonCharSettings.Wall.CharacterColorText);
	wallCharacterColorBackground = outputService.HexadecimalStringToColor(dungeonCharSettings.Wall.CharacterColorBackground);
	floorCharacterColorText = outputService.HexadecimalStringToColor(dungeonCharSettings.Floor.CharacterColorText);
	floorCharacterColorBackground = outputService.HexadecimalStringToColor(dungeonCharSettings.Floor.CharacterColorBackground);
}
catch (Exception)
{
	throw new Exception();
}

TitleObject nothingTitleObject = new TitleObject
{
	CharacterChar = dungeonCharSettings.Nothing.CharacterChar[0],
	CharacterColorText = nothingCharacterColorText,
	CharacterColorBackground = nothingCharacterColorBackground
};

TitleObject wallTitleObject = new TitleObject
{
	CharacterChar = dungeonCharSettings.Wall.CharacterChar[0],
	CharacterColorText = wallCharacterColorText,
	CharacterColorBackground = wallCharacterColorBackground
};

TitleObject floorTitleObject = new TitleObject
{
	CharacterChar = dungeonCharSettings.Floor.CharacterChar[0],
	CharacterColorText = floorCharacterColorText,
	CharacterColorBackground = floorCharacterColorBackground
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