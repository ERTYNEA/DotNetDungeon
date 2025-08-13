namespace DotNetDungeon_Models;

public class CharacterSettings
{
    public string? CharacterChar { get; set; }
    public string? CharacterColorText { get; set; }
    public string? CharacterColorBackground { get; set; }
}

public class DungeonCharSettingsModel
{
    public CharacterSettings? Nothing { get; set; }
    public CharacterSettings? Wall { get; set; }
    public CharacterSettings? Floor { get; set; }
}