namespace DotNetDungeon_Objets;

public class RoomObject
{
	public int Y { get; }
	public int X { get; }
	public int Height { get; }
	public int Width { get; }

	// TODO - COPILOT: We need to put a summary here that indicates what this method does
	public RoomObject(int y, int x, int height, int width)
	{
		Y = y;
		X = x;
		Height = height;
		Width = width;
	}
}
