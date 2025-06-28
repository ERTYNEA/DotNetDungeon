using DotNetDungeon_Console.Models;
using DotNetDungeon_Console.Models.Consts;
using DotNetDungeon_Console.Utils;

// Load Jsons
var worldSettings = JsonUtil.ConvertJsonPathToModelObject(PathsConst.WorldSettingsPath, new WorldSettingsModel());
var dungeonSettings = JsonUtil.ConvertJsonPathToModelObject(PathsConst.DungeonSettingsPath, new DungeonSettingsModel());

// Display World Settings
Console.WriteLine("World Settings:");
Console.WriteLine($"Width: {worldSettings.Width}");
Console.WriteLine($"Height: {worldSettings.Height}");

Console.WriteLine();

// Display Dungeon Settings
Console.WriteLine("Dungeon Settings:");
Console.WriteLine($"Air character: '{dungeonSettings.Air}'");
Console.WriteLine($"Wall character: '{dungeonSettings.Wall}'");